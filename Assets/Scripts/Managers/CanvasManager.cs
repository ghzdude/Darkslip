using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class CanvasManager : MonoBehaviour
{
    // todo deal wuth all of these goddamn variables
    // public GameObject fullbright;
    public Sprite[] HeartIcons; // 0 is heartFull, 1 is heartHalf, 2 is heartEmpty
    public GameObject HeartPrefab;
    public float horizontalOffset;
    private List<RectTransform> Hearts;
    private int maxHearts;
    private Transform GameMenu;
    private Transform MainMenu;
    private Transform PauseMenu;
    private Transform InventoryPanel;
    private Transform DebugPanel;
    private Transform HealthContainer;
    private Transform TramSelector;
    [Header("Canvas Objects")]
    public DialogueManager dialogueManager;
    public GameObject Credits;
    public Keypad keypad;
    private RectTransform canvas;
    private Button btn_lobby;
    private Button btn_office;
    private Slider sdr_music;
    private Slider sdr_sfx;

    public void SetCanvasObjects ()
    {
        // GetChild() central wtf
        canvas = GetComponent<RectTransform>();

        GameMenu = canvas.GetChild(1);
        MainMenu = canvas.GetChild(0);

        InventoryPanel = GameMenu.GetChild(3);
        PauseMenu = GameMenu.GetChild(4);
        HealthContainer = GameMenu.GetChild(5);
        TramSelector = GameMenu.GetChild(7);
        DebugPanel = GameMenu.GetChild(8);

        sdr_music = PauseMenu.GetChild(2).GetComponent<Slider>();
        sdr_sfx = PauseMenu.GetChild(3).GetComponent<Slider>();


        btn_lobby = TramSelector.GetChild(2).GetChild(0).GetComponent<Button>();
        btn_office = TramSelector.GetChild(2).GetChild(1).GetComponent<Button>();
    }

    public void InitializeHealthObjects() {
        maxHearts = Managers.GetPlayerController().maxHealth / 2;

        // todo replace with health bar instead of hearts
        if (HealthContainer.childCount == 0) {
            Hearts = new List<RectTransform>();
            for (int i = 0; i<maxHearts; i++) {
                Hearts.Add(Instantiate(HeartPrefab, HealthContainer).GetComponent<RectTransform>());
                Hearts[i].anchoredPosition = new Vector2(Hearts[i].anchoredPosition.x + (horizontalOffset* i), Hearts[i].anchoredPosition.y);
}
            // Debug.Log("Hearts generated");
        }
    }

    public void InitializeCanvas() {
        SetCanvasObjects();
        // dialogueManager.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        InventoryPanel.gameObject.SetActive(false);
        keypad.gameObject.SetActive(false);
        TramSelector.gameObject.SetActive(false);
        DebugPanel.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void InitializeMainMenu() {
        MainMenu.gameObject.SetActive(true);
        GameMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void InitializeGameMenu() {
        GameMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void InitializeCredits() {
        GameMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(true);
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeInHierarchy);
            InventoryPanel.gameObject.SetActive(!InventoryPanel.gameObject.activeInHierarchy);
        }
        /*
        if (DialoguePanel.gameObject.activeInHierarchy || InfoPanel.gameObject.activeInHierarchy) {
            dialogueActive = true;
        }*/

        // LShift + Q + E for debug
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
            DebugPanel.gameObject.SetActive(!DebugPanel.gameObject.activeInHierarchy);
        }

    }

    
    public void UpdateHealthGUI(PlayerController Player) {
        int health = Player.GetHealth();
        // DebugPanel.GetComponentInChildren<Text>().text = string.Format("Health: {0}", health);
        int index = Mathf.CeilToInt((float)health / 2) - 1;

        // health us 8, 7; index is 3; even should have 
        // health is 6, 5; index is 2; even should have 3 full hearts, 0 empty; odd should have 2 full hearts, 0 empty
        // health is 4, 3; index is 1; even should have 2 full hearts, 1 empty; odd should have 1 full heart, 1 empty
        // health is 2, 1; index is 0; even should have 1 full heart, 2 empty; odd should have 0 full hearts, 2 empty
        // health is 0; index is -1?; should have 3 empty
        // even full hearts index + 1; even empty is 2 - index
        for (int i = 0; i < Hearts.Count; i++) {
            if (health % 2 == 1 && health > 0) { // odd > 0 
                if (i < index) {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[0]; // set full
                }
                if (i == index) {
                    Hearts[index].GetComponent<Image>().sprite = HeartIcons[1]; // set half heart
                }
                if (index < i) {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[2]; // set empty
                }
            }
            if (health % 2 == 0 && health > 0) { // even > 0
                if (i <= index) {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[0]; // set full
                }
                if (index < i) {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[2]; // set empty
                }
            }
        }

        if (health <= 0) { // if health is less than zero
            for (int i = 0; i < Hearts.Count; i++) {
                Hearts[i].GetComponent<Image>().sprite = HeartIcons[2]; // set empty
            }
        }
        // Debug.Log(string.Format("health: {0} equates to index: {1}", health, index));
        
    }

    public void TestHealth(bool shouldDamage) {
            PlayerController Player = Managers.GetPlayerController();

        if (shouldDamage)
            Player.DecHealth();
        else
            Player.IncHealth();
    }

    public int GetMaxHearts() => maxHearts;

    public float GetMusicSliderValue() => sdr_music.normalizedValue;
    
    public float GetSFXSliderValue() => sdr_sfx.normalizedValue;

    public void InitializeTramButtons() {
        Tram tram = GameObject.FindGameObjectWithTag("Tram").GetComponent<Tram>();

        foreach (var dest in tram.Destinations) {
            if (dest.name.ToLower() == "lobby") {
                btn_lobby.onClick.AddListener(() => tram.Move(dest));
            }
            if (dest.name.ToLower() == "office") {
                btn_office.onClick.AddListener(() => tram.Move(dest));
            }
        }
    }

    public void OpenTramSelector() {
        InitializeTramButtons();
        TramSelector.gameObject.SetActive(true);
        btn_lobby.onClick.AddListener(CloseTramSelector);
        btn_office.onClick.AddListener(CloseTramSelector);
    }

    public void CloseTramSelector() {
        TramSelector.gameObject.SetActive(false);
        btn_lobby.onClick.RemoveAllListeners();
        btn_office.onClick.RemoveAllListeners();
    }
}
