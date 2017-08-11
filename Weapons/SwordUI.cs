using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRTK;

public class SwordUI : MonoBehaviour {

    public VRTK_Pointer pointer;
    float teleportCoolDownPeriod = 1;

    float fireHealth = 0;
    float maxFireValue = 100;
    float usageCost = 2;
    float timeRemaining = 200;
    int score = 0;
    int goalsCollected;
    int maxGoals;

    float currentPlayerHealth = 0;
    float maxPlayerHealth = 100;

    Input_Listeners IPL;

    public GameObject shurikenPrefab;

    public Sheath sheath;

    public Left_VR_Cont leftCont;
    public Right_VR_Cont rightCont;

    public Slider healthHUD;
    public Slider fireHUD;
    public Text goalsRemainingHUD;

    public AudioSource mainTheme;
    [SerializeField]
    bool playMainTheme;
    public AudioSource bossTheme;
    [SerializeField]
    bool playBossTheme;

    public AudioSource dashSound;
    public AudioSource hurtSound;

    [SerializeField]
    int currLevel;

    GM gm;
   
    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance<SwordUI>(this);      
        UpdateStatsOnUI();
    }

    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance<SwordUI>(this);
    }

    void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        if (Singleton_Service.GetSingleton<GM>())
        {
            gm = Singleton_Service.GetSingleton<GM>();
        }
        fireHealth = maxFireValue;
        currentPlayerHealth = maxPlayerHealth;
        UpdateStatsOnUI();
        if (playMainTheme)
        {
            mainTheme.Play();
        }
        if (playBossTheme)
        {
            bossTheme.Play();
        }
        //StartCoroutine("Countdown");
        
    }

    public void StartTeleportationCoolDown()
    {
        StartCoroutine("TeleportationCoolDown");
    }

    IEnumerator TeleportationCoolDown()
    {
        pointer.enabled = false;
        yield return new WaitForSeconds(teleportCoolDownPeriod);
        pointer.enabled = true;
    }

    public void PlayDashSound()
    {
        dashSound.Play();
    }

    public void DamagePlayer(float damageValue)
    {
        GameObject LH = GameObject.FindGameObjectWithTag("Left Hand");
        GameObject RH = GameObject.FindGameObjectWithTag("Right Hand");
        if (LH != null)
        {
            leftCont = LH.GetComponent<Left_VR_Cont>();
            StartCoroutine(leftCont.PulsedVibration(10, 10, 0.1f, 1));
        }
        if (RH != null)
        {
            rightCont = RH.GetComponent<Right_VR_Cont>();
            StartCoroutine(rightCont.PulsedVibration(10, 10, 0.1f, 1));
        }
        
        hurtSound.Play();
        currentPlayerHealth -= damageValue;
        if (currentPlayerHealth <= 0)
        {
            currentPlayerHealth = 0;
            UpdateStatsOnUI();
            GameOver(false);
        }
        else
        {
            UpdateStatsOnUI();
        }
        
    }

    IEnumerator Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeRemaining--;
            UpdateStatsOnUI();
            if (timeRemaining == 0)
            {
                GameOver(false);
            }
        }
    }

    public void IncrementGoalsCollected()
    {
        goalsCollected++;
    }

    public void UpdateStatsOnUI()
    {
        if (gm != null)
        {
            goalsRemainingHUD.text = "Goals: " + goalsCollected + " / " + gm.GetGoals().childCount;
        }

        healthHUD.value = currentPlayerHealth / maxPlayerHealth;
        fireHUD.value = fireHealth / maxFireValue;
    }

    public void IncreaseScore(int s)
    {
        this.score += s;
        UpdateStatsOnUI();
    }

    public void GameOver(bool won)
    {
        switch (currLevel)
        {
            
            case 1:
                PlayerPrefs.SetInt("CurrentLevel", 1);
                if (won)
                {
                    score += (int)currentPlayerHealth * 20;
                }
                if (!PlayerPrefs.HasKey("HighScoreLevel1"))
                {
                    PlayerPrefs.SetInt("HighScoreLevel1", 0);
                }

                PlayerPrefs.SetInt("CurrentScoreLevel1", score);

                PlayerPrefs.Save();

                if (won)
                {
                    SceneManager.LoadScene("Win Screen");
                }
                else
                {
                    SceneManager.LoadScene("Game Over Screen");
                }
                break;
            case 2:
                PlayerPrefs.SetInt("CurrentLevel", 2);
                if (won)
                {
                    score += (int)currentPlayerHealth * 20;
                }
                if (!PlayerPrefs.HasKey("HighScoreLevel2"))
                {
                    PlayerPrefs.SetInt("HighScoreLevel2", 0);
                }

                PlayerPrefs.SetInt("CurrentScoreLevel2", score);

                PlayerPrefs.Save();

                if (won)
                {
                    SceneManager.LoadScene("Win Screen");
                }
                else
                {
                    SceneManager.LoadScene("Game Over Screen");
                }
                break;
            case 3:
                PlayerPrefs.SetInt("CurrentLevel", 3);
                if (won)
                {
                    score += (int)currentPlayerHealth * 20;
                }
                if (!PlayerPrefs.HasKey("HighScoreLevel3"))
                {
                    PlayerPrefs.SetInt("HighScoreLevel3", 0);
                }

                PlayerPrefs.SetInt("CurrentScoreLevel3", score);

                PlayerPrefs.Save();

                if (won)
                {
                    SceneManager.LoadScene("Win Screen");
                }
                else
                {
                    SceneManager.LoadScene("Game Over Screen");
                }
                break;
            case 4:
                PlayerPrefs.SetInt("CurrentLevel", 4);
                if (won)
                {
                    score += (int)currentPlayerHealth * 20;
                }
                if (!PlayerPrefs.HasKey("HighScoreLevel4"))
                {
                    PlayerPrefs.SetInt("HighScoreLevel4", 0);
                }

                PlayerPrefs.SetInt("CurrentScoreLevel4", score);

                PlayerPrefs.Save();

                if (won)
                {
                    SceneManager.LoadScene("Win Screen");
                }
                else
                {
                    SceneManager.LoadScene("Game Over Screen");
                }
                break;
            case 0:
                PlayerPrefs.SetInt("CurrentLevel", 0);
                if (won)
                {
                    score += (int)currentPlayerHealth * 20;
                }
                if (!PlayerPrefs.HasKey("HighScoreBoss"))
                {
                    PlayerPrefs.SetInt("HighScoreBoss", 0);
                }

                PlayerPrefs.SetInt("CurrentScoreBoss", score);

                PlayerPrefs.Save();

                if (won)
                {
                    SceneManager.LoadScene("Win Screen");
                }
                else
                {
                    SceneManager.LoadScene("Game Over Screen");
                }
                break;
            
        }
        
        
    }

    public void ExitToMain()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public bool FireIsDepleted()
    {
        return (fireHealth <= 0);
    }

    public bool HealthIsDepleted()
    {
        return (currentPlayerHealth <= 0);
    }

    public void DepleteFire(float value)
    {
        fireHealth -= value;
        if (fireHealth < 0)
        {
            fireHealth = 0;
        }
        UpdateStatsOnUI();
    }
    
    public void ReplenishFire(float value)
    {
        fireHealth += value;
        if (fireHealth > maxFireValue)
        {
            fireHealth = maxFireValue;
        }
        UpdateStatsOnUI();
    }

    public void ReplenishHealth(float value)
    {
        currentPlayerHealth += value;
        if (currentPlayerHealth > maxPlayerHealth)
        {
            currentPlayerHealth = maxPlayerHealth;
        }
        UpdateStatsOnUI();
    }

}
