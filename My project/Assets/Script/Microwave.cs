using UnityEngine;

public class MicroWave : MonoBehaviour
{
    [Header("Movimentação Vertical")]
    public float speed = 2f;
    public float verticalDistance = 2f;

    [Header("Detecção")]
    public float visionRadius = 5f;

    [Header("Disparo")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    [Header("Ajuste de Mira")]
    [Tooltip("Quanto abaixo do centro do player a fireball deve mirar")]
    public float aimYOffset = 0.5f;

    private Vector2 startPos;
    private Vector2 targetPos;
    private int direction = 1;       
    private float fireCooldown = 0f;
    private Transform player;
    private Collider2D shooterCollider;

    void Awake()
    {
        startPos       = transform.position;
        targetPos      = startPos + Vector2.up * verticalDistance;
        shooterCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else           Debug.LogError("MicroWave: não encontrou GameObject com tag 'Player'.");
    }

    void Update()
    {
        MoveVertical();
        HandleShooting();
    }

    void MoveVertical()
    {
        Vector2 destino = (direction == 1) ? targetPos : startPos;
        transform.position = Vector2.MoveTowards(transform.position, destino, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPos) < 0.01f) direction = -1;
        else if (Vector2.Distance(transform.position, startPos) < 0.01f) direction = 1;
    }

    void HandleShooting()
    {
        if (player == null) return;

        fireCooldown -= Time.deltaTime;
        float dist = Vector2.Distance(transform.position, player.position);
        if (fireCooldown <= 0f && dist <= visionRadius)
        {
            ShootAtPlayer();
            fireCooldown = fireRate;
        }
    }

    void ShootAtPlayer()
    {
        // 1) Calcula o centro do collider do player
        Vector2 aimPoint = player.position;
        Collider2D playerCol = player.GetComponent<Collider2D>();
        if (playerCol != null)
            aimPoint = playerCol.bounds.center;

        // 2) Aplica offset para mirar mais para baixo
        aimPoint += Vector2.down * aimYOffset;

        // 3) Normaliza direção
        Vector2 dir = (aimPoint - (Vector2)firePoint.position).normalized;

        // 4) Instancia a fireball
        GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        // 5) Ignora colisão com o mago
        Collider2D fbCol = fb.GetComponent<Collider2D>();
        if (fbCol != null && shooterCollider != null)
            Physics2D.IgnoreCollision(fbCol, shooterCollider);

        // 6) Dá velocidade
        Rigidbody2D rb = fb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            FireBall fbScript = fb.GetComponent<FireBall>();
            float fbSpeed = (fbScript != null) ? fbScript.speed : 5f;
            rb.velocity = dir * fbSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
