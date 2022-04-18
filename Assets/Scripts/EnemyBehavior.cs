using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
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
    private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        aggroTimer = 5;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attacking = false;
        dazed = false;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) // On death
        {
            gameObject.SetActive(false);
        }

        if (CastAggroCircle() && target) // If target is found
        {
            CalculateTargetVector();
            CalculateFacing();
        }

        if (targetVector.magnitude < attackRange && !dazed) {
            if (CastAttackCircle()) {
                Attack();
                attacking = true;
            }
        } else {
            attacking = false;
        }

        if (timer <= 0) { // Once timer <= 0, perform movement and checks

            timer = 1f;
            if (!dazed){
                if (CheckSightLine() && !attacking) {
                    ApproachTarget();
                    aggroTimer = 5;
                } else if (aggroTimer > 0 && !attacking) {
                    aggroTimer -= Time.deltaTime;
                } else {
                    rb.velocity = Vector2.zero;
                    target = null;
                    ResetAnimationState();
                }

                
            }
            dazed = false;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private bool CastAggroCircle()
    {
        hits = Cast(aggroRange);
        bool hit = false;

        if (hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
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

    private void CalculateTargetVector()
    {
        targetVector = target.transform.GetChild(5).position - transform.position;
        // targetAngle = LookAt2D(target.transform.position);
    }

    private bool CheckSightLine()
    {
        bool canSee = true;
        hits = Physics2D.LinecastAll(transform.position, target.transform.position);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("Wall"))
            {
                canSee = false;
                break;
            }
        }
        return canSee;

    }

    private void ApproachTarget()
    {
        rb.isKinematic = true;
        rb.velocity = targetVector.normalized * speed;
        // CalculateFacing(speed / 2);
        

        // Debug.Log(string.Format("Target Vector: {0}x, {1}y", CalculateTargetVector().x, CalculateTargetVector().y));
    }

    private void CalculateFacing()
    {
        if (targetVector.x > Mathf.Abs(targetVector.y)) {
            UpdateAnimationState(true, Enums.direction.Right);
            Debug.Log("attack right");
        } else if (targetVector.x < -Mathf.Abs(targetVector.y)) {
            UpdateAnimationState(true, Enums.direction.Left);
            Debug.Log("attack left");
        } else if (targetVector.y > Mathf.Abs(targetVector.x)) {
            UpdateAnimationState(true, Enums.direction.Up);
            Debug.Log("attack up");
        } else if (targetVector.y < -Mathf.Abs(targetVector.x)) {
            UpdateAnimationState(true, Enums.direction.Down);
            Debug.Log("attack down");
        } else {
            ResetAnimationState();
        }
    }

    private bool CastAttackCircle()
    {
        hits = Cast(attackRange);
        bool canAttack = false;
        
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.GetComponent<PlayerController>() != null)
            {
                canAttack = true;
                break;
            }
        }

        return canAttack;
        
    }

    private RaycastHit2D[] Cast(float radius)
    {
        return Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
    }

    private void Attack()
    {
        ApproachTarget();
        anim.SetTrigger("attack");
        rb.velocity = Vector2.zero;
    }

    private void UpdateAnimationState(bool moving, Enums.direction dir)
    {
        anim.SetBool("moving", moving);
        anim.SetBool("idle", !moving);
        switch (dir)
        {
            case Enums.direction.Left:
                anim.SetBool("up", false);
                anim.SetBool("down", false);
                anim.SetBool("left", true);
                anim.SetBool("right", false);
                break;
            case Enums.direction.Right:
                anim.SetBool("up", false);
                anim.SetBool("down", false);
                anim.SetBool("left", false);
                anim.SetBool("right", true);
                break;
            case Enums.direction.Up:
                anim.SetBool("up", true);
                anim.SetBool("down", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
                break;
            case Enums.direction.Down:
                anim.SetBool("up", false);
                anim.SetBool("down", true);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
                break;
            default:
                break;
        }
        
    }

    private void ResetAnimationState()
    {
        anim.SetBool("moving", false);
        anim.SetBool("idle", true);
        anim.SetBool("up", false);
        anim.SetBool("down", false);
        anim.SetBool("left", false);
        anim.SetBool("right", false);
    }

    private void CastDamageBox(Enums.direction dir)
    {
        switch (dir)
        {
            case Enums.direction.Left:
                hits = Physics2D.BoxCastAll(attackPositions[2].position, Vector2.one /2, 0, Vector2.left);
                break;
            case Enums.direction.Right:
                hits = Physics2D.BoxCastAll(attackPositions[3].position, Vector2.one /2, 0, Vector2.right);
                break;
            case Enums.direction.Up:
                hits = Physics2D.BoxCastAll(attackPositions[0].position, Vector2.one /2, 0, Vector2.up);
                break;
            case Enums.direction.Down:
                hits = Physics2D.BoxCastAll(attackPositions[1].position, Vector2.one /2, 0, Vector2.down);
                break;
            default:
                break;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.GetComponent<PlayerController>() != null)
            {
                hits[i].collider.GetComponent<PlayerController>().DecHealth();
                return;
            }
        }
    }

    public void DecHealth()
    {
        health--;
        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
        rb.AddForce(-targetVector * knockback, ForceMode2D.Impulse);
        dazed = true;
        timer = 2;
    }

    public void IncHealth()
    {
        health++;
    }

    public void DecHealth(int amt)
    {
        health -= amt;
        rb.AddForce(-targetVector * knockback, ForceMode2D.Impulse);
        dazed = true;
    }

    public void IncHealth(int amt)
    {
        health += amt;
    }
}
