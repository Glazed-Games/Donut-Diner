using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DonutDiner.UIModule.Menu
{
    // First implemented by Yuval Vinter

    public class LoadingBehavior : MonoBehaviour
    {
        [SerializeField] private Image _loadBar;

        private void Start()
        {
            StartCoroutine(LoadAsyncOperation());
        }

        private IEnumerator LoadAsyncOperation()
        {
            AsyncOperation gameLevel = SceneManager.LoadSceneAsync(SceneLoadData.LevelToLoad);
            gameLevel.allowSceneActivation = false;

            while (gameLevel.progress < 0.9f)
            {
                _loadBar.fillAmount = gameLevel.progress / 2.0f;
                yield return new WaitForEndOfFrame();
            }

            while (Time.timeSinceLevelLoad < 1.0f)
            {
                _loadBar.fillAmount = gameLevel.progress / 2.0f + (Time.timeSinceLevelLoad / 2.0f);
                yield return new WaitForSecondsRealtime(0.01f);
            }

            gameLevel.allowSceneActivation = true;
        }
    }
}