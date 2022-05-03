using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    private string text;
    private TypeWriterFX typer;
    [Header("Dialogue Sounds")]
    public AudioClip dialogueOpen;
    public AudioClip dialogueClose;
    [Header("Character")]
    public Sprite seanSprite;
    public Sprite doctorSprite;
    public RuntimeAnimatorController staticSprite;
    public GameObject fullbright;
    public Sprite[] HeartIcons; // 0 is heartFull, 1 is heartHalf, 2 is heartEmpty
    public GameObject Heart;
    private List<RectTransform> Hearts;
    public float horizontalOffset;
    public int maxHearts;
    public bool dialogueActive;
    private CreateDialogue nextDialogue;
    private Image Holder;
    private Transform Player;
    private Transform GameMenu;
    private Transform MainMenu;
    private Transform PauseMenu;
    private Transform InventoryPanel;
    private Transform DialoguePanel;
    private Transform InfoPanel;
    private Transform DebugPanel;
    private Transform HealthContainer;
    private Transform CodeEntry;
    private Transform TramSelector;
    [Header("Canvas Objects")]
    public Transform Credits;
    private Text CodeEntryField;
    private Button[] KeyPadButtons;
    private Button KeyCancel;
    private Button KeyEnter;
    private Button KeyClose;
    private Button btn_lobby;
    private Button btn_office;
    private Slider sdr_music;
    private Slider sdr_sfx;
    private Text DialogueText;
    private Text InfoText;
    private AudioSource src;
    private float timer = 0;
    private Terminal terminal;

    private void Awake()
    {
        typer = GetComponent<TypeWriterFX>();
    }

    public void SetCanvasObjects (RectTransform canvas)
    {
        GameMenu = canvas.GetChild(1);
        MainMenu = canvas.GetChild(0);

        InfoPanel = GameMenu.GetChild(0);
        DialoguePanel = GameMenu.GetChild(1);
        InventoryPanel = GameMenu.GetChild(3);
        PauseMenu = GameMenu.GetChild(4);
        HealthContainer = GameMenu.GetChild(5);
        CodeEntry = GameMenu.GetChild(6);
        TramSelector = GameMenu.GetChild(7);
        DebugPanel = GameMenu.GetChild(8);

        
        DialogueText = DialoguePanel.GetChild(0).GetComponent<Text>();
        InfoText = InfoPanel.GetChild(0).GetComponent<Text>();
        Holder = DialoguePanel.GetChild(1).GetChild(0).GetComponent<Image>();
        Holder.sprite = null;
        sdr_music = PauseMenu.GetChild(2).GetComponent<Slider>();
        sdr_sfx = PauseMenu.GetChild(3).GetComponent<Slider>();
        KeyPadButtons = CodeEntry.GetChild(0).GetChild(2).GetComponentsInChildren<Button>();
        KeyCancel = CodeEntry.GetChild(0).GetChild(3).GetComponent<Button>();
        KeyEnter = CodeEntry.GetChild(0).GetChild(4).GetComponent<Button>();
        KeyClose = CodeEntry.GetChild(0).GetChild(5).GetComponent<Button>();
        CodeEntryField = CodeEntry.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        btn_lobby = TramSelector.GetChild(2).GetChild(0).GetComponent<Button>();
        btn_office = TramSelector.GetChild(2).GetChild(1).GetComponent<Button>();

        if (fullbright != null && fullbright.activeInHierarchy)
        {
            fullbright.SetActive(false);
        }
    }

    public void InitializeHealthObjects() {
        if (HealthContainer.childCount == 0) {
            Hearts = new List<RectTransform>();
            for (int i = 0; i<maxHearts; i++) {
                Hearts.Add(Instantiate(Heart, HealthContainer).GetComponent<RectTransform>());
                Hearts[i].anchoredPosition = new Vector2(Hearts[i].anchoredPosition.x + (horizontalOffset* i), Hearts[i].anchoredPosition.y);
}
            // Debug.Log("Hearts generated");
        }
    }

    public void SetPlayer(Transform player)
    {
        Player = player;
        src = Player.GetComponent<AudioSource>();
    }

    public void InitializeCanvas()
    {
        DialoguePanel.gameObject.SetActive(false);
        InfoPanel.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        InventoryPanel.gameObject.SetActive(false);
        CodeEntry.gameObject.SetActive(false);
        TramSelector.gameObject.SetActive(false);
        DebugPanel.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void InitializeMainMenu()
    {
        MainMenu.gameObject.SetActive(true);
        GameMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void InitializeGameMenu()
    {
        GameMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void InitializeCredits() {
        GameMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(true);
    }

    public RectTransform GetInventoryPanel() => InventoryPanel.GetComponent<RectTransform>();


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeInHierarchy);
            InventoryPanel.gameObject.SetActive(!InventoryPanel.gameObject.activeInHierarchy);
        }

        if (DialoguePanel.gameObject.activeInHierarchy || InfoPanel.gameObject.activeInHierarchy) {
            dialogueActive = true;
        }

        // LShift + Q + E for debug
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
            DebugPanel.gameObject.SetActive(!DebugPanel.gameObject.activeInHierarchy);
        }
        if (dialogueActive && typer.completed && Player != null) {
            if (Input.GetKeyUp(KeyCode.Z)) {
                EnableNextDialogue();
            }
            
            if (timer <= 0f) {
                EnableNextDialogue();
            } else {
                timer -= 1 * Time.unscaledDeltaTime;
            }
        }
    }

    public void SetSprite(Enums.Character character) {
        Animator anim = Holder.GetComponent<Animator>();
        anim.runtimeAnimatorController = null;
        anim.enabled = false;

        switch (character) {
            case Enums.Character.Sean:
                Holder.sprite = seanSprite;
                break;
            case Enums.Character.Doctor:
                Holder.sprite = doctorSprite;
                break;
            case Enums.Character.Static:
                anim.runtimeAnimatorController = staticSprite;
                anim.enabled = true;
                break;
            default:
                Debug.Log("Error reading character in GetSprite()");
                break;
        }
    }

    public void SetNextDialogue(CreateDialogue nextDialogue) => this.nextDialogue = nextDialogue;

    public void EnableNextDialogue() {
        ClearDialogue();

        if (nextDialogue != null && nextDialogue.infoBox) {
            EnableDialogueBox(nextDialogue.text);
            if (nextDialogue.nextDialogue != null) {
                SetNextDialogue(nextDialogue.nextDialogue);
            } else {
                nextDialogue = null;
            }
        } else if (nextDialogue != null) {
            EnableDialogueBox(nextDialogue.text, nextDialogue.character);
            if (nextDialogue.nextDialogue != null) {
                SetNextDialogue(nextDialogue.nextDialogue);
            } else {
                nextDialogue = null;
            }
        } else {
            DisableDialogueBox();
        }
    }

    private void InitializeDialogue(Text text) {
        src.PlayOneShot(dialogueOpen, GetSFXSliderValue());
        Time.timeScale = 0;
        typer.TypeWriter(text, Player.GetComponent<AudioSource>());
        timer = 10f;
        dialogueActive = true;
    }

    public void EnableDialogueBox(string text, Enums.Character character) {
        if (!DialoguePanel.gameObject.activeInHierarchy) {
            DialogueText.text = text;
            InitializeDialogue(DialogueText);
            SetSprite(character);
            DialoguePanel.gameObject.SetActive(true);
            
        }
    }

    public void EnableDialogueBox(string text) {
        if (!InfoPanel.gameObject.activeInHierarchy) {
            InfoText.text = text;
            InitializeDialogue(InfoText);
            InfoPanel.gameObject.SetActive(true);
        }
    }

    public void DisableDialogueBox() {
        typer.Stop();
        dialogueActive = false;
        timer = -1f;

        if (src != null)
            src.PlayOneShot(dialogueClose, GetSFXSliderValue());

        if (InfoPanel.gameObject.activeInHierarchy)
            InfoPanel.gameObject.SetActive(false);

        if (DialoguePanel.gameObject.activeInHierarchy)
            DialoguePanel.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    private void ClearDialogue() {
        DialogueText.text = "";
        InfoText.text = "";
        DialoguePanel.gameObject.SetActive(false);
        InfoPanel.gameObject.SetActive(false);
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

    public void TestHealth(bool shouldDamage)
    {
        if (shouldDamage)
            Player.GetComponent<PlayerController>().DecHealth();
        else
            Player.GetComponent<PlayerController>().IncHealth();
    }

    public int GetMaxHearts()
    {
        return maxHearts;
    }

    public float GetMusicSliderValue()
    {
        return sdr_music.normalizedValue;
    }

    public float GetSFXSliderValue()
    {
        return sdr_sfx.normalizedValue;
    }

    public void ToggleFullbright()
    {
        fullbright.SetActive(!fullbright.activeInHierarchy);
    }

    public void OpenCodeEntry(Terminal sendingTerminal)
    {
        terminal = sendingTerminal;
        ResetField();
        CodeEntry.gameObject.SetActive(true);
        for (int i = 0; i < KeyPadButtons.Length; i++)
        {
            string num = KeyPadButtons[i].transform.GetChild(0).GetComponent<Text>().text; 
            KeyPadButtons[i].onClick.AddListener(() => EnterChar(num));
        }
        KeyCancel.onClick.AddListener(ResetField);
        KeyEnter.onClick.AddListener(() => CheckInput(CodeEntryField.text));
        KeyClose.onClick.AddListener(CloseCodeEntry);
        Time.timeScale = 0;
    }

    public void CloseCodeEntry()
    {
        CodeEntry.gameObject.SetActive(false);
        for (int i = 0; i < KeyPadButtons.Length; i++)
        {
            KeyPadButtons[i].onClick.RemoveAllListeners();
        }
        KeyCancel.onClick.RemoveAllListeners();
        KeyEnter.onClick.RemoveAllListeners();
        KeyClose.onClick.RemoveAllListeners();
        Time.timeScale = 1;
    }

    private void EnterChar(string s) {
        if (CodeEntryField.text.Length < 4) {
            CodeEntryField.text += s;
        }
    }

    public void ResetField() {
        CodeEntryField.text = "";
    }

    public void CheckInput(string userInput) {
        if (terminal.correctCode.Equals(userInput)) {
            terminal.Fire(true);
            CloseCodeEntry();
            terminal.Disable();
            terminal = null;

        } else {
            ResetField();
        }
    }

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
