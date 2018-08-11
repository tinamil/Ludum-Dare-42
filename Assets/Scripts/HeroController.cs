using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    public float runSpeed = 6;
    public float dodgeSpeed = 18;
    public float jumpAttackSpeed = 12;
    public float rotationSpeed = 30;
    public Image damageIndicator;

    public bool IsInvincible = false;

    private CharacterController controller;
    private Animator animator;
    private Vector3 currentDirection;
    private float currentVertical;
    private float currentHorizontal;
    private Coroutine runningDamageRoutine;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var isDodgeStart = Input.GetButton("Jump");
        var leftClick = Input.GetMouseButton(0);
        var rightClick = Input.GetMouseButton(1);
        var mouseLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        var direction = transform.forward * vertical + transform.right * horizontal;

        if (controller.isGrounded == false)
        {
            Fall();
            return;
        }
        if (IsInState("Dodge"))
        {
            Dodge(currentDirection, currentHorizontal, currentVertical);
            return;
        }
        else if (IsInState("DodgeRecovery") || IsInState("Attack Run Recovery"))
        {
            Idle();
            return;
        }
        else if (IsInState("Attack Run"))
        {
            Move(currentDirection, jumpAttackSpeed);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y + mouseLook.x, 0), Time.deltaTime * rotationSpeed);

        animator.SetBool("Attack", leftClick);
        animator.SetBool("Block", rightClick);


        if (direction.magnitude < 1e-3)
        {
            Idle();
            return;
        }

        if (isDodgeStart)
        {
            Dodge(direction, horizontal, vertical);
        }
        else if (IsInState("RunForward"))
        {
            Run(direction, horizontal, vertical);
        }
    }

    private void Move(Vector3 direction, float speed, float horizontal = 0, float vertical = 0)
    {
        var gravity = Physics.gravity * Time.deltaTime;
        animator.SetFloat("Speed", speed);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Horizontal", horizontal);
        controller.Move(direction.normalized * speed * Time.deltaTime + gravity);
        currentDirection = direction;
        currentVertical = vertical;
        currentHorizontal = horizontal;
    }

    private void Run(Vector3 direction, float horizontal, float vertical)
    {
        Move(direction, runSpeed, horizontal, vertical);
    }

    private void Idle()
    {
        Move(Vector3.zero, 0);
    }

    private void Fall()
    {
        Idle();
    }

    private void Dodge(Vector3 direction, float horizontal, float vertical)
    {
        Move(direction, dodgeSpeed, horizontal, vertical);
    }

    private bool IsInState(string stateName)
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        var nextState = animator.GetNextAnimatorStateInfo(0);
        return state.IsName(stateName) || nextState.IsName(stateName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInvincible) return;
        Debug.Log($"{name} hit by enemy {other.name}");
        if (IsInState("Block"))
            animator.SetTrigger("OnHit");
        GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
        if (runningDamageRoutine != null)
        {
            StopCoroutine(runningDamageRoutine);
        }
        runningDamageRoutine = StartCoroutine(FadeOut(1, damageIndicator));
    }

    private IEnumerator FadeOut(float time, Image image)
    {
        image.gameObject.SetActive(true);
        var timeRemaining = time;
        while (timeRemaining > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(1, 0, (time - timeRemaining) / time));
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        image.gameObject.SetActive(false);
    }
}
