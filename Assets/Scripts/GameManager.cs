// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;
// using System.Collections;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     [Header("Player Settings")]
//     public Transform player;
//     public Transform spawnPoint;
//     public int lives = 3;
//     public float respawnDelay = 1.0f;

//     [Header("UI Elements")]
//     public TextMeshProUGUI livesText;
//     public TextMeshProUGUI stateText;
//     public TextMeshProUGUI levelCompleteText; // Level Completed UI
//     public float messageDuration = 3f;        // duration to show level complete

//     [Header("Optional Effects")]
//     public ParticleSystem respawnEffect;
//     public AudioSource audioSource;
//     public AudioClip fallSound;
//     public AudioClip goalSound;

//     [Header("Next Level Settings")]
//     public string nextLevelName = "Level_2"; // Name of next scene

//     void Awake()
//     {
//         if (Instance == null) Instance = this;
//         else Destroy(gameObject);
//     }

//     void Start()
//     {
//         UpdateUI();
//         if (levelCompleteText != null)
//             levelCompleteText.text = ""; // hide initially
//     }

//     public void PlayerFellIntoHole(Transform holeRespawn)
//     {
//         lives--;
//         UpdateUI();

//         if (audioSource && fallSound)
//             audioSource.PlayOneShot(fallSound);

//         StartCoroutine(RespawnRoutine(holeRespawn));
//     }

//     IEnumerator RespawnRoutine(Transform holeRespawn)
//     {
//         Rigidbody rb = player.GetComponent<Rigidbody>();
//         rb.isKinematic = true;
//         player.gameObject.SetActive(false);

//         if (respawnEffect) respawnEffect.Play();

//         yield return new WaitForSeconds(respawnDelay);

//         if (lives <= 0)
//         {
//             stateText.text = "Game Over";
//             yield return new WaitForSeconds(2f);
//             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//         }
//         else
//         {
//             Vector3 targetPos = (holeRespawn != null) ? holeRespawn.position + Vector3.up * 0.5f : spawnPoint.position;
//             player.position = targetPos;
//             rb.linearVelocity = Vector3.zero;       // fixed property
//             rb.angularVelocity = Vector3.zero;
//             rb.isKinematic = false;
//             player.gameObject.SetActive(true);
//         }
//     }

//     public void PlayerReachedGoal()
//     {
//         stateText.text = "You Win!";
//         Rigidbody rb = player.GetComponent<Rigidbody>();
//         rb.isKinematic = true; // freeze player

//         if (audioSource && goalSound)
//             audioSource.PlayOneShot(goalSound);

//         if (levelCompleteText != null)
//         {
//             levelCompleteText.text = "Level Completed!";
//             StartCoroutine(NextLevelAfterDelay(messageDuration));
//         }
//     }

//     // Coroutine to load next level after showing message
//     IEnumerator NextLevelAfterDelay(float delay)
//     {
//         yield return new WaitForSeconds(delay);

//         if (!string.IsNullOrEmpty(nextLevelName))
//         {
//             SceneManager.LoadScene(nextLevelName);
//         }
//         else
//         {
//             // fallback: reload current scene
//             SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//         }
//     }

//     public void PlayerHitByGhost()
//     {
//         PlayerFellIntoHole(null);
//     }

//     void UpdateUI()
//     {
//         if (livesText != null)
//             livesText.text = "Lives: " + lives;
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Settings")]
    public Transform player;
    public Transform spawnPoint;
    public int lives = 3;
    public float respawnDelay = 1.0f;

    [Header("UI Elements")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI levelCompleteText; 
    public float messageDuration = 3f;        

    [Header("Optional Effects")]
    public ParticleSystem respawnEffect;
    public AudioSource audioSource;
    public AudioClip fallSound;
    public AudioClip goalSound;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        if (levelCompleteText != null)
            levelCompleteText.text = ""; 
    }

    public void PlayerFellIntoHole(Transform holeRespawn)
    {
        lives--;
        UpdateUI();

        if (audioSource && fallSound)
            audioSource.PlayOneShot(fallSound);

        StartCoroutine(RespawnRoutine(holeRespawn));
    }

    IEnumerator RespawnRoutine(Transform holeRespawn)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        player.gameObject.SetActive(false);

        if (respawnEffect) respawnEffect.Play();

        yield return new WaitForSeconds(respawnDelay);

        if (lives <= 0)
        {
            stateText.text = "Game Over";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Vector3 targetPos = (holeRespawn != null) ? holeRespawn.position + Vector3.up * 0.5f : spawnPoint.position;
            player.position = targetPos;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            player.gameObject.SetActive(true);
        }
    }

    public void PlayerReachedGoal()
    {
        stateText.text = "You Win!";
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (audioSource && goalSound)
            audioSource.PlayOneShot(goalSound);

        if (levelCompleteText != null)
        {
            levelCompleteText.text = "Level Completed!";
            StartCoroutine(LoadNextLevelAfterDelay(messageDuration));
        }
    }

    // ✅ Automatically loads next level in build order
    IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // If no next level, show message or return to main menu
            stateText.text = "All Levels Completed!";
        }
    }

    public void PlayerHitByGhost()
    {
        PlayerFellIntoHole(null);
    }

    void UpdateUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }
}

