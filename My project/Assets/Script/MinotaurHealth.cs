using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurHealth : MonoBehaviour {
    private Animator animator; // componente para animações

    [Header("Vida")]
    public int vidas = 5; // quantidade de vidas do minotauro
    private bool isDead = false; // flag para saber se o minotauro já morreu

    [Header("Invulnerabilidade")]
    public float invulnDuration = 0.5f; // tempo em que fica invulnerável após sofrer dano
    private bool isInvulnerable = false; // saber se está numa janela de invulnerabilidade
    
    [Header("Morte")]
    public float dieDuration; // duração da animação de morte antes de destruir


    private void Awake() { // executa ao instanciar o objeto
   
        animator = GetComponent<Animator>(); // obtém o Animator anexado
    }

    void Start() {

    }

    void Update() {

    }

    public void TakeDamage() {
        if(isDead || isInvulnerable) { // se já morreu ou esta invulneravel ignora
            return;
        }

        // entra em invulnerabilidade
        isInvulnerable = true;
        Invoke("EndInvulnerability", invulnDuration);

        vidas--; // menos uma vida

        if(vidas >= 1) { // quando so tiver recebido um ataque
            animator.SetTrigger("hurt"); // animcao de tomar dano
        }
        else { // quando for receber o segundo ataque
            isDead = true; // morreu
            animator.SetBool("death", true); // animacao de morrer
            var ia = GetComponent<Knight>();
            if(ia != null) {
                ia.enabled = false;
            }

            Invoke("Die", dieDuration); // sumir do mapa depois de um X tempo
        }

    }

    private void EndInvulnerability() {
        isInvulnerable = false; // pode voltar a tomar dano
    }

    private void Die() { // executa a destruição do objeto
    
        Destroy(gameObject); // remove o minotauro da cena
    }
}
