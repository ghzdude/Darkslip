using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.WSA;

public class DialogueManager : MonoBehaviour
{
    public RectTransform InfoPanel;
    public RectTransform DialoguePanel;
    [SerializeField] private Image Portrait;
    [SerializeField] private Text InfoText;
    [SerializeField] private Text DialogueText;
    [SerializeField] private Sprite SeanSprite;
    [SerializeField] private Sprite DoctorSprite;
    public AudioClip dialogueOpen;
    public AudioClip dialogueClose;
    private TypeWriterFX typer;
    private bool dialogueActive;
    private CreateDialogue nextDialogue;
    private float timer;
    private AudioController audioController;

    // Start is called before the first frame update
    void Start()
    {
        typer = gameObject.AddComponent<TypeWriterFX>();
        timer = 0;
        audioController = Managers.GetMusic().GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && typer.completed && Managers.GetPlayerController() != null) {
            if (Input.GetKeyUp(KeyCode.Z)) {
                EnableNextDialogue();
            }

            if (timer <= 0f) {
                EnableNextDialogue();
            }

            timer -= Time.unscaledDeltaTime;
        }
    }
    public void SetSprite(Enums.Character character) {
        Animator anim = Portrait.GetComponent<Animator>();
        anim.runtimeAnimatorController = null;
        anim.enabled = false;

        switch (character) {
            case Enums.Character.Sean:
                Portrait.sprite = SeanSprite;
                break;
            case Enums.Character.Doctor:
                Portrait.sprite = DoctorSprite;
                break;
            case Enums.Character.Static:
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(Paths.StaticAnimator);
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
        audioController = Managers.GetMusic().GetComponent<AudioController>();
        audioController.PlayClip(dialogueOpen);
        Time.timeScale = 0;
        typer.Run(text);
        timer = 10;
        dialogueActive = true;
    }

    public void EnableDialogueBox(string text, Enums.Character character) {
        if (!dialogueActive && !DialoguePanel.gameObject.activeInHierarchy) {
            DialogueText.text = text;
            InitializeDialogue(DialogueText);
            SetSprite(character);
            DialoguePanel.gameObject.SetActive(true);

        }

        if (nextDialogue != null) {
            SetNextDialogue(nextDialogue);
        }
    }

    public void EnableDialogueBox(string text) {
        if (!dialogueActive && !InfoPanel.gameObject.activeInHierarchy) {
            InfoText.text = text;
            InitializeDialogue(InfoText);
            InfoPanel.gameObject.SetActive(true);
        }

        if (nextDialogue != null) {
            SetNextDialogue(nextDialogue);
        }

    }

    public void DisableDialogueBox() {
        typer.Stop();
        dialogueActive = false;
        timer = -1f;

        audioController.PlayClip(dialogueClose);

        if (InfoPanel.gameObject.activeInHierarchy)
            InfoPanel.gameObject.SetActive(false);

        if (DialoguePanel.gameObject.activeInHierarchy)
            DialoguePanel.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    public bool IsActive() {
        return dialogueActive;
    }

    private void ClearDialogue() {
        DialogueText.text = "";
        InfoText.text = "";
        DialoguePanel.gameObject.SetActive(false);
        InfoPanel.gameObject.SetActive(false);
    }

}
