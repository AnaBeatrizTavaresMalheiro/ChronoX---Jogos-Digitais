using UnityEngine;

public class FireBallPlayer : MonoBehaviour {
    [Tooltip("Velocidade da bola de fogo")]
    public float speed = 5f;

    [Tooltip("Tempo de vida da bola antes de ser destruída")]
    public float lifeTime = 3f;

    [Tooltip("Tempo para destruir o objeto após a explosão começar")]
    public float explosionDuration = 0.5f;
    private Animator animator;
    private Rigidbody2D rb;

    void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col) {
        // Aqui você pode detectar colisão com o jogador ou cenário
        if (col.CompareTag("MicroWave")) {
            // Exemplo: colide com o player
            col.GetComponent<MicroWaveHealth>()?.TakeDamage();
            animator.SetTrigger("explode");
        }
        if (col.gameObject.layer == 6) {
            animator.SetTrigger("explode");
        }
        if (col.gameObject.tag == "LaserMicroWave") {
            animator.SetTrigger("explode");
        }
        if (col.gameObject.tag == "Robot")
        {
            animator.SetTrigger("explode");
            col.GetComponent<RobotHealth>()?.TakeDamage();
        }
        if (col.gameObject.tag == "Projectile") {
            animator.SetTrigger("explode");
        }
        // Destrói ao colidir em qualquer coisa
        rb.velocity = Vector2.zero;
        Destroy(gameObject, explosionDuration);
    }
}
