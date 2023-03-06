using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CreateDialogue : MonoBehaviour
{
    [TextArea(10, 10)] public string text;
    public bool infoBox = true;
    public bool shouldDisable = true;
    public Enums.Character character;
    private DialogueManager DialogueManager;
    public CreateDialogue nextDialogue;

    private void Awake() {
        DialogueManager = Managers.GetDialogueManager();
    }

    private void OnDrawGizmos() {
        Collider2D col = GetComponent<Collider2D>();

        if (col == null) {
            return;
        }
        Bounds bounds = col.bounds;

        Gizmos.color = new Color(0,1,0);
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision != null && collision.GetComponent<PlayerController>() != null) {
            EnableDialogueBox();
        }
    }

    public void EnableDialogueBox() {
        if (!DialogueManager.dialogueActive) {
            if (infoBox)
                DialogueManager.EnableDialogueBox(text);
            else
                DialogueManager.EnableDialogueBox(text, character);
        }

        if (nextDialogue != null) {
            DialogueManager.SetNextDialogue(nextDialogue);
        }

        if (shouldDisable)
            gameObject.SetActive(false);
    }
}
