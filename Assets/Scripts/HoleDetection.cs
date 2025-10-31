using UnityEngine;
using System;

public class HoleDetection : MonoBehaviour
{
    // Attach this script to holes to detect crates above them

    [Header("Audio Settings")]
    // holeFilledSound
    [SerializeField] private AudioClip holeFilledSound;
    // Volume for hole completion sound
    [SerializeField] [Range(0f, 1f)] private float soundVolume = 0.5f;
    // Victory sound when all holes filled
    [SerializeField] private AudioClip victorySound;
    [SerializeField] [Range(0f, 1f)] private float victoryVolume = 0.7f;

    // Static event that fires when all holes are filled
    public static event Action OnAllHolesFilled;

    // Track if we've already triggered the win for this configuration
    private static bool winTriggered = false;
    // Track if we've reset the trigger this session
    private static bool hasResetThisSession = false;

    // Audio source component
    private AudioSource audioSource;
    // Track which crate is in this hole to avoid repeating sound
    private GameObject currentCrateInHole = null;

    void Start()
    {
        // Setup audio source for hole completion sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    void Update()
    {
        // Check all crates in the scene
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        GameObject[] holes = GameObject.FindGameObjectsWithTag("Hole");

        // Count filled holes while checking crates
        int filledHoleCount = 0;

        // Track if any crate is currently in this hole
        bool crateFoundInHole = false;
        GameObject crateInHole = null;

        // Check each crate to see if it's in any hole
        foreach (GameObject crate in crates)
        {
            int crateX = Mathf.RoundToInt(crate.transform.position.x);
            int crateZ = Mathf.RoundToInt(crate.transform.position.z);

            // Check if this crate is in any hole
            foreach (GameObject hole in holes)
            {
                int holeX = Mathf.RoundToInt(hole.transform.position.x);
                int holeZ = Mathf.RoundToInt(hole.transform.position.z);

                if (crateX == holeX && crateZ == holeZ)
                {
                    filledHoleCount++;

                    // Check if this crate is in THIS specific hole
                    if (hole == gameObject)
                    {
                        crateFoundInHole = true;
                        crateInHole = crate;

                        // Check if this is a new crate entering the hole
                        if (currentCrateInHole != crate)
                        {

                            // Play the hole filled sound
                            PlayHoleFilledSound();

                            // Remember this crate is in the hole
                            currentCrateInHole = crate;
                        }
                    }
                    break;
                }
            }
        }

        // If no crate is in hole anymore, reset tracking
        if (!crateFoundInHole)
        {
            currentCrateInHole = null;
        }

        // Check for win condition
        if (filledHoleCount == holes.Length && holes.Length > 0 && !winTriggered)
        {
            winTriggered = true;

            Debug.Log("VICTORY! All holes are filled!");

            // Play victory sound
            if (victorySound != null)
            {
                audioSource.clip = victorySound;
                audioSource.volume = victoryVolume;
                audioSource.Play();
            }
        }
        else if (filledHoleCount == holes.Length && holes.Length > 0 && winTriggered)
        {
            // Already triggered, don't spam the log
        }
        // Reset win trigger if not all holes are filled anymore
        else if (filledHoleCount < holes.Length)
        {
            winTriggered = false;
        }
    }

    void PlayHoleFilledSound()
    {
        // Only play if sound clip is assigned and audio source exists
        if (holeFilledSound != null && audioSource != null)
        {
            audioSource.clip = holeFilledSound;
            audioSource.volume = soundVolume;
            audioSource.Play();
        }
    }
}