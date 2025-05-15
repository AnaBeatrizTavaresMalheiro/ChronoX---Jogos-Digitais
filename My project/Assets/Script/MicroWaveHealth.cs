using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MicroWaveHealth : MonoBehaviour {

    private Animator animator; // pode fazer as animações
    private MicroWave microWaveMovement; // para fazer ele parar de movimentar depois

    [Header("Vida")]
    public int vidas = 2; // quantidade de vidas do knight
    private bool isDead = false; // saber se está morto ou não

    [Header("Invulnerabilidade")]
    public float invulnDuration = 0.5f; // tempo em que fica invulnerável após sofrer dano
    private bool isInvulnerable = false; // saber se está numa janela de invulnerabilidade

    [Header("Morte")]
    public float dieDuration; // duração da animação de morrer para o inimigo sumir depois

    private void Awake() { 
        animator = GetComponent<Animator>();
        microWaveMovement = GetComponent<MicroWave>();
    }

    void Start() {
           
    }

    void Update() {
        
    }

    public void TakeDamage() {
        if(isDead || isInvulnerable) { // se já morreu ignora
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

            if(microWaveMovement != null) {
                microWaveMovement.enabled = false; // desliga o script de movimento
            }

            var ia = GetComponent<Knight>(); // desabilita todo o Kniht, nenhuma função dele funciona mais -> Update, Attack etc
            if(ia != null) {
                ia.enabled = false;
                ia.CancelInvoke(); // cancela todos os Invokes pendentes no Knight
            }

            Invoke("Die", dieDuration); // sumir do mapa depois de um X tempo
        }

    }

    private void EndInvulnerability() {
        isInvulnerable = false; // pode voltar a tomar dano
    }

    private void Die() {
        Destroy(gameObject); // destruir o inimigo depois de morrer
    }

}
