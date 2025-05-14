using UnityEngine;

public class MicroWave : MonoBehaviour
{
    [Header("Movimentação")]
    [Tooltip("Velocidade de descida do mago")]
    public float speed = 2f;

    [Header("Detecção")]
    [Tooltip("Raio em que o mago consegue 'ver' o player")]
    public float visionRadius = 5f;

    [Header("Disparo")]
    [Tooltip("Prefab da bola de fogo (deve ter Rigidbody2D)")]
    public GameObject fireballPrefab;
    [Tooltip("Ponto de onde a bola de fogo é instanciada")]
    public Transform firePoint;
    [Tooltip("Tempo entre cada disparo em segundos")]
    public float fireRate = 1f;

    private float fireCooldown = 0f;
    private Transform player;

    void Start()
    {
        // Assume um único objeto com tag "Player" na cena
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("WizardEnemy: não encontrou GameObject com tag 'Player' na cena.");
    }

    void Update()
    {
        // 1) Move sempre para baixo
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // 2) Atualiza cooldown de disparo
        fireCooldown -= Time.deltaTime;

        // 3) Se o player existir e estiver dentro do raio de visão, atira
        if (player != null && fireCooldown <= 0f)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= visionRadius)
            {
                ShootAt(player.position);
                fireCooldown = fireRate;
            }
        }
    }

    void ShootAt(Vector2 targetPosition)
    {
        // Calcula direção normalizada do firePoint até o player
        Vector2 dir = (targetPosition - (Vector2)firePoint.position).normalized;

        // Instancia bola de fogo sem rotação
        GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        // Ajusta velocidade do Rigidbody2D da bola
        Rigidbody2D rb = fb.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = dir * fb.GetComponent<FireBall>().speed; 
        else
            Debug.LogWarning("WizardEnemy: prefab de fireball não tem Rigidbody2D.");
    }

    // Desenha gizmo do raio de visão no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
