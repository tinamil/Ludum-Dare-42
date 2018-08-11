using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    GameObject hero;
    Coroutine runningFade;

    // Use this for initialization
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(hero.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.GetComponent<Weapon>();
        if (weapon != null)
        {
            Debug.Log($"{name} hit by hero's {other.name} and took {weapon.currentDamage} damage");
            TakeDamage(weapon.currentDamage);
        }

    }

    void TakeDamage(float damage)
    {
        GetComponent<Animator>().SetTrigger("OnHit");
        var textComponent = GetComponentInChildren<TMPro.TMP_Text>(true);
        textComponent.gameObject.SetActive(true);
        textComponent.text = damage.ToString();
        if (runningFade != null)
        {
            StopCoroutine(runningFade);
        }
        runningFade = StartCoroutine(FadeOut(textComponent));
    }


    IEnumerator FadeOut(TMPro.TMP_Text text, float time = 1f)
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
