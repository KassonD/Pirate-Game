using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Variables
    public float walkSpeed = 1;
    public float runSpeed = 2;
    public float sneakSpeed = 0.5f;
    public float friction = 1;
    public float cameraSpeed = 1;
    public Transform mainCamera;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isAttacking = false;
    public int movementState = 0;  //  0-idle, 1-idle crouch, 2-walk, 3-run, 4-sneak

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Attack
        if (Input.GetMouseButton(0))
        {
            movementState = 5;
        }

        // Movement states
        if (moveDirection.magnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                movementState = 4;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                movementState = 3;
            }
            else
                movementState = 2;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            movementState = 1;
        }
        else
            movementState = 0;


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

        if (movementState == 3)
            multiplier = runSpeed;

        if (movementState == 4)
            multiplier = sneakSpeed;

        rb.AddForce(moveDirection * multiplier);

        // Friction
        rb.AddForce(-rb.linearVelocity * friction);
    }
}
