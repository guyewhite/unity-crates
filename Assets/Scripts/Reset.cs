using UnityEngine;

public class Reset : MonoBehaviour
{
    // Store the initial position when the object spawns
    private Vector3 initialPosition;

    void Start()
    {
        // Remember where this object started
        initialPosition = transform.position;
    }

    // Reset this specific object to its starting position
    public void ResetPosition()
    {
        transform.position = initialPosition;
    }

    // Static method to reset all objects with Reset component
    public static void ResetAll()
    {
        // Find all Reset components in the scene
        Reset[] allResetables = FindObjectsOfType<Reset>();

        foreach (Reset obj in allResetables)
        {
            obj.ResetPosition();
        }
    }
}