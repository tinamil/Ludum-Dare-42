using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    static SoundManager instance;
    public AudioClip[] backgroundMusic;

    private void Start()
    {
        instance = this;
        StartCoroutine(BackgroundMusic());
    }

    static public void PlayClip(params AudioClip[] clip)
    {
        instance?.GetComponent<AudioSource>().PlayOneShot(clip[Random.Range(0, clip.Length - 1)]);
    }

    private IEnumerator BackgroundMusic()
    {
        while (true)
        {
            if (backgroundMusic.Length > 0)
            {
                GetComponent<AudioSource>().clip = backgroundMusic[Random.Range(0, backgroundMusic.Length - 1)];
                GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(95);
        }
    }
}
