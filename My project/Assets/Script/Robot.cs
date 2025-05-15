using UnityEngine;

public class Robot : MonoBehaviour
{
    private Transform target;          // referência ao player
    private Animator animator;         // animator do robo
    private Collider2D shooterCollider;

    [Header("Movimento")]
    public float speed = 2f;
    public float visionRadius = 5f;

    [Header("Disparo")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float aimYOffset = 0.5f;

    [Header("Tempo")]
    public float attackAnimDuration = 0.3f; // tempo que dura a animação de ataque

    private float fireCooldown = 0f;

    void Awake()
    {
        shooterCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            target = p.transform;
        else
            Debug.LogError("RoboAtirador: Player com tag 'Player' não encontrado!");
    }

    void Update()
    {
        if (target == null)
        {
            animator.SetBool("walk", false);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= visionRadius)
        {
            // Perseguir o player
            FollowPlayer();

            // Tentar atirar
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                ShootAtPlayer();
                fireCooldown = fireRate;
                Invoke(nameof(EndAttackAnim), attackAnimDuration);
            }
        }
        else
        {
            // Não vê o player, para de andar e atacar
            animator.SetBool("walk", false);
        }

        // Virar para o player
        FaceTarget();
    }

    private void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        animator.SetBool("walk", true);
    }

    private void FaceTarget()
    {
        Vector3 scale = transform.localScale;
        if (target.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void ShootAtPlayer()
    {
        if (animator != null)
            animator.SetTrigger("attack");

        Vector2 aimPoint = target.position;

        Collider2D playerCol = target.GetComponent<Collider2D>();
        if (playerCol != null)
            aimPoint = playerCol.bounds.center;

        aimPoint += Vector2.down * aimYOffset;

        Vector2 direction = (aimPoint - (Vector2)firePoint.position).normalized;

        GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Projectile fbScript = fb.GetComponent<Projectile>();
        if (fbScript != null)
        {
            fbScript.SetVelocity(direction);
        }
        else
        {
            // fallback se não tiver Projectile script, aplica diretamente no Rigidbody
            Rigidbody2D rb = fb.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = direction * 5f;
}

    }

    private void EndAttackAnim() { // animação para quando acabar o ataque
        animator.CrossFade("robot_walk", 0f); // faz a transição imediata para estado "knight_idle"
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
