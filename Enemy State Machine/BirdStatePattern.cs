using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStatePattern : MonoBehaviour {

    public static List<BirdStatePattern> boidList = new List<BirdStatePattern>();
    public Rigidbody body;
    public float speed = 3f;
    public float minFlockDistance = 2f;
    public float Xmin = -10f, Xmax = 10f, Ymin = -10f, Ymax = 10f, Zmin = -10f, Zmax = 10f;
    float maxDistance = 15;
    float damage = 5;
    float bounceBack = 100;
    float distanceBeforeAttack = 2;
    public Transform playerPosition;
    public Transform lookPosition;

    public GameObject normalHitEffect;
    public GameObject explosion;
    public AudioSource enemyAudio;
    public AudioClip normalHitSound;

    public AudioSource wingFlapSounds;


    GM gm;
    BossGM gmb;

    SwordUI swordUI;

    Animator anim;

    int health = 3;

    void Start()
    {
        //Register this boid with the list of boids.
        boidList.Add(this.GetComponent<BirdStatePattern>());
        //Cache the rigidbody for later...
        body = GetComponent<Rigidbody>();
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        gm = Singleton_Service.GetSingleton<GM>();
        gmb = Singleton_Service.GetSingleton<BossGM>();
        playerPosition = GameObject.FindGameObjectWithTag("Player Position").transform;
        StartCoroutine("WingFlapTimer");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            swordUI.DamagePlayer(damage);
            Invoke("InvokeDeath", 0.5f);
            return;
        }

        ContactPoint[] points = collision.contacts;
        foreach (ContactPoint point in points)
        {
            if (point.otherCollider.CompareTag("Player Sword") || point.otherCollider.CompareTag("Shuriken"))
            {
                if (this.gameObject.activeSelf)
                {
                    DecreaseHealth(1);
                    StartCoroutine(HitEffects(point.point));
                }
            }
        }

    }

    IEnumerator WingFlapTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            wingFlapSounds.Play();
        }
    }

    void DecreaseHealth(int i)
    {
        health -= i;
        if (health <= 0)
        {
            swordUI.ReplenishHealth(5);
            InvokeDeath();
        }
    }

    IEnumerator HitEffects(Vector3 point)
    {
        GameObject go = Instantiate(normalHitEffect, point, Quaternion.identity);
        Destroy(go, 3);
        enemyAudio.clip = normalHitSound;
        enemyAudio.volume = 0.3f;
        enemyAudio.Play();
        yield return new WaitForSeconds(1);

    }

    void InvokeDeath()
    {
        if (gm != null && gmb == null)
        {
            gm.enemyBirds.Remove(this.GetComponent<BirdStatePattern>());
        }
        else if (gm == null && gmb != null)
        {
            gmb.enemyBirds.Remove(this.GetComponent<BirdStatePattern>());
        }
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            swordUI.DamagePlayer(damage);
            Invoke("InvokeDeath", 0.5f);

        }
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(playerPosition.position, this.transform.position) <= distanceBeforeAttack)
        {
            body.AddForce((body.velocity).normalized * bounceBack, ForceMode.Impulse);
        }
        Collider[] detected = Physics.OverlapSphere(this.transform.position, maxDistance);
        foreach (Collider c in detected)
        {
            if (c.CompareTag("Player"))
            {
                Attack();
                transform.rotation = Quaternion.LookRotation(body.velocity);
                return;
            }
        }
        //Add the rigid body of this boid to the gather() an flock() rules.
        body.velocity += gather() + flock();
        this.transform.rotation = Quaternion.LookRotation(body.velocity);
        this.transform.rotation = Quaternion.Euler(-90, transform.rotation.y, transform.rotation.z);
        //make sure this boid stays in boundaries...
        bound();
        transform.rotation = Quaternion.LookRotation(body.velocity);
    }

    private Vector3 gather()
    {
        Vector3 centerOfSwarm = new Vector3();

        //Add all boid positions together...
        foreach (BirdStatePattern b in boidList)
        {
            if (b != this)
            {
                if (b != null)
                {
                    centerOfSwarm += b.gameObject.transform.position;
                }

            }
        }
        //Divide by the total number to find the average...
        centerOfSwarm /= boidList.Count - 1;
        //find the delta of the center and this boid position and add a percentage to move towards the center..
        return (centerOfSwarm - this.transform.position) / 100f;
    }

    private Vector3 flock()
    {
        Vector3 flockModifier = new Vector3();
        //loop over all other boids...
        foreach (BirdStatePattern b in boidList)
        {
            //if boid isn't me..
            if (b != this && b != null)
            {
                //if boid is too close to me...
                if (Mathf.Abs(Vector3.Distance(b.transform.position, this.transform.position)) < minFlockDistance)
                {
                    //tell boid to go away...
                    flockModifier = flockModifier - (b.transform.position - this.transform.position);
                }
            }
        }
        return flockModifier;
    }

    private void bound()
    {
        Vector3 v = body.velocity;
        if (this.transform.localPosition.x < Xmin)
        {
            v.x = speed;
        }
        else if (this.transform.localPosition.x > Xmax)
        {
            v.x = -speed;
        }
        if (this.transform.localPosition.y < Ymin)
        {
            v.y = speed;
        }
        else if (this.transform.localPosition.y > Ymax)
        {
            v.y = -speed;
        }
        if (this.transform.localPosition.z < Zmin)
        {
            v.z = speed;
        }
        else if (this.transform.localPosition.z > Zmax)
        {
            v.z = -speed;
        }
        body.velocity = v;
    }

    void Attack()
    {
        body.velocity = (playerPosition.position - lookPosition.position).normalized * speed;
    }
}
