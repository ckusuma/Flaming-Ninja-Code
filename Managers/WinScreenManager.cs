using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class WinScreenManager : MonoBehaviour {

    public Text score;
    public Text beatHighScore;

    Input_Listeners IPL;

    private Transform reticle;
    private LineRenderer laser;
    public float range = 10;

    public Color enabledColor;
    public Color disabledColor;

    private Light reticleLight;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    private RaycastHit target;
    private bool isHovering;

    public GameObject laserPrefab;
    public GameObject reticlePrefab;

    private bool canChangeScenes;

    void Start()
    {
        beatHighScore.enabled = false;
        int currLevel = PlayerPrefs.GetInt("CurrentLevel");
        switch (currLevel)
        {
            case 1:
                int currScore = PlayerPrefs.GetInt("CurrentScoreLevel1");
                score.text = "Your Score: " + currScore;
                if (currScore > PlayerPrefs.GetInt("HighScoreLevel1"))
                {
                    beatHighScore.enabled = true;
                    beatHighScore.text = "You've beaten the previous" + System.Environment.NewLine + "high score of " + PlayerPrefs.GetInt("HighScoreLevel1", 0);
                    PlayerPrefs.SetInt("HighScoreLevel1", currScore);
                    PlayerPrefs.SetInt("CurrentScoreLevel1", 0);

                }
                break;
            case 2:
                currScore = PlayerPrefs.GetInt("CurrentScoreLevel2");
                score.text = "Your Score: " + currScore;
                if (currScore > PlayerPrefs.GetInt("HighScoreLevel2"))
                {
                    beatHighScore.enabled = true;
                    beatHighScore.text = "You've beaten the previous" + System.Environment.NewLine + "high score of " + PlayerPrefs.GetInt("HighScoreLevel2", 0);
                    PlayerPrefs.SetInt("HighScoreLevel2", currScore);
                    PlayerPrefs.SetInt("CurrentScoreLevel2", 0);

                }
                break;
            case 3:
                currScore = PlayerPrefs.GetInt("CurrentScoreLevel3");
                score.text = "Your Score: " + currScore;
                if (currScore > PlayerPrefs.GetInt("HighScoreLevel3"))
                {
                    beatHighScore.enabled = true;
                    beatHighScore.text = "You've beaten the previous" + System.Environment.NewLine + "high score of " + PlayerPrefs.GetInt("HighScoreLevel3", 0);
                    PlayerPrefs.SetInt("HighScoreLevel3", currScore);
                    PlayerPrefs.SetInt("CurrentScoreLevel3", 0);

                }
                break;
            case 4:
                currScore = PlayerPrefs.GetInt("CurrentScoreLevel4");
                score.text = "Your Score: " + currScore;
                if (currScore > PlayerPrefs.GetInt("HighScoreLevel4"))
                {
                    beatHighScore.enabled = true;
                    beatHighScore.text = "You've beaten the previous" + System.Environment.NewLine + "high score of " + PlayerPrefs.GetInt("HighScoreLevel4", 0);
                    PlayerPrefs.SetInt("HighScoreLevel4", currScore);
                    PlayerPrefs.SetInt("CurrentScoreLevel4", 0);

                }
                break;
            case 0:
                currScore = PlayerPrefs.GetInt("CurrentScoreBoss");
                score.text = "Your Score: " + currScore;
                if (currScore > PlayerPrefs.GetInt("HighScoreBoss"))
                {
                    beatHighScore.enabled = true;
                    beatHighScore.text = "You've beaten the previous" + System.Environment.NewLine + "high score of " + PlayerPrefs.GetInt("HighScoreBoss", 0);
                    PlayerPrefs.SetInt("HighScoreBoss", currScore);
                    PlayerPrefs.SetInt("CurrentScoreBoss", 0);
                }
                break;
        }

        PlayerPrefs.Save();

        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        GameObject laserObj = (GameObject)Instantiate(laserPrefab);
        GameObject reticleObj = (GameObject)Instantiate(reticlePrefab);

        laserObj.transform.SetParent(this.transform);
        reticleObj.transform.SetParent(this.transform);

        reticle = reticleObj.transform;
        laser = laserObj.GetComponent<LineRenderer>();

        reticleLight = reticle.gameObject.GetComponent<Light>();

        laser.gameObject.SetActive(true);
        reticle.gameObject.SetActive(true);
    }

    void Update()
    {

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        List<Vector3> waypoints = new List<Vector3>();
        waypoints.Add(transform.position);

        reticle.position = ray.origin + ray.direction * range;

        reticleLight.color = disabledColor;
        laser.startColor = disabledColor;
        laser.endColor = disabledColor;

        if (Physics.Raycast(ray, out hit, range))
        {

            if (hit.collider.CompareTag("Play"))
            {

                reticle.position = hit.point;
                reticleLight.color = enabledColor;

                laser.startColor = enabledColor;
                laser.endColor = enabledColor;

                target = hit;
                canChangeScenes = true;
            }
        }

        waypoints.Add(reticle.position);

        laser.positionCount = waypoints.Count;
        laser.SetPositions(waypoints.ToArray());

        if (IPL.GetRightTriggerInteracting() && canChangeScenes)
        {
            canChangeScenes = false;
            SceneManager.LoadScene("Main Menu");
        }

    }


}
