using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Victory : MonoBehaviour
{

    public Transform standPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag == "Player")
        {
            var player = other.transform.root.gameObject;
            player.transform.position = standPosition.position;
            player.transform.rotation = standPosition.rotation;
            player.GetComponent<Animator>().SetBool("Victory", true);
            player.GetComponent<PlayableDirector>().Play();
        }
    }

}
