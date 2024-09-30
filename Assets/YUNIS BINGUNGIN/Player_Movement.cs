//using UnityEngine;

//public class Player_Movement : Core
//{
//    [Header("=====States=======")]
//    public AirState airState;
//    public IdleState idleState;
//    public RunState runState;
//    [Header("==================\n\nPlayer Movement Settings")]
//    [SerializeField] private float Walking_Speed;
//    [SerializeField] float Acceleration;

//    [SerializeField] KeyCode JumpButton;
//    [SerializeField] float jumpPower;

//    bool stateComplete;
//    float xInput;

//    private void Start()
//    {
//        SetupInstances();
//        SM.Set(idleState);
//    }
//    private void Update()
//    {
//        GetMainInput();
//        HandleJump();
//        FaceInput();
//        HandleMovement();

//        stateSelection();
//        SM.state.OnLogic();
//    }
//    public void HandleJump()
//    {
//        if(isGrounded && Input.GetKeyDown(JumpButton))
//        {
//            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
//        }
//    }
//    public void HandleMovement()
//    {
//        if(xInput != 0)
//        {
//            //get the value of force / velocity difference (makin deket makin kecil nambah, makin ngelawan makin besar nambah)
//            //        <=       [][][]       =>
//            //     direction            -rb.velocity
//            float accelX = xInput * Walking_Speed - rb.velocity.x;

//            float movement = accelX * Acceleration;
//            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
//        }
//        else
//        {
//            rb.velocity = new Vector2(0, rb.velocity.y);
//        }
//    }
//    void stateSelection()
//    {
//        if (isGrounded)
//        {
//            if (xInput == 0)
//            {
//                SM.Set(idleState);
//            }
//            else
//            {
//                SM.Set(runState);
//            }
//        }
//        else
//        {
//            SM.Set(airState);
//        }
//        SM.state.OnEnter();
//    }
//    public void GetMainInput()
//    {
//        xInput = Input.GetAxis("Horizontal");
//    }
//    public void FaceInput()
//    {
//        if (xInput != 0)
//        {
//            float direction = Mathf.Sign(xInput);
//            transform.localScale = new Vector3(direction, 1, 1);
//        }
//    }
//}
