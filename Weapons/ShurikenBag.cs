using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenBag : MonoBehaviour {

    [SerializeField]
    int numShurikens;
    int maxNumShurikens = 10;
    [SerializeField]
    bool shurikenInstantiatedInBag;

    public Sheath sheath;

    Input_Listeners IPL;

    public GameObject shurikenPrefab;

    void Start()
    {
        numShurikens = 0;
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Shuriken"))
        {
            shurikenInstantiatedInBag = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shuriken"))
        {
            shurikenInstantiatedInBag = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Left Hand") || other.CompareTag("Right Hand")) && //numShurikens < maxNumShurikens && 
            !shurikenInstantiatedInBag && !IPL.GetRightTriggerInteracting())
        {
            GameObject go = Instantiate(shurikenPrefab, transform.position, Quaternion.identity);
            numShurikens++;
        }
    }

    public void DecrementNumShurikens(int num)
    {
        numShurikens -= num;
        if (numShurikens < 0)
        {
            numShurikens = 0;
        }
    }

    public int GetNumShurikens()
    {
        return numShurikens;
    }

    public void IncrementNumShurikens(int num)
    {
        numShurikens += num;
        if (numShurikens > maxNumShurikens)
        {
            numShurikens = maxNumShurikens;
        }

    }


}
