using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    private Transform track;

    // Start is called before the first frame update
    void Awake()
    {
        track = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetTrack;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetTrack;
    }

    private void ResetTrack(Scene scene, LoadSceneMode mode)
    {
        track = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(track.position.x, track.position.y, transform.position.z);
    }
}
