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
    public CreateDialogue nextDialogue;

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
            TriggerDialogue();
        }
    }

    public void TriggerDialogue() {
        if (infoBox)
            Managers.GetDialogueManager().EnableDialogueBox(text);
        else
            Managers.GetDialogueManager().EnableDialogueBox(text, character);
    }
}
