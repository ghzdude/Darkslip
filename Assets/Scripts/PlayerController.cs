using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float speed;
    private float angryTime;
    public float attackDuration; // Time between shots when holding down 'SPACE'
    private float attackCooldown;
    private float h;
    private float v;
    private int health;
    private DialogueManager DialogueManager;
    [HideInInspector] public int maxHealth;
    private Transform Camera;
    public GameObject[] castPositions;
    public GameObject[] interactPositions;
    private RaycastHit2D[] hits;
    public AudioClip gunShot;
    private AudioSource src;
    private NavigationController nav;
    public Enums.Direction dir;
    private SceneController SceneController;
    private bool active;

    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        src = gameObject.GetComponent<AudioSource>();
        nav = gameObject.GetComponent<NavigationController>();

        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        SceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        maxHealth = DialogueManager.GetMaxHearts() * 2;
        health = maxHealth; // Set Health
        DialogueManager.UpdateHealthGUI(this);
        active = true;
        
    }
    
    void FixedUpdate() {
        if (active) {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            rb.velocity = new Vector2(h * speed, v * speed);

            if (Input.GetKey(KeyCode.Space)) {
                rb.velocity = Vector2.zero;
            }
        } else {
            rb.velocity = Vector2.zero;
            anim.SetTrigger("faceDownImmediately");
            ResetAnimState();
            angryTime = 0;
            CheckAnger();
        }
    }

    private void Update()
    {
        if (!DialogueManager.dialogueActive) {
            if (Input.GetKeyDown(KeyCode.Z))
                Interact();
        }

        if (CheckHealth())
            SceneController.ResetScene();

        Camera.position = new Vector3(transform.position.x, transform.position.y, Camera.position.z);

        if (active) {
            ResetAnimState();
            CheckAnger();
            CheckMovement(rb.velocity.x, rb.velocity.y);
            
            if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0) {
                // anim.SetBool("attack", true);
                anim.SetTrigger("attack");
                angryTime = 10;
                attackCooldown = attackDuration;
            }

            if (attackCooldown <= 0) {
                attackCooldown = 0;
                if (Input.GetKey(KeyCode.Space) && Time.timeScale > 0) {
                    // anim.SetBool("attack", true);
                    anim.SetTrigger("attack");
                    angryTime = 10;
                    attackCooldown = attackDuration;
                }
            } else {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    public void Shoot(Enums.Direction dir) {
        switch (dir) {
            case Enums.Direction.Right:
                hits = Physics2D.RaycastAll(castPositions[1].transform.position, Vector2.right, 16f);
                break;
            case Enums.Direction.Left:
                hits = Physics2D.RaycastAll(castPositions[0].transform.position, Vector2.left, 16f);
                break;
            case Enums.Direction.Up:
                hits = Physics2D.RaycastAll(castPositions[2].transform.position, Vector2.up, 16f);
                break;
            case Enums.Direction.Down:
                hits = Physics2D.RaycastAll(castPositions[2].transform.position, Vector2.down, 16f);
                break;
            default:
                break;
        }
        src.PlayOneShot(gunShot, Managers.GetDialogueManager().GetSFXSliderValue() * 1.1f);

        if (hits != null && hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform.CompareTag("Wall")) {
                    return;
                }

                if (hits[i].transform.GetComponentInParent<EnemyBehavior>() != null) {
                    hits[i].collider.GetComponentInParent<EnemyBehavior>().DecHealth();
                    return;
                }

                if (hits[i].transform.CompareTag(Tags.Destructible)) {
                    hits[i].transform.GetComponent<HealthManager>().DecHealth(1, src);
                    return;
                }
                /*
                if (hits[i].collider.GetComponent<CreateDialogue>() != null) {
                    hits[i].collider.GetComponent<CreateDialogue>().EnableDialogueBox();
                    return;
                }*/
            }
        }
    }

    public void SetDirection(Enums.Direction dir)
    {
        this.dir = dir;
    }

    public void ResetDuration()
    {
        attackDuration = 0;
    }

    public void Interact()
    {
        switch (dir)
        {
            case Enums.Direction.Right:
                hits = Physics2D.BoxCastAll(interactPositions[1].transform.position, new Vector2(1, 2), 0f, Vector2.zero);
                break;
            case Enums.Direction.Left:
                hits = Physics2D.BoxCastAll(interactPositions[0].transform.position, new Vector2(1, 2), 0f, Vector2.zero);
                break;
            case Enums.Direction.Up:
                hits = Physics2D.BoxCastAll(interactPositions[3].transform.position, Vector2.one, 0f, Vector2.zero);
                break;
            case Enums.Direction.Down:
                hits = Physics2D.BoxCastAll(interactPositions[2].transform.position, Vector2.one, 0f, Vector2.zero);
                break;
            default:
                break;
        }

        if (hits != null && hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject hit = hits[i].transform.gameObject;
                if (hit.GetComponent<Terminal>() != null) {
                    hit.GetComponent<Terminal>().Fire();
                    return;
                }

                if (hit.GetComponent<Collectable>()  != null) {
                    hit.GetComponent<Collectable>().Fire();
                    SceneController.GetInventoryManager().AddItem(hit.GetComponent<Collectable>());
                    return;
                }

                if (hit.GetComponent<CreateDialogue>() != null) {
                    hit.GetComponent<CreateDialogue>().EnableDialogueBox();
                }
            }
        }
    }

    // Decrement Health by Amount
    public void DecHealth(int amount) {
        health -= amount;
        DialogueManager.UpdateHealthGUI(this);
    }

    // Decrement Health by 1
    public void DecHealth() {
        DecHealth(1);
    }

    // Increment Health by Amount
    public void IncHealth(int amount) {
        health += amount;
        DialogueManager.UpdateHealthGUI(this);
    }
    // Increment Health by 1
    public void IncHealth() {
        IncHealth(1);
    }

    public int GetHealth() => health;

    public void SetHealth(int v) => health = v;

    private bool CheckHealth() {
        if (health > maxHealth) {
            health = maxHealth;
            return false;
        } else if (health <= 0) {
            health = 0;
            return true;
        } else {
            return false;
        }
    }

    private void ResetAnimState() {
        anim.SetBool("movingRight", false);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingUp", false);
        anim.SetBool("movingDown", false);
        anim.SetBool("idle", true);
    }

    private void CheckAnger()
    {
        if (angryTime > 0)
        {
            angryTime -= Time.deltaTime;
            anim.SetBool("angry", true);
        }
        else if (angryTime <= 0)
        {
            angryTime = 0;
            anim.SetBool("angry", false);
        }
    }

    private void CheckMovement(float x, float y)
    {
        if (x > 0)
        {
            anim.SetBool("movingRight", true);
            anim.SetBool("movingLeft", false);
        }
        else if (x < 0)
        {
            anim.SetBool("movingRight", false);
            anim.SetBool("movingLeft", true);
        }
        if (y > 0)
        {
            anim.SetBool("movingUp", true);
            anim.SetBool("movingDown", false);
        }
        else if (y < 0)
        {
            anim.SetBool("movingUp", false);
            anim.SetBool("movingDown", true);
        }

        if (x != 0 || y != 0)
            anim.SetBool("idle", false);
        else
            anim.SetBool("idle", true);
    }

    public void SetActive(bool b)
    {
        active = b;
        rb.isKinematic = !b;
    }
}
