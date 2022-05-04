using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public GameObject target;
    public AudioClip activate;
    private AudioSource src;
    public Transform nextMarker;
    public bool allowNextMarker;
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
            // Debug.Log("Fire");
            src.PlayOneShot(activate, Managers.GetDialogueManager().GetSFXSliderValue());

            if (requiresCode) {
                Managers.GetDialogueManager().OpenCodeEntry(this);
            } else {
                if (target.GetComponent<DoorController>() != null) {
                    target.GetComponent<DoorController>().OpenDoor();
                    SetMarker();
                    Debug.Log("open door");
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
            if (target.GetComponent<DoorController>() != null) {
                target.GetComponent<DoorController>().OpenDoor();
                SetMarker();
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
        if (allowNextMarker)
            Managers.GetPlayerController().GetComponent<NavigationController>().SetTarget(nextMarker);
        allowNextMarker = false;
    }
}
