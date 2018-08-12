using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnTargetDeath : MonoBehaviour {

    public GameObject target;
	
	// Update is called once per frame
	void Update () {
        if (target == null) Destroy(this.gameObject);
	}
}
