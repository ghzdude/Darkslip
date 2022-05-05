using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    private int health;
    public int maxHealth;
    public Transform[] attackPositions;
    private float timer;
    private float aggroTimer;
    private RaycastHit2D[] hits;
    private bool dazed;
    private PlayerController target;
    private Vector3 targetVector;
    private Rigidbody2D rb;
    private Animator anim;
    public float aggroRange;
    public float attackRange;
    public float speed;
    public float knockback;
    public float dazedTimer;
    [Header("Audio")]
    public AudioClip hurt;
    public AudioClip death;
    public AudioClip attack;
    private AudioSource src;
    private bool attacking;
    private bool inSight;

    // Start is called before the first frame update
    void Start() {
        timer = 0;
        aggroTimer = 5;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        src = Managers.GetPlayerController().GetComponent<AudioSource>();
        attacking = false;
        dazed = false;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            src.PlayOneShot(death, Managers.GetDialogueManager().GetSFXSliderValue());
            gameObject.SetActive(false);
        }

        if (CastAggroCircle() && !dazed) {
            // If target is found
            CalculateTargetVector();
            CalculateFacing();
        }

        if (target != null) {
            if (!dazed) {
                if (targetVector.magnitude < attackRange) {
                    if (CastAttackCircle()) {
                        Attack();
                        attacking = true;
                    }
                } else {
                    attacking = false;
                }
                if (CheckSightLine() && !attacking) {
                    ApproachTarget();
                    aggroTimer = 5;
                } else {
                    rb.velocity = Vector2.zero;
                    target = null;
                    ResetAnimationState();
                }
            }
            if (aggroTimer > 0) {
                aggroTimer -= Time.deltaTime;
            }

            if (timer <= 0) { // Once timer <= 0, calculate vector and other checks
                timer = 1f;
                dazed = false;
                CalculateTargetVector();
            } else {
                timer -= Time.deltaTime;
            }
        }
    }

    private bool CastAggroCircle() {
        hits = Cast(aggroRange);
        bool hit = false;

        if (hits != null && hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].collider.GetComponent<PlayerController>() != null)
                {
                    target = hits[i].collider.GetComponent<PlayerController>();
                    hit = true;
                    break;
                }
            }
        }
        return hit;
    }

    private void CalculateTargetVector() {
        if (target != null) {
            targetVector = target.transform.GetChild(5).position - transform.position;
        }
        // targetAngle = LookAt2D(target.transform.position);
    }

    private bool CheckSightLine() {
        bool canSee = true;
        hits = Physics2D.LinecastAll(transform.position, target.transform.position);

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider.CompareTag("Wall")) {
                canSee = false;
                break;
            }
        }
        return canSee;
    }

    private void ApproachTarget() {
        rb.velocity = targetVector.normalized * speed;
    }

    private void CalculateFacing() {
        if (targetVector.x > Mathf.Abs(targetVector.y)) {
            UpdateAnimationState(true, Enums.Direction.Right);
            // Debug.Log("attack right");
        } else if (targetVector.x < -Mathf.Abs(targetVector.y)) {
            UpdateAnimationState(true, Enums.Direction.Left);
            // Debug.Log("attack left");
        } else if (targetVector.y > Mathf.Abs(targetVector.x)) {
            UpdateAnimationState(true, Enums.Direction.Up);
            // Debug.Log("attack up");
        } else if (targetVector.y < -Mathf.Abs(targetVector.x)) {
            UpdateAnimationState(true, Enums.Direction.Down);
            // Debug.Log("attack down");
        } else {
            ResetAnimationState();
        }
    }

    private bool CastAttackCircle() {
        hits = Cast(attackRange);
        bool canAttack = false;
        
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider.GetComponent<PlayerController>() != null) {
                canAttack = true;
                break;
            }
        }
        CalculateFacing();
        return canAttack;
    }

    private RaycastHit2D[] Cast(float radius) => Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
    

    private void Attack() {
        ApproachTarget();
        anim.SetTrigger("attack");
        rb.velocity = Vector2.zero;
    }

    private void UpdateAnimationState(bool moving, Enums.Direction dir) {
        anim.SetBool("moving", moving);
        anim.SetBool("idle", !moving);
        switch (dir) {
            case Enums.Direction.Left:
                anim.SetBool("up", false);
                anim.SetBool("down", false);
                anim.SetBool("left", true);
                anim.SetBool("right", false);
                break;
            case Enums.Direction.Right:
                anim.SetBool("up", false);
                anim.SetBool("down", false);
                anim.SetBool("left", false);
                anim.SetBool("right", true);
                break;
            case Enums.Direction.Up:
                anim.SetBool("up", true);
                anim.SetBool("down", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
                break;
            case Enums.Direction.Down:
                anim.SetBool("up", false);
                anim.SetBool("down", true);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
                break;
            default:
                break;
        }
    }

    private void ResetAnimationState() {
        anim.SetBool("moving", false);
        anim.SetBool("idle", true);
        anim.SetBool("up", false);
        anim.SetBool("down", false);
        anim.SetBool("left", false);
        anim.SetBool("right", false);
    }

    private void CastDamageBox(Enums.Direction dir) {
        float scalar = 2.5f;
        switch (dir) {
            case Enums.Direction.Left:
                hits = Physics2D.BoxCastAll(attackPositions[2].position, Vector2.one / scalar, 0, Vector2.zero);
                break;
            case Enums.Direction.Right:
                hits = Physics2D.BoxCastAll(attackPositions[3].position, Vector2.one / scalar, 0, Vector2.zero);
                break;
            case Enums.Direction.Up:
                hits = Physics2D.BoxCastAll(attackPositions[0].position, Vector2.one / scalar, 0, Vector2.zero);
                break;
            case Enums.Direction.Down:
                hits = Physics2D.BoxCastAll(attackPositions[1].position, Vector2.one / scalar, 0, Vector2.zero);
                break;
            default:
                break;
        }

        src.PlayOneShot(attack, Managers.GetDialogueManager().GetSFXSliderValue());

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider.GetComponent<PlayerController>() != null) {
                hits[i].collider.GetComponent<PlayerController>().DecHealth();
                return;
            }
        }
    }

    public void DecHealth(int amt) {
        health -= amt;
        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
        rb.AddForce(-targetVector * knockback, ForceMode2D.Impulse);
        dazed = true;
        ResetAnimationState();
        timer = dazedTimer;

        if (health > 0) {
            src.PlayOneShot(hurt, Managers.GetDialogueManager().GetSFXSliderValue());
        }
    }

    public void IncHealth(int amt) {
        health += amt;
    }

    public void DecHealth() {
        DecHealth(1);
    }

    public void IncHealth() {
        IncHealth(1);
    }
}
