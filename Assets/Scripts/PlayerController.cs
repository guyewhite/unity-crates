using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            // Only freeze Y position and rotation
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            rb.mass = 2f; // Make player heavier than crates
        }
    }

    void Update()
    {
        // Get input
        moveDirection = Vector3.zero;
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.upArrowKey.isPressed)
                moveDirection += Vector3.forward;
            if (keyboard.downArrowKey.isPressed)
                moveDirection += Vector3.back;
            if (keyboard.leftArrowKey.isPressed)
                moveDirection += Vector3.left;
            if (keyboard.rightArrowKey.isPressed)
                moveDirection += Vector3.right;
        }
    }

    void FixedUpdate()
    {
        // Apply physics-based movement
        if (rb != null && moveDirection != Vector3.zero)
        {
            // Use velocity for smoother physics interaction
            Vector3 targetVelocity = moveDirection * moveSpeed;
            targetVelocity.y = rb.linearVelocity.y; // Preserve Y velocity
            rb.linearVelocity = targetVelocity;
        }
        else if (rb != null)
        {
            // Stop moving when no input
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Apply extra push force to crates
        if (collision.gameObject.CompareTag("Crate"))
        {
            Rigidbody crateRb = collision.gameObject.GetComponent<Rigidbody>();
            if (crateRb != null && moveDirection != Vector3.zero)
            {
                crateRb.AddForce(moveDirection * pushForce, ForceMode.Force);
            }
        }
    }
}