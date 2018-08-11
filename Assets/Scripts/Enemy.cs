using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.GetComponent<Weapon>();
        if(weapon != null)
        {
            Debug.Log($"{name} hit by hero's {other.name} and took {weapon.currentDamage} damage");
        }
        
    }
}
