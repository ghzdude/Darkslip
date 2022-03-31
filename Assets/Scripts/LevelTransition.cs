using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private Scene currentScene;
    private Scene manager;
    public string nextScene;

    private void Awake()
    {
        manager = SceneManager.GetSceneByBuildIndex(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentScene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
            // manager.GetRootGameObjects();
        }
    }
}
