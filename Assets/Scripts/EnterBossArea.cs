using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossArea : MonoBehaviour {

    public GameObject oldArena;
    public GameObject newArena;

    private void OnTriggerEnter(Collider other)
    {
        oldArena.SetActive(false);
        newArena.SetActive(true);
    }
}
