using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    List<GameObject> hitMarkers;
    List<string> hitMarkerPatterns;

    public Transform origPosInBossLevel;
    public GameObject fireHitMarkerPrefab;
    public GameObject destroyParticles;
    Element currElement;

    [SerializeField]
    Vector3 markerScale;
    [SerializeField]
    float localHeightToStartMarkers;
    [SerializeField]
    float localLengthToStartMarkers;
    [SerializeField]
    float localDistanceOfMarkersFromPlayer;
    [SerializeField]
    float vertDistanceBetweenMarkers;
    [SerializeField]
    float horizDistanceBetweenMarkers;
    [SerializeField]
    bool disableRandomHM;
    [SerializeField]
    int indexOfHitMarker;
    [SerializeField]
    int indexOfPattern;

    public GameObject normalHitEffect;
    public GameObject criticalHitEffect;

    float slashWaitTime = 1;
    float respawnTime = 15;
    [SerializeField]
    float health = 100;
    float maxHealth = 100;
    [SerializeField]
    float damage = 10;

    int minNumHitMarkersGenerated = 4;
    int maxNumHitMarkersGenerated = 7;

    [SerializeField]
    bool isDead;
    bool signalExecuted;
    [SerializeField]
    bool autoRespawn = true;

    public Transform player;

    Left_VR_Cont leftController;
    Right_VR_Cont rightController;
    SwordUI swordUI;
    GM gm;

    public EnemyStatePattern aiStates;

    public GameObject explosion;

    [SerializeField]
    bool isBossEnemy;

    void Start()
    {
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        gm = Singleton_Service.GetSingleton<GM>();
        hitMarkerPatterns = new List<string>();
        hitMarkers = new List<GameObject>();

        hitMarkerPatterns.Add("diagonal");
        hitMarkerPatterns.Add("vertical");
        hitMarkerPatterns.Add("horizontal");
        hitMarkerPatterns.Add("cross");

        aiStates = GetComponent<EnemyStatePattern>();
        RandomizeHitMarkerPlacement();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead)
        {
            return;
        }
        ContactPoint[] points = collision.contacts;
        foreach(ContactPoint point in points)
        {
            if (point.otherCollider.CompareTag("Player Sword") || point.otherCollider.CompareTag("Shuriken"))
            {
                if (this.gameObject.activeSelf)
                {
                    aiStates.currentState = aiStates.chaseState;
                    StartCoroutine(HitEffects(point.point));
                }
            }
        }

    }

    public float GetDamage()
    {
        return damage;
    }

    public void StartCriticalHit(Vector3 point)
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(CriticalHitEffects(point));
        }

    }

    public void ResetOrigPos()
    {
        if (origPosInBossLevel != null)
        {
            this.transform.position = origPosInBossLevel.position;
        }
    }

    IEnumerator HitEffects(Vector3 point)
    {
        GameObject go = Instantiate(normalHitEffect, point, Quaternion.identity);
        Destroy(go, 3);
        aiStates.enemyAudio.clip = aiStates.normalHitSound;
        aiStates.enemyAudio.volume = 0.3f;
        aiStates.enemyAudio.Play();
        yield return new WaitForSeconds(1);
        
    }

    IEnumerator CriticalHitEffects(Vector3 point)
    {
        GameObject go = Instantiate(criticalHitEffect, point, Quaternion.identity);
        Destroy(go, 3);
        aiStates.enemyAudio.clip = aiStates.criticalHitSound;
        aiStates.enemyAudio.volume = 0.3f;
        aiStates.enemyAudio.Play();
        yield return new WaitForSeconds(1);
    }
    void RandomizeHitMarkerPlacement()
    {
        //randomize which hit marker to spawn
        currElement = fireHitMarkerPrefab.GetComponent<Fire>();
        //randomize how many hit markers you want
        int numToGenerate = Random.Range(minNumHitMarkersGenerated, maxNumHitMarkersGenerated + 1);
        //Instantiate the hit markers in a random pattern
        int patternIndex = Random.Range(0, hitMarkerPatterns.Count);
        GeneratePattern(hitMarkerPatterns[patternIndex], numToGenerate, fireHitMarkerPrefab);
        
    }

    void GeneratePattern(string pattern, int numToGenerate, GameObject randHitMarker)
    {
        switch (pattern)
        {
            case "vertical":
                for (int i = 0; i < numToGenerate; i++)
                {
                    GameObject go = Instantiate(randHitMarker, Vector3.zero, Quaternion.identity, this.transform);
                    go.transform.localScale = markerScale;
                    go.transform.localPosition = new Vector3(0, localHeightToStartMarkers - vertDistanceBetweenMarkers * i, localDistanceOfMarkersFromPlayer);
                    hitMarkers.Add(go);

                }
                break;
            case "diagonal":
                for (int i = 0; i < numToGenerate; i++)
                {
                    GameObject go = Instantiate(randHitMarker, Vector3.zero, Quaternion.identity, this.transform);
                    go.transform.localScale = markerScale;
                    go.transform.localPosition = new Vector3(localLengthToStartMarkers - horizDistanceBetweenMarkers * i, localHeightToStartMarkers - vertDistanceBetweenMarkers * i, localDistanceOfMarkersFromPlayer);
                    hitMarkers.Add(go);

                }
                break;
            case "horizontal":
                for (int i = 0; i < numToGenerate; i++)
                {
                    GameObject go = Instantiate(randHitMarker, Vector3.zero, Quaternion.identity, this.transform);
                    go.transform.localScale = markerScale;
                    go.transform.localPosition = new Vector3(localLengthToStartMarkers - horizDistanceBetweenMarkers * i, 0, localDistanceOfMarkersFromPlayer);
                    hitMarkers.Add(go);

                }
                break;
            case "cross":
                for (int i = 0; i < numToGenerate/2; i++)
                {
                    GameObject go = Instantiate(randHitMarker, Vector3.zero, Quaternion.identity, this.transform);
                    go.transform.localScale = markerScale;
                    go.transform.localPosition = new Vector3(localLengthToStartMarkers - horizDistanceBetweenMarkers * i, localHeightToStartMarkers - vertDistanceBetweenMarkers * i, localDistanceOfMarkersFromPlayer);
                    hitMarkers.Add(go);

                }
                if (numToGenerate % 2 == 1)
                {
                    for (int i = 0; i < numToGenerate / 2 + 1; i++)
                    {
                        GameObject go = Instantiate(randHitMarker, Vector3.zero, Quaternion.identity, this.transform);
                        go.transform.localScale = markerScale;
                        go.transform.localPosition = new Vector3(-localLengthToStartMarkers + horizDistanceBetweenMarkers * i, localHeightToStartMarkers/2 - vertDistanceBetweenMarkers * i, localDistanceOfMarkersFromPlayer);
                        hitMarkers.Add(go);
                    }
                }
                else
                {
                    for (int i = 0; i < numToGenerate/2; i++)
                    {
                        GameObject go = Instantiate(randHitMarker, Vector3.zero, Quaternion.identity, this.transform);
                        go.transform.localScale = markerScale;
                        go.transform.localPosition = new Vector3(-localLengthToStartMarkers + horizDistanceBetweenMarkers * i, localHeightToStartMarkers/2 - vertDistanceBetweenMarkers * i, localDistanceOfMarkersFromPlayer);
                        hitMarkers.Add(go);
                    }
                }

                break;
        }
    }

    public void SignalAHit(GameObject marker)
    {
        signalExecuted = true;
        if (this.gameObject.activeSelf)
        {
            StartCoroutine("AllSlashedChecker");
        }
  
    }

    IEnumerator AllSlashedChecker()
    {
        yield return new WaitForSeconds(slashWaitTime);
        foreach (GameObject hitMarker in hitMarkers)
        {
            if (!hitMarker.GetComponent<FireHitMarker>().GetHitStatus())
            {
                foreach (GameObject hm in hitMarkers)
                {
                    hm.GetComponent<FireHitMarker>().SetHitStatus(false);
                }
                yield break;
            }
        }
        //TODO: if the function reaches this point, all hitMarkers have been disabled
        //and the enemy is dead. 
        swordUI.IncreaseScore(150);
        signalExecuted = false;
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(TriggerDeath());
        }       

        
    }

    public bool HitAlreadySignaled()
    {
        return signalExecuted;
    }

    public void DamageEnemy(float damage)
    {
        //TODO: TRIGGER ANIMATION AND SOUND
        aiStates.PlaySound(aiStates.enemyAudio, aiStates.damageSound);
        health -= damage;

        rightController = GameObject.FindGameObjectWithTag("Right Hand").GetComponent<Right_VR_Cont>();

        if (this.gameObject.activeSelf)
        {
            StartCoroutine(rightController.PulsedVibration(30, 10, 0.001f, 1f));
        }


        if (health <= 0)
        {
            swordUI.IncreaseScore(75);
            if (this.gameObject.activeSelf)
            {
                StartCoroutine(TriggerDeath());
            }

        }

        
    }

    public float GetEnemyHealth()
    {
        return health;
    }

    public void SetIsDead(bool b)
    {
        isDead = b;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void ReplenishHealth()
    {
        health = maxHealth;
        isDead = false;
    }

    public void SetHealth(float h)
    {
        health = h;
    }

    public float GetHealth()
    {
        return health;
    }

    IEnumerator TriggerDeath()
    {
        swordUI.ReplenishHealth(5);
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        if (aiStates.enemyAudio.isPlaying)
        {
            aiStates.enemyAudio.Stop();
        }
        aiStates.PlaySound(aiStates.enemyAudio, aiStates.dieSound);
        if (autoRespawn)
        {
            gm.StartRespawnTimer(this.gameObject);
        }
        if (isBossEnemy)
        {
            Destroy(this.gameObject);
        }

        yield return null;
    }

    IEnumerator MoveToPosition(GameObject obj, Vector3 position, float timeToMove)
    {
        Vector3 currentPos = obj.transform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            obj.transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }

}
