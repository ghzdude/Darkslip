using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Collectable : MonoBehaviour
{
    public string internalName;
    [TextArea(10,10)] public string TextOnPickup;
    private Transform Player;
    private MarkerTrigger MarkerTrigger;
    public bool characterResponse;
    public bool shouldDisable;
    private GameObject glowingSprite;
    [HideInInspector] public Sprite icon;

    private void Start() {
        icon = GetComponent<SpriteRenderer>().sprite;
        Player = Managers.GetPlayerController().transform;
        glowingSprite = Resources.Load<GameObject>(Paths.ObjectGlow);
        MarkerTrigger = GetComponent<MarkerTrigger>();

        if (transform.Find(glowingSprite.name) == null)
            Instantiate(glowingSprite, transform).transform.localPosition = Vector3.zero;
    }

    public void Fire() {
        DialogueManager dialogueManager = Managers.GetDialogueManager();
        if (!dialogueManager.IsActive() && Player != null) {
            
            // retrofit a Dialogue instead of using Collectable
            Dialogue dialogue = gameObject.AddComponent<Dialogue>();
            dialogue.infoBox = !characterResponse;
            dialogue.text = TextOnPickup;
            dialogue.character = Enums.Character.Sean;

            dialogueManager.StartDialogue(dialogue);
            if (MarkerTrigger != null) {
                MarkerTrigger.GenericTrigger(Player.GetComponent<NavigationController>());
            }
            transform.SetParent(Player.GetChild(0));
            
        } else {
            Debug.Log("Player transform is null, collectable not parented to player inventory!");
        }

        if (GetComponent<DoorTrigger>() != null) {
            GetComponent<DoorTrigger>().TriggerDoor();
        }

        if (shouldDisable) {
            gameObject.SetActive(false);
        }
    }
}
