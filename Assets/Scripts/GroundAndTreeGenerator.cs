using UnityEngine;

public class GroundAndTreeGenerator : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private Vector3 groundPosition = new Vector3(0, -8, 0);

    [Header("Tree Settings")]
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private int treeCount = 50;
    [SerializeField] private float spawnRadius = 20f;
    [SerializeField] private float treeYOffset = 0.5f; // Height above ground

    [Header("Tree Variation")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private bool randomRotation = true;

    [Header("Tree Sway Settings")]
    [SerializeField] private bool enableTreeSway = true;
    [SerializeField] private float swayAmount = 5f; // Degrees of rotation
    [SerializeField] private float swaySpeed = 1.5f; // Speed multiplier

    [Header("Ambient Audio")]
    [SerializeField] private AudioClip forestAmbience;
    [SerializeField] [Range(0f, 1f)] private float ambientVolume = 0.5f; // Volume slider 0-100%
    [SerializeField] private bool playAmbientSound = true; // Toggle ambient sound on/off

    void Start()
    {
        GenerateGround();
        GenerateTrees();
        SetupAmbientAudio(); // Initialize background forest sounds
    }

    void GenerateGround()
    {
        // Spawn the ground prefab at specified position
        if (groundPrefab != null)
        {
            GameObject ground = Instantiate(groundPrefab, groundPosition, Quaternion.identity);
            ground.name = "Ground";
        }
    }

    void GenerateTrees()
    {
        // Skip if no tree prefab assigned
        if (treePrefab == null)
        {
            return;
        }

        // Create parent object for organization
        GameObject treeParent = new GameObject("Forest");
        treeParent.transform.position = groundPosition + Vector3.up * treeYOffset;

        for (int i = 0; i < treeCount; i++)
        {
            // Random position within radius
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 treePosition = groundPosition + new Vector3(randomCircle.x, treeYOffset, randomCircle.y);

            // Random rotation around Y axis
            Quaternion rotation = randomRotation
                ? Quaternion.Euler(0, Random.Range(0f, 360f), 0)
                : Quaternion.identity;

            // Spawn tree
            GameObject tree = Instantiate(treePrefab, treePosition, rotation);

            // Apply random scale
            float scale = Random.Range(minScale, maxScale);
            tree.transform.localScale = Vector3.one * scale;

            // Add sway simulation if enabled
            if (enableTreeSway)
            {
                TreeSwaySimulator sway = tree.AddComponent<TreeSwaySimulator>();
                // Use the sway settings directly from the inspector
                sway.swayAmount = swayAmount;
                sway.swaySpeed = swaySpeed;
            }

            // Parent to forest object
            tree.transform.parent = treeParent.transform;
            tree.name = $"Tree_{i}";
        }
    }

    void SetupAmbientAudio()
    {
        // Skip if audio is disabled or no clip assigned
        if (!playAmbientSound || forestAmbience == null)
        {
            return;
        }

        // Add AudioSource component to this GameObject
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        // Set the forest sound clip
        audioSource.clip = forestAmbience; 
        
        // Set volume from inspector slider       
        audioSource.volume = ambientVolume;

        // Loop continuously
        audioSource.loop = true;

        // We'll start it manually
        audioSource.playOnAwake = false;

        // 2D sound (not positional)
        audioSource.spatialBlend = 0f;        

        // Start playing the ambient sound
        audioSource.Play();

    }
}