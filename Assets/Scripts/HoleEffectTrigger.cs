using UnityEngine;
using UnityEngine.SceneManagement;

public class HoleEffectTrigger : MonoBehaviour
{
    public GameObject smokeEffectPrefab;  // Assign your smoke/magic prefab
    public Transform spawnPoint;          // Optional custom spawn position
    private GameObject currentEffect;     // Keep reference to spawned effect
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return; // Prevent multiple triggers

        if (other.CompareTag("Player"))
        {
            // Spawn the effect
            Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
            currentEffect = Instantiate(smokeEffectPrefab, position, Quaternion.identity);

            hasPlayed = true;

            // Optional: destroy effect after few seconds
            Destroy(currentEffect, 1.5f);
        }
    }

    // This will be called when scene reloads or resets
    private void OnEnable()
    {
        // Subscribe to scene reload event
        SceneManager.sceneLoaded += OnSceneReloaded;
    }

    private void OnDisable()
    {
        // Unsubscribe when object is disabled
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        // Destroy any effect that might still exist
        if (currentEffect != null)
        {
            Destroy(currentEffect);
        }

        // Reset trigger state for fresh start
        hasPlayed = false;
    }
}
