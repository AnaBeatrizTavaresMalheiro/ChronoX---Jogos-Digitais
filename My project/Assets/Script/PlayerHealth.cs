using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    private Animator animator; // pode fazer as animações

    public float dieDuration = 0.8f; // saber o tempo de animação de morte
    public float invulnDuration = 0.6f; // tem de invulnerabilidade
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
        if(vidas >= 1) {
            animator.SetTrigger("hurt");
        }
        if (vidas <= 0) {
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

    private void EndInvulnerability() { // invulnerabilidade
        isInvulnerable = false; // pode levar dano de volta
    }

}
