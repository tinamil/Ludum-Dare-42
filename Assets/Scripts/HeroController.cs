using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    public float runSpeed = 6;
    public float dodgeSpeed = 18;
    public float dodgeDuration = 1;
    public float dodgeCooldown = 2;

    private CharacterController controller;
    private Animator animator;
    private float lastDodgeTime = float.NegativeInfinity;
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
        if (controller.isGrounded)
        {
            if (Time.time < lastDodgeTime + dodgeDuration)
            {
                Move(dodgeDirection, dodgeSpeed);
            }
            else if (Time.time < lastDodgeTime + dodgeCooldown)
            {
                Move(Vector3.zero, 0);
            }

            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");
            var isDodgeStart = Input.GetButton("Jump");

            var direction = transform.forward * vertical + transform.right * horizontal;

            if (direction.magnitude > 1e-3)
            {
                if (isDodgeStart)
                {
                    Dodge(direction);
                }
                else
                {
                    Move(direction, runSpeed);
                }
            }
            else
            {
                Move(Vector3.zero, 0);
            }

        }
        else
        {
            Move(Vector3.zero, 0);
        }
    }

    private void Move(Vector3 direction, float speed)
    {
        var gravity = Physics.gravity * Time.deltaTime;
        animator.SetFloat("Speed", speed);
        controller.Move(direction.normalized * speed * Time.deltaTime + gravity);
    }

    private void Dodge(Vector3 direction)
    {
        Move(direction, dodgeSpeed);
        lastDodgeTime = Time.time;
        animator.SetTrigger("Dodge");
        dodgeDirection = direction.normalized;
    }
}
