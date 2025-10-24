using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    public bool isGoal = false;
    public Transform respawnPoint; // optional: different respawn for this hole
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isGoal)
            {
                GameManager.Instance.PlayerReachedGoal();
            }
            else
            {
                GameManager.Instance.PlayerFellIntoHole(respawnPoint);
            }
        }
    }
}
