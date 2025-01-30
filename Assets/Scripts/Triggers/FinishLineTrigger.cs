using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLineTrigger : MonoBehaviour
{
    public GameObject winUIPanel; // Assign the "You Win" UI panel in the Inspector
    public MonoBehaviour playerScript; // Reference to the player movement script

    private void Start()
    {
        // Ensure the UI panel is hidden at game start
        if (winUIPanel != null)
            winUIPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            // Disable player controls
            if (playerScript != null)
                playerScript.enabled = false;

            // Show the "You Win" UI
            if (winUIPanel != null)
                winUIPanel.SetActive(true);

            // Freeze time
            Time.timeScale = 0f;
        }
    }
}
