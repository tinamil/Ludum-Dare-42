using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeroController : MonoBehaviour
{
    public float runSpeed = 6;
    public float dodgeSpeed = 18;
    public float dodgeDuration = 1;
    public float dodgeCooldown = 2;

    private CharacterController controller;
    private float lastDodgeTime = float.NegativeInfinity;
    private Vector3 dodgeDirection;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            var currentRunSpeed = runSpeed * Time.deltaTime;
            var currentDodgeSpeed = dodgeSpeed * Time.deltaTime;
            var gravity = Physics.gravity * Time.deltaTime;
            if (Time.time < lastDodgeTime + dodgeDuration)
            {
                var flags = controller.Move(dodgeDirection.normalized * currentDodgeSpeed + gravity);
                return;
            }
            else if (Time.time < lastDodgeTime + dodgeCooldown)
            {
                return;
            }

            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");
            var isDodgeStart = Input.GetButton("Jump");

            var direction = transform.forward * vertical + transform.right * horizontal;

            if (direction.magnitude > 1e-3)
            {
                if (isDodgeStart)
                {
                    controller.Move(direction.normalized * currentDodgeSpeed + gravity);
                    lastDodgeTime = Time.time;
                    dodgeDirection = direction.normalized;
                }
                else
                {
                    controller.Move(direction.normalized * currentRunSpeed + gravity);
                }
            }

        }
        else
        {
            var flags = controller.Move(Physics.gravity * Time.deltaTime);
        }
    }
}
