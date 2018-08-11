using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    public float runSpeed = 6;
    public float dodgeSpeed = 18;
    public float jumpAttackSpeed = 12;
    public float rotationSpeed = 3600;

    private CharacterController controller;
    private Animator animator;
    private Vector3 currentDirection;

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
            Dodge(currentDirection);
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

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y + mouseLook.x * rotationSpeed, 0), Time.deltaTime * rotationSpeed);
  
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
        currentDirection = direction;
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
