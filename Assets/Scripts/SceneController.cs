using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private RectTransform Canvas;
    private DialogueManager DialogueManager;
    private Transform Player;
    private InventoryManager InventoryManager;
    private MusicManager MusicManager;
    private Vector3 plrOldPosition;
    private AudioSource Music;
    private AudioSource SFX;
    private bool initialized;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float sfxVolume;

    private void Update()
    {
        if (Music != null)
            Music.volume = DialogueManager.GetMusicSliderValue();
        if(SFX != null)
            SFX.volume = DialogueManager.GetSFXSliderValue();
    }
    private void Awake()
    {
        initialized = false;
    }

    // Required Signature (Scene scene, LoadSceneMode mode)
    private void OnEnable() {
        SceneManager.sceneLoaded += ResetSceneParams;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= ResetSceneParams;
    }

    private void ResetSceneParams(Scene scene, LoadSceneMode mode) {
        bool hasAudioListener = false;

        // Search Any Scene After Manager
        if (scene.buildIndex > 0) {

            if (Player != null && initialized) {
                Player.position = plrOldPosition;
            }
            
            // First Time Initializing
            if (!initialized) {
                DialogueManager.InitializeCanvas();
                DialogueManager.InitializeGameMenu();
                DialogueManager.InitializeHealthObjects();
                initialized = true;
            }

            GameObject[] Objects = scene.GetRootGameObjects();
            for (int i = 0; i < Objects.Length; i++) {
                GameObject obj = Objects[i];
                switch (obj.tag) {
                    case "Player":
                        Player = obj.transform;
                        SFX = Player.GetComponent<AudioSource>();
                        break;
                    default:
                        break;
                }
                
                if (obj.GetComponent<AudioListener>() != null) {
                    hasAudioListener = true;
                }
            }

            if (hasAudioListener) {
                Destroy(gameObject.GetComponent<AudioListener>());
            } else if (gameObject.GetComponent<AudioListener>() == null) {
                gameObject.AddComponent<AudioListener>();
            }
        }
        SceneManager.SetActiveScene(scene);

        // Search Any Loaded Scene
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        InventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        Music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        MusicManager = Music.GetComponent<MusicManager>();

        DialogueManager.SetCanvasObjects(Canvas);

        if (scene.buildIndex < SceneManager.sceneCountInBuildSettings - 1) {
            if (Player != null) {
                DialogueManager.SetPlayer(Player);
            }
            InventoryManager.InventoryPanel = DialogueManager.GetInventoryPanel();
        } else if (scene.buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
            MusicManager.SetMusic(Enums.Music.Credits);
            DialogueManager.InitializeCredits();
            Managers.GetCamera().position = Vector3.zero;
        }

        // Main Menu Only
        if (scene.buildIndex == 0) {
            DialogueManager.InitializeMainMenu();
            MusicManager.SetMusic(Enums.Music.MainMenu);
        }

        // First Level only
        if (scene.buildIndex == 1) { 
            InventoryManager.ClearInventory();
            MusicManager.SetMusic(Enums.Music.DockingBay3);
        }
    }

    public void ResetScene() {
        // InventoryManager.ClearInventory();
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex > 0) {
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Additive);
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

    private void Deduplicate(string name) {
        GameObject[] obj = GameObject.FindGameObjectsWithTag(name);

        for (int i = 1; i < obj.Length; i++) {
            Destroy(obj[i]);
        }
    }

    public InventoryManager GetInventoryManager() => InventoryManager;

    public DialogueManager GetDialogueManager() => DialogueManager;

    public void SetOffset(Vector3 offset) {
        this.plrOldPosition = offset;
    }
}
