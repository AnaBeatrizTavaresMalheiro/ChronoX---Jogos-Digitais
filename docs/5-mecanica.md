## Mecânica

### Elementos Formais
- **Padrão de interação do jogador**
    - O jogo terá o formato single-player. Onde o jogador será responsável
 por conduzir e acompanhar a jornada de Elena Voss uma cientista especialistas
 em física quântica.

- **Objetivos** 
    - **Quando o jogador ganha?**
 
      O jogador tem como objetivo fazer com que Elena Voss volte para sua
 linha temporal, através da coleta das peças de sua máquina do tempo ChronoX
 que estão espalhadas em 3 eras.
    - **Quando o jogador perde?**
 
      O jogador perde quando a personagem principal perde suas 3 vidas ao ser golpeada por inimigos ou elementos de perigo espalhados pelo mapa.
      
- **Regras**
    - O jogador deve coletar os fragmentos da ChronoX em cada era para avançar
 para a próxima fase.
    - O jogador pode interagir com elementos do cenário e evitar obstáculos para
 sobreviver.
    - Emalgumas fases, Elena pode usar armas (ex.: espada na Idade Média, arma
 no Futuro) para se defender.
    - Sofrer dano de inimigos ou armadilhas pode reduzir a vida da personagem.
    - Se a barra de vida chegar a zero, o jogador perde e deve reiniciar a fase.

 - **Procedimentos**
     - **Exploração:** O jogador pode se mover pelo cenário e interagir com objetos
 relevantes.
    - **Coleta de Fragmentos:** O jogador deve localizar e coletar fragmentos da
 ChronoX para avançar.
    - **Evasão e Defesa:** O jogador pode desviar de obstáculos e, em algumas fases,
 usar armas para derrotar inimigos.
    - **Progressão:** O jogador avança para a próxima era ao coletar todos os
 fragmentos disponíveis na fase atual.

- **Recursos**
    - **Vida:** Representa a resistência da protagonista. Se chegar a zero, o jogo é
 perdido.
    - **Fragmentos da ChronoX:** Elemento essencial para avançar no jogo.
    - **Armas:** Disponíveis em algumas fases (espada na Idade Média, arma no
 Futuro).
    - **Ambientes:** Cada fase tem características e desafios próprios, exigindo
 adaptação.

- **Limites**
    - O limite físico do jogo são os cenários de cada época.
    - A mudança de eras reforça essa separação, pois cada cenário possui suas
 próprias regras e desafios exclusivos.
    - No contexto narrativo, a protagonista está presa em um ciclo de viagem no
 tempo, e o único caminho para retornar à realidade é completar sua missão.

- **Resultado**
    - **Como ele termina depois da vitoria** 
        - O jogo termina com a Elena Voss voltando para a sua realidade após juntar
 todas as peças de sua máquina do tempo.
    - **Como ele termina depois da derrota**
        - Com a derrota, o jogo é reiniciado de acordo com a fase que o jogador se encontra no momento.
     
- **Loop Principal**
    - **Movimentação do Jogador:** A movimentação do jogador é um dos aspectos
 fundamentais da jogabilidade. O jogador poderá se deslocar livremente
 pelo cenário utilizando as setas do teclado. Esse sistema de movimentação
 permitirá que o personagem avance, recue e altere sua posição lateralmente
 para explorar o ambiente e evitar obstáculos. Para garantir uma resposta fluida
 e natural, a movimentação será implementada utilizando detecção de eventos
 e atualização contínua da posição do personagem.
    - **Interação do Jogador:** Além da movimentação básica, o jogador poderá
 interagir com o ambiente de diferentes formas. Entre as principais interações,
 destacam-se:
        - **Pulo:** O jogador poderá pular para evitar obstáculos no caminho. A
 altura e a distância do pulo serão ajustáveis de acordo com a física do
 jogo.
         - **Mudança de trilha:** Em cenários onde existam múltiplas trilhas de
 movimentação, o jogador poderá alternar entre elas para escapar de
 obstáculos ou acessar áreas específicas.
        - **Coleta de itens:** O jogo poderá contar com itens colecionáveis que forne
cem benefícios ao jogador, como aumento de pontuação ou habilidades
 temporárias.
    - **Troca de Eras Temporais:** Um dos elementos mais inovadores do jogo é a
 troca deerastemporais, queocorredeformadinâmica. Essamecânicaintroduz
 variação visual e novos desafios ao jogador, garantindo uma experiência mais
 imersiva. A mudança de época pode trazer modificações no cenário, nos
 inimigos, na física do ambiente e até nas regras de jogabilidade. Para tornar
 essa transição mais envolvente, serão utilizadas animações e efeitos visuais
 que indicam a passagem de tempo de maneira fluida.
    - **Temporizador:** O jogo contará com um sistema de temporizador que deter
 minará o momento exato da troca de era. O temporizador será exibido na
 interface do usuário para que o jogador tenha ciência da iminente mudança
 de cenário. Assim que o tempo estipulado for atingido, o jogo carregará um
 novo ambiente, ajustando os desafios e variando os elementos visuais. Esse
 mecanismo adiciona uma camada estratégica ao jogo, pois o jogador deverá
 se preparar para a transição e adaptar sua jogabilidade de acordo com a nova
 realidade.

