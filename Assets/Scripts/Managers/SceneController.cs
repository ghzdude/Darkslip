using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;
using System.IO;

public class SceneController : MonoBehaviour
{
    private Transform SpawnPoint;
    private CanvasManager CanvasManager;
    private GameObject Player;
    private InventoryManager InventoryManager;
    private MusicManager MusicManager;
    private Vector3 plrOldPosition;
    private bool initialized;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float sfxVolume;

    private void Awake()
    {
        CanvasManager = Managers.GetCanvasManager();
        InventoryManager = Managers.GetInventoryManager();
        MusicManager = Managers.GetMusic();
        Player = Resources.Load<GameObject>(Paths.Player);
        initialized = false;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += ResetSceneParams;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= ResetSceneParams;
    }

    // Required Signature (Scene scene, LoadSceneMode mode)
    private void ResetSceneParams(Scene scene, LoadSceneMode mode) {

        // Search Any Scene After Manager
        if (scene.buildIndex > 0) {

            if (Player != null && initialized) {
                Player.transform.position = plrOldPosition;
            }
            
            // First Time Initializing
            if (!initialized) {

                CanvasManager.InitializeCanvas();
                CanvasManager.InitializeGameMenu();
                CanvasManager.InitializeHealthObjects();
                Destroy(gameObject.GetComponent<AudioListener>());
                
                initialized = true;
            }
            
            Debug.Log(string.Format("spawned player: {0}", Player));

        }
        SceneManager.SetActiveScene(scene);

        // Search Any Loaded Scene

        if (scene.buildIndex < SceneManager.sceneCountInBuildSettings - 1) {
            // InventoryManager.InventoryPanel = DialogueManager.GetInventoryPanel();
            SpawnPoint = Managers.GetSpawnPoint();
        } else if (scene.name == Scenes.Credits) {
            MusicManager.SetMusic(Enums.Music.Credits);
            CanvasManager.InitializeCredits();
            Managers.GetCamera().position = Vector3.zero;
        }

        // Main Menu Only
        if (scene.buildIndex == 0) {
            CanvasManager.InitializeMainMenu();
            MusicManager.SetMusic(Enums.Music.MainMenu);
            return;
        }

        // First Level only
        if (scene.buildIndex == 1) { 
            InventoryManager.ClearInventory();
            MusicManager.SetMusic(Enums.Music.DockingBay3);
            SpawnPlayer();
            return;
        }
    }

    public void ResetScene() {
        // InventoryManager.ClearInventory();
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex > 0) {
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Additive);
            SpawnPlayer();

        }
        Time.timeScale = 1;
        // print(string.Format("scene: {0} has been reset", currentScene.name));
    }

    public void StartGame() {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadCredit() {
        if (SceneManager.GetActiveScene().buildIndex > 0) {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
        SceneManager.LoadScene("credits", LoadSceneMode.Additive);
    }

    public void SetOffset(Vector3 offset) {
        this.plrOldPosition = offset;
    }

    public void SpawnPlayer() {
        Player = Instantiate(Player, SpawnPoint.position, Quaternion.identity);
    }
}
