using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerExistsTest
{
    [UnityTest]
    [Order(1)]
    public IEnumerator PlayerExistsInScene()
    {
        // Load the game scene
        SceneManager.LoadScene("SampleScene");

        // Wait for scene to load
        yield return null;

        // Wait for Start() methods to execute
        yield return new WaitForSeconds(1f);

        // Try to find the player
        GameObject player = GameObject.FindWithTag("Player");

        // Log only if test will fail
        if (player == null)
        {
            Debug.LogError("FAILED: Player not found in scene");
            Debug.LogError("  Check that PlayerOrb has 'Player' tag");
        }

        // Check if player exists
        Assert.IsNotNull(player, "Player should exist in the scene");
    }
}