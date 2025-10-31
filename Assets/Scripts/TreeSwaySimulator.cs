using UnityEngine;

public class TreeSwaySimulator : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 2f;
    public float swaySpeed = 1f;
    [SerializeField] private float randomOffset = 0f;

    private float timeOffset;
    private Quaternion originalRotation;

    void Start()
    {
        // Store original rotation
        originalRotation = transform.rotation;

        // Random offset so trees don't all sway in sync
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
        randomOffset = Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        // Calculate sway based on time
        float swayX = Mathf.Sin((Time.time * swaySpeed * randomOffset) + timeOffset) * swayAmount;
        float swayZ = Mathf.Sin((Time.time * swaySpeed * 0.7f * randomOffset) + timeOffset) * swayAmount * 0.5f;

        // Apply rotation (simulating wind from east/west affects X rotation more)
        Quaternion swayRotation = Quaternion.Euler(swayX, 0, swayZ);
        transform.rotation = originalRotation * swayRotation;
    }
}