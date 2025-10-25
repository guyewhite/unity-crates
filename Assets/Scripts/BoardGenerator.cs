using UnityEngine;

// for the HashSet
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject holeTilePrefab;
    [SerializeField] private float tileSize = 1f;


    // board size
    public int width = 6;
    public int height = 6;

    // board attributes
    public int holeCount = 2;
    
    void Start()
    {
        GenerateBoard();
    }
    
    void GenerateBoard()
    {

        // generate unique coordinates for the holes
        HashSet<(int x, int z)> holePositions = GenerateHolePositions();
        Debug.Log("Hole positions: " + string.Join(", ", holePositions));

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);

                GameObject tile;

                // wall or floor?
                if (IsEdgeTile(x, z))
                {
                    // generator wall tile
                    tile = Instantiate(wallTilePrefab, position, Quaternion.identity);
                }
                else if (holePositions.Contains((x, z)))
                {
                    // generator hole tile
                    tile = Instantiate(holeTilePrefab, position, Quaternion.identity);
                }
                else
                {
                    // generator floor tile
                    tile = Instantiate(floorTilePrefab, position, Quaternion.identity);
                }
                tile.transform.parent = transform;
                tile.name = $"Floor_{x}_{z}";
            }
        }
    }

    // detect if the tile is on the edge of the board
    bool IsEdgeTile(int x, int y)
    {
        return x == 0 || y == 0 || x == width - 1 || y == height - 1;
    }

    private HashSet<(int x, int z)> GenerateHolePositions()
    {
        // Create a place to store the hole positions
        HashSet<(int x, int z)> holePositions = new HashSet<(int x, int z)>();
        
        // Calculate available positions (exclude edges)
        List<(int x, int z)> availablePositions = new List<(int x, int z)>();
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < height - 1; z++)
            {
                availablePositions.Add((x, z));
            }
        }
        
        // Randomly select positions for holes
        for (int i = 0; i < holeCount && availablePositions.Count > 0; i++)
        {
            // Select a random index from the available positions
            int randomIndex = Random.Range(0, availablePositions.Count);
            
            // Add the hole position to the set
            holePositions.Add(availablePositions[randomIndex]);

            // Remove the position from the list so we don't select it again
            availablePositions.RemoveAt(randomIndex);
        }
        
        return holePositions;
    }
}