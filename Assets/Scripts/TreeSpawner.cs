using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("Tree Settings")]
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private int treeCount = 50;
    [SerializeField] private float spawnRadius = 20f;
    [SerializeField] private float treeYPosition = -10f;

    [Header("Variation Settings")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private bool randomRotation = true;

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        // Create parent object to keep hierarchy clean
        GameObject treeParent = new GameObject("Forest");
        treeParent.transform.position = new Vector3(0, treeYPosition, 0);

        for (int i = 0; i < treeCount; i++)
        {
            // Generate random position within radius
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(randomCircle.x, treeYPosition, randomCircle.y);

            // Random rotation (Y axis only for trees)
            Quaternion rotation = randomRotation
                ? Quaternion.Euler(0, Random.Range(0f, 360f), 0)
                : Quaternion.identity;

            // Spawn tree
            GameObject tree = Instantiate(treePrefab, spawnPosition, rotation);

            // Random scale for variety
            float scale = Random.Range(minScale, maxScale);
            tree.transform.localScale = Vector3.one * scale;

            // Parent to forest object
            tree.transform.parent = treeParent.transform;
            tree.name = $"Tree_{i}";
        }

        Debug.Log($"Spawned {treeCount} trees below the board");
    }
}