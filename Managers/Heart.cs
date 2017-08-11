using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    public GameObject hitEffects;
    AudioSource heartAudio;
    public AudioClip hitSound;
    BossGM bossGM;

    float damageFromCollision = 10;

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
                    bossGM.DamageBoss(damageFromCollision);
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
