using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Dialogue : MonoBehaviour
{
    [TextArea(10, 10)] public string text;
    public bool infoBox = true;
    public bool shouldDisable = true;
    public Enums.Character character;
    public Dialogue nextDialogue;

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
        if (collision.GetComponent<PlayerController>() != null) {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue() {
        Managers.GetDialogueManager().StartDialogue(this);
        gameObject.SetActive(!shouldDisable);
    }
}
