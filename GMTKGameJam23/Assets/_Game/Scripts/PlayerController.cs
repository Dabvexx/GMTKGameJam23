using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Custom player controller using character controller and unity input action system.
/// Credit to Kurt-Dekker for base code since unity documentation sucks sometimes.
/// https://forum.unity.com/threads/how-to-correctly-setup-3d-character-movement-in-unity.981939/#post-6379746
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    // TODO: it would likely be more effecient to have a state machine / enum for to find what state we are in since we have multiple states the player can be in.

    // Variables.
    public static PlayerController Instance { get; private set; }
    public CharacterController controller { get; private set; }

    // Input action vars.
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction sprintAction;

    // Movement Vars
    private float verticalVelocity;
    private Vector3 prevDir;
    private Vector3 move;

    [Space(10), Header("Ground Variables")]
    [SerializeField, Tooltip("Speed of player on the ground.")] private float groundSpeed = 15f;
    [SerializeField, Tooltip("Speed of player on the ground.")] private float sprintSpeed = 25f;
    [SerializeField, Tooltip("Speed of player turning to face the movement angle.")] private float turnSpeed = 2f;
    // The hit data of the ground the controller is standing on.
    [HideInInspector] public ControllerColliderHit groundData { get; private set; } = null;
    [HideInInspector] public bool isPlayerGrounded { get; private set; } = true;

    [Space(10), Header("Air Variables")]
    [SerializeField, Tooltip("Speed of player in the air.")] private float airSpeed = 13f;
    [SerializeField, Tooltip("Speed of player in the air.")] private float airSprintSpeed = 20f;
    [SerializeField, Tooltip("Amount of time before the player can jump again."), Range(0f, 2f)] private float jumpDelay = .2f;
    [SerializeField, Tooltip("Height of jump off the ground."), Min(0f)] private float jumpHeight = 2.0f;
    [SerializeField, Tooltip("Amount of jumps in the air off the ground."), Min(0)] private int maxMultiJumps = 1;
    [SerializeField, Tooltip("Height of jumps in midair."), Min(0)] private float multiJumpHeight = 1.0f;
    [SerializeField, Tooltip("How fast the player is pulled down.")] public float gravity = -9.81f;
    // The current jumps you have.
    private int multiJumpAmount;
    // The current timer counting down before the player can jump.
    private float jumpDelayTimer = 0f;
    // ground timer so the player can go down slopes and stairs without being off the ground.
    private float groundedTimer = 0f;

    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        // Singleton moment.

        if (Instance != null && Instance != this)
        {
            // That ain't Drake.
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
        sprintAction = playerInput.actions["Sprint"];
        

        multiJumpAmount = maxMultiJumps;
        groundData = new ControllerColliderHit();
    }

    private void LateUpdate()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Reset the movement
        move = new Vector3(moveInput.x, 0, 0);//moveInput.y);

        // Apply gravity then check if grounded.
        ApplyGravity();
        CheckIfGrounded();

        if (groundedTimer > 0)
        {
            move = CalculateGroundMovement();
            MoveRelativeToCam();
            FaceTowardMovementAngle();
        }

        else
        {
            move = CalculateAirMovement();
            MoveRelativeToCam();
            FaceTowardMovementAngle();
        }

        // Reduce the delay timer every frame
        if (jumpDelayTimer > 0)
        {
            jumpDelayTimer -= Time.deltaTime;
        }

        if (jumpAction.triggered)
        {
            HandleJump();
        }

        // Add the veritcal velocity at the end.
        // Adding to the velocity since it gets reset to zero the beginning of the next frame.
        // Also needs to be added or sliding doesnt function propperly
        move.y += verticalVelocity;


        // Apply movement.
        controller.Move(move * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        groundData = hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + move / 5);
    }

    #endregion Unity Methods

    #region Private Methods

    // Private Methods.

    // TODO: Check if the input is within a radius behind the player (eg. 170 - 190), if its in this range. do a turn around anim.
    // Probably do with the dot product (eg, if the normalized angle we are going dot product the new angle <= -.5)

    // TODO: make this into a calculate x and z and calculate Y so that sliding can skip lateral movement so you can just go down the slope.
    // Slope movement could be able to be nudged left and right.
    /// <summary>
    /// Calculates the players movement while walking on the ground.
    /// </summary>
    /// <param name="moveInput">The player input this frame.</param>
    /// <returns></returns>
    private Vector3 CalculateGroundMovement()
    {
        if (sprintAction.IsPressed())
        {
            move *= sprintSpeed;
        }
        else
        {
            move *= groundSpeed;
        }

        // Project on plane to make sure the player interacts properly with slopes.
        return move;
    }

    /// <summary>
    /// Calculates the players movement while moving in the air.
    /// </summary>
    /// <param name="moveInput">The player input this frame.</param>
    /// <returns></returns>
    private Vector3 CalculateAirMovement()
    {
        //Debug.Log(Vector3.ProjectOnPlane(move, groundData.normal));
        move = Vector3.ProjectOnPlane(move, groundData.normal);

        if (sprintAction.IsPressed())
        {
            move *= airSprintSpeed;
        }
        else
        {
            move *= airSpeed;
        }

        return move;
    }

    /// <summary>
    /// Handles checking if you can and executing jumps.
    /// </summary>
    private void HandleJump()
    {
        // TODO: Make it so the longer you press space the higher you go to a certain max.

        // When the player jumps, the slope no longer affects them.
        // The easiest way I found to do this is to remove hit from ground data so the slide checker retruns false.
        groundData = new ControllerColliderHit();

        if (jumpDelayTimer > 0)
        {
            // Can't jump yet.
            return;
        }

        if (groundedTimer > 0)
        {
            // When the player jumps on the ground they are no longer grounded, but can still double jump.
            groundedTimer = 0;
            jumpDelayTimer = jumpDelay;
            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
        else if (multiJumpAmount > 0)
        {
            multiJumpAmount--;
            jumpDelayTimer = jumpDelay;
            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(multiJumpHeight * -3 * gravity);
        }

        // Player isnt on the ground and is out of double jumps
    }

    /// <summary>
    /// Checks if the player is grounded, as well as handling a few things around touching the ground.
    /// </summary>
    private void CheckIfGrounded()
    {
        // BUG: Player doesnt walk down slopes propperly
        // possible solution, do a projection on plane to make sure they stay on the slope.
        isPlayerGrounded = controller.isGrounded;

        if (isPlayerGrounded) //&& !isSliding)
        {
            // cooldown interval to allow reliable jumping even whem coming down ramps
            groundedTimer = 0.2f;
            multiJumpAmount = maxMultiJumps;
        }

        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        // slam into the ground
        if (isPlayerGrounded && verticalVelocity < 0)
        {
            // hit ground
            verticalVelocity = 0f;
        }
    }

    /// <summary>
    /// Make movement relative to the camera by getting the cameras forward and right transform.
    /// </summary>
    private void MoveRelativeToCam()
    {
        var cam = Camera.main;

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        forward.Normalize();

        right.y = 0;
        right.Normalize();

        move = forward * move.z + right * move.x;
    }

    /// <summary>
    /// Applies gravity. Simple as that.
    /// </summary>
    private void ApplyGravity()
    {
        // For some reason, this will pull far too hard unless multiplied by Time.deltaTime.
        // However, changing gravity itself messes with the jump function as it refrences gravity for its calculation.
        verticalVelocity += gravity * Time.deltaTime;
    }

    /// <summary>
    /// Helper function to make the character face the direction we are moving in.
    /// </summary>
    private void FaceTowardMovementAngle()
    {
        if (-move.x == 0)
        {
            return;
        }

        if (move.magnitude > Mathf.Epsilon)
        {
            // Spherically interp for smooth turning.
            transform.rotation = Quaternion.Slerp
                (
                transform.rotation,
                Quaternion.LookRotation(new Vector3(-move.x, 0, 0)),
                turnSpeed * Time.deltaTime
                );
        }
    }

    #endregion Private Methods
}
