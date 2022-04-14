using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terminal : MonoBehaviour
{
    public GameObject target;
    public AudioClip activate;
    private AudioSource src;
    public int markerIndex;
    public bool allowMarkerIndex;

    // Start is called before the first frame update
    void Start()
    {
        src = gameObject.GetComponent<AudioSource>();
    }

    public void Fire(float volume, Transform Player)
    {
        // Debug.Log("Fire");
        src.PlayOneShot(activate, volume);
        switch (target.tag)
        {
            case "Door":
                target.GetComponent<DoorController>().OpenDoor();
                break;
            case "SceneController":
                target.GetComponent<SceneController>().LoadCredit();
                break;
            default:
                Debug.Log("Nothing Happened, Check Your Tags.");
                break;
        }

        if (allowMarkerIndex)
            Player.GetComponent<NavigationController>().SetMarkerIndex(markerIndex);
        allowMarkerIndex = false;
    }
}
