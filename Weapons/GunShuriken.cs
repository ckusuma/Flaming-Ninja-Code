using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShuriken : MonoBehaviour {

    public Transform ref1;
    public Transform ref2;

    public GameObject hitEffects;
    float destroyTime = 1;

    float damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hit Marker"))
        {
            Enemy enemyRef = collision.gameObject.GetComponentInParent<Enemy>();

            if (enemyRef != null)
            {
                enemyRef.DamageEnemy(damage * 2);
            }

            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyRef = collision.gameObject.GetComponent<Enemy>();

            enemyRef.DamageEnemy(damage);

            Destroy(this.gameObject);
        }
        if (!collision.collider.CompareTag("Shuriken") && !collision.collider.CompareTag("Player Sword"))
        {
            Destroy(this.gameObject);
        }
    }



}
