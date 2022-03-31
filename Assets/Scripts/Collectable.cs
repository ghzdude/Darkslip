using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    public string internalName;
    [TextArea(10,10)] public string TextOnPickup;
    private DialogueManager DialogueManager;
    public bool characterResponse;
    public bool shouldDisable;
    [HideInInspector] public Sprite icon;

    private void Start()
    {
        icon = GetComponent<SpriteRenderer>().sprite;
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }
    public void Fire(AudioController controller, Transform plrInventory)
    {
        if (plrInventory != null && !characterResponse)
        {
            DialogueManager.EnableDialogueBox(TextOnPickup);
            transform.SetParent(plrInventory);
        }
        else if (plrInventory != null && characterResponse)
        {
            DialogueManager.EnableDialogueBox(TextOnPickup, Enums.character.Sean);
            transform.SetParent(plrInventory);
        }
        else
            Debug.Log("Invalid transform, player inventory not updated!");

        if (shouldDisable)
        {
            gameObject.SetActive(false);
        }
    }
}
