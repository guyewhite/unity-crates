using UnityEngine;

public class HoleDetection : MonoBehaviour
{
    // Attach this script to holes to detect crates above them

    void Update()
    {
        // Check all crates in the scene
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");

        foreach (GameObject crate in crates)
        {
            // Get positions rounded to nearest tile
            int holeX = Mathf.RoundToInt(transform.position.x);
            int holeZ = Mathf.RoundToInt(transform.position.z);

            int crateX = Mathf.RoundToInt(crate.transform.position.x);
            int crateZ = Mathf.RoundToInt(crate.transform.position.z);

            // Check if crate is on same tile as hole
            if (crateX == holeX && crateZ == holeZ)
            {
                // Display specific crate and hole information
                Debug.Log($"{crate.name} is in {gameObject.name} at position ({holeX}, {holeZ})");
            }
        }
    }
}