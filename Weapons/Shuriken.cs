using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour {

    Input_Listeners IPL;

    [SerializeField]
    float damage = 20;

    SwordUI swordUI;

    public Transform ref1;
    public Transform ref2;

    public Transform direction1;
    public Transform direction2;

    public GameObject hitEffects;

    Rigidbody rb;
    float destroyTime = 1;

    bool thrown;


    void Start()
    {
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        rb = GetComponent<Rigidbody>();
        //StartCoroutine("CheckShurikenParent");
    }

    void Update()
    {
        if (!IPL.GetLeftTriggerInteracting() && !IPL.GetRightTriggerInteracting())
        {
            thrown = true;
        }
        if (thrown && rb.velocity.magnitude < 0.01f)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator CheckShurikenParent()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (this != null)
            {
                if (this.transform.parent == null)
                {
                    Destroy(this.gameObject);
                }
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hit Marker"))
        {
            Enemy enemyRef = collision.gameObject.GetComponentInParent<Enemy>();

            enemyRef.DamageEnemy(damage * 2);

            StartCoroutine("DestroyShuriken");
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyRef = collision.gameObject.GetComponent<Enemy>();

            enemyRef.DamageEnemy(damage);


            StartCoroutine("DestroyShuriken");
        }
        if (!collision.collider.CompareTag("Shuriken") && !collision.collider.CompareTag("Player Sword") && !collision.collider.CompareTag("Prop"))
        {
            StartCoroutine("DestroyShuriken");
        }      

    }

    public void SetThrown(bool b)
    {
        thrown = b;
    }

    public bool GetThrown()
    {
        return thrown;
    }

    IEnumerator DestroyShuriken()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

    //TODO: DESTROY SHURIKENS
    /*void OnTriggerEnter(Collider other)
    {
         Destroy(this.gameObject);
    }*/

    public void SetShurikenDamage(float d)
    {
        damage = d;
    }

    public float GetShurikenDamage()
    {
        return damage;
    }
}
