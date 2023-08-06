using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tram : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 starting;
    private Vector2 destination;
    [HideInInspector] public List<Transform> Destinations;
    private Transform sitting;
    private Transform exit;
    private Transform Player;
    private CanvasManager DialogueManager;
    private Collider2D extraCollider;
    private bool moving;
    public float speed;
    public float margin;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
        sitting = transform.GetChild(4);
        exit = transform.GetChild(5);
        Destinations.AddRange(GameObject.Find("TramPositions").GetComponentsInChildren<Transform>());
        Destinations.RemoveAt(0);
        transform.position = Destinations[0].position;
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<CanvasManager>();
    }

    // Update is called once per frame
    void Update() {
        if (moving) {
            // on arrival
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
                    Player.position = exit.position;
                    Player = null;
                }

                if (extraCollider != null) {
                    extraCollider.enabled = false;
                }
            }
        }
    }
    
    public void Move(Transform a, Transform b) {
        starting = a.position;
        destination = b.position;
        if (b.GetComponent<Collider2D>() != null) {
            extraCollider = b.GetComponent<Collider2D>();
        }
        moving = true;
        gameObject.SetActive(true);
        rb.velocity = CalculateVector(starting, destination).normalized  * speed;
    }

    public void Move(Transform b) {
        Move(transform, b);
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

        if (extraCollider != null) {
            extraCollider.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (!moving) {
            ChangeSortingLayer("foreground");
            if (extraCollider != null) {
                extraCollider.enabled = false;
            }
        }
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
