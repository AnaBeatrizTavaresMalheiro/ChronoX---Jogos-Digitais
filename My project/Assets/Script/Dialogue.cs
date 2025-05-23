using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image characterImage;
    public float textSpeed = 0.05f;

    public Sprite elenaSprite;
    public Sprite hostSprite;
    public TextMeshProUGUI speaker;

    public DialogueLine[] dialogoFase1;
    public DialogueLine[] dialogoFase2;
    private DialogueLine[] dialogoAtual;

    public DialogueLine[] dialogoFase3;

    private int index = 0;

    void Start()
    {
        // Define falas da Fase 1
        dialogoFase1 = new DialogueLine[]
        {
            new DialogueLine { speaker = "Elena Voss", text = "Ugh... minha cabeça... onde eu estou?", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Essa roupa... isso não é meu jaleco de laboratório!", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Host", text = "Saudações, Dra. Elena Voss. Você está na Era Pré-Histórica.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "A ChronoX foi destruída. Seus fragmentos estão espalhados por diferentes linhas temporais.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Seu traje foi adaptado automaticamente para aumentar suas chances de sobrevivência.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Muito real. E perigoso. Há pequenos dinossauros espalhados pela floresta — eles são rápidos, agressivos e vão tentar impedi-la de avançar. Para derrotá-los, pule sobre eles com precisão.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Ao final desta era, você enfrentará um Velociraptor — ele protege um dos fragmentos da ChronoX. Seu ponto fraco está nas costas, então ataque por trás para vencê-lo. Você só poderá seguir para a próxima fase se derrotá-lo.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Você possui três vidas. Se perdê-las, ficará presa nesta linha temporal para sempre.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Ah, e mais um aviso: devido à instabilidade da realidade, ao olhar para trás você poderá sentir um leve deslocamento espacial. Sua posição aparente pode não corresponder exatamente ao lugar onde você está de fato.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Em outras palavras, cuidado ao recuar — você pode acabar mais à frente do que imagina.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Comandos básicos: pressione 'A' para mover-se para a esquerda, 'D' para a direita e 'espaço' para pular.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Boa sorte, Dra. Voss. O tempo está contra você.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Ok... respira. Dinossauros, fragmentos, máquina do tempo... é só mais um dia maluco na vida de uma cientista.", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Vamos nessa. Eu preciso encontrar esse fragmento!", characterSprite = elenaSprite }
        };

        // Define falas da Fase 2
        dialogoFase2 = new DialogueLine[]
        {
            new DialogueLine { speaker = "Elena Voss", text = "Ai, minhas pernas... dessa vez foi uma queda mais longa.", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Castelo medieval? Isso só pode ser um pesadelo com armaduras.", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Host", text = "Dra. Voss, você está na Idade Média — em uma fortaleza dominada por forças hostis.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Guardas patrulham os corredores e atacarão se a virem. Fique atenta aos padrões de movimento.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "O caminho está repleto de serras giratórias no chão. Um passo em falso e você será cortada.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Há também smashers — blocos de pedra que despencam do teto. Eles esmagam qualquer coisa abaixo.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Você poderá se defender com uma espada nesta fase. Pressione o botão esquerdo do mouse para atacar.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "No final desta era, enfrentará o Minotauro — uma criatura feroz que guarda um dos fragmentos da ChronoX.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Derrote-o para recuperar o fragmento e avançar. Se falhar, ficará presa neste castelo para sempre.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Ah, e mais um aviso: devido à instabilidade da realidade, ao olhar para trás você poderá sentir um leve deslocamento espacial. Sua posição aparente pode não corresponder exatamente ao lugar onde você está de fato.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Em outras palavras, cuidado ao recuar — você pode acabar mais à frente do que imagina.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Comandos: 'A' para andar à esquerda, 'D' para direita, 'Espaço' para pular e botão esquerdo do mouse para atacar.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Serras no chão, guardas, blocos esmagadores... ótimo! Tudo que eu precisava.", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Vamos lá. Mais um fragmento me espera.", characterSprite = elenaSprite }
        };

        dialogoFase3 = new DialogueLine[]
        {
            new DialogueLine { speaker = "Elena Voss", text = "Uau... onde estou agora? Isso parece... o futuro?", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Arranha-céus, hologramas, e... espera, aquilo era um drone armado?", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Host", text = "Dra. Voss, você está em uma metrópole do século XXV, sob domínio de uma inteligência artificial hostil chamada SynTek.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Esta realidade abriga mini-robôs patrulheiros equipados com canhões de laser. Eles estão espalhados por todo o mapa e atacarão ao menor sinal de movimento.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Evite encostar nos feixes de laser que cruzam o ambiente. Eles estão ligados diretamente ao sistema de defesa da IA e causarão danos imediatos.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Você poderá destruir os mini-robôs e atacar a IA SynTek disparando bolas de fogo. Curiosamente, sua espada foi afetada por esta realidade e agora consegue projetar essas esferas de energia.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "O fragmento da ChronoX está no centro de comando da SynTek, protegido por um robô de combate. Ataque seus circuitos dorsais para sobrecarregá-lo.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Ah, e mais um aviso: devido à instabilidade da realidade, ao olhar para trás você poderá sentir um leve deslocamento espacial. Sua posição aparente pode não corresponder exatamente ao lugar onde você está de fato.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Em outras palavras, cuidado ao recuar — você pode acabar mais à frente do que imagina.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Comandos: 'A' para esquerda, 'D' para direita, 'Espaço' para pular e botão esquerdo do mouse para disparar bolas de fogo.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Host", text = "Boa sorte, Dra. Voss. Este é o teste mais tecnológico até agora.", characterSprite = hostSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Tecnologia assassina, robôs e uma IA com complexo de deus... só mais uma terça-feira.", characterSprite = elenaSprite },
            new DialogueLine { speaker = "Elena Voss", text = "Ok, foco! Estou perto do último fragmento. Nada vai me parar agora.", characterSprite = elenaSprite }
        };



        // Seleciona o diálogo com base na cena atual
        string nomeCena = SceneManager.GetActiveScene().name;
        if (nomeCena == "Pre Historia")
        {
            dialogoAtual = dialogoFase1;
        }
        else if (nomeCena == "Idade Média")
        {
            dialogoAtual = dialogoFase2;
        }
        else if (nomeCena == "Futuro")
        {
            dialogoAtual = dialogoFase3;
        }
        else
        {
            dialogoAtual = new DialogueLine[0];
        }

        index = 0;
        text.text = string.Empty;

        if (dialogoAtual.Length > 0)
        {
            speaker.text = dialogoAtual[index].speaker;
            StartDialogue();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (text.text == dialogoAtual[index].text)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = dialogoAtual[index].text;
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        characterImage.sprite = dialogoAtual[index].characterSprite;
        speaker.text = dialogoAtual[index].speaker;
        text.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char letter in dialogoAtual[index].text.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < dialogoAtual.Length - 1)
        {
            index++;
            DisplayCurrentLine();
        }
        else
        {
            gameObject.SetActive(false); // Oculta o painel de diálogo
        }
    }
}
