using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBoss : MonoBehaviour {
    [Header("Tilemap a habilitar")]
    public GameObject gameObjectToEnable; // instanciar o tilemap la no inspector

    void Start() {
        
    }

    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") { // quando o player tocar no chao da fase1 do boss
            gameObjectToEnable.gameObject.SetActive(true); // a parede ao lado fica ativa
        }
    }
}
