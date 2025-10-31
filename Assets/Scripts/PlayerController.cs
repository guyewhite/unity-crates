using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip cratePushSound; // Drag stone-slab.wav here
    [SerializeField] [Range(0f, 1f)] private float cratePushVolume = 0.5f; // Volume for crate push sound

    private AudioSource audioSource; // Audio source component
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;

        // Setup audio source for crate push sounds
        audioSource = gameObject.AddComponent<AudioSource>();

        // We'll start it manually
        audioSource.playOnAwake = false;
        // 2D sound
        audioSource.spatialBlend = 0f;
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
        Vector3 nextPosition = transform.position + (direction * tileSize);

        // Check what's in front of player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, tileSize))
        {
            if (hit.collider.CompareTag("Crate"))
            {
                // Check if crate can be pushed
                Vector3 cratePushPosition = hit.collider.transform.position + (direction * tileSize);

                // Check if push position is blocked
                if (IsPositionBlocked(cratePushPosition))
                {
                    // Can't push crate, so don't move player
                    return;
                }

                // Push is valid, move the crate
                hit.collider.transform.position = cratePushPosition;

                // Play crate push sound if audio clip is assigned
                PlayCratePushSound();
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                // Can't move into walls
                return;
            }
        }

        // Move the player
        targetPosition = nextPosition;
        isMoving = true;
    }

    bool IsPositionBlocked(Vector3 position)
    {
        // Check for any blocking objects at the position
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);

        foreach (Collider col in colliders)
        {
            // Position is blocked by wall or another crate
            if (col.CompareTag("Wall") || col.CompareTag("Crate"))
            {
                return true;
            }
        }

        return false;
    }

    void PlayCratePushSound()
    {
        // Only play if sound clip is assigned and audio source exists
        if (cratePushSound != null && audioSource != null)
        {
            audioSource.clip = cratePushSound;
            audioSource.volume = cratePushVolume;
            audioSource.Play();
        }
    }
}