using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace DonutDiner.FrameworkModule.Data
{
    // Serialize functions for many data structures

    // First implemented by Ben Russell

    public static class Serialize
    {
        #region Fields

        // Assign current stream writer/reader before utilizing functions
        public static StreamWriter CurrentStreamWriter;
        public static StreamReader CurrentStreamReader;

        #endregion

        #region Write Functions

        public static void Write(string s)
        {
            Assert.IsNotNull(CurrentStreamWriter);
            CurrentStreamWriter.WriteLine(s);
        }

        public static void Write(bool b)
        {
            Assert.IsNotNull(CurrentStreamWriter);
            CurrentStreamWriter.WriteLine(b.ToString());
        }

        public static void Write(int i)
        {
            Assert.IsNotNull(CurrentStreamWriter);
            CurrentStreamWriter.WriteLine(i.ToString());
        }

        public static void Write(float f)
        {
            Assert.IsNotNull(CurrentStreamWriter);
            CurrentStreamWriter.WriteLine(f.ToString());
        }

        public static void Write(Vector2 v2)
        {
            Assert.IsNotNull(CurrentStreamWriter);
            CurrentStreamWriter.WriteLine(v2.x.ToString() + " " + v2.y.ToString());
        }

        public static void Write(Vector3 v3)
        {
            Assert.IsNotNull(CurrentStreamWriter);
            CurrentStreamWriter.WriteLine(v3.x.ToString() + " " + v3.y.ToString() + " " + v3.z.ToString());
        }

        public static void Write(DateTime dt)
        {
            Assert.IsNotNull(CurrentStreamWriter);
            CurrentStreamWriter.WriteLine(dt.Year.ToString() + " " + dt.Month.ToString() + " " + dt.Day.ToString() + " " 
                + dt.Hour.ToString() + " " + dt.Minute.ToString() + " " + dt.Second.ToString() + " " + dt.Millisecond.ToString());
        }

        #endregion

        #region Read Functions

        public static string ReadString()
        {
            Assert.IsNotNull(CurrentStreamReader);
            return CurrentStreamReader.ReadLine();
        }

        public static bool ReadBool()
        {
            Assert.IsNotNull(CurrentStreamReader);
            return bool.Parse(CurrentStreamReader.ReadLine());
        }

        public static int ReadInt()
        {
            Assert.IsNotNull(CurrentStreamReader);
            return int.Parse(CurrentStreamReader.ReadLine());
        }

        public static float ReadFloat()
        {
            Assert.IsNotNull(CurrentStreamReader);
            return float.Parse(CurrentStreamReader.ReadLine());
        }

        public static Vector2 ReadVector2()
        {
            Assert.IsNotNull(CurrentStreamReader);
            string[] words = CurrentStreamReader.ReadLine().Split(' ');
            Assert.IsTrue(words.Length == 2);
            return new Vector2(float.Parse(words[0]), float.Parse(words[1]));
        }

        public static Vector3 ReadVector3()
        {
            Assert.IsNotNull(CurrentStreamReader);
            string[] words = CurrentStreamReader.ReadLine().Split(' ');
            Assert.IsTrue(words.Length == 3);
            return new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2]));
        }

        public static DateTime ReadDateTime()
        {
            Assert.IsNotNull(CurrentStreamReader);
            string[] words = CurrentStreamReader.ReadLine().Split(' ');
            Assert.IsTrue(words.Length == 7);
            return new DateTime(int.Parse(words[0]), int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3]), int.Parse(words[4]), int.Parse(words[5]), int.Parse(words[6]));
        }

        #endregion
    }
}