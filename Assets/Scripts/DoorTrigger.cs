using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController door;
    public bool toggle;
    public bool shouldDisable;

    private void OnTriggerEnter2D(Collider2D collision) {
        TriggerDoor();
        gameObject.SetActive(!shouldDisable);
    }

    public void TriggerDoor() {
        if (toggle) {
            door.ToggleDoor();
        } else if (door.isOpen) {
            door.CloseDoor();
        } else if (!door.isOpen) {
            door.OpenDoor();
        }
    }
}
