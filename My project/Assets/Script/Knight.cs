using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour {
        private Transform target; // variável para saber quem o inimigo vai perseguir
    private Animator animator; // pode fazer as animações

    [Header("Movimento")] // cabeçalho no Inspector para variáveis de movimento
    public float speed; // velocidade do inimigo
    public float visionRadius; // raio de visão para o inimigo ver o player

    [Header("Ataque")] // cabeçalho para variáveis de ataque
    public float attackOffset; // distância horizontal do ponto de ataque a partir do centro
    public float attackRadius; // raio para o ataque do minotauro
    public LayerMask Player; // saber a layer do player para atacar ele

    [Header("Tempos")] // cabeçalho para variáveis de tempo
    public float attackHitDelay; // delay antes do hit do ataque
    public float attackAnimDuration; // tempo de duração da animação de ataque
    public float attackCooldown; // cooldown entre ataques

    private bool canAttack = true; // controlar se pode atacar novamente

    void Start() {
        animator = GetComponent<Animator>();
        target   = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // targetar o player para perseguir
    }

    void Update() {
        LookPlayer();

        if (target != null) { // se tiver um alvo
            FaceTarget();

            Collider2D[] hits = Physics2D.OverlapCircleAll(GetAttackPosition(), attackRadius, Player); // colisão com o Player
            if (hits.Length > 0) { // se colidir, ataca
                Attack();
            } else {
                FollowPlayer(); // se não colidir, segue o player
            }
        } else { // sem alvo
            StopMoving(); // para de se mover
        }
    }

    private void LookPlayer() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionRadius); // pegar todos colisores na cena
        target = null; // resetar target
        foreach (var hit in hits) {
            if (hit.CompareTag("Player")) { // se achar o Player
                target = hit.transform; // seta o target
                break; // sai do loop
            }
        }
    }

    private void FaceTarget() { // vira para o player
        Vector3 scale = transform.localScale;
        if (target.position.x > transform.position.x) {
            scale.x = Mathf.Abs(scale.x); // fica normal
        } else {
            scale.x = -Mathf.Abs(scale.x); // inverte no eixo X
        }
        transform.localScale = scale;
    }

    private Vector2 GetAttackPosition() {
        float dir = Mathf.Sign(transform.localScale.x); // 1 direita, -1 esquerda
        return (Vector2)transform.position + Vector2.right * attackOffset * dir; // posição de ataque
    }

    private void FollowPlayer() { // segue o player
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        animator.SetBool("walk", true); // animação de andar
    }

    private void StopMoving() { // para de andar
        animator.SetBool("walk", false);
    }

    private void Attack() {
        if (!canAttack) return; // se não pode atacar, sai
        canAttack = false;

        animator.SetBool("walk", false);
        animator.SetTrigger("attack"); // dispara animação de ataque

        Invoke("PerformAttackHit", attackHitDelay); // delay antes do hit
        Invoke("EndAttackAnim", attackAnimDuration); // fim da animação
        Invoke("ResetCanAttack", attackCooldown); // resetar cooldown
    }

    private void PerformAttackHit() { // aplica o dano
        Collider2D[] hits = Physics2D.OverlapCircleAll(GetAttackPosition(), attackRadius, Player);
        foreach (var hit in hits) {
            PlayerHealth.Instance.TakeDamage(); // dá dano ao player
        }
    }

    private void EndAttackAnim() { // ao terminar a animação
        animator.CrossFade("minotauro_idle", 0f); // volta para idle
    }

    private void ResetCanAttack() { // liberar novo ataque
        canAttack = true;
    }

    private void OnDrawGizmosSelected() { // desenhar gizmos no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius); // raio de visão
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRadius); // raio de ataque
    }

}
