using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public Transform[] attackPositions;
    private float timer;
    public float aggroTimer;
    private RaycastHit2D[] hits;
    private PlayerController target;
    private Vector3 targetAngle;
    private Rigidbody2D rb;
    public float aggroRange;
    public float attackRange;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
        aggroTimer = 5;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            timer = 1;
            CastAggroCircle();
            if (CheckSightLine())
            {
                ApproachTarget();
                aggroTimer = 5;
            }
            else if (aggroTimer > 0)
            {
                aggroTimer -= Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

            if (CastAttackCircle())
                Attack();

        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void CastAggroCircle()
    {
        hits = Cast(aggroRange);

        if (hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.GetComponent<PlayerController>() != null)
                {
                    target = hits[i].collider.GetComponent<PlayerController>();
                    break;
                }
            }

            
        }
    }

    private Vector3 CalculateTargetVector()
    {
        return target.transform.position - transform.position;
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
        rb.velocity = CalculateTargetVector().normalized * speed;
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
        Debug.Log("try attack");
    }
}
