using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    public float runSpeed = 6;
    public float dodgeSpeed = 18;

    private CharacterController controller;
    private Animator animator;
    private Vector3 dodgeDirection;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var isDodgeStart = Input.GetButton("Jump");
        var leftClick = Input.GetMouseButton(0);
        var rightClick = Input.GetMouseButton(1);

        var direction = transform.forward * vertical + transform.right * horizontal;

        if (controller.isGrounded == false)
        {
            Fall();
            return;
        }
        if (IsInState("Dodge"))
        {
            Dodge(dodgeDirection);
            return;
        }
        else if (IsInState("DodgeRecovery"))
        {
            Idle();
            return;
        }


        animator.SetBool("Attack Right", leftClick);
        animator.SetBool("Attack Down", rightClick);


        if (direction.magnitude < 1e-3)
        {
            Idle();
            return;
        }

        if (isDodgeStart)
        {
            Dodge(direction);
        }
        else if (IsInState("Idle") || IsInState("RunForward"))
        {
            Run(direction);
        }
    }

    private void Move(Vector3 direction, float speed)
    {
        var gravity = Physics.gravity * Time.deltaTime;
        animator.SetFloat("Speed", speed);
        controller.Move(direction.normalized * speed * Time.deltaTime + gravity);
    }

    private void Run(Vector3 direction)
    {
        Move(direction, runSpeed);
    }

    private void Idle()
    {
        Move(Vector3.zero, 0);
    }

    private void Fall()
    {
        Idle();
    }

    private void Dodge(Vector3 direction)
    {
        Move(direction, dodgeSpeed);
        dodgeDirection = direction.normalized;
    }

    private bool IsInState(string stateName)
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        var nextState = animator.GetNextAnimatorStateInfo(0);
        return state.IsName(stateName) || nextState.IsName(stateName);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name} hit by enemy {other.name}");
    }
}
