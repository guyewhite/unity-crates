using UnityEngine;

public class ForceFullHD : MonoBehaviour
{
    [SerializeField] private bool forceResolution = true;
    [SerializeField] private int targetWidth = 1920;
    [SerializeField] private int targetHeight = 1080;
    [SerializeField] private bool fullscreen = true;

    void Awake()
    {
        if (forceResolution)
        {
            // Set the resolution
            Screen.SetResolution(targetWidth, targetHeight, fullscreen);

            // Optional: Set target framerate
            Application.targetFrameRate = 60;

            // Log the resolution
            Debug.Log($"Resolution set to: {targetWidth}x{targetHeight}, Fullscreen: {fullscreen}");
        }
    }

    // Optional: Prevent resolution changes
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && forceResolution)
        {
            Screen.SetResolution(targetWidth, targetHeight, fullscreen);
        }
    }
}