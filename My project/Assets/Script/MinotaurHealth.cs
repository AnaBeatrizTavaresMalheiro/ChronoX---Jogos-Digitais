using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinotaurHealth : MonoBehaviour {
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
            sr.sortingOrder     = 10;
            hearts.Add(sr);
        }
    }

    public void TakeDamage() {
        if(isDead || isInvulnerable) { // se já morreu ou esta invulneravel ignora
            return;
        }

        // entra em invulnerabilidade
        isInvulnerable = true;
        Invoke("EndInvulnerability", invulnDuration);

        vidas--; // menos uma vida
        hearts[vidas].enabled = false; // desativa

        if(vidas >= 1) { // quando so tiver recebido um ataque
            animator.SetTrigger("hurt"); // animcao de tomar dano
        }
        else { // quando for receber o segundo ataque
            isDead = true; // morreu
            animator.SetBool("death", true); // animacao de morrer
            var ia = GetComponent<Minotauro>();
            if(ia != null) {
                ia.enabled = false;
                ia.CancelInvoke(); // cancela todos os Invokes pendentes no Minotauro
            }

            Invoke("Die", dieDuration); // sumir do mapa depois de um X tempo
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
