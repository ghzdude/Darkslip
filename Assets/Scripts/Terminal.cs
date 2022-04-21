using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terminal : MonoBehaviour
{
    public GameObject target;
    public AudioClip activate;
    private AudioSource src;
    private DialogueManager DialogueManager;
    private Transform Player;
    public int markerIndex;
    public bool allowMarkerIndex;
    public bool quitGame;
    public bool requiresCode;
    public string correctCode;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        src = gameObject.GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        active = true;
    }

    public void Fire()
    {
        if (active)
        {
            // Debug.Log("Fire");
            src.PlayOneShot(activate, DialogueManager.GetSFXSliderValue());

            if (requiresCode)
                DialogueManager.OpenCodeEntry(this);
            else
            {
                if (target.GetComponent<DoorController>() != null)
                {
                    target.GetComponent<DoorController>().OpenDoor();
                    return;
                }

            }

            if (quitGame)
            {
                GameObject.FindGameObjectWithTag("SceneController ").GetComponent<SceneController>().QuitGame();
            }

            // Debug.Log("Nothing Happened, Check Your Tags.");


            if (allowMarkerIndex)
                Player.GetComponent<NavigationController>().SetMarkerIndex(markerIndex);
            allowMarkerIndex = false;
        }
    }

    public void Fire (bool succeed)
    {
        if (active)
        {
            if (succeed)
            {
                if (target.GetComponent<DoorController>() != null)
                {
                    target.GetComponent<DoorController>().OpenDoor();
                    return;
                }
            }

            if (allowMarkerIndex)
                Player.GetComponent<NavigationController>().SetMarkerIndex(markerIndex);
            allowMarkerIndex = false;
        }
    }

    public void Disable()
    {
        active = false;
    }

    public void Enable()
    {
        active = true;
    }
}
