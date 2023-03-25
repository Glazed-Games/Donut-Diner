using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonutDiner.UIModule.Menu
{
    // First implemented by Yuval Vinter

    public class UIMenuButtons : MonoBehaviour
    {
        public void LoadScene(int index)
        {
            SceneLoadData.LevelToLoad = index;
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}