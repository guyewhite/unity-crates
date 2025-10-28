using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float snapSpeed = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Check if over a hole first - don't snap if falling
        if (IsOverHole())
            return;

        // Only snap when object is nearly stopped
        if (rb && rb.linearVelocity.magnitude < 0.1f)
        {
            // Calculate nearest grid position
            Vector3 gridPos = new Vector3(
                Mathf.Round(transform.position.x / tileSize) * tileSize,
                transform.position.y,  // Keep current Y
                Mathf.Round(transform.position.z / tileSize) * tileSize
            );

            // Smoothly move to grid position
            transform.position = Vector3.Lerp(transform.position, gridPos, snapSpeed * Time.fixedDeltaTime);
        }
    }

    // Check if this object is over a hole
    bool IsOverHole()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Hole"))
                return true;
        }
        return false;
    }
}