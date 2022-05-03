using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    public string internalName;
    [TextArea(10,10)] public string TextOnPickup;
    private Transform Player;
    private MarkerTrigger MarkerTrigger;
    public bool characterResponse;
    public bool shouldDisable;
    public GameObject glowingSprite;
    [HideInInspector] public Sprite icon;

    private void Start() {
        icon = GetComponent<SpriteRenderer>().sprite;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        MarkerTrigger = GetComponent<MarkerTrigger>();

        if (transform.Find(glowingSprite.name) == null)
            Instantiate(glowingSprite, transform).transform.localPosition = Vector3.zero;
    }

    public void Fire() {
        if (!Managers.GetDialogueManager().dialogueActive && Player != null) {
            if (!characterResponse)
                Managers.GetDialogueManager().EnableDialogueBox(TextOnPickup);
            else
                Managers.GetDialogueManager().EnableDialogueBox(TextOnPickup, Enums.Character.Sean);

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
