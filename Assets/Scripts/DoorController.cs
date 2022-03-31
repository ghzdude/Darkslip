using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public AudioClip openDoor;
    public AudioClip closeDoor;
    private AudioSource src;
    public Transform doorSprite;
    public Transform closed;
    public Transform opened;
    private bool opening;
    private bool closing;
    public float speed = 1;

    private void Start()
    {
        gameObject.transform.position = closed.position;
        src = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (opening)
            doorSprite.position = Vector2.Lerp(doorSprite.position, opened.position, speed * Time.deltaTime);
        else if (closing)
            doorSprite.position = Vector2.Lerp(doorSprite.position, closed.position, speed * Time.deltaTime);


        if (gameObject.transform == opened)
            closing = false;
        else if (gameObject.transform == closed)
            opening = false;

    }

    public void OpenDoor(AudioController audio)
    {
        // Debug.Log("method OpenDoor has been fired");
        if (!opening)
        {
            opening = true;
            audio.PlayClip(openDoor, 0.66f, src);
        }
        

    }

    public void CloseDoor()
    {
        gameObject.SetActive(true);
    }

    public void ToggleDoor()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
