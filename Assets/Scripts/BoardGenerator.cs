using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject wallTilePrefab;
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
}