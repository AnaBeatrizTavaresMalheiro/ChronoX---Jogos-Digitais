using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Velocidade do projétil")]
    public float speed = 5f;

    [Tooltip("Tempo de vida do projétil antes de ser destruído")]
    public float lifeTime = 3f;

    [Tooltip("Tempo para destruir o objeto após a explosão começar")]
    public float explosionDuration = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isExploding = false; // Flag para evitar múltiplas explosões

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Destrói o projétil depois do tempo de vida
        Destroy(gameObject, lifeTime);
    }

    // Método para configurar a velocidade, deve ser chamado na instanciação
    public void SetVelocity(Vector2 direction)
    {
        if (rb != null)
            rb.velocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploding)
            return; // já está explodindo, ignora colisões adicionais

        // Verifica colisões que causam explosão
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("explode");
            collision.GetComponent<PlayerHealth>()?.TakeDamage();
            Explode();
        }
        else if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9 || collision.CompareTag("FireBall"))
        {
            animator.SetTrigger("explode");
            Explode();
        }
    }

    private void Explode()
    {
        isExploding = true;
        rb.velocity = Vector2.zero;
        Destroy(gameObject, explosionDuration);
    }
}
