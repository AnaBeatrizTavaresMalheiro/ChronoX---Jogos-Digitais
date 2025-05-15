## Prefabs

### Elena Voss - Personagem Principal
- **Descrição**
    - Personagem principal do jogo, pelo qual o jogador é responsável por conduzir ao longo da história.
- **Quando são utilizados**
    - A protagonista e suas funcionalidades estarão disponíveis para as 3 diferentes fases do jogo. 
- **Componentes**
    - **Sprites**
 
      ![{EF8E13F9-9483-4D8F-967D-247A532F43EA}](https://github.com/user-attachments/assets/740bb3c6-0365-4ca8-8791-f1c7a803e859)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Possui funcionalidades de andar, pular, escalar e defeder-se (atirar ou golpear).
     
### Host do Game
- Descrição
    - Host responsável por direcionar a personagem principal.
- **Quando são utilizados**
    - Será utilizado todo início de fase.
- **Componentes**
    - **Sprites**

      ![{AD7DAE67-9BE4-4D9B-A01F-282352E2AE0A}](https://github.com/user-attachments/assets/3a3e7ea5-4fe7-43fd-bdc3-b8d45572edbf)
    - **Scripts**
        - Sua funcionalidade será se comunicar com Elena Voss através de uma caixa de diálogo.

### Dinossauro Pré-Histórico - Fase 1
- **Descrição**
    - Inimigos que perseguirão Elena Voss durante toda a fase.
- **Quando são utilizados**
    - Serão utilizados durante a fase 1.
- **Componentes**
    - **Sprites**
 
     
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Possui funcionalidades de andar.

### Guarda do Castelo - Fase 2
- **Descrição**
    - Inimigos que atiraram na Elena Voss durante toda a fase.
- **Quando são utilizados**
    - Serão utilizados durante toda a fase 2.
- **Componentes**
    - **Sprites**
      ![{062CCD8D-92D2-4E48-8BDF-D7FE494A8DBC}](https://github.com/user-attachments/assets/a8dc092b-46ac-4521-8fb2-238248ff895b)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Possui funcionalidades de andar e atirar.

### Robô - Fase 3
- **Descrição**
    - Inimigos que atiraram na Elena Voss durante toda a fase.
- **Quando são utilizados**
    -  Serão utilizados durante toda a fase 3.
- **Componentes**
    - **Sprites**
 
      ![{3E752F33-C27D-420B-82D2-7CC6737F7075}](https://github.com/user-attachments/assets/e67b10ec-7589-4bc2-9480-cb4c6a14ce72)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Possui funcionalidades de andar e atirar.

### Dragão - Boss Fase 1
- **Descrição**
    - Será um boss que tentará fazer com que Elena Voss não retorne para a sua realidade.
- **Quando são utilizados**
    - Será utilizado na fase 1.
- **Componentes**
    - **Sprites**
 
      ![{B894D1FA-FB8D-4F8F-A979-B5EA722FCF3D}](https://github.com/user-attachments/assets/265d39eb-ecd2-4285-8df9-515b73d83ab9)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Possui funcionalidades de andar e soltar bolas de fogo.


### Minotauro - Boss Fase 2
- **Descrição**
    - Será um boss que tentará fazer com que Elena Voss não retorne para a sua realidade.
- **Quando são utilizados**
    - Será utilizado na fase 2.
- **Componentes**
    - **Sprites**
 
      ![{AF572C86-7C4B-49D2-99B7-C3F027FCEE07}](https://github.com/user-attachments/assets/4ab005c3-a993-4994-a765-f3ab9609581f)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Possui funcionalidades de andar e golpear.

### Robô - Boss Fase 3
- **Descrição**
    - Será um boss que tentará fazer com que Elena Voss não retorne para a sua realidade.
- **Quando são utilizados**
    - Será utilizado na fase 3.
- **Componentes**
    - **Sprites**
 
      ![image](https://github.com/user-attachments/assets/0a7f3870-d600-44c8-a0df-f8d080ed20ae)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Possui funcionalidades de andar e soltar laser.

### Bola de Fogo
- **Descrição**
    - Será uma bola de fogo atirada pelo Dragão pelo Robô 
- **Quando são utilizados**
    - Será utilizado na fase 1 e 3.
- **Componentes**
    - **Sprites**
 
      ![{F78808AF-0537-459F-9E99-342517CD07D7}](https://github.com/user-attachments/assets/4d9ebfce-8fc2-46e3-b9f4-dcec5a7a5a63)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Soltar a bola de fogo.

### Raio Laser
- **Descrição**
    - Será uma raio laser atirado pelo Boss Robô 
- **Quando são utilizados**
    - Será utilizado na fase 3.
- **Componentes**
    - **Sprites**
 
      ![{F5867B4C-35CB-465C-9A27-90D5FF186B8E}](https://github.com/user-attachments/assets/a9c48e2c-bf8b-4066-abc3-b69b7434c1da)
    - **Colisores**
        - Box Collider 2D
        - Rigidbody 2D
    - **Fontes de audio**
    - **Scripts**
        - Soltar um raio laser.
          
### Vida
- **Descrição**
    - Representação de quantas vidas a personagem tem.
- **Quando são utilizados**
    - Será utilizada em todas as 3 fases.
- **Componentes**
    - **Sprites**
 
      ![{4D3542D3-8312-44D8-965A-8C30D36E2363}](https://github.com/user-attachments/assets/0d679d2b-871a-4b87-97ed-85702070c23f)
    - **Scripts**
        - Sua funcionalidade será remover um coração a cada vez que a personagem sofre algum tipo de dano.







