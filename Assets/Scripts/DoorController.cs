using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public AudioClip doorMoving;
    public AudioClip doorStopped;
    public GameObject doorLockedRef;
    public bool startOpen;
    public bool locked;
    private Animator anim;
    private AudioSource src;
    [HideInInspector] public bool isOpen;


    private void Awake() {
        anim = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }

    private void Start() {
        isOpen = false;

        if (startOpen) {
            anim.SetBool("startOpen", startOpen);
            isOpen = true;
        }
        doorLockedRef.SetActive(locked);
    }

    public void OpenDoor() {
        anim.SetBool("startOpen", false);
        anim.SetTrigger("open");
        // OnMoving();
        isOpen = true;
    }

    public void CloseDoor() {
        anim.SetBool("startOpen", false);
        anim.SetTrigger("close");
        // OnMoving();
        isOpen = false;
    }

    public void ToggleDoor() {
        if (!isOpen) {
            OpenDoor();
        } else if (isOpen) {
            CloseDoor();
        }

        isOpen = !isOpen;
    }

    public void OnComplete() {
        src.Stop();
        if (doorStopped != null) {
            src.clip = doorStopped;
            src.Play();
        }
    }

    public void OnMoving() {
        if (doorMoving != null && Time.timeScale >= 1) {
            doorLockedRef.SetActive(false);
            src.clip = doorMoving;
            src.Play();
        }
    }
}
