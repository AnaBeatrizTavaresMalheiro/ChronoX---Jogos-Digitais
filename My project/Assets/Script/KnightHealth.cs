using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightHealth : MonoBehaviour {

    private Animator animator; // pode fazer as animações
    public int vidas = 2; // quantidade de vidas do knight
    public float dieDuration; // duração da animação de morrer para o inimigo sumir depois
    private bool isDead = false; // saber se está morto ou não

    private void Awake() { 
        animator = GetComponent<Animator>();
    }

    void Start() {
           
    }

    void Update() {
        
    }

    public void TakeDamage() {
        if(isDead) { // se já morreu ignora
            return;
        }

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

    private void Die() {
        Destroy(gameObject); // destruir o inimigo depois de morrer
    }

}
