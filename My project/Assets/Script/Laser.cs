using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class LaserStepMovement : MonoBehaviour
{
    [Header("Configuração de Passos")]
    [Tooltip("Quanto ele anda em X e Y antes de voltar")]
    public Vector2 steps = new Vector2(0, 5);  // ex: (0,5) sobe 5; (3,0) vai 3 à direita

    [Tooltip("Velocidade de movimento")]
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 targetPos;
    public bool rotationZ = true;  // se for true fica normal, se for false eh para fazer a movimentação com o laser em 13 graus
    private int direction = 1;  //  1: indo de start→target,  -1: voltando de target→start

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType     = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.constraints  = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<BoxCollider2D>().isTrigger = false;

        startPos = rb.position;

        // 2) Escolhe steps “puros” ou rotacionados
        Vector2 usedSteps;
        if (!rotationZ)
        {
            // aplica a rotação Z do GameObject
            Vector3 w = transform.rotation * new Vector3(steps.x, steps.y, 0f);
            usedSteps = new Vector2(w.x, w.y);
        }
        else
        {
            // mantém passos originais sem rotação
            usedSteps = steps;
        }

        // 3) Define targetPos uma única vez
        targetPos = startPos + usedSteps;
    }

    void FixedUpdate()
    {
        // 1) Calcula direção de movimento (normalizada)
        Vector2 dirVec;
        if (direction == 1)
        {
            Vector2 diff = targetPos - startPos;
            dirVec = diff.normalized;
        }
        else
        {
            Vector2 diff = startPos - targetPos;
            dirVec = diff.normalized;
        }

        // 2) Próxima posição considerando velocidade
        Vector2 nextPos = rb.position + dirVec * speed * Time.fixedDeltaTime;

        // 3) Verifica se ultrapassou o ponto final (start ou target)
        bool reachedEnd = false;

        if (direction == 1)
        {
            // indo de start → target
            if (steps.x != 0)
            {
                float distX = Mathf.Abs(nextPos.x - startPos.x);
                if (distX >= Mathf.Abs(steps.x))
                    reachedEnd = true;
            }

            if (reachedEnd == false && steps.y != 0)
            {
                float distY = Mathf.Abs(nextPos.y - startPos.y);
                if (distY >= Mathf.Abs(steps.y))
                    reachedEnd = true;
            }
        }
        else
        {
            // voltando de target → start
            if (steps.x != 0)
            {
                float distX = Mathf.Abs(nextPos.x - targetPos.x);
                if (distX >= Mathf.Abs(steps.x))
                    reachedEnd = true;
            }

            if (reachedEnd == false && steps.y != 0)
            {
                float distY = Mathf.Abs(nextPos.y - targetPos.y);
                if (distY >= Mathf.Abs(steps.y))
                    reachedEnd = true;
            }
        }

        // 4) Move de fato e, se chegou, encaixa na extremidade e inverte direção
        if (reachedEnd)
        {
            if (direction == 1)
                rb.MovePosition(targetPos);
            else
                rb.MovePosition(startPos);

            direction = direction * -1;
        }
        else
        {
            rb.MovePosition(nextPos);
        }
    }
}
