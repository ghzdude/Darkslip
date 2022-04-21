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
    /*
    public Transform doorSprite;
    public Transform closed;
    public Transform opened;
    private bool opening;
    private bool closing;
    */
    public float speed = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
        if (startOpen)
        {
            anim.SetBool("startOpen", startOpen);
        }
    }

    private void Update()
    {

    }

    public void OpenDoor()
    {
        anim.SetTrigger("open");
        src.PlayOneShot(openDoor);
    }

    public void CloseDoor()
    {
        anim.SetTrigger("close");
        src.PlayOneShot(closeDoor);
    }

    public void ToggleDoor()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("doorClosed"))
        {
            anim.SetTrigger("open");
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("doorOpened"))
        {
            anim.SetTrigger("closed");
        }
    }
}
