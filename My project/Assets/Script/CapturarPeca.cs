using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapturarPeca : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         Debug.Log("Colidiu com: " );
    }

   void OnCollisionEnter2D(Collision2D collision)
{
    Debug.Log("Colidiu com: " + collision.gameObject.name);

    if (collision.gameObject.CompareTag("Player"))
    {
        SceneManager.LoadScene("Idade Média");
    }
}
}
