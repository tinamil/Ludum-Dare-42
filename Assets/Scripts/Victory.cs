using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{

    public Transform standPosition;
    public Cinemachine.CinemachineVirtualCamera vmCamera;
    public GameObject victoryText;
    bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag == "Player" && done == false)
        {
            var player = other.transform.root.gameObject;
            player.transform.position = standPosition.position;
            player.transform.rotation = standPosition.rotation;
            player.GetComponent<Animator>().SetBool("Victory", true);
            vmCamera.GetComponent<Animator>().SetTrigger("Victory");
            StartCoroutine(DisplayText());
            done = true;
        }
    }

    IEnumerator DisplayText()
    {
        var time = Time.time;
        yield return new WaitForSeconds(6);
        victoryText.SetActive(true);
        victoryText.GetComponent<TMPro.TextMeshProUGUI>().text = $"Congratulations. \n\n  " +
            $"You completed the run in \n " +
            $"{time:f2} seconds";
        yield return new WaitForSeconds(18);
        SceneManager.LoadScene(0);
    }
}
