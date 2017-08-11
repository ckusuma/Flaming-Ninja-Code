using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;
using UnityEngine.UI;

public class Sword : MonoBehaviour {

    bool equipped;
    Input_Listeners IPL;
    SwordUI swordUI;
    Rigidbody rb;
    [SerializeField] float damage = 10;

    Vector3 positionForParenting = new Vector3(.05f, -.11f, -.22f);
    Vector3 rotationForParenting = new Vector3(90, 0, 0);

    AudioSource swordSource;

    void Start()
    {
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        rb = GetComponent<Rigidbody>();
        this.GetComponent<CapsuleCollider>().isTrigger = true;
        swordSource = GetComponent<AudioSource>();

    }

    void OnCollisionEnter(Collision other)
    {
        if (this.transform.parent != null)
        {
            if (this.transform.parent.name == "Sheath")
            {
                return;
            }
        }
        swordSource.Play();
        if (other.collider.CompareTag("Ground") && !GetComponent<NVRInteractableItem>().IsAttached)
        {
            Destroy(this.gameObject);
        }
        if (other.collider.CompareTag("Hit Marker") || other.collider.CompareTag("Enemy"))
        {
            //damage the enemy
            if (other.collider.CompareTag("Hit Marker"))
            {
                Enemy enemyRef = other.gameObject.GetComponentInParent<Enemy>();
                if (enemyRef != null)
                {
                    enemyRef.DamageEnemy(damage * 2);
                }
                else
                {
                    return;
                }
            }
            else if (other.collider.CompareTag("Enemy"))
            {
                Enemy enemyRef = other.collider.GetComponent<Enemy>();
                if (enemyRef != null)
                {
                    enemyRef.DamageEnemy(damage);
                }

            }

            swordUI.UpdateStatsOnUI();
        }
    }

    public void SetEquipped(bool b)
    {
        equipped = b;
    }

    public bool GetEquipped()
    {
        return equipped;
    }
    
}
