using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum MovementStates
{
    idle,
    idleSneak,
    walk,
    run,
    sneak
}

public class playerController : MonoBehaviour
{
    // Variables
    public float maxHealth = 100;
    public float walkSpeed = 1;
    public float runSpeed = 2;
    public float sneakSpeed = 0.5f;
    public float friction = 1;
    public float cameraSpeed = 1;
    public Transform mainCamera;
    public AudioClip walkAudio;
    public AudioClip runAudio;
    public AudioClip sneakAudio;
    public AudioClip takeDamageAudio;
    public AudioClip deathAudio;

    private Rigidbody2D rb;
    private AudioSource audioSrc;
    private Animator anim;
    private Vector2 moveDirection;
    private MovementStates movementState;
    private MovementStates prevMovementState;
    private bool isAttacking = false;
    private float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Attack
        if (Input.GetMouseButton(0))
        {
            isAttacking = true;
        }
        else
            isAttacking = false;

        // Check for state change
        bool stateChanged = false;
        if (movementState != prevMovementState)
        {
            stateChanged = true;
            prevMovementState = movementState;
        }

        // Movement states
        if (moveDirection.magnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                movementState = MovementStates.sneak;

                if (stateChanged)
                    PlayAudio(sneakAudio, true);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                movementState = MovementStates.run;

                if (stateChanged)
                    PlayAudio(runAudio, true);
            }
            else
            {
                movementState = MovementStates.walk;

                if (stateChanged)
                    PlayAudio(walkAudio, true);
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            movementState = MovementStates.idleSneak;
        }
        else
        {
            movementState = MovementStates.idle;
            audioSrc.Stop();
        }


    }

    private void LateUpdate()
    {
        // Camera
        Vector2 targetPosition = transform.position - mainCamera.position;
        mainCamera.position += new Vector3(targetPosition.x, targetPosition.y, 0) * Time.deltaTime * cameraSpeed;
    }

    private void FixedUpdate()
    {
        // Movement
        float multiplier = walkSpeed;

        if (movementState == MovementStates.run)
            multiplier = runSpeed;

        if (movementState == MovementStates.sneak)
            multiplier = sneakSpeed;

        rb.AddForce(moveDirection * multiplier);

        // Friction
        rb.AddForce(-rb.linearVelocity * friction);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        
        if (currentHealth == 0)
        {
            PlayAudio(takeDamageAudio);
        }
        else
            PlayAudio(deathAudio);
    }

    private void PlayAudio(AudioClip clip, bool loop = false)
    {
        audioSrc.Stop();
        audioSrc.clip = clip;
        audioSrc.loop = loop;
        audioSrc.Play();
    }
}
