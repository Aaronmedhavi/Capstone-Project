﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerOld : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] Animator lowerBody;
    [SerializeField] Animator upperBody;

    [Header("Player Script Settings")]
    [SerializeField] Player_Movement playerMovement;
    [SerializeField] Player_Attack playerAttack;

    [Header("Sensor Settings")]
    [SerializeField] GroundSensor groundSensor;

    float xInput;
    bool spaceInput, attackInput, dashInput;
    private Rigidbody2D rb2d;
    private readonly EventStateMachine<playerState, playereventState> SM = new();
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        SM.AddStates(new List<(playerState, BaseState)>()
        {
            //(playerState.idle, new IdleState(lowerBody, upperBody)),
            //(playerState.run, new RunState(lowerBody, upperBody, rb2d, playerMovement.speed)),
            //(playerState.air, new AirState(lowerBody, upperBody, rb2d, playerMovement.jumpPower * 2)),
        });
        SM.AddTriggerEvents(new()
        {
            (playereventState.grounded, playerState.idle),
            (playereventState.onAir, playerState.air)
        });
        SM.AddConditions(new List<EventTransition<playerState>>()
        {
            new EventTransition<playerState>(playerState.idle, playerState.run, () => Mathf.Abs(rb2d.velocity.x) > 0.05f && isGrounded),
            new EventTransition<playerState>(playerState.run, playerState.idle, () => Mathf.Abs(rb2d.velocity.x) <= 0.05f && isGrounded),
            new EventTransition<playerState>(playerState.air, playerState.idle, () => isGrounded)
        });
        SM.AddGlobalConditions(new List<EventTransition<playerState>>()
            {
            //    (new EventTransition<playerState>(playerState.air, playerState.idle, () => !IsGrounded)),
                (new EventTransition<playerState>(playerState.idle, playerState.air, () => isGrounded))
            });
        playerMovement.Set(upperBody, lowerBody, groundSensor, rb2d);
        playerAttack.Set(upperBody, lowerBody, rb2d);
        //groundSensor.OnEnter += GroundSensor_OnTouchingGround;
        //groundSensor.OnExit += GroundSensor_OnAir;
        SM.SetState(playerState.idle);
    }
    private void Start()
    {
        SM.OnEnter();
    }
    private void Update()
    {
        HandleInput();
        SM.OnLogic();
        playerMovement.Move(xInput, isGrounded);
        playerMovement.Jump(spaceInput);
        playerMovement.Dash(dashInput);
        playerAttack.Attack(attackInput);
    }
    private void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");
        spaceInput = Input.GetKeyDown(KeyCode.Space);
        attackInput = Input.GetKeyDown(KeyCode.Mouse0);
        dashInput = Input.GetKeyDown(KeyCode.Mouse1);
    }
    bool isGrounded => groundSensor.IsGrounded;
    void GroundSensor_OnTouchingGround() => SM.Trigger(playereventState.grounded);
    void GroundSensor_OnAir() => SM.Trigger(playereventState.onAir);
}
