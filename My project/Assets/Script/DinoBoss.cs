// DinoBoss.cs
using System.Collections;
using UnityEngine;

public class DinoBoss : MonoBehaviour {
    private Transform target;      // quem o dino vai perseguir
    private Animator animator;     // para tocar as animações

    [Header("Movimento")]
    public float speed = 2f;              // velocidade do dino
    public float visionRadius = 5f;       // raio de “visão” para seguir o Player

    [Header("Ataque")]
    public float attackCooldown = 1f;     // intervalo mínimo entre ataques
    private bool canAttack = true;        // controla se pode atacar de novo

    // Coroutine atual de stun (para prevenir ataques)
    private Coroutine attackStunCoroutine;

    void Start() {
        animator = GetComponent<Animator>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) target = go.transform;
    }

    void Update() {
        LookPlayer();

        if (target != null) {
            FaceTarget();
            FollowPlayer();
        }
        else {
            animator.SetBool("walk", false);
        }
    }

    private void LookPlayer() {
        // detecta se o Player está dentro do raio de visão
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionRadius);
        target = null;
        foreach (var c in hits) {
            if (c.CompareTag("Player")) {
                target = c.transform;
                break;
            }
        }
    }

    private void FaceTarget() {
        // vira para encarar o Player
        Vector3 s = transform.localScale;
        s.x = target.position.x > transform.position.x
            ? Mathf.Abs(s.x)
            : -Mathf.Abs(s.x);
        transform.localScale = s;
    }

    private void FollowPlayer() {
        // anda em direção ao Player e toca animação de walk
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
        animator.SetBool("walk", true);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // só ataca se bater no Player e se o cooldown já tiver passado
        if (!canAttack) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        canAttack = false;
        animator.SetBool("walk", false);
        animator.SetTrigger("attack");             // dispara animação de ataque

        // dá dano imediato ao Player
        PlayerHealth.Instance.TakeDamage();

        // libera o próximo ataque após o cooldown
        Invoke(nameof(ResetCanAttack), attackCooldown);
    }

    private void ResetCanAttack() {
        canAttack = true;
    }

    /// <summary>
    /// Desativa o ataque por 'duration' segundos (stun).
    /// </summary>
    public void StunAttack(float duration) {
        if (attackStunCoroutine != null)
            StopCoroutine(attackStunCoroutine);
        attackStunCoroutine = StartCoroutine(StunAttackCoroutine(duration));
    }

    private IEnumerator StunAttackCoroutine(float duration) {
        canAttack = false;
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    private void OnDrawGizmosSelected() {
        // mostra no Scene View o raio de visão
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
