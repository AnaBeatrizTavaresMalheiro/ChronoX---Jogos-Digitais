using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoHeadCollider : MonoBehaviour {
    void Start() {
        
    }

    void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == "DinoHead") {
            Dinossaur dino = collider.GetComponentInParent<Dinossaur>();
            if(dino != null) {
                dino.Die();
            }
            GetComponentInParent<Rigidbody2D>().AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
        }
        else if (collider.gameObject.tag == "BossHead") {
            DinossauroHealth boss_preHistoria = collider.GetComponentInParent<DinossauroHealth>();
            boss_preHistoria.TakeDamage();
            GetComponentInParent<Rigidbody2D>().AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            GetComponentInParent<Rigidbody2D>().AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
        }
    }

}
