using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Terminal : MonoBehaviour
{
    public GameObject target;
    public AudioClip activate;
    private AudioSource src;
    public Transform nextMarker;
    public bool quitGame;
    public bool requiresCode;
    public string correctCode;
    private bool active;

    // Start is called before the first frame update
    void Start() {
        src = gameObject.GetComponent<AudioSource>();
        active = true;
    }

    public void Fire() {
        if (active) {
            src.PlayOneShot(activate, Managers.GetDialogueManager().GetSFXSliderValue());

            if (requiresCode) {
                Managers.GetDialogueManager().OpenCodeEntry(this);
            } else {
                if (target.GetComponent<DoorController>() != null) {
                    target.GetComponent<DoorController>().OpenDoor();
                    SetMarker();
                    CheckDialogue();
                    return;
                }
            }

            if (quitGame) {
                Managers.GetSceneController().QuitGame();
            }
        }
    }

    public void Fire (bool succeed) {
        if (active && succeed) {
            src.PlayOneShot(activate, Managers.GetDialogueManager().GetSFXSliderValue());

            if (target.GetComponent<DoorController>() != null) {
                target.GetComponent<DoorController>().OpenDoor();
                SetMarker();
                CheckDialogue();
                return;
            }
        }
    }

    public void Disable() {
        active = false;
    }

    public void Enable() {
        active = true;
    }

    private void SetMarker() {
        if (nextMarker != null)
            Managers.GetPlayerController().GetComponent<NavigationController>().SetTarget(nextMarker);
    }

    private void CheckDialogue() {
        if (GetComponent<CreateDialogue>() != null) {
            GetComponent<CreateDialogue>().EnableDialogueBox();
        }
    }
}
