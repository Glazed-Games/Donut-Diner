using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace DonutDiner.FrameworkModule.Data
{
    // Static class for keeping track of game progress, and serializing data to save file.

    // First implemented by Ben Russell

    public static class GameData
    {
        // _currentGameProgress keeps track of the player's linear progress through the game's story.
        //  - Access _currentGameProgress with the CurrentGameProgress property.
        //  - Update the stage of progress with TryUpdateProgress() which will not allow backwards movement.
        //  - It is planned to add binary values to GameData to reflect important player choices in the narrative.
        public enum GameProgress // Set all values so they do not drift with additions, leave room!
        {
            NULL = -1,

            NewGame = 0,

            EnterWorld1 = 100,

            EnterWorld2 = 200,

            EnterWorld3 = 300,
        }

        #region Fields

        private static GameProgress _currentGameProgress = GameProgress.NULL;

        public static GameProgress CurrentGameProgress => _currentGameProgress;

        // Current Save File Location:  C:\Users\*USER*\AppData\LocalLow\GlazedGames\Welcome to the Donut Shop
        private static readonly string _filePath = Application.persistentDataPath + "/";

        // File Version is used to allow the handling of files created from all stages of development
        // Version 0: Initial format
        // ...
        private static readonly int _currentFileVersion = 0;

        // Variable to save the culture setting of user's OS.
        // This is so the culture setting can be changed temporarily, allowing predictable data serialization.
        // See SetMonocultureForDataSerialization() and ReturnToSystemCulture()
        private static CultureInfo _systemCulture = null;

        #endregion

        #region Data Serialization Methods

        public static void InitData()
        {
            // Instantiate new-game data
            _currentGameProgress = GameProgress.NewGame;

            //...
        }

        public static string GetSaveFilePath()
        {
            return _filePath + "SaveFile" + GetCurrentFileNum().ToString() + ".txt";
        }

        // Returns whether a valid file was found. If not, must be a new game.
        public static bool LoadFromFile(int fileNum = -1, bool reloadingFile = false)
        {
            if (fileNum != -1)
            {
                // Loading a different save file
                SetCurrentFileNum(fileNum);
            }

            SetMonocultureForDataSerialization();

            try
            {
                using (var streamReader = new StreamReader(GetSaveFilePath()))
                {
                    // Save file exists, load data
                    Serialize.CurrentStreamReader = streamReader;
                    string line, str;
                    string[] lineWords;
                    int thisfileVersion = 0;

                    // Read file
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        // Split line into words
                        lineWords = line.Split(" "[0]);

                        switch (lineWords[0])
                        {
                            case "FileVersion":
                                thisfileVersion = int.Parse(lineWords[1]);
                                break;

                            case "CurrentScene":
                                if (!reloadingFile)
                                {
                                    if (Application.CanStreamedLevelBeLoaded(lineWords[1]))
                                    {
                                        // Load scene and wait to finish reading save-file until systems are ready to accept data
                                        GameManager.Instance.LoadScene(lineWords[1], true);
                                        return true;
                                    }
                                    else
                                    {
                                        throw new Exception("Scene named in save file " + GetCurrentFileNum() + " not found!");
                                    }
                                }
                                break;

                            case "Progress":
                                _currentGameProgress = (GameProgress)int.Parse(lineWords[1]);
                                break;

                            case "PlayerPosition":
                                // Must override player controller, as it has no methods for this
                                SerializationManager.Instance().PlayerController.enabled = false;
                                SerializationManager.Instance().PlayerController.transform.position = Serialize.ReadVector3();
                                SerializationManager.Instance().PlayerController.enabled = true;
                                break;

                            case "PlayerRotation":
                                // Must override player controller, as it has no methods for this
                                SerializationManager.Instance().PlayerController.enabled = false;
                                SerializationManager.Instance().PlayerController.transform.rotation = Quaternion.Euler(Serialize.ReadVector3());
                                SerializationManager.Instance().PlayerController.enabled = true;
                                break;

                            case "CameraRotation":
                                Camera.main.transform.rotation = Quaternion.Euler(Serialize.ReadVector3());
                                break;

                            case "Obj":
                                // Serialized Object
                                Serialize.ReadString(); // Skip object name
                                str = Serialize.ReadString(); // Read GUID
                                SerializableObject serializableObject = SerializationManager.Instance().GetObjectByGuid(str);

                                if (serializableObject)
                                {
                                    serializableObject.ReadData();
                                }
                                else
                                {
                                    // Object not found. Scan to end of entry to continue processing
                                    while ((line = streamReader.ReadLine()) != null)
                                    {
                                        if (line == "End Obj")
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }

                ReturnToSystemCulture();

                // Destroy Serialized Objects not found in save file (They weren't around to be saved)
                SerializationManager.Instance().DestroyUnserializedObjects();

                // Valid file
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Save file invalid, a new one has been started. " + e.Message);

                // Can't find save file, create one- assuming new game
                InitData();
                SaveToFile();
                ReturnToSystemCulture();

                // TODO: Load appropriate scene for new game

                // Invalid file
                return false;
            }
        }

        public static void SaveToFile()
        {
            SetMonocultureForDataSerialization();

            // Overwrite save file
            using (var streamWriter = new StreamWriter(GetSaveFilePath(), false))
            {
                Serialize.CurrentStreamWriter = streamWriter;

                // Current file version
                streamWriter.WriteLine("FileVersion " + _currentFileVersion.ToString());

                // Current Scene info
                streamWriter.WriteLine("CurrentScene " + GameManager.Instance.GetCurrentSceneName());

                // Game Progress
                streamWriter.WriteLine("Progress " + (int)_currentGameProgress + " " + _currentGameProgress.ToString());

                // Write player data
                if (SerializationManager.Instance().PlayerController != null)
                {
                    Serialize.Write("\nPlayerPosition");
                    Serialize.Write(SerializationManager.Instance().PlayerController.transform.position);
                    Serialize.Write("PlayerRotation");
                    Serialize.Write(SerializationManager.Instance().PlayerController.transform.rotation.eulerAngles);
                    Serialize.Write("CameraRotation");
                    Serialize.Write(Camera.main.transform.rotation.eulerAngles);
                }

                // Write Serializable Objects of scene
                foreach (var serializableObject in SerializationManager.Instance().SerializedObjects)
                {
                    if (serializableObject)
                    {
                        Serialize.Write("\nObj");
                        serializableObject.WriteData();
                        Serialize.Write("End Obj");
                    }
                }
            }

            ReturnToSystemCulture();
        }

        public static int GetCurrentFileNum()
        {
            SetMonocultureForDataSerialization();

            // Read CurrentSave file
            try
            {
                using (var streamReader = new StreamReader(_filePath + "CurrentSave.txt"))
                {
                    // Save file exists, load data
                    Serialize.CurrentStreamReader = streamReader;
                    int fileNum = Serialize.ReadInt();

                    if (fileNum > 0 && fileNum < 100)
                    {
                        ReturnToSystemCulture();
                        return fileNum;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Current Save not found, creating file. " + e.Message);
            }

            ReturnToSystemCulture();

            // Failed to get valid value, overwrite with default
            SetCurrentFileNum(1);
            return 1;
        }

        public static void SetCurrentFileNum(int fileNum)
        {
            SetMonocultureForDataSerialization();

            // Overwrite CurrentSave.txt
            using (var streamWriter = new StreamWriter(_filePath + "CurrentSave.txt", false))
            {
                Serialize.CurrentStreamWriter = streamWriter;
                Serialize.Write(fileNum);
            }

            ReturnToSystemCulture();
        }

        #endregion

        #region Data Update Methods

        public static bool TryUpdateProgress(GameProgress gameProgress)
        {
            if (gameProgress <= _currentGameProgress)
            {
                // Cannot progress backwards, fail
                return false;
            }
            else
            {
                Debug.Log("Game has progressed from (" + _currentGameProgress.ToString() + ") to (" + gameProgress.ToString() + ")");
                _currentGameProgress = gameProgress;
                return true;
            }
        }

        #endregion

        #region Internal Methods

        // See _systemCulture comment for explanation
        private static void SetMonocultureForDataSerialization()
        {
            // Save system's culture
            _systemCulture = CultureInfo.DefaultThreadCurrentCulture;

            // Set culture for predictable data serialization
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }

        private static void ReturnToSystemCulture()
        {
            // Reassign proper culture
            CultureInfo.DefaultThreadCurrentCulture = _systemCulture;
            CultureInfo.DefaultThreadCurrentUICulture = _systemCulture;
        }

        #endregion
    }
}