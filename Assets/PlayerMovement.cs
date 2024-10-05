using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSmoothTime;
    public float walkSpeed;
    public float runSpeed;

    private CharacterController controller;

    private Vector3 CurrentMoveVelocity;
    private Vector3 MoveDampvelocity;

    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 PlayerInput = new Vector3()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };

        if (PlayerInput.magnitude > 1f)
        {
            PlayerInput.Normalize();
        }

        Vector3 MoveVector = transform.TransformDirection(PlayerInput);

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        CurrentMoveVelocity = Vector3.SmoothDamp(
            CurrentMoveVelocity,
            MoveVector * currentSpeed,
            ref MoveDampvelocity,
            MoveSmoothTime
            );

        controller.Move(CurrentMoveVelocity * Time.deltaTime);
    }
}
