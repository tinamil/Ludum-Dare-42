using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossArea : MonoBehaviour {

    public GameObject[] deactivate;
    public GameObject[] activate;

    private void OnTriggerEnter(Collider other)
    {
        foreach(var obj in deactivate)
            obj.SetActive(false);
        foreach(var obj in activate) 
            obj.SetActive(true);
    }
}
