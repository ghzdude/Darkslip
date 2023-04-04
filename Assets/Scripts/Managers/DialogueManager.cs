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
    public AudioClip dialogueText;
    [Tooltip("Reveal Speed / 100")]
    public int textRevealSpeed;
    private TypeWriterFX typer;
    private bool dialogueActive;
    private Dialogue currentDialogue;
    private float timer;
    private AudioController audioController;

    // Start is called before the first frame update
    void Awake()
    {
        typer = gameObject.AddComponent<TypeWriterFX>();
        typer.revealSpeed = textRevealSpeed;
        typer.typeSound = dialogueText;
        timer = 0;
        audioController = Managers.GetMusic().GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update() {
        if (dialogueActive && typer.completed) {
            if (Input.GetKeyUp(KeyCode.Z) || timer <= 0f) {
                StartDialogue(currentDialogue);
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

    public void StartDialogue(Dialogue dialogue) {
        ClearDialogue();

        currentDialogue = dialogue;
        if (currentDialogue == null) {
            DisableDialogueBox();
            return;
        }

        if (currentDialogue.infoBox)
            EnableDialogueBox(currentDialogue.text);
        else
            EnableDialogueBox(currentDialogue.text, currentDialogue.character);

        currentDialogue = dialogue.nextDialogue;
    }

    private void InitializeDialogue(Text text) {
        if (text == null) {
            Debug.Log("Text for Dialogue is null!");
            return;
        }

        audioController = Managers.GetMusic().GetComponent<AudioController>();
        audioController.PlayClip(dialogueOpen);
        Time.timeScale = 0;
        typer.Run(text);
        timer = 10;
        dialogueActive = true;
    }

    private void EnableDialogueBox(string text, Enums.Character character) {
        DialogueText.text = text;
        InitializeDialogue(DialogueText);
        SetSprite(character);
        DialoguePanel.gameObject.SetActive(true);
    }

    private void EnableDialogueBox(string text) {
        InfoText.text = text;
        InitializeDialogue(InfoText);
        InfoPanel.gameObject.SetActive(true);
    }

    public void DisableDialogueBox() {
        typer.Stop();
        dialogueActive = false;
        timer = -1f;

        audioController.PlayClip(dialogueClose);

        ClearDialogue();

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
