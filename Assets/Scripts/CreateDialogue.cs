using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDialogue : MonoBehaviour
{
    [TextArea(10, 10)] public string text;
    public bool infoBox = true;
    public bool shouldDisable = true;
    public Enums.character character;
    private DialogueManager DialogueManager;
    public CreateDialogue nextDialogue;

    private void Awake()
    {
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnableDialogueBox();
    }

    public void EnableDialogueBox()
    {
        if (!DialogueManager.dialogueActive)
        {
            if (infoBox)
                DialogueManager.EnableDialogueBox(text);
            else
                DialogueManager.EnableDialogueBox(text, character);
        }

        if (nextDialogue != null)
        {
            DialogueManager.SetNextDialogue(nextDialogue);
        }

        if (shouldDisable)
            gameObject.SetActive(false);
    }
}
