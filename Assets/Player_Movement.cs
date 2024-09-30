using System.Collections.Generic;
using UnityEngine;
public enum playerState
{
    idle, run, attack, air, run_air
}
//private StateMachine<playerState, playereventState> SM = new();

//SM.AddStates(new List<(playerState, BaseState)>()
//{
//    (playerState.idle, new IdleState(animator)),
//    (playerState.run, new RunState(this, Walking_Speed, Acceleration, rb, animator)),
//    (playerState.air, new AirState(rb, animator, jumpPower)),
//    (playerState.run_air, new RunState(this, Walking_Speed, flying_Acceleration, rb, animator))
//});
//SM.AddTriggerEvents(new List<(playereventState, Transition<playerState>)>()
//{
//    (playereventState.grounded, new Transition<playerState>(playerState.air, playerState.idle)),
//    (playereventState.onAir, new Transition<playerState>(playerState.idle, playerState.air))
//});
//SM.AddConditions(new List<Transition<playerState>>()
//{
//    new Transition<playerState>(playerState.idle, playerState.run, () => xInput != 0 && isGrounded),
//    new Transition<playerState>(playerState.run, playerState.idle, () => xInput == 0 && isGrounded)
//});
//groundSensor.OnEnter += groundSensor_OnEnter;
public class GroundedState : BaseState
{
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
    public override void OnLogic()
    {
    }
}
public class AiredState : BaseState
{
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
    public override void OnLogic()
    {
    }
}
public class Player_Movement : MonoBehaviour
{
    public enum terrainState { air, ground }
    public enum playereventState { }

    private StateMachine<terrainState, playereventState> SM = new();

    [SerializeField] Animator upperBody;
    [SerializeField] Animator lowerBody;

    [Header("Player Movement Settings")]
    [SerializeField] float Walking_Speed;
    [SerializeField] float Acceleration;
    [SerializeField] float jumpPower;
    [SerializeField] float flying_Acceleration;

    [Header("Sensors")]
    [SerializeField] GroundSensor groundSensor;
    
    Rigidbody2D rb;
    public float xInput { get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        SM.AddStates(new List<(terrainState, BaseState)>()
        {
            (terrainState.ground, new GroundedState()),
            (terrainState.air, new AiredState())
        });
        SM.AddConditions(new List<Transition<terrainState>>()
        {
            new Transition<terrainState>(terrainState.ground, terrainState.air, () => !isGrounded),
            new Transition<terrainState>(terrainState.air, terrainState.ground, () =>  isGrounded)
        });

        SM.SetState(terrainState.ground);
        SM.OnEnter();
    }
    private void Update()
    {
        HandleInput();
        HandleJumpInput();
        SM.OnLogic();   
    }
    void HandleJumpInput()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }
    bool isGrounded => groundSensor.isGrounded;
}