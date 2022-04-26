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
    private AudioSource Music;
    private AudioSource SFX;
    private bool initialized;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float sfxVolume;

    private void Update()
    {
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

        // Search Any Scene After Manager
        if (scene.buildIndex > 0) {

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

                if (obj.GetComponent<AudioListener>() != null &&
                    gameObject.GetComponent<AudioListener>() != null &&
                    !gameObject.Equals(obj)) {
                    Destroy(gameObject.GetComponent<AudioListener>());
                }
            }
            SceneManager.SetActiveScene(scene);
        }

        // Search Any Loaded Scene
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        InventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        Music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        DialogueManager.SetCanvasObjects(Canvas);

        if (scene.name != "credits") {
            if (Player != null) {
                DialogueManager.SetPlayer(Player);
            }
            InventoryManager.InventoryPanel = DialogueManager.GetInventoryPanel();
        }

        // Main Menu Only
        if (scene.buildIndex == 0) {
            DialogueManager.InitializeMainMenu();
        }

        // First Level only
        if (scene.buildIndex == 1) { 
            InventoryManager.ClearInventory();
        }
    }

    public void ResetScene()
    {
        InventoryManager.ClearInventory();
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex > 0)
        {
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Additive);
            initialized = false;
        }
        Time.timeScale = 1;
        // print(string.Format("scene: {0} has been reset", currentScene.name));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCredit()
    {
        SceneManager.LoadScene("credits");
    }

    private void Deduplicate(string name)
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag(name);

        for (int i = 1; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }
    }

    public InventoryManager GetInventoryManager()
    {
        return InventoryManager;
    }

    public DialogueManager GetDialogueManager()
    {
        return DialogueManager;
    }
}
