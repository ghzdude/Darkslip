using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tram : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 starting;
    private Vector2 destination;
    // private Vector3 departPos;
    [HideInInspector] public List<Transform> Destinations;
    private Transform sitting;
    private Transform Player;
    private DialogueManager DialogueManager;
    private bool moving;
    // private bool departing;
    // private bool hasPlayer;
    public float speed;
    public float margin;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
        // startingPos = transform.GetChild(0).transform.position;
        // destinationPos = transform.GetChild(1).transform.position;
        // departPos = transform.GetChild(2).transform.position;
        sitting = transform.GetChild(4).transform;
        Destinations.AddRange(GameObject.Find("TramPositions").GetComponentsInChildren<Transform>());
        Destinations.RemoveAt(0);
        transform.position = Destinations[0].position;
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        // transform.position = starting.position;
    }

    // Update is called once per frame
    void Update() {
        if (moving) {
            if (CalculateVector(transform.position, destination).magnitude <= margin) {
                transform.position = destination;
                starting = destination;
                destination = Vector2.zero;
                rb.velocity = Vector2.zero;
                moving = false;
                anim.SetTrigger("open");

                if (Player != null) {
                    Player.GetComponent<PlayerController>().SetActive(true);
                    Player.parent = null;
                }
            }
        }
    }
    
    public void Move(Transform a, Transform b) {
        starting = a.position;
        destination = b.position;
        moving = true;
        gameObject.SetActive(true);
        rb.velocity = CalculateVector(starting, destination).normalized * (speed * Time.deltaTime);
        // Debug.Log("move from " + a.position + " to " + b.position + " at " + rb.velocity.magnitude);
        Debug.Log(
            "vector: " + CalculateVector(starting, destination) +
            " | normalized: " + CalculateVector(starting, destination).normalized +
            " | speed * vector: " + CalculateVector(starting, destination) * (speed * Time.deltaTime) +
            " | velocity: " + rb.velocity.magnitude
            );
    }

    public void Move(Transform b) {
        destination = b.position;
        moving = true;
        gameObject.SetActive(true);
        rb.velocity = CalculateVector(starting, destination).normalized * (speed * Time.deltaTime);
        // Debug.Log("move from " + starting.position + " to " + b.position + " at " + rb.velocity.magnitude);
        Debug.Log(
            "vector: " + CalculateVector(starting, destination) + 
            " | normalized: " + CalculateVector(starting, destination).normalized +
            " | speed * vector: " + CalculateVector(starting, destination) * (speed * Time.deltaTime) +
            " | velocity: " + rb.velocity.magnitude
            );
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            ChangeSortingLayer("player");
            Player = collision.transform;
            Player.GetComponent<PlayerController>().SetActive(false);
            Player.position = sitting.position;
            Player.transform.SetParent(transform);
            anim.SetTrigger("close");
            DialogueManager.OpenTramSelector();
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        // ChangeSortingLayer("foreground");
    }

    private void ChangeSortingLayer (string layer)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = layer;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = layer;
        transform.GetChild(2).GetComponent<SpriteRenderer>().sortingLayerName = layer;
    }

    private Vector2 CalculateVector(Vector2 a, Vector2 b) { 
        // (0,10) & (10,20) should return (10,10) which is b - a
        // (0,-10) & (20, -30) should return (20, -20) which is b - a
        return b - a;
    }
}
