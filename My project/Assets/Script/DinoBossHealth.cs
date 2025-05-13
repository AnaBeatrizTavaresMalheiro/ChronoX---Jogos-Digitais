// DinossauroHealth.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinossauroHealth : MonoBehaviour {
    private Animator animator;

    [Header("Vida")]
    public int maxVidas = 5;
    private int vidas;
    private bool isDead = false;

    [Header("Invulnerabilidade")]
    public float invulnDuration = 0.5f;
    private bool isInvulnerable = false;

    [Header("Morte")]
    public float dieDuration = 1f;
    [Header("Objeto ao morrer")]
    public GameObject timeMachine; // peça da máquina do tempo

    [Header("Coração 2D (SpriteRenderer)")]
    public Sprite heartSprite;
    [Tooltip("Escala aplicada a cada coração")]
    public float heartScale   = 0.3f;
    public float heartSpacing = 0.4f;
    public float heartYOffset = 1.8f;

    private List<SpriteRenderer> hearts = new List<SpriteRenderer>();

    private void Awake() {
        animator = GetComponent<Animator>();
        vidas = maxVidas;
        CreateHearts();
    }

    private void CreateHearts() {
        float totalWidth = (maxVidas - 1) * heartSpacing;
        Vector3 origin = new Vector3(-totalWidth * 0.5f, heartYOffset, 0f);

        for (int i = 0; i < maxVidas; i++) {
            GameObject go = new GameObject("Heart" + i);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = origin + Vector3.right * (i * heartSpacing);
            go.transform.localScale = Vector3.one * heartScale;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = heartSprite;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 10;
            hearts.Add(sr);
        }
    }

    public void TakeDamage() {

        if (isDead || isInvulnerable) {
            return;
        }

        // janela de invulnerabilidade
        isInvulnerable = true;
        Invoke(nameof(EndInvulnerability), invulnDuration);

        // decrementa vida
        vidas--;
        if (vidas >= 0 && vidas < hearts.Count) {
            hearts[vidas].enabled = false;
        }

        // stun no ataque do boss
        var bossAI = GetComponent<DinoBoss>();
        if (bossAI != null)
            bossAI.StunAttack(invulnDuration);

        if (vidas >= 1) {
            animator.SetTrigger("hurt");
        }
        else {
            StartCoroutine(HandleDeath());
        }
    }

    private void EndInvulnerability() {
        isInvulnerable = false;
    }

    private IEnumerator HandleDeath() {
        isDead = true;
        animator.SetBool("death", true);

        // desativa AI do boss para não continuar movendo ou atacando
        var bossAI = GetComponent<DinoBoss>();
        if (bossAI != null) {
            bossAI.enabled = false;
            bossAI.CancelInvoke(); 
        }

        // espera a animação de morte
        yield return new WaitForSeconds(dieDuration);

        // ativa máquina do tempo, se houver
        if (timeMachine != null)
            timeMachine.SetActive(true);

        Destroy(gameObject);
    }
}
