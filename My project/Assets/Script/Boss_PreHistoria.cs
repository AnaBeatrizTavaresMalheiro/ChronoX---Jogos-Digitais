using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_PreHistoria : MonoBehaviour
{
    [Header("Movimentação")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 2f;
    public float stap = 4f;
    public float visionRange = 10f;

    [Header("Vida e Estado")]
    public int live_boss = 4;
    public bool IsDead = false;

    [Header("Ponto e Prefab")]
    public Transform headPoint;
    public BoxCollider2D boxCollider2D;
    public GameObject pecaPrefab;
    public Transform peca;

    [Header("Combate")]
    public float damageCooldown = 1.5f;
    public float stopDistance = 1.5f;

    private float leftLimit;
    private float rightLimit;
    private int direction = 1;
    private float lastDamageTime = -Mathf.Infinity;

    private Rigidbody2D rb2d;
    private Animator anim;
    private Transform player;
    private bool isReturningToPatrol = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        float startX = transform.position.x;
        leftLimit = startX - stap;
        rightLimit = startX + stap;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Jogador com tag 'Player' não encontrado!");
    }

    void Update()
    {
        if (IsDead) return;

        if (player != null && IsPlayerInSight())
        {
            isReturningToPatrol = false;
            FollowPlayer();
        }
        else if (isReturningToPatrol)
        {
            ReturnToPatrol();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        float newX = transform.position.x + direction * patrolSpeed * Time.deltaTime;

        if (direction == 1 && newX >= rightLimit)
        {
            ReverseDirection();
            newX = rightLimit;
        }
        else if (direction == -1 && newX <= leftLimit)
        {
            ReverseDirection();
            newX = leftLimit;
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void ReverseDirection()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    bool IsPlayerInSight()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= visionRange)
        {
            float directionToPlayer = player.position.x - transform.position.x;
            if ((directionToPlayer > 0 && transform.localScale.x > 0) ||
                (directionToPlayer < 0 && transform.localScale.x < 0))
            {
                return true;
            }
        }

        if (!isReturningToPatrol)
            isReturningToPatrol = true;

        return false;
    }

    void FollowPlayer()
    {
        if (transform.position.x > 95)
        {
            float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);
            if (distanceToPlayer > stopDistance)
            {
                float step = chaseSpeed * Time.deltaTime;
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }

            if (player.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                direction = 1;
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                direction = -1;
            }
        }
        else
        {
            ReturnToPatrol();
        }
    }

    void ReturnToPatrol()
    {
        float step = patrolSpeed * Time.deltaTime;
        float distanceToLeft = Mathf.Abs(transform.position.x - leftLimit);
        float distanceToRight = Mathf.Abs(transform.position.x - rightLimit);
        float targetX = (distanceToLeft < distanceToRight) ? leftLimit : rightLimit;

        Vector3 patrolTarget = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, step);

        if (targetX > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction = 1;
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction = -1;
        }

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
            isReturningToPatrol = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                ReverseDirection();
                PlayerHealth.Instance.TakeDamage();
                lastDamageTime = Time.time;
            }
        }
    }

    public void Live()
    {
        live_boss -= 1;
        Debug.Log("Boss HP: " + live_boss);

        if (live_boss == 0)
        {
            IsDead = true;
            anim.SetTrigger("raptor-dead_Clip");
            Instantiate(pecaPrefab, peca.position, Quaternion.identity);
        }

        Destroy(GameObject.FindGameObjectWithTag("HeartBoss_1"));
    }
}
