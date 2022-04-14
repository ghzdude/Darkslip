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
    [HideInInspector] public int maxHealth;
    private Transform Camera;
    public GameObject[] castPositions;
    public GameObject[] interactPositions;
    private RaycastHit2D[] hits;
    public AudioClip gunShot;
    private AudioController audioController;
    private NavigationController nav;
    public Enums.direction dir;
    [HideInInspector] public SceneController SceneController;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        audioController = gameObject.GetComponent<AudioController>();
        nav = gameObject.GetComponent<NavigationController>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        maxHealth = SceneController.GetDialogueManager().GetMaxHearts() * 2;
        health = maxHealth; // Set Health
    }
    
    void FixedUpdate()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        
        rb.velocity = new Vector2(h * speed, v * speed);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (CheckHealth())
            SceneController.ResetScene();

        ResetAnimState();
        CheckAnger();
        CheckMovement(rb.velocity.x, rb.velocity.y);
        Camera.position = new Vector3(transform.position.x, transform.position.y, Camera.position.z);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0)
        {
            // anim.SetBool("attack", true);
            anim.SetTrigger("attack");
            angryTime = 10;
            attackCooldown = attackDuration;
        }

        if (attackCooldown <= 0)
        {
            attackCooldown = 0;
            if (Input.GetKey(KeyCode.Space) && Time.timeScale > 0)
            {
                // anim.SetBool("attack", true);
                anim.SetTrigger("attack");
                angryTime = 10;
                attackCooldown = attackDuration;
            }
        }
        else
        {
            attackCooldown -=  Time.deltaTime;
        }
    }

    public void Shoot(Enums.direction dir)
    {
        switch (dir)
        {
            case Enums.direction.Right:
                hits = Physics2D.RaycastAll(castPositions[1].transform.position, Vector2.right, 16f);
                break;
            case Enums.direction.Left:
                hits = Physics2D.RaycastAll(castPositions[0].transform.position, Vector2.left, 16f);
                break;
            case Enums.direction.Up:
                hits = Physics2D.RaycastAll(castPositions[2].transform.position, Vector2.up, 16f);
                break;
            case Enums.direction.Down:
                hits = Physics2D.RaycastAll(castPositions[2].transform.position, Vector2.down, 16f);
                break;
            default:
                break;
        }
        audioController.PlayClip(gunShot, 1f);

        if (hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Wall"))
                {
                    return;
                }

                if (hits[i].collider.GetComponent<HealthManager>() != null)
                {
                    hits[i].transform.GetComponent<HealthManager>().DecHealth(1, audioController);
                    // Debug.Log("object hit");
                    return;
                }
            }
        }
    }

    public void SetDirection(Enums.direction dir)
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
            case Enums.direction.Right:
                hits = Physics2D.BoxCastAll(interactPositions[1].transform.position, Vector2.one, 0f, Vector2.zero);
                break;
            case Enums.direction.Left:
                hits = Physics2D.BoxCastAll(interactPositions[0].transform.position, Vector2.one, 0f, Vector2.zero);
                break;
            case Enums.direction.Up:
                hits = Physics2D.BoxCastAll(interactPositions[3].transform.position, Vector2.one, 0f, Vector2.zero);
                break;
            case Enums.direction.Down:
                hits = Physics2D.BoxCastAll(interactPositions[2].transform.position, Vector2.one, 0f, Vector2.zero);
                break;
            default:
                break;
        }

        if (hits != null && hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject hit = hits[i].transform.gameObject;
                if (hit.GetComponent<Terminal>() != null)
                {
                    audioController.PlayClip(hits[i].transform.GetComponent<Terminal>().activate, 0.66f);
                    hit.GetComponent<Terminal>().Fire(0.66f, transform);
                    return;
                }

                if (hit.GetComponent<Collectable>()  != null)
                {
                    hit.GetComponent<Collectable>().Fire(audioController, transform.GetChild(0));
                    SceneController.GetInventoryManager().AddItem(hit.GetComponent<Collectable>());
                    return;
                }

                if (hit.GetComponent<CreateDialogue>() != null)
                {
                    hit.GetComponent<CreateDialogue>().EnableDialogueBox();
                }
            }
        }
    }

    public void DecHealth(int amount)
    {
        health -= amount;
    } // Decrement Health by Amount

    public void DecHealth()
    {
        health--;
    } // Decrement Health by 1

    public void IncHealth(int amount)
    {
        health += amount;
    } // Increment Health by Amount

    public void IncHealth()
    {
        health++;
    } // Increment Health by 1

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int v)
    {
        health = v;
    }

    private bool CheckHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
            return false;
        }
        else if (health <= 0)
        {
            health = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ResetAnimState()
    {
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

    public void SetVolume(float volume)
    {
        // GetComponent<AudioSource>().volume = SceneController.musicVolume;

    }
}
