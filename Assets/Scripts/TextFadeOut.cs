using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class TextFadeOut : MonoBehaviour
{

    private Coroutine runningFade;

    public void SetText(string text)
    {
        GetComponent<TextMeshPro>().text = text;
        if (runningFade != null)
        {
            StopCoroutine(runningFade);
        }
        runningFade = StartCoroutine(FadeOut(GetComponent<TextMeshPro>()));
    }


    IEnumerator FadeOut(TMP_Text text, float time = 1f)
    {
        var remainingTime = time;
        while (remainingTime >= 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1, 0, (time - remainingTime) / time));
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        text.gameObject.SetActive(false);
    }

}
