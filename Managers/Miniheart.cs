using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniheart : MonoBehaviour {

    BossGM bossGM;

    float currentHealth;
    float maxHealth = 2000;
    float damageFromCollision = 100;

    float bossDamageFromCollision = 2;

    public GameObject explosionPrefab;

    public GameObject hitEffects;

    AudioSource heartAudio;
    public AudioClip hitSound;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        bossGM = Singleton_Service.GetSingleton<BossGM>();
        heartAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] points = collision.contacts;
        foreach (ContactPoint point in points)
        {
            if (point.otherCollider.CompareTag("Player Sword") || point.otherCollider.CompareTag("Shuriken"))
            {
                if (this.gameObject.activeSelf)
                {
                    StartCoroutine(HitEffects(point.point));
                    currentHealth -= damageFromCollision;
                    bossGM.DamageBoss(bossDamageFromCollision);
                    if (currentHealth <= 0)
                    {
                        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                        Destroy(go, 3);
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    IEnumerator HitEffects(Vector3 point)
    {
        GameObject go = Instantiate(hitEffects, point, Quaternion.identity);
        Destroy(go, 3);
        heartAudio.clip = hitSound;
        heartAudio.volume = 0.3f;
        heartAudio.Play();
        yield return new WaitForSeconds(1);

    }

}
