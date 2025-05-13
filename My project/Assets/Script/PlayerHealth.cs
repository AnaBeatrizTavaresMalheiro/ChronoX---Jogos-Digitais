using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    private Animator animator; // pode fazer as animações

    public float dieDuration = 0.8f; // saber o tempo de animação de morte
    public float invulnDuration = 0.6f; // tem de invulnerabilidade
    public float hurtDuration    = 0.5f; // duração exata do seu clip "hurt"

    public bool isInvulnerable = false; // saber se esta invulneravel ou nao
    private bool isDead = false; // saber se morreu ou não

    [Header ("Vidas")]
    public int vidas = 3;
    public Image[] iconesVidas; // arrays de imagens de vidas

    private void Awake() { // garantir que so vai ter uma instancia
        animator = GetComponent<Animator>();

        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void TakeDamage() {
        if(isDead || isInvulnerable) { // se tiver morto ou invulneravel nem entra aqui
            return;
        }
        isInvulnerable = true;

        vidas--; // perde uma vida

        if (vidas >= 0 && vidas < iconesVidas.Length && iconesVidas[vidas] != null) {
            iconesVidas[vidas].enabled = false; // desativa um icone de vida

        }
        if(vidas >= 1) { // levar dano quendo ainda tier 3 ou 2 coracoes
            animator.SetTrigger("hurt");
            Invoke(nameof(EndHurtAnim), hurtDuration);
        }
        if (vidas <= 0) { // quando so tiver 1 coracao, apenas a animacao de morrer
            isDead = true; // morreu
            animator.SetBool("death", true);
            Invoke("Die", dieDuration);
        }

        Invoke("EndInvulnerability", invulnDuration);
    }

    void Die() {
        PlayerPrefs.SetString("Fase", SceneManager.GetActiveScene().name); // pega o nome da cena atual e coloca na variavel Fase
        SceneManager.LoadScene("GameOver"); // chama o GameOver
    }

    private void EndHurtAnim() {
        // volta para idle (ou outro estado padrão) quando o hurt terminar
        animator.CrossFade("player_idle", 0f);
    }

    private void EndInvulnerability() { // invulnerabilidade
        isInvulnerable = false; // pode levar dano de volta
    }

    public void InstaKill() {
        if(isDead) { // se ja tiver morto retorna
            return;
        }
        
        isDead = true;
        isInvulnerable = true;

        foreach(var icon in iconesVidas) { // tira todas as vidas da cena
            if(icon) {
                icon.enabled = false;  
            } 
        }

        animator.SetBool("death", true); // animacao de morte
        Invoke(nameof(Die), dieDuration);
    }

}
