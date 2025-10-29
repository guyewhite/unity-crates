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

    [Header("Wind Settings")]
    [SerializeField] private bool generateWind = true;
    [SerializeField] private float windStrength = 1f;
    [SerializeField] private float windPulseFrequency = 0.5f;
    [SerializeField] private float windPulseMagnitude = 0.5f;
    [SerializeField] private bool windFromEast = true; // true = east, false = west

    void Start()
    {
        GenerateGround();
        GenerateTrees();
        GenerateWind();
    }

    void GenerateGround()
    {
        // Spawn the ground prefab at specified position
        if (groundPrefab != null)
        {
            GameObject ground = Instantiate(groundPrefab, groundPosition, Quaternion.identity);
            ground.name = "Ground";
            Debug.Log($"Ground spawned at {groundPosition}");
        }
        else
        {
            Debug.LogWarning("Ground prefab not assigned!");
        }
    }

    void GenerateTrees()
    {
        // Skip if no tree prefab assigned
        if (treePrefab == null)
        {
            Debug.LogWarning("Tree prefab not assigned, skipping tree generation");
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

            // Add sway simulation if wind is enabled
            if (generateWind)
            {
                TreeSwaySimulator sway = tree.AddComponent<TreeSwaySimulator>();
                // Configure sway based on wind settings - increased for visibility
                sway.swayAmount = windStrength * 5f;  // Increased from 2f to 5f for more visible movement
                sway.swaySpeed = windStrength * 1.5f;  // Increased speed for more noticeable effect
                Debug.Log($"Added TreeSwaySimulator to {tree.name} with amount: {sway.swayAmount}, speed: {sway.swaySpeed}");
            }

            // Parent to forest object
            tree.transform.parent = treeParent.transform;
            tree.name = $"Tree_{i}";
        }

        Debug.Log($"Generated {treeCount} trees on ground");
    }

    void GenerateWind()
    {
        // Skip if wind generation is disabled
        if (!generateWind)
            return;

        // Create Wind Zone GameObject
        GameObject windObject = new GameObject("Wind Zone");
        WindZone wind = windObject.AddComponent<WindZone>();

        // Configure wind properties
        wind.mode = WindZoneMode.Directional;
        wind.windMain = windStrength;
        wind.windTurbulence = windStrength * 0.5f;
        wind.windPulseMagnitude = windPulseMagnitude;
        wind.windPulseFrequency = windPulseFrequency;

        // Set wind direction (East = positive X, West = negative X)
        if (windFromEast)
        {
            // Wind from east (blowing west) - rotate to face west (-90 on Y)
            windObject.transform.rotation = Quaternion.Euler(0, -90, 0);
            Debug.Log("Wind blowing from East to West");
        }
        else
        {
            // Wind from west (blowing east) - rotate to face east (90 on Y)
            windObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            Debug.Log("Wind blowing from West to East");
        }

        // Position wind at ground level
        windObject.transform.position = groundPosition;

        Debug.Log($"Wind Zone created with strength {windStrength}");
    }
}