using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    public GameObject explosion;
    public GameObject playerExplosion;
    public int health;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Boundary") {
            return;
        }
        if (other.tag == "Player") {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        }
        Damage(1);
        Destroy(other.gameObject);
    }

    public void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
