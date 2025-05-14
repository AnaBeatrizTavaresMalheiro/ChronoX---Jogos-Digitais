using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Tooltip("Velocidade da bola de fogo")]
    public float speed = 5f;

    [Tooltip("Tempo de vida da bola antes de ser destruída")]
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Aqui você pode detectar colisão com o jogador ou cenário
        if (col.CompareTag("Player"))
        {
            // Exemplo: colide com o player
            // col.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
        // Destrói ao colidir em qualquer coisa
        Destroy(gameObject);
    }
}
