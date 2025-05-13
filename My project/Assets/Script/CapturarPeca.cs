using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapturarPeca : MonoBehaviour {
    public string phase_name; // nome da fase atribuido la no inspector

    void Start() {
        
    }

    void Update() {

    }

   void OnCollisionEnter2D(Collision2D collision) {

    if (collision.gameObject.CompareTag("Player")) {
        SceneManager.LoadScene(phase_name);
    }
}
}
