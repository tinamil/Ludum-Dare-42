using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    GameObject hero;
    Coroutine runningFade;
    public GameObject[] HitTargets;
    public float RotationSpeed;
    public float HitPoints = 50;
    public bool CanMove = false;

    private readonly float HitCooldown = .5f;
    private float lastHit;
    public AudioClip[] deathSounds;
    public AudioClip[] onHitSounds;

    // Use this for initialization
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var heroVector = hero.transform.position - transform.position;
        if (heroVector.magnitude < 10)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heroVector), Time.deltaTime * RotationSpeed);
            foreach (var obj in HitTargets)
            {
                obj.transform.position = hero.transform.position + new Vector3(0, 1f, 0);
            }
        }

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
        GetComponent<Animator>().SetTrigger("OnHit");
        var textComponent = GetComponentInChildren<TextFadeOut>(true);
        textComponent.gameObject.SetActive(true);
        textComponent.SetText(damage.ToString("f0"));
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            SoundManager.PlayClip(deathSounds);
            textComponent.transform.SetParent(null);
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(onHitSounds[Random.Range(0, onHitSounds.Length - 1)]);
        }
    }


}
