using System.Collections; // sistema de coleções genéricas
using System.Collections.Generic; // sistema de coleções genéricas avançadas
using UnityEngine; // classes principais do Unity Engine
using UnityEngine.SceneManagement; // gerenciamento de cenas

public class Boss_PreHistoria : MonoBehaviour // define a classe do boss pré-história
{ // início da classe
    [Header("Movimentação")] // cabeçalho para agrupar variáveis de movimentação
    public float patrolSpeed = 2f; // velocidade de patrulha do boss
    public float chaseSpeed = 2f; // velocidade de perseguição ao player
    public float stap = 4f; // alcance horizontal da patrulha do ponto inicial
    public float visionRange = 10f; // distância máxima para detectar o player

    [Header("Vida e Estado")] // cabeçalho para agrupar variáveis de vida e estado
    public int live_boss = 4; // quantidade de vidas do boss
    public bool IsDead = false; // flag que indica se o boss está morto

    [Header("Ponto e Prefab")] // cabeçalho para ponto de spawn e prefab de item
    public Transform headPoint; // ponto de referência na cabeça (não usado neste script)
    public BoxCollider2D boxCollider2D; // collider do boss (não usado diretamente)
    public GameObject pecaPrefab; // prefab da peça que o boss solta ao morrer
    public Transform peca; // ponto de spawn da peça

    [Header("Combate")] // cabeçalho para variáveis de combate
    public float damageCooldown = 1.5f; // tempo mínimo entre danos aplicados ao player
    public float stopDistance = 1.5f; // distância mínima para parar de perseguir o player

    private float leftLimit; // limite esquerdo calculado da área de patrulha
    private float rightLimit; // limite direito calculado da área de patrulha
    private int direction = 1; // direção atual: 1 para direita, -1 para esquerda
    private float lastDamageTime = -Mathf.Infinity; // momento do último dano ao player

    private Rigidbody2D rb2d; // referência ao componente Rigidbody2D
    private Animator anim; // referência ao componente Animator
    private Transform player; // referência ao transform do player
    private bool isReturningToPatrol = false; // flag para retornar à patrulha quando perde o player

    void Start() // executa ao iniciar o jogo
    {
        rb2d = GetComponent<Rigidbody2D>(); // obtém o Rigidbody2D anexado
        anim = GetComponent<Animator>(); // obtém o Animator anexado

        float startX = transform.position.x; // armazena a posição X inicial
        leftLimit = startX - stap; // calcula o limite esquerdo da patrulha
        rightLimit = startX + stap; // calcula o limite direito da patrulha

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // busca o objeto do player pela tag
        if (playerObj != null) // se encontrou o player
            player = playerObj.transform; // armazena a referência ao transform do player
        else // se não encontrou o player
            Debug.LogWarning("jogador com tag 'player' não encontrado!"); // exibe alerta no console
    }

    void Update() // chamado a cada frame
    {
        if (IsDead) return; // se o boss estiver morto, interrompe lógica

        if (player != null && IsPlayerInSight()) // se o player existe e está à vista
        { // segue o player
            isReturningToPatrol = false; // cancela retorno à patrulha
            FollowPlayer(); // chama a lógica de seguir o player
        }
        else if (isReturningToPatrol) // se deve retornar à patrulha
        { // retorna
            ReturnToPatrol(); // chama a lógica de retornar à patrulha
        }
        else // caso contrário
        { // patrulha normalmente
            Patrol(); // chama a lógica de patrulha
        }
    }

    void Patrol() // lógica de patrulha horizontal
    {
        float newX = transform.position.x + direction * patrolSpeed * Time.deltaTime; // calcula nova posição X

        if (direction == 1 && newX >= rightLimit) // se indo para direita e alcançou o limite direito
        { // inverte direção
            ReverseDirection(); // inverte direção e escala
            newX = rightLimit; // corrige posição para o limite
        }
        else if (direction == -1 && newX <= leftLimit) // se indo para esquerda e alcançou o limite esquerdo
        { // inverte direção
            ReverseDirection(); // inverte direção e escala
            newX = leftLimit; // corrige posição para o limite
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z); // aplica nova posição ao boss
    }

