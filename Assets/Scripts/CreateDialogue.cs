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
    public DialogueManager DialogueManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnableDialogueBox();
    }

    public void EnableDialogueBox()
    {
        if (infoBox)
            DialogueManager.EnableDialogueBox(text);
        else
            DialogueManager.EnableDialogueBox(text, character);

        if (shouldDisable)
            gameObject.SetActive(false);
    }
}
