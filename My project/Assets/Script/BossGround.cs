using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossGround : MonoBehaviour {

    [Header("Tilemap a habilitar")]
    public Tilemap tilemapToEnable; // instanciar o tilemap la no inspector

    void Start() {
        
    }

    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") { // quando o player tocar no chao da fase1 do boss
            tilemapToEnable.gameObject.SetActive(true); // a parede ao lado fica ativa
        }
    }

}
