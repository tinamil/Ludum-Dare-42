using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossArea : MonoBehaviour {

    public GameObject OldArea, NewArea;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Swapping in original (non-hole) boss area");
        OldArea.SetActive(false);
        NewArea.SetActive(true);
    }
}
