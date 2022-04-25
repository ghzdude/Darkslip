using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramTrigger : MonoBehaviour
{
    public Tram Tram;
    public Transform starting;
    public Transform destination;

    // Start is called before the first frame update
    void Start() {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (Tram != null || starting != null || destination != null) {
            Tram.Move(starting, destination);
            gameObject.SetActive(false);
        } else {
            Debug.Log("Tram, starting, or destination is null!");
        }
    }
}
