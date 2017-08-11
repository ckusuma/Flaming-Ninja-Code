using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public Text score;

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

    void OnEnable()
    {
        int currLevel = PlayerPrefs.GetInt("CurrentLevel");
        switch (currLevel)
        {
            case 1:
                int s = PlayerPrefs.GetInt("CurrentScoreLevel1");
                score.text = "Your Score: " + s;
                PlayerPrefs.SetInt("CurrentScoreLevel1", 0);
                break;
            case 2:
                s = PlayerPrefs.GetInt("CurrentScoreLevel2");
                score.text = "Your Score: " + s;
                PlayerPrefs.SetInt("CurrentScoreLevel2", 0);
                break;
            case 3:
                s = PlayerPrefs.GetInt("CurrentScoreLevel3");
                score.text = "Your Score: " + s;
                PlayerPrefs.SetInt("CurrentScoreLevel3", 0);
                break;
            case 4:
                s = PlayerPrefs.GetInt("CurrentScoreLevel4");
                score.text = "Your Score: " + s;
                PlayerPrefs.SetInt("CurrentScoreLevel4", 0);
                break;
            case 0:
                s = PlayerPrefs.GetInt("CurrentScoreBoss");
                score.text = "Your Score: " + s;
                PlayerPrefs.SetInt("CurrentScoreBoss", 0);
                break;
        }

        PlayerPrefs.Save();


    }

    void Start()
    {
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
