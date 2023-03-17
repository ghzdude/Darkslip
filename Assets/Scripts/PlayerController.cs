using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float speed;
    private float angryTime;
    public float attackDuration; // Time between shots when holding down 'SPACE'
    private float attackCooldown;
    private bool attacking;
    private Vector2 controlVector;
    private int health;
    private CanvasManager CanvasManager;
    public int maxHealth;
    private Transform Camera;
    public GameObject[] castPositions;
    public GameObject[] interactPositions;
    public AudioClip gunShot;
    private AudioSource src;
    public Enums.Direction dir;
    private SceneController SceneController;
    private bool active;

    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        src = gameObject.GetComponent<AudioSource>();

        Camera = Managers.GetCamera();
        CanvasManager = Managers.GetCanvasManager();
        SceneController = Managers.GetSceneController();
        health = maxHealth; // Set Health
        CanvasManager.UpdateHealthGUI(this);
        active = true;
        
    }
    
    void FixedUpdate() {
        if (active) {
            controlVector = new Vector2(
                Input.GetAxisRaw("Horizontal"), 
                Input.GetAxisRaw("Vertical") );

            rb.velocity = controlVector * speed;

            if (attacking || Input.GetKey(KeyCode.Space))
                rb.velocity = Vector2.zero;
            
        } else {
            rb.velocity = Vector2.zero;
            // anim.SetTrigger("faceDownImmediately");
            anim.Play(SeanAnimationStates.IdleDown);
            // ResetAnimState();
            angryTime = 0;
            // CheckAnger();
        }
    }

    private void Update()
    {
        if (Managers.GetDialogueManager().IsActive()) {
            if (Input.GetKeyDown(KeyCode.Z))
                Interact();
        }

        if (CheckHealth())
            SceneController.ResetScene();

        Camera.position = new Vector3(transform.position.x, transform.position.y, Camera.position.z);

        if (active && Time.timeScale > 0) {
            // ResetAnimState();
            CheckAnger();

            if (!attacking)
                CheckMovement(rb.velocity.x, rb.velocity.y);
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                // anim.SetBool("attack", true);
                // anim.SetTrigger("attack");
                Attack();
                angryTime = 10;
                attackCooldown = attackDuration;
            }

            if (attackCooldown <= 0) {
                attackCooldown = 0;
                if (Input.GetKey(KeyCode.Space)) {
                    // anim.SetBool("attack", true);
                    // anim.SetTrigger("attack");
                    Attack();
                    angryTime = 10;
                    attackCooldown = attackDuration;
                }
            } else {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    private void Attack() {
        attacking = true;
        
        switch (dir) {
            case Enums.Direction.Left:
                anim.Play(SeanAnimationStates.AttackLeft);
                break;
            case Enums.Direction.Right:
                anim.Play(SeanAnimationStates.AttackRight);
                break;
            case Enums.Direction.Up:
                anim.Play(SeanAnimationStates.AttackUp);
                break;
            case Enums.Direction.Down:
                anim.Play(SeanAnimationStates.AttackDown);
                break;
            default:
                break;
        }
    }

    public void Shoot() {
        RaycastHit2D[] hits;

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
                hits = null;
                break;
        }

        src.PlayOneShot(gunShot, Managers.GetCanvasManager().GetSFXSliderValue() * 1.1f);

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


    public void SetDirection(Enums.Direction dir) {
        this.dir = dir;
    }

    public void ResetDuration() {
        attackDuration = 0;
    }

    public void Interact() {
        RaycastHit2D[] hits;

        switch (dir) {
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
                hits = null;
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
                    Managers.GetInventoryManager().AddItem(hit.GetComponent<Collectable>());
                    return;
                }

                if (hit.GetComponent<Dialogue>() != null) {
                    hit.GetComponent<Dialogue>().TriggerDialogue();
                }
            }
        }
    }

    // Decrement Health by Amount
    public void DecHealth(int amount) {
        health -= amount;
        CanvasManager.UpdateHealthGUI(this);
    }

    // Decrement Health by 1
    public void DecHealth() {
        DecHealth(1);
    }

    // Increment Health by Amount
    public void IncHealth(int amount) {
        health += amount;
        CanvasManager.UpdateHealthGUI(this);
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
    /*
    private void ResetAnimState() {
        anim.SetBool("movingRight", false);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingUp", false);
        anim.SetBool("movingDown", false);
        anim.SetBool("idle", true);
    }*/

    private void CheckAnger() {
        if (angryTime > 0)
            angryTime -= Time.deltaTime;
            // anim.SetBool("angry", true);
        else 
            angryTime = 0;
            // anim.SetBool("angry", false);
        
    }

    private void CheckMovement(float x, float y) {
        if (angryTime > 0) {
            if (x > 0) {
                // anim.SetBool("movingRight", true);
                // anim.SetBool("movingLeft", false);
                anim.Play(SeanAnimationStates.WalkRightAttack);
                dir = Enums.Direction.Right;
            } else if (x < 0) {
                // anim.SetBool("movingRight", false);
                // anim.SetBool("movingLeft", true);
                anim.Play(SeanAnimationStates.WalkLeftAttack);
                dir = Enums.Direction.Left;
            } else if (y > 0) {
                // anim.SetBool("movingUp", true);
                // anim.SetBool("movingDown", false);
                anim.Play(SeanAnimationStates.WalkUp);
                dir = Enums.Direction.Up;
            } else if (y < 0) {
                // anim.SetBool("movingUp", false);
                // anim.SetBool("movingDown", true);
                anim.Play(SeanAnimationStates.WalkDownAttack);
                dir = Enums.Direction.Down;
            }
        } else {
            if (x > 0) {
                // anim.SetBool("movingRight", true);
                // anim.SetBool("movingLeft", false);
                anim.Play(SeanAnimationStates.WalkRight);
                dir = Enums.Direction.Right;
            } else if (x < 0) {
                // anim.SetBool("movingRight", false);
                // anim.SetBool("movingLeft", true);
                anim.Play(SeanAnimationStates.WalkLeft);
                dir = Enums.Direction.Left;
            } else if (y > 0) {
                // anim.SetBool("movingUp", true);
                // anim.SetBool("movingDown", false);
                anim.Play(SeanAnimationStates.WalkUp);
                dir = Enums.Direction.Up;
            } else if (y < 0) {
                // anim.SetBool("movingUp", false);
                // anim.SetBool("movingDown", true);
                anim.Play(SeanAnimationStates.WalkDown);
                dir = Enums.Direction.Down;
            }
        }

        if (x != 0 || y != 0)
            // anim.SetBool("idle", false);
            return;
        
        // anim.SetBool("idle", true);
        Idle();

    }

    private void Idle() {
        if (angryTime > 0)
            switch (dir) {
                case Enums.Direction.Left:
                    anim.Play(SeanAnimationStates.IdleLeftAttack);
                    break;
                case Enums.Direction.Right:
                    anim.Play(SeanAnimationStates.IdleRightAttack);
                    break;
                case Enums.Direction.Up:
                    anim.Play(SeanAnimationStates.IdleUp);
                    break;
                case Enums.Direction.Down:
                    anim.Play(SeanAnimationStates.IdleDownAttack);
                    break;
                default:
                    break;
        }
        else
            switch (dir) {
                case Enums.Direction.Left:
                    anim.Play(SeanAnimationStates.IdleLeft);
                    break;
                case Enums.Direction.Right:
                    anim.Play(SeanAnimationStates.IdleRight);
                    break;
                case Enums.Direction.Up:
                    anim.Play(SeanAnimationStates.IdleUp);
                    break;
                case Enums.Direction.Down:
                    anim.Play(SeanAnimationStates.IdleDown);
                    break;
                default:
                    break;
            }

    }

    public void SetActive(bool b)
    {
        active = b;
        rb.isKinematic = !b;
    }

    public void StopAttacking() { attacking = false; }
}
