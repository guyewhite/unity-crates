using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerBoundsTest
{
    [UnityTest]
    [Order(2)]
    public IEnumerator PlayerSpawnsWithinBoardBounds()
    {
        // Load the game scene
        SceneManager.LoadScene("SampleScene");

        // Wait for scene to load
        yield return null;

        // Wait for Start() methods to execute
        yield return new WaitForSeconds(1f);

        // Find the player
        GameObject player = GameObject.FindWithTag("Player");

        // Count wall tiles to determine board size
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        // Find min and max positions from walls
        float minX = float.MaxValue, maxX = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        foreach (GameObject wall in walls)
        {
            Vector3 pos = wall.transform.position;
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.z < minZ) minZ = pos.z;
            if (pos.z > maxZ) maxZ = pos.z;
        }

        // Board dimensions based on wall positions
        int boardWidth = Mathf.RoundToInt(maxX) + 1;
        int boardHeight = Mathf.RoundToInt(maxZ) + 1;

        // Get player position
        Vector3 playerPos = player.transform.position;

        // Check X bounds (1 to width-2 for inner tiles)
        bool withinXBounds = playerPos.x >= 1 && playerPos.x <= boardWidth - 2;

        // Check Z bounds (1 to height-2 for inner tiles)
        bool withinZBounds = playerPos.z >= 1 && playerPos.z <= boardHeight - 2;

        // Log only if test will fail
        if (!withinXBounds || !withinZBounds)
        {
            Debug.LogError($"FAILED: Player out of bounds at position {playerPos}");
            Debug.LogError($"  Board size: {boardWidth}x{boardHeight}");
            Debug.LogError($"  Valid X range: 1 to {boardWidth - 2}");
            Debug.LogError($"  Valid Z range: 1 to {boardHeight - 2}");
        }

        // Assert player is within bounds
        Assert.IsTrue(withinXBounds, $"Player X position {playerPos.x} is outside board bounds (1 to {boardWidth - 2})");
        Assert.IsTrue(withinZBounds, $"Player Z position {playerPos.z} is outside board bounds (1 to {boardHeight - 2})");
    }
}