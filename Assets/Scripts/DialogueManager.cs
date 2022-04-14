using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [TextArea(10, 10)]
    public string text;
    private TypeWriterFX typer;
    public AudioClip dialogueOpen;
    public AudioClip dialogueClose;
    public Sprite seanSprite;
    public Sprite doctorSprite;
    public GameObject fullbright;
    public Sprite[] HeartIcons; // 0 is heartFull, 1 is heartHalf, 2 is heartEmpty
    public GameObject Heart;
    private List<RectTransform> Hearts;
    public float horizontalOffset;
    public int maxHearts;
    private Image Holder;
    private Transform Player;
    private Transform PauseMenu;
    private Transform InventoryPanel;
    private Transform DialoguePanel;
    private Transform InfoPanel;
    private Transform DebugPanel;
    private Transform HealthContainer;
    private Slider sdr_music;
    private Slider sdr_sfx;
    private Text DialogueText;
    private Text InfoText;
    private AudioController Audio;
    private float timer = 0;
    private List<CreateDialogue> Dialogues;

    private void Awake()
    {
        typer = GetComponent<TypeWriterFX>();
    }

    public void SetCanvasObjects (RectTransform canvas)
    {
        PauseMenu = canvas.GetChild(4);
        InventoryPanel = canvas.GetChild(3);
        DialoguePanel = canvas.GetChild(1);
        InfoPanel = canvas.GetChild(0);
        HealthContainer = canvas.GetChild(5);
        DebugPanel = PauseMenu.GetChild(0);
        DialogueText = DialoguePanel.GetChild(0).GetComponent<Text>();
        InfoText = InfoPanel.GetChild(0).GetComponent<Text>();
        Holder = DialoguePanel.GetChild(1).GetChild(0).GetComponent<Image>();
        Holder.sprite = null;
        sdr_music = PauseMenu.GetChild(5).GetComponent<Slider>();
        sdr_sfx = PauseMenu.GetChild(6).GetComponent<Slider>();

        if (HealthContainer.childCount == 0)
        {
            Hearts = new List<RectTransform>();
            for (int i = 0; i < maxHearts; i++)
            {
                Hearts.Add(Instantiate(Heart, HealthContainer).GetComponent<RectTransform>());
                Hearts[i].anchoredPosition = new Vector2(Hearts[i].anchoredPosition.x + (horizontalOffset * i), Hearts[i].anchoredPosition.y);
            }
            // Debug.Log("Hearts generated");
        }

    }

    public void SetPlayer(Transform player)
    {
        Player = player;
        Audio = Player.GetComponent<AudioController>();
    }

    public void SetDialogue(List<CreateDialogue> dialogues)
    {
        Dialogues = dialogues;

        if (Dialogues != null)
        {
            foreach (var item in Dialogues)
            {
                item.DialogueManager = this;
            }
        }
    }

    public void InitializeCanvas()
    {
        DialoguePanel.gameObject.SetActive(false);
        InfoPanel.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        InventoryPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeInHierarchy);
            InventoryPanel.gameObject.SetActive(!InventoryPanel.gameObject.activeInHierarchy);
        }

        if (InfoPanel.gameObject.activeInHierarchy || DialoguePanel.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Z) && typer.completed)
            {
                DisableDialogueBox();
            }

            if (timer <= 0)
            {
                timer = -1f;
                DisableDialogueBox();
            }
            else
            {
                timer -= 1 * Time.unscaledDeltaTime;
            }
        }

        UpdateHealthGUI(Player.GetComponent<PlayerController>());
        
    }

    public void SetSprite(Enums.character character)
    {
        switch (character)
        {
            case Enums.character.Sean:
                Holder.sprite = seanSprite;
                break;
            case Enums.character.Doctor:
                Holder.sprite = doctorSprite;
                break;
            default:
                Debug.Log("Error reading character in GetSprite()");
                break;
        }
    }

    public void EnableDialogueBox(string text, Enums.character character)
    {
        Audio.PlayClip(dialogueOpen, 0.66f);
        // Debug.Log("dialogue opened");
        Time.timeScale = 0;
        SetSprite(character);
        DialoguePanel.gameObject.SetActive(true);
        DialogueText.text = text;
        typer.TypeWriter(DialogueText);
        timer = 10f;
    }

    public void EnableDialogueBox(string text)
    {
        Audio.PlayClip(dialogueOpen, 0.66f);
        Time.timeScale = 0;
        InfoPanel.gameObject.SetActive(true);
        InfoText.text = text;
        typer.TypeWriter(InfoText);
        timer = 10f;
        // Debug.Log("info box opened");
    }

    public void DisableDialogueBox()
    {
        typer.Stop();
        Audio.PlayClip(dialogueClose, 0.66f);
        if (InfoPanel.gameObject.activeInHierarchy && InfoPanel != null && Audio != null)
        {
            // Debug.Log("dialogue closed");
            InfoPanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        if (DialoguePanel.gameObject.activeInHierarchy && DialoguePanel != null && Audio != null)
        {
            DialoguePanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void UpdateHealthGUI(PlayerController Player)
    {
        int health = Player.GetHealth();
        // DebugPanel.GetComponentInChildren<Text>().text = string.Format("Health: {0}", health);
        int index = Mathf.CeilToInt((float)health / 2) - 1;

        // health us 8, 7; index is 3; even should have 
        // health is 6, 5; index is 2; even should have 3 full hearts, 0 empty; odd should have 2 full hearts, 0 empty
        // health is 4, 3; index is 1; even should have 2 full hearts, 1 empty; odd should have 1 full heart, 1 empty
        // health is 2, 1; index is 0; even should have 1 full heart, 2 empty; odd should have 0 full hearts, 2 empty
        // health is 0; index is -1?; should have 3 empty
        // even full hearts index + 1; even empty is 2 - index
        for (int i = 0; i < Hearts.Count; i++)
        {
            if (health % 2 == 1 && health > 0) // odd > 0
            {
                if (i < index)
                {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[0]; // set full
                }
                if (i == index)
                {
                    Hearts[index].GetComponent<Image>().sprite = HeartIcons[1]; // set half heart
                }
                if (index < i)
                {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[2]; // set empty
                }
            }
            if (health % 2 == 0 && health > 0) // even > 0
            {
                if (i <= index)
                {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[0]; // set full
                }
                if (index < i)
                {
                    Hearts[i].GetComponent<Image>().sprite = HeartIcons[2]; // set empty
                }
            }
        }

        if (health <= 0) // if health is less than zero
        {
            for (int i = 0; i < Hearts.Count; i++)
            {
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
}
