using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public AudioClip openDoor;
    public AudioClip closeDoor;
    public bool startOpen;
    private Animator anim;
    private AudioSource src;
    [HideInInspector] public bool isOpen;

    private void Start() {
        anim = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
        isOpen = false;

        if (startOpen) {
            anim.SetBool("startOpen", startOpen);
            isOpen = true;
        }
    }

    public void OpenDoor()
    {
        anim.SetBool("startOpen", false);
        anim.SetTrigger("open");
        if (openDoor != null) {
            src.PlayOneShot(openDoor);
        }
        isOpen = true;
    }

    public void CloseDoor()
    {
        anim.SetBool("startOpen", false);
        anim.SetTrigger("close");
        if (closeDoor != null) {
            src.PlayOneShot(closeDoor);
        }
        isOpen = false;
    }

    public void ToggleDoor()
    {
        anim.SetBool("startOpen", false);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("doorClosed"))
        {
            anim.SetTrigger("open");
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("doorOpened"))
        {
            anim.SetTrigger("closed");
        }

        isOpen = !isOpen;
    }
}
