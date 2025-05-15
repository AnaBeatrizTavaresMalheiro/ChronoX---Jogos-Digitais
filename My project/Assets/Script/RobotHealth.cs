using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotHealth : MonoBehaviour {
    private Animator animator; // componente para animações

    [Header("Vida")]
    public int maxVidas = 5; // vida total
    private int vidas; // quantidade de vidas do minotauro atual
    private bool isDead = false; // flag para saber se o minotauro já morreu

    [Header("Invulnerabilidade")]
    public float invulnDuration = 0.5f; // tempo em que fica invulnerável após sofrer dano
    private bool isInvulnerable = false; // saber se está numa janela de invulnerabilidade
    
    [Header("Morte")]
    public float dieDuration; // duração da animação de morte antes de destruir

    [Header("Objeto ao morrer")]
    public GameObject timeMachine; // peça da máquina do tempo

    [Header("Coração 2D (SpriteRenderer)")]
    public Sprite heartSprite;
    [Tooltip("Escala aplicada a cada coração")]
    public float heartScale = 0.3f;  
    public float heartSpacing = 0.4f;
    public float heartYOffset  = 1.8f;

    private List<SpriteRenderer> hearts = new List<SpriteRenderer>();


    private void Awake() { // executa ao instanciar o objeto
        animator = GetComponent<Animator>(); // obtém o Animator anexado
        vidas = maxVidas;
        CreateHearts();
    }

    void Start() {

    }

    void Update() {

    }

    private void CreateHearts() {
        float totalWidth = (maxVidas - 1) * heartSpacing;
        Vector3 origin = new Vector3(-totalWidth * 0.5f, heartYOffset, 0f);

        for (int i = 0; i < maxVidas; i++) {
            GameObject go = new GameObject("Heart" + i);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = origin + Vector3.right * (i * heartSpacing);

            // aqui definimos a escala
            go.transform.localScale = Vector3.one * heartScale;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = heartSprite;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 10;
            hearts.Add(sr);
        }
    }

    public void TakeDamage() {
        if(isDead || isInvulnerable) { 
            return;
        }

        isInvulnerable = true;
        Invoke("EndInvulnerability", invulnDuration);

        vidas--; 
        hearts[vidas].enabled = false;

        if(vidas >= 1) {
            // animator.SetTrigger("hurt");
        }
        else {
            isDead = true;

            // Para o Animator rodar só a animação de morte
            animator.CrossFade("robot_death", 0f);

            // Parar movimentação e ataque:
            var ia = GetComponent<Minotauro>();
            if(ia != null) {
                ia.enabled = false; // desliga o script do inimigo
                ia.CancelInvoke();
            }

            // Caso tenha Rigidbody2D ou CharacterController, para eles também:
            var rb2d = GetComponent<Rigidbody2D>();
            if(rb2d != null) {
                rb2d.velocity = Vector2.zero;
                rb2d.isKinematic = true; // impede física e movimento
            }

            // Se tiver scripts de ataque/atirando, desabilite-os também
            var shooter = GetComponent<Robot>(); 
            if(shooter != null) shooter.enabled = false;

            // Invoca o método de morrer depois do tempo da animação
            Invoke("Die", dieDuration);
        }
    }


    private void EndInvulnerability() {
        isInvulnerable = false; // pode voltar a tomar dano
    }

    private void Die() { // executa a destruição do objeto
        if(timeMachine != null) {
            timeMachine.SetActive(true); // faço ela aparecer
        }
    
        Destroy(gameObject); // remove o minotauro da cena
    }
}
