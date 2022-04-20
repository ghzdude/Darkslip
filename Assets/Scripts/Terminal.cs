using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terminal : MonoBehaviour
{
    public GameObject target;
    public AudioClip activate;
    private AudioSource src;
    public GameObject glowingSprite;
    public int markerIndex;
    public bool allowMarkerIndex;
    public bool quitGame;

    // Start is called before the first frame update
    void Start()
    {
        src = gameObject.GetComponent<AudioSource>();
    }

    public void Fire(float volume, Transform Player)
    {
        // Debug.Log("Fire");
        src.PlayOneShot(activate, volume);
        if (target.GetComponent<DoorController>() != null)
        {
            target.GetComponent<DoorController>().OpenDoor();
            return;
        }
        if (quitGame)
        {
            GameObject.FindGameObjectWithTag("SceneController ").GetComponent<SceneController>().QuitGame();
        }

        Debug.Log("Nothing Happened, Check Your Tags.");
                

        if (allowMarkerIndex)
            Player.GetComponent<NavigationController>().SetMarkerIndex(markerIndex);
        allowMarkerIndex = false;
    }
}
