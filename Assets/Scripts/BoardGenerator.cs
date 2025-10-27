using UnityEngine;

// for the HashSet
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject holeTilePrefab;
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private GameObject playerOrbPrefab;
    [SerializeField] private float tileSize = 1f;


    // board size
    public int width = 6;
    public int height = 6;

    // board attributes
    public int holeCount = 2;
    public int crateCount = 2;
    
    // player, hole and crate positions
    private List<(int x, int z)> availablePositions;
    private HashSet<(int x, int z)> holePositions;
    private HashSet<(int x, int z)> cratePositions;
    private (int x, int z) playerSpawnPosition;
    
    void Start()
    {
        GenerateBoard();
    }
    
    void GenerateBoard()
    {
        // discover available positions on this board
        availablePositions = GetAvailablePositions();

        // generate unique coordinates for the holes
        holePositions = GenerateHolePositions();

        // generate unique coordinates for the crates
        cratePositions = GenerateCratePositions();

        // generate the player spawn position
        playerSpawnPosition = GeneratePlayerSpawnPosition();

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

                    // Place crate on top of floor if this position has a crate
                    if (cratePositions.Contains((x, z)))
                    {
                        Vector3 cratePosition = new Vector3(x * tileSize, 0.5f, z * tileSize);
                        GameObject crate = Instantiate(cratePrefab, cratePosition, Quaternion.identity);
                        crate.transform.parent = transform;
                        crate.name = $"Crate_{x}_{z}";
                    }
                }
                tile.transform.parent = transform;
                tile.name = $"Floor_{x}_{z}";
            }
        }

        // generate player OrbPulsate at the player spawn position
        GameObject playerOrb = Instantiate(playerOrbPrefab, new Vector3(playerSpawnPosition.x * tileSize, 0, playerSpawnPosition.z * tileSize), Quaternion.identity);
        playerOrb.transform.parent = transform;
        playerOrb.name = $"PlayerOrb_{playerSpawnPosition.x}_{playerSpawnPosition.z}";
    }

    // detect if the tile is on the edge of the board
    bool IsEdgeTile(int x, int y)
    {
        return x == 0 || y == 0 || x == width - 1 || y == height - 1;
    }

    private List<(int x, int z)> GetAvailablePositions()
    {
        // Create a place to store the available positions
        List<(int x, int z)> positions = new List<(int x, int z)>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (!IsEdgeTile(x, z))
                {
                    positions.Add((x, z));
                }
            }
        }
        return positions;
    }

    private HashSet<(int x, int z)> GenerateHolePositions()
    {
        // Create a place to store the hole positions
        holePositions = new HashSet<(int x, int z)>();
               
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

     private HashSet<(int x, int z)> GenerateCratePositions()
    {
        // Create a place to store the crate positions
        HashSet<(int x, int z)> cratePositions = new HashSet<(int x, int z)>();
        
        // Randomly select positions for crates, but only if there are available positions
        for (int i = 0; i < crateCount && availablePositions.Count > 0; i++)
        {

            // Select a random index from the available positions
            int randomIndex = Random.Range(0, availablePositions.Count);

            // If the selected position is an inner edge tile, select a new position
            while (IsInnerEdgeTile(availablePositions[randomIndex].x, availablePositions[randomIndex].z))
            {
                randomIndex = Random.Range(0, availablePositions.Count);
            }
            
            // Add the crate position to the set
            cratePositions.Add(availablePositions[randomIndex]);

            // Remove the position from the list so we don't select it again
            availablePositions.RemoveAt(randomIndex);
        }
        
        return cratePositions;
    }

    private (int x, int z) GeneratePlayerSpawnPosition()
    {
        // calculate available positions (exclude edges and holes)
        List<(int x, int z)> availablePositions = GetAvailablePositions();

        // Remove positions that already have holes or crates
        availablePositions.RemoveAll(pos => holePositions.Contains(pos) || cratePositions.Contains(pos));

        // select a random index from the available positions
        int randomIndex = Random.Range(0, availablePositions.Count);

        // return the selected player spawn position
        return availablePositions[randomIndex];
    }

    // Check if tile is exactly 1 tile in from any edge
    bool IsInnerEdgeTile(int x, int z)
    {
        return (x == 1 || x == width - 2 || z == 1 || z == height - 2) 
              && !IsEdgeTile(x, z);
    }
}