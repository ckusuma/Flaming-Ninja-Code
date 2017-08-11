using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour 
{

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

	private bool canChangeToFirstLevel;
    private bool canChangeToTutorial;
	// Use this for initialization
	void Start()
	{
		IPL = Singleton_Service.GetSingleton<Input_Listeners> ();
		trackedObj = GetComponent<SteamVR_TrackedObject> ();

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

	// Update is called once per frame
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
                canChangeToFirstLevel = true;
            }
            else if (hit.collider.CompareTag("Tutorial"))
            {

                reticle.position = hit.point;
                reticleLight.color = enabledColor;

                laser.startColor = enabledColor;
                laser.endColor = enabledColor;

                target = hit;
                canChangeToTutorial = true;
            }
        }
        else
        {
            canChangeToTutorial = false;
            canChangeToFirstLevel = false;
        }

        waypoints.Add(reticle.position);

        laser.positionCount = waypoints.Count;
        laser.SetPositions(waypoints.ToArray());

        if (IPL.GetRightTriggerInteracting() && canChangeToFirstLevel)
        {
            canChangeToFirstLevel = false;
            SceneManager.LoadScene("Prototype Scene");
        }
        else if (IPL.GetRightTriggerInteracting() && canChangeToTutorial)
        {
            canChangeToTutorial = false;
            SceneManager.LoadScene("Tutorial Level");
        }

	}

}
