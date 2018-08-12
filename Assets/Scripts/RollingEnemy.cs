using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollingEnemy : MonoBehaviour
{

    GameObject hero;
    Coroutine runningFade;
    public float HitPoints = 50;
    public TextFadeOut HPText;
    public GameObject rollingModel;
    public float radius;
    public float speed;

    public AudioClip[] deathSounds;
    public AudioClip[] onHitSounds;

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
    void Update()
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
        HPText.SetText(damage.ToString("f0"));
        if (runningFade != null)
        {
            StopCoroutine(runningFade);
        }
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            SoundManager.PlayClip(deathSounds);
            HPText.transform.SetParent(null);
            Destroy(this.gameObject);
        }
    }
    

    void Roll(Vector3 direction)
    {
        var distance = Time.deltaTime * speed;
        var rollDegrees = distance / circumference * 360;
        var axis = Vector3.Cross(Vector3.up, direction);
        rollingModel.transform.Rotate(axis, rollDegrees, Space.World);
        transform.Translate(direction.normalized * distance);
    }
}
