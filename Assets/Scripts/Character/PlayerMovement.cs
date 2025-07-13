using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    /// <summary>
    ///  Walking speed of the character. Set this to the desired value in the editor. Default is 4f (which is fine)
    /// </summary>
    [Header("Movement")]
    public float walkSpeed = 4f;
    
    /// <summary>
    /// Sprinting speed of the character. Always aim for 1.5x or higher than walking speed.
    /// </summary>
    public float sprintSpeed = 7f;
    
    /// <summary>
    /// Drag value of the ground. Without this, character feels flying on the ground.
    /// </summary>
    public float groundDrag;
    
    /// <summary>
    /// Jumping force. Affects how high the character can jump agains the gravity.
    /// </summary>
    public float jumpForce;
    
    /// <summary>
    /// Determines how frequent the character can jump. Never set to 0.
    /// </summary>
    public float jumpCooldown;
    
    /// <summary>
    /// Forgot why did i wrote this. 
    /// </summary>
    public float airMultiplier;
    
    /// <summary>
    /// Character can not jump if this is not true.
    /// </summary>
    bool readyToJump;

    /// <summary>
    /// Current speed of the character. Dont fuck with this & never hardcode.
    /// </summary>
    private float currentSpeed;
    
    /// <summary>
    /// Stamina bar UI element.
    /// </summary>
    [Header("UI")]
    public Slider staminaBar;
    
    /// <summary>
    /// Stamina bar canvas group. We use this to set it visible.
    /// </summary>
    public CanvasGroup staminaBarGroup;
    
    /// <summary>
    /// Max value of the stamina that can get.
    /// </summary>
    [Header("Stamina")]
    public float maxStamina = 100f;
    
    /// <summary>
    /// Current stamina value. Also dont harcdoce this.
    /// </summary>
    public float stamina;
    
    /// <summary>
    /// Stamina draining rate when sprinting or doing other activities. Drains by xf in seconds.
    /// </summary>
    public float staminaDrainRate = 20f;
    
    /// <summary>
    /// Stamina regeneration rate. Also in seconds.
    /// </summary>
    public float staminaRegenRate = 15f;   
    
    /// <summary>
    /// Determines if the character is sprinting.
    /// </summary>
    private bool isSprinting;

    /// <summary>
    /// Jumping key. Set this to the desired key in the editor.
    /// </summary>
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    
    /// <summary>
    /// Sprinting key. Set this to the desired key in the editor.
    /// </summary>
    public KeyCode sprintKey = KeyCode.LeftShift;

    /// <summary>
    /// Sets the character height.
    /// </summary>
    [Header("Ground Check")]
    public float playerHeight;
    
    /// <summary>
    /// Determines what layer is the ground.
    /// </summary>
    public LayerMask whatIsGround;
    
    /// <summary>
    /// Determines if the character is grounded.
    /// </summary>
    bool grounded;

    /// <summary>
    /// Orientation of the character. 
    /// </summary>
    public Transform orientation;

    /// <summary>
    /// Horizontal input value.
    /// </summary>
    float horizontalInput;
    
    /// <summary>
    /// Vertical input value.
    /// </summary>
    float verticalInput;

    /// <summary>
    /// Direction of the characters movement.
    /// </summary>
    Vector3 moveDirection;

    /// <summary>
    /// The one and only, The rigidbody.
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// Unity engine constructor method. Dont fuck with this if you dont know what you are doing.
    /// </summary>
    /// <returns></returns>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        stamina = maxStamina;
        currentSpeed = walkSpeed;
    }

    /// <summary>
    /// Unity engine method that called every frame. Very costly for performance, thread lightly.
    /// </summary>
    /// <returns></returns>
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        HandleStamina();
        SpeedControl();

        rb.linearDamping = grounded ? groundDrag : 0;
    }

    /// <summary>
    /// Unity engine method that is called every fraction of time. Even more costly than update but physix can work
    /// wonders with this.
    /// </summary>
    /// <returns></returns>
    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Input handler. Gets the keys & calls the functions.
    /// </summary>
    /// <returns></returns>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    /// <summary>
    /// Moves the player. Orientation based.
    /// </summary>
    /// <returns></returns>
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    /// <summary>
    /// Controls the speed of the character
    /// </summary>
    /// <returns> void </returns>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Handles the stamina management & stamina bar canvas.
    /// </summary>
    /// <returns> void </returns>
    private void HandleStamina()
    {
        // update bar when sprint toggles
        if (staminaBar != null)
            staminaBar.value = stamina;

        // show bar only while sprinting
        if (staminaBarGroup != null)
            staminaBarGroup.alpha = isSprinting ? 1f : 0f;
        
        bool wantsToSprint = Input.GetKey(sprintKey) && grounded && (horizontalInput != 0 || verticalInput != 0);

        if (wantsToSprint && stamina > 0f)
        {
            isSprinting = true;
            currentSpeed = sprintSpeed;
            stamina -= staminaDrainRate * Time.deltaTime;
            stamina = Mathf.Max(stamina, 0f);
        }
        else
        {
            isSprinting = false;
            currentSpeed = walkSpeed;
            if (stamina < maxStamina)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
            }
        }
    }

    /// <summary>
    /// Handles jumping logic.
    /// </summary>
    /// <returns></returns>
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Must be called when returned to the ground.
    /// </summary>
    /// <returns></returns>
    private void ResetJump()
    {
        readyToJump = true;
    }
}