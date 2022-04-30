using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private Scene currentScene;
    private Scene manager;
    private SceneController SceneController;
    public string nextScene;

    private void Awake() {
        manager = SceneManager.GetSceneByBuildIndex(0);
        SceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            currentScene = SceneManager.GetActiveScene();

            SceneController.SetOffset(collision.transform.position - transform.position);
            if (currentScene == manager) {
                Debug.Log("Manager is the active scene, cannot unload!");
                return;
            }
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
            
            // Debug.Log("scene unloaded: " + currentScene.name + " | attempting to load: " + nextScene);
        }
    }
}
