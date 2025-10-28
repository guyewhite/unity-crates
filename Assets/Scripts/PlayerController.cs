using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // Handle input only when not moving
        if (!isMoving)
        {
            Keyboard kb = Keyboard.current;
            if (kb != null)
            {
                // Check for any key press (not held)
                if (kb.upArrowKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame)
                    Move(Vector3.forward);
                else if (kb.downArrowKey.wasPressedThisFrame || kb.sKey.wasPressedThisFrame)
                    Move(Vector3.back);
                else if (kb.leftArrowKey.wasPressedThisFrame || kb.aKey.wasPressedThisFrame)
                    Move(Vector3.left);
                else if (kb.rightArrowKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame)
                    Move(Vector3.right);
            }
        }

        // Smoothly move to target tile
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    void Move(Vector3 direction)
    {
        // Calculate next tile position
        targetPosition = transform.position + (direction * tileSize);
        isMoving = true;

        // Push crate if present
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, tileSize))
        {
            if (hit.collider.CompareTag("Crate"))
            {
                // Move crate to next tile instantly
                hit.collider.transform.position += direction * tileSize;
            }
        }
    }
}