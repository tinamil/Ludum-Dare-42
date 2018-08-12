using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollingEnemy : MonoBehaviour
{

    GameObject hero;
    Coroutine runningFade;
    public float HitPoints = 50;
    public TMP_Text HPText;
    public GameObject rollingModel;
    public float radius;
    public float speed;

    private readonly float HitCooldown = .5f;
    private float lastHit;

    private float circumference;

    // Use this for initialization
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        circumference = 2 * Mathf.PI * radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var heroVector = hero.transform.position - transform.position;
        heroVector.y = 0;
        Roll(heroVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.GetComponent<Weapon>();
        if (weapon != null)
        {
            if (Time.time < lastHit + HitCooldown)
            {
                return;
            }
            else
            {
                Debug.Log($"{name} hit by hero's {other.name} and took {weapon.currentDamage} damage");
                TakeDamage(weapon.currentDamage);
                lastHit = Time.time;
            }
        }

    }

    void TakeDamage(float damage)
    {
        HPText.gameObject.SetActive(true);
        HPText.text = damage.ToString("f0");
        if (runningFade != null)
        {
            StopCoroutine(runningFade);
        }
        HitPoints -= damage;
        if (HitPoints <= 0) Destroy(this.gameObject);
        runningFade = StartCoroutine(FadeOut(HPText));
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

    void Roll(Vector3 direction)
    {
        var distance = Time.fixedDeltaTime * speed;
        var rollDegrees = distance / circumference * 360;
        var axis = Vector3.Cross(Vector3.up, direction);
        rollingModel.transform.Rotate(axis, rollDegrees, Space.World);
        transform.Translate(direction.normalized * distance);
    }
}
