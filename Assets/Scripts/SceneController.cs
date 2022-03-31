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
    private List<CreateDialogue> Dialogue;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float sfxVolume;
    // public List<Scene> Levels;

    private void Awake()
    {
        // Deduplicate("Player");
        // Deduplicate("MainCamera");
        // Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        // Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Required Signature (Scene scene, LoadSceneMode mode)
    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetSceneParams;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetSceneParams;
    }

    private void ResetSceneParams(Scene scene, LoadSceneMode mode)
    {
        // print(string.Format("Active Scene was: {0}", SceneManager.GetActiveScene().name));
        if (scene.buildIndex > 0)
            SceneManager.SetActiveScene(scene);

        if (scene.buildIndex == 0)
            SceneManager.LoadScene(1, LoadSceneMode.Additive);

        // print(string.Format("Current Active Scene is: {0}", SceneManager.GetActiveScene().name));

        // Search Active Scene
        GameObject[] Objects = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < Objects.Length; i++)
        {
            GameObject obj = Objects[i];
            switch (obj.tag)
            {
                case "Player":
                    Player = obj.transform;
                    Music = Player.GetChild(2).GetComponent<AudioSource>();
                    break;
                case "Dialogue":
                    Dialogue = new List<CreateDialogue>();
                    Dialogue.AddRange(obj.GetComponentsInChildren<CreateDialogue>());
                    break;
                default:
                    break;
            }
        }
        // Search Managers Scene
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        InventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();

        if (scene.buildIndex > 0 && scene.name != "credits") // Once level1... is loaded
        {
            DialogueManager.UpdateCanvas(Canvas, Player, Dialogue);
            Player.GetComponent<PlayerController>().SceneController = this;
            InventoryManager.InventoryPanel = Canvas.GetChild(3).GetComponent<RectTransform>();
            musicVolume = DialogueManager.GetSlider("musicVolume").value;
            sfxVolume = DialogueManager.GetSlider("sfxVolume").value;
            Deduplicate("Canvas");
            Deduplicate("DialogueManager");
            Deduplicate("SceneController");
        }

        if (scene.buildIndex == 1) // First Level only
        {
            DialogueManager.InitializeCanvas();
            InventoryManager.ClearInventory();
        }

        /*
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        infoPanel = canvas.Find("Info Panel").gameObject;
        dialoguePanel = canvas.Find("Dialogue Panel").gameObject;
        pauseMenu = canvas.Find("Pause Menu").gameObject;
        dialogueText = dialoguePanel.GetComponentInChildren<Text>();
        infoText = infoPanel.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>();
        typer = gameObject.GetComponent<TypeWriterFX>();

        Inventory = canvas.Find("Inventory Panel").GetComponent<InventoryManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        // infoText = infoPanel.GetComponentInChildren<Text>();
        // dialogueText = dialoguePanel.GetComponentInChildren<Text>();

        */
    }

    private void Start()
    {

        /*
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        infoPanel = canvas.Find("Info Panel").gameObject;
        dialoguePanel = canvas.Find("Dialogue Panel").gameObject;
        pauseMenu = canvas.Find("Pause Menu").gameObject;
        dialogueText = dialoguePanel.GetComponentInChildren<Text>();
        infoText = infoPanel.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>();
        typer = gameObject.GetComponent<TypeWriterFX>();

        Inventory = canvas.Find("Inventory Panel").GetComponent<InventoryManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        // infoText = infoPanel.GetComponentInChildren<Text>();
        // dialogueText = dialoguePanel.GetComponentInChildren<Text>();

        dialoguePanel.SetActive(false);
        infoPanel.SetActive(false);
        pauseMenu.SetActive(false);
        Inventory.gameObject.SetActive(false);
        */
    }

    public void ResetScene()
    {
        InventoryManager.ClearInventory();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadScene(currentScene.name, LoadSceneMode.Additive);
        Time.timeScale = 1;
        // print(string.Format("scene: {0} has been reset", currentScene.name));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCredit()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
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
