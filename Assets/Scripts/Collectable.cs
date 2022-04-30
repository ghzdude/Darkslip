using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    public string internalName;
    [TextArea(10,10)] public string TextOnPickup;
    private DialogueManager DialogueManager;
    private Transform Player;
    private MarkerTrigger MarkerTrigger;
    public bool characterResponse;
    public bool shouldDisable;
    public GameObject glowingSprite;
    [HideInInspector] public Sprite icon;

    private void Start()
    {
        icon = GetComponent<SpriteRenderer>().sprite;
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        MarkerTrigger = GetComponent<MarkerTrigger>();

        if (transform.Find(glowingSprite.name) == null)
            Instantiate(glowingSprite, transform).transform.localPosition = Vector3.zero;
    }

    public void Fire() {
        if (!DialogueManager.dialogueActive && Player != null) {
            if (!characterResponse)
                DialogueManager.EnableDialogueBox(TextOnPickup);
            else
                DialogueManager.EnableDialogueBox(TextOnPickup, Enums.Character.Sean);

            if (MarkerTrigger != null) {
                MarkerTrigger.GenericTrigger(Player.GetComponent<NavigationController>());
            }

            transform.SetParent(Player.GetChild(0));

        } else {
            Debug.Log("Player transform is null, collectable not parented to player inventory!");
        }

        if (shouldDisable) {
            gameObject.SetActive(false);
        }
    }
}
