using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGM : MonoBehaviour {

    public Text highScoreLevel1;
    public Text highScoreLevel2;
    public Text highScoreLevel3;
    public Text highScoreLevel4;
    public Text highScoreBoss;

    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance<MenuGM>(this);
        highScoreLevel1.text = "Your high score:" + System.Environment.NewLine + PlayerPrefs.GetInt("HighScoreLevel1", 0);
        highScoreLevel2.text = "Your high score:" + System.Environment.NewLine + PlayerPrefs.GetInt("HighScoreLevel2", 0);
        highScoreLevel3.text = "Your high score:" + System.Environment.NewLine + PlayerPrefs.GetInt("HighScoreLevel3", 0);
        highScoreLevel4.text = "Your high score:" + System.Environment.NewLine + PlayerPrefs.GetInt("HighScoreLevel4", 0);
        if (AllLevelsHaveBeenCompleted())
        {
            highScoreBoss.text = "Your high score:" + System.Environment.NewLine + PlayerPrefs.GetInt("HighScoreBoss", 0);
        }
        else
        {
            highScoreBoss.text = "Unlocked when all" + System.Environment.NewLine + "4 missions are completed";
        }

    }

    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance<MenuGM>(this);
    }

    public void PlayLevel(int levelNo)
    {
        switch (levelNo)
        {
            case 1:
                SceneManager.LoadScene("Level 4");
                break;
            case 2:
                SceneManager.LoadScene("Level 5");
                break;
            case 3:
                SceneManager.LoadScene("Prototype Scene");
                break;
            case 4:
                SceneManager.LoadScene("Level 2");
                break;
            case 5:
                Application.Quit();
                break;
            case 0:
                if (AllLevelsHaveBeenCompleted())
                {
                    SceneManager.LoadScene("Boss Level");
                }
                break;
        }
    }

    bool AllLevelsHaveBeenCompleted()
    {
        return ((PlayerPrefs.GetInt("HighScoreLevel1", 0) != 0) && (PlayerPrefs.GetInt("HighScoreLevel2", 0) != 0) && (PlayerPrefs.GetInt("HighScoreLevel3", 0) != 0) && (PlayerPrefs.GetInt("HighScoreLevel4", 0) != 0));
    }
}
