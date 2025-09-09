using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        // Find the board
        BoardGenerator board = FindObjectOfType<BoardGenerator>();
        
        // Calculate center of board
        float centerX = (board.width - 1) / 2f;
        float centerZ = (board.height - 1) / 2f;
        
        // Auto-calculate height based on board size (with padding)
        float maxDimension = Mathf.Max(board.width, board.height);
        float cameraHeight = (maxDimension + 2) * 0.9f;
        
        // Position camera above center
        transform.position = new Vector3(centerX, cameraHeight, centerZ);
        
        // Rotate to look straight down
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}