    void ReverseDirection() // inverte direção e flipa o sprite
    {
        direction *= -1; // multiplica direção por -1 para inverter
        Vector3 scale = transform.localScale; // obtém escala atual
        scale.x *= -1; // inverte eixo X para flipar sprite
        transform.localScale = scale; // aplica nova escala
    }

    bool IsPlayerInSight() // verifica se o player está à vista
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position); // distância até o player

        if (distanceToPlayer <= visionRange) // se dentro do alcance de visão
        { // verifica alinhamento de direção
            float directionToPlayer = player.position.x - transform.position.x; // diferença de X
            if ((directionToPlayer > 0 && transform.localScale.x > 0) || // player à direita, boss virado pra direita
                (directionToPlayer < 0 && transform.localScale.x < 0)) // player à esquerda, boss virado pra esquerda
            { // retorna verdadeiro
                return true; // player à vista e alinhado
            }
        }

        if (!isReturningToPatrol) // se ainda não iniciou retorno
            isReturningToPatrol = true; // marca retorno à patrulha

        return false; // player não está à vista
    }

    void FollowPlayer() // lógica de perseguição ao player
    {
        if (transform.position.x > 95) // só persegue se estiver além de x = 95
        { // calcula distância horizontal
            float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x); // distância absoluta
            if (distanceToPlayer > stopDistance) // se acima da distância de parada
            { // move em direção ao player
                float step = chaseSpeed * Time.deltaTime; // passo de movimento
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z); // destino personalizado
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step); // move o boss
            }

            if (player.position.x > transform.position.x) // se o player está à direita
            { // ajusta escala para olhar pra direita
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                direction = 1; // determina direção pra direita
            }
            else // se o player está à esquerda
            { // ajusta escala para olhar pra esquerda
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                direction = -1; // determina direção pra esquerda
            }
        }
        else // caso esteja antes de x = 95
        {
            ReturnToPatrol(); // chama rotina de retorno
        }
    }

    void ReturnToPatrol() // lógica de retorno à patrulha
    {
        float step = patrolSpeed * Time.deltaTime; // passo de movimento de patrulha
        float distanceToLeft = Mathf.Abs(transform.position.x - leftLimit); // distância até limite esquerdo
        float distanceToRight = Mathf.Abs(transform.position.x - rightLimit); // distância até limite direito
        float targetX = (distanceToLeft < distanceToRight) ? leftLimit : rightLimit; // escolhe limite mais próximo

        Vector3 patrolTarget = new Vector3(targetX, transform.position.y, transform.position.z); // posição alvo
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, step); // move o boss

        if (targetX > transform.position.x) // se o alvo estiver à direita
        { // ajusta escala pra direita
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction = 1; // direção pra direita
        }
        else // se o alvo estiver à esquerda
        { // ajusta escala pra esquerda
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction = -1; // direção pra esquerda
        }

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f) // se chegou próximo o suficiente
            isReturningToPatrol = false; // cancela retorno
    }

    void OnCollisionEnter2D(Collision2D collision) // tratador de colisão
    {
        if (collision.gameObject.CompareTag("Player")) // se colidiu com o player
        { // verifica cooldown de dano
            if (Time.time - lastDamageTime >= damageCooldown) // tempo suficiente passou?
            { // ao bater, inverte direção
                ReverseDirection(); // evita empurrão infinito
                PlayerHealth.Instance.TakeDamage(); // aplica dano ao player
                lastDamageTime = Time.time; // atualiza timestamp
            }
        }
    }

    public void Live() // chamada externa para reduzir vida do boss
    {
        live_boss -= 1; // decrementa a vida
        Debug.Log("Boss HP: " + live_boss); // exibe no console

        if (live_boss == 0) // se vida chegar a zero
        {
            IsDead = true; // bloqueia Update
            anim.SetTrigger("raptor-dead_Clip"); // dispara animação de morte
            Instantiate(pecaPrefab, peca.position, Quaternion.identity); // cria peça de recompensa
        }

        Destroy(GameObject.FindGameObjectWithTag("HeartBoss_1")); // remove um ícone de vida do HUD
    }
}