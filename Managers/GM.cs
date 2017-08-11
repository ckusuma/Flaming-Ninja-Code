using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    [SerializeField]
    Transform goals;
    SwordUI swordUI;

    float timeBeforeRespawn = 25;

    public List<BirdStatePattern> enemyBirds;
    public Transform spawnPoints;

    int minBirds = 3;
    int maxBirds = 8;

    public GameObject birdPrefab;

    private void Start()
    {
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        enemyBirds = new List<BirdStatePattern>();
        if (spawnPoints != null && birdPrefab != null)
        {
            StartCoroutine("SpawnFlyingEnemies");
        }

    }

    public void CheckForWinStart(GameObject goalToDisable)
    {
        StartCoroutine(CheckForWin(goalToDisable));
    }

    IEnumerator CheckForWin(GameObject goalToDisable)
    {
        goalToDisable.GetComponent<Goal>().moneySound.Play();
        yield return new WaitForSeconds(1);
        goalToDisable.SetActive(false);
        swordUI.IncrementGoalsCollected();
        foreach (Transform goal in goals)
        {
            if (goal.gameObject.activeSelf)
            {
                yield break;
            }
        }
        swordUI.GameOver(true);
    }

    IEnumerator SpawnFlyingEnemies()
    {
        while (true)
        {
            if (enemyBirds.Count < maxBirds)
            {
                foreach (Transform point in spawnPoints)
                {
                    GameObject go = Instantiate(birdPrefab, point.position, Quaternion.identity);
                    enemyBirds.Add(go.GetComponent<BirdStatePattern>());
                }
            }
            yield return new WaitForSeconds(timeBeforeRespawn);
        }
    }

    public Transform GetGoals()
    {
        return goals;
    }

    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance<GM>(this);
    }

    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance<GM>(this);
    }

    public void StartRespawnTimer(GameObject enemy)
    {
        StartCoroutine(RespawnTimer(enemy));
    }

    IEnumerator RespawnTimer(GameObject enemy)
    {
        enemy.SetActive(false);
        yield return new WaitForSeconds(timeBeforeRespawn);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().ReplenishHealth();
    }

}
