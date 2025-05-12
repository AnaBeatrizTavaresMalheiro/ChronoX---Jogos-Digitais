using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot_controller : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
         anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {// Exemplo: ativar animação de ataque
        if (Input.GetKeyDown(KeyCode.Space)) {
            anim.Play("Idle");
        }
        
    }
}
