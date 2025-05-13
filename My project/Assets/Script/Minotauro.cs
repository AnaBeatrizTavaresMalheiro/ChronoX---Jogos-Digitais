using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotauro : MonoBehaviour {
    private Transform target; // variável para saber quem o inimigo vai perseguir
    private Animator animator; // pode fazer as animações

    [Header("Movimento")] // cria um cabeçalho no Inspector para variáveis de movimento
    public float speed; // velocidade do inimigo
    public float visionRadius; // radio de visão para o inimigo ver o player

    [Header("Ataque")] // cria um cabeçalho no Inspector para variáveis de ataque
    public float attackOffset; // distância horizontal do ponto de ataque a partir do centro
    public float attackRadius; // raio para o ataque da espada
    public LayerMask Player; // saber a layer do player para atacar ele

    [Header("Tempos")] // cabeçalho para variáveis de tempo
    public float attackHitDelay; // isso para o ataque sair antes de dar o dano
    public float attackAnimDuration; // tempo que dura a animação de ataque
    public float attackCooldown; // cooldown entre um ataque e outro

    private bool canAttack = true; // saber se ele pode atacar novamente ou não
    void Start() {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // targetar o player para ele seguir
        
    }

    void Update() {
        LookPlayer();

        if(target != null) { // se tiver um alvo
            FaceTarget();

            Collider2D[] hits = Physics2D.OverlapCircleAll(GetAttackPosition(), attackRadius, Player); // vai fazer pela colisão com o Player
            if(hits.Length > 0) { // se colidir com o Player (o attackRadius)
                Attack(); // ataca
            }
            else { // se não colidir
                FollowPlayer(); // segue o player
            }
        }
        else { // se não tiver um alvo
            StopMoving(); // para de se mover
        }
    }

    private void FaceTarget() { // mudar a escala quando virar de lado
        Vector3 scale = transform.localScale; // pegar a posição
        if(target.position.x > transform.position.x) { // se o player estiver na esquerda
            scale.x = Mathf.Abs(scale.x); // fica normal
        }
        else { // se o player estiver na direita
            scale.x = -Mathf.Abs(scale.x); // inverte o lado
        }
        transform.localScale = scale;
    }

    private Vector2 GetAttackPosition() {
        float dir = Mathf.Sign(transform.localScale.x); // obtém direção: 1 para direita, -1 para esquerda
        return (Vector2)transform.position + Vector2.right * attackOffset * dir; // retorna posição de ataque em mundo
    }

    private void StopMoving() { // função para ele ficar parado quando não ver o player
        // fica parado, não faz nada
        animator.SetBool("walk", false);
    }

    private void LookPlayer() { // função para procurar o player
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionRadius); // pega todos os colisores presentes na cena -> player, ground etc
        target = null; // seta como null

        foreach (var hit in hits) { // percorre todos os colisores
            if (hit.CompareTag("Player")) { // se um dos colisores for player
                target = hit.transform; // seta o targer para o player
                break; // não precisa mais percorrer a lista
            }
        }
    }

    private void Attack() {
        if(!canAttack) {
            return; // se estiver atacando retorna
        }
        canAttack = false; // não pode mais atacar, está em cooldown

        //animator.SetBool("walk", false);
        //animator.SetTrigger("attack"); // realiza a animação de ataque

        Invoke("PerformAttackHit", attackHitDelay); // espera um pouco para atacar com o attackHitDelay
        Invoke("EndAttackAnim", attackAnimDuration);
        Invoke("ResetCanAttack", attackCooldown);
    }

    private void EndAttackAnim() { // animação para quando acabar o ataque
        animator.CrossFade("minotauro_walk", 0f); // faz a transição imediata para estado "knight_idle"
    }

    private void PerformAttackHit() { // função para atacar o player e dar dano nele
        Collider2D[] player = Physics2D.OverlapCircleAll(GetAttackPosition(), attackRadius, Player);
        foreach (Collider2D playerGameObject in player) {
            PlayerHealth.Instance.TakeDamage();
        }
    }

    private void ResetCanAttack() { // após a cooldown de ataque
        canAttack = true; // e pode atacar novamente
    }

    private void OnDrawGizmosSelected() { // apenas representação visual desse raio na unity para testar
        Gizmos.color = Color.yellow; // define a cor como amarelo para identificação
        Gizmos.DrawWireSphere(transform.position, visionRadius); // raio de visão

        Gizmos.color = Color.red; // define a cor como vermelha
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRadius); // raio de ataque
    }
    
    private void FollowPlayer() { // função para seguir a posição do jogador
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // atualizar sua posição para seguir o player
        animator.SetBool("walk", true);
    }

}
