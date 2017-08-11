using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossGM : MonoBehaviour {

    public UnityEvent OnSummon;

    public AudioSource summoningSound;

    SwordUI swordUI;

    public Transform enemies;

    public Transform birdSpawnPoints;
    public GameObject birdPrefab;
    public Transform spikeSpawnPoints;
    public GameObject spikePrefab;

    public Transform playerPosition;

    public Animator bossAnimator;

    public AudioSource bossAudio;
    public AudioClip spawnSound;

    public Slider bossHealthBar;

    float pauseTime = 0.7f;
    float timeUntilNextDecision = 7;

    float health = 10000;
    float maxHealth = 10000;

    public List<BirdStatePattern> enemyBirds;
    int maxBirds = 15;
    int minBirds = 3;

    public GameObject enemyPrefab;

    public GameObject heart1;
    public GameObject heart2;

    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance<BossGM>(this);
    }

    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance<BossGM>(this);
    }

    void Start()
    {
        enemyBirds = new List<BirdStatePattern>();
        OnSummon = new UnityEvent();
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        playerPosition = GameObject.FindGameObjectWithTag("Player Position").transform;
        UpdateBossUI();
        StartCoroutine("BossBehavior");
    }

    void Update()
    {
        if (!heart1.activeSelf && !heart2.activeSelf)
        {
            StartCoroutine("Flinch");
        }
    }

    public void DamageBoss(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //WIN
            swordUI.GameOver(true);
        }
        UpdateBossUI();
    }

    public void UpdateBossUI()
    {
        bossHealthBar.value = health / maxHealth;
    }

    IEnumerator BossBehavior()
    {
        while (true)
        {
            int decision = Random.Range(0, 3);
            PlaySummoningSound();
            switch (decision)
            {
                case 0:
                    bossAnimator.SetBool("isSummoning", true);
                    yield return new WaitForSeconds(pauseTime);
                    bossAnimator.SetBool("isSummoning", false);
                    foreach (Transform spawn in birdSpawnPoints)
                    {
                        if (enemyBirds.Count < maxBirds)
                        {
                            GameObject bird = Instantiate(birdPrefab, spawn.position, Quaternion.identity);
                            enemyBirds.Add(bird.GetComponent<BirdStatePattern>());
                            PlaySound(spawnSound);
                            yield return new WaitForSeconds(pauseTime);
                        }
                    }
                    break;
                case 1:
                    bossAnimator.SetBool("isSummoning", true);
                    yield return new WaitForSeconds(pauseTime);
                    bossAnimator.SetBool("isSummoning", false);
                    foreach (Transform enemy in enemies)
                    {
                        GameObject e = Instantiate(enemyPrefab, enemy.position, enemy.rotation);
                        e.GetComponent<EnemyStatePattern>().currentState = e.GetComponent<EnemyStatePattern>().chaseState;
                        PlaySound(spawnSound);
                        yield return new WaitForSeconds(pauseTime);             
                    }
                    break;
                case 2:
                    bossAnimator.SetBool("isSummoning", true);
                    yield return new WaitForSeconds(pauseTime);
                    bossAnimator.SetBool("isSummoning", false);
                    foreach (Transform spawn in spikeSpawnPoints)
                    {
                        Instantiate(spikePrefab, spawn.position, spawn.rotation);
                        PlaySound(spawnSound);
                        yield return new WaitForSeconds(pauseTime);
                    }
                    break;
            }

            yield return new WaitForSeconds(timeUntilNextDecision);
            
        }
    }

    IEnumerator Flinch()
    {
        //StopCoroutine("BossBehavior");
        bossAnimator.SetBool("isVulnerable", true);
        yield return new WaitForSeconds(7);
        bossAnimator.SetBool("isVulnerable", false);
        heart1.SetActive(true);
        heart2.SetActive(true);
        //StartCoroutine("BossBehavior");
    }

    public void PlaySummoningSound()
    {
        summoningSound.Play();
    }

    public void PlaySound (AudioClip c)
    {
        bossAudio.clip = c;
        bossAudio.Play();
    }
}
