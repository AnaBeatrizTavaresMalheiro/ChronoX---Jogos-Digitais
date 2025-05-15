using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {
    private SpriteRenderer sr; // para rotacionar o player quando vira de lado
    private Rigidbody2D rb2d; // me permite manipular qualquer variavel no rigidbody la do inspector
    private Animator animator; // poder animar o player

    [Header("Movimento")] // cria um cabeçalho no Inspector para variáveis de movimento
    public float speed = 5f; // velocidade personagem, public para poder mudar la no unity    public float JumpForce; // velocidade do pulo
    public float speedClimb = 4f; // velocidade para escalar a escada
    public float jump = 10f; // é a força do pulo do personagem

    [Header("Ataque")] // cria um cabeçalho no Inspector para variáveis de ataque
    public float attackOffset; // distância horizontal do ponto de ataque a partir do centro
    public float attackRadius; // raio para o ataque da espada
    public LayerMask Enemy; // saber a layer do knight e do minotauro para atacar ele -> juntar as duas no inspector

    [Header("Tempos")] // cabeçalho para variáveis de tempo
    public float attackHitDelay; // isso para o ataque sair antes de dar o dano
    public float attackAnimDuration; // tempo que dura a animação de ataque
    public float attackCooldown; // cooldown entre um ataque e outro

    // --- NOVO: configuração de fase ---
    [Header("Fase de Combate")] 
    [Tooltip("2 = espada, 3 = magia")]
    public int currentPhase = 2;

    // --- NOVO: campos para ataque mágico ---
    [Header("Ataque Mágico")]
    [Tooltip("Prefab da bola de fogo (deve ter Rigidbody2D)")]
    public GameObject fireballPrefab;
    [Tooltip("Ponto de onde a bola de fogo é instanciada")]
    public Transform firePoint;
    [Tooltip("Tempo entre cada disparo em segundos")]
    public float fireRate = 0.5f;
    private float fireCooldown = 0f;

    [Header("Ajuste de Mira")]
    [Tooltip("Quanto abaixo do cursor a fireball deve mirar")]
    public float aimYOffset = 0.5f;

    public bool isJumping; // saber se ele esta pulando ou nao
    public bool doubleJump; // saber se ele esta dando um pulo duplo ou nao
    public bool inStairs; // saber se o personagem esta em cima da escada
    private bool facingLeft = false; // saber qual lado ta
    private bool canAttack = true; // saber se o player pode atacar ou não
    private Collider2D playerCollider;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update() {
        Move(); // se mover
        Jump(); // pular
        Climb(); // escalar

        // decrementa cooldown de magia
        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;

        if(Input.GetMouseButtonDown(0)) { // se apertar o botão esquerdo do mouse
            if (currentPhase == 2) {
                Attack(); // ataca com espada
            }
            else if (currentPhase == 3) {
                TryMagicAttack(); // atira fireball
            }
        }
    }

    void Move() {
        // o GetAxis ja detecta a movimentacao e teclas, ta pronta na Unity
        float h = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(h, 0f, 0f); // recebe apenas movimentacao lateral (x) -> y e z ficam em 0
        transform.position += movement * Time.deltaTime * speed; // adiciona velocidade

        bool running = Mathf.Abs(h) > 0.01f; // saber se esta se movendo, true or false
        animator.SetBool("run", running);

        if (h > 0f) {
            facingLeft = false;
        }
        else if (h < 0f) {
            facingLeft = true;
        }
        sr.flipX = facingLeft;

    }

    void Jump() {
        if (Input.GetButtonDown("Jump")) { // ja eh pre setado como space
            if (isJumping == false) {
                rb2d.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse); // so muda o eixo Y (eixo X = 0f)
                animator.SetBool("jump", true);
                doubleJump = true;
            }
            else {
                if (doubleJump == true) {
                    rb2d.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
                    doubleJump = false; // assim nao pode pudar varias vezes
                }
            }
        }
    }

    void Climb() {
        if (inStairs) {
            Vector3 movement = new Vector3(0f, Input.GetAxis("Vertical"), 0f); // recebe apenas movimentacao lateral (x) - y e z ficam em 0
            transform.position += movement * Time.deltaTime * speedClimb; // adiciona velocidade
            animator.SetBool("climb", true);
        }
        else {
            animator.SetBool("climb", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9) { // 6 é o layer que eu criei para Ground e 9 para plataforma
            isJumping = false;
            animator.SetBool("jump", false);
        }
        if(collision.gameObject.tag == "Lava"){ // se ele cair na lava
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse); // para dar um pulinho depois de bater
            PlayerHealth.Instance.TakeDamage();
        }
        if(collision.gameObject.tag == "Espinho") { // se ele cair nos espinhos
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse); // para dar um pulinho depois de bater
            PlayerHealth.Instance.TakeDamage();
        }
        if(collision.gameObject.tag == "Saw") { // se tocar na serra
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse); // para dar um pulinho depois de bater
            PlayerHealth.Instance.TakeDamage();
        }
        if(collision.gameObject.tag == "Smash") { // se tocar no esmagador
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 15, ForceMode2D.Impulse); // para dar um empurrão para a esquerda
            PlayerHealth.Instance.TakeDamage();
        }
        if(collision.gameObject.tag == "Hole") { // se cair no buraco
            PlayerHealth.Instance.InstaKill();
        }
        if(collision.gameObject.tag == "LaserUp") { // se cair no laser que te empurra pra cima
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 15, ForceMode2D.Impulse); // para dar um empurrão para a esquerda
            PlayerHealth.Instance.TakeDamage();
        }
        if(collision.gameObject.tag == "LaserLeft") { // se cair no laser que te empurra pra esquerda
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 10, ForceMode2D.Impulse); // para dar um empurrão para a esquerda
            PlayerHealth.Instance.TakeDamage();
        }
        if(collision.gameObject.tag == "LaserRight") { // se cair no laser que te empurra pra direita
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10, ForceMode2D.Impulse); // para dar um empurrão para a esquerda
            PlayerHealth.Instance.TakeDamage();
        }
        if(collision.gameObject.tag == "Laser") { // se cair no laser que te empurra pra direita
            PlayerHealth.Instance.TakeDamage();
        }

    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9) {
            // garante que enquanto em contato com o chão isJumping fique false
            isJumping = false;
        }
    }

    // metodo para detectar sempre que o personagem deixar de tocar em alguma coisa
    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9) { // 6 é o layer que eu criei para Ground e 9 para plataforma
            isJumping = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) { // trigger eh quando o personagem pode passar por cima no objeto (se fosse uma bola nao teria trigger por exemplo)
        if(collider.gameObject.layer == 7) { // 7 é o layer criado para Escadas
            inStairs = true; // variavel na escada passa a ser verdadeiro
            rb2d.gravityScale = 0f; // deixo a gravidade como 0 para ele nao ficar descendo sozinho
        }
    }

    void OnTriggerExit2D(Collider2D collider) { // trigger eh quando o personagem pode passar por cima no objeto (se fosse uma bola nao teria trigger por exemplo)
        if(collider.gameObject.layer == 7) { // 7 é o layer criado para Escadas
            inStairs = false; // variavel na escada passa a ser falso
            rb2d.gravityScale = 3f; // deixo a gravidade como 3, que é a padrão do jogo
        }
    }

    private Vector2 GetAttackPosition() {
        if(facingLeft) { // se estiver virado para a esquerda
            return (Vector2)transform.position + Vector2.left  * attackOffset; // retorna posição de ataque em mundo
        }
        else { // se estiver virado para a direita
            return (Vector2)transform.position + Vector2.right * attackOffset; // retorna posição de ataque em mundo
        }
    }

    private void Attack() {
        if(!canAttack) { // se estiver atacando 
            return; // retorna
        }
        canAttack = false;
        animator.SetTrigger("attack");

        Invoke("PerformAttackHit", attackHitDelay);
        Invoke("EndAttackAnim", attackAnimDuration);
        Invoke("ResetCanAttack", attackCooldown);
    }

    private void TryMagicAttack() { // aqui ele realiza o ataque se não estiver no cooldown
        if (fireCooldown > 0f) return;
        ShootFireball();
        animator.SetTrigger("attack");
        fireCooldown = fireRate;
        Invoke("EndAttackAnim", attackAnimDuration);
    }

    private void ShootFireball() { // mecanica para atirar a bola de fogo
        // converte posição do mouse para world point
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld  = Camera.main.ScreenToWorldPoint(mouseScreen);
        // aplica offset para mirar um pouco abaixo
        Vector2 aimPoint = (Vector2)mouseWorld + Vector2.down * aimYOffset;
        Vector2 aimDir = (aimPoint - (Vector2)firePoint.position).normalized;

        // instancia a fireball
        GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        // ignora colisão com o player
        Collider2D fbCol = fb.GetComponent<Collider2D>();
        if (fbCol != null && playerCollider != null)
            Physics2D.IgnoreCollision(fbCol, playerCollider);

        // aplica velocidade (Rigidbody2D deve ser Dynamic)
        Rigidbody2D rb = fb.GetComponent<Rigidbody2D>();
        if (rb != null) {
            FireBallPlayer fbScript = fb.GetComponent<FireBallPlayer>();
            float speed = fbScript != null ? fbScript.speed : 5f;
            rb.velocity = aimDir * speed;
        }
    }


    private void EndAttackAnim() { // chamar a animação de idle depois do ataque
        animator.CrossFade("player_idle", 0f); // faz a transição imediata para estado "player_idle"
    }

    private void PerformAttackHit() { // realizar o ataque
        Collider2D[] player = Physics2D.OverlapCircleAll(GetAttackPosition(), attackRadius, Enemy); // procura com a layer Knight e Minotaur
        foreach (Collider2D playerGameObject in player) {
            // se for um Knight, aplica dano nele
            var each_knight = playerGameObject.GetComponent<KnightHealth>(); // cada knight é próprio, tem suas próprias vidas e tomam seu próprio dano
            if(each_knight != null) {
                each_knight.TakeDamage(); // da dano
            }

            // se for um Minotaur, aplica dano nele
            var mh = playerGameObject.GetComponent<MinotaurHealth>();
            if (mh != null) {
                mh.TakeDamage(); // da dano
            }
        }
    }

    private void ResetCanAttack() { // retar o ataque do player
        canAttack = true; // agora pode atacar dnv
    }

    private void OnDrawGizmosSelected() { // desenha o circulo de ataque para poder ver dentro da unity
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRadius);
    }

}
