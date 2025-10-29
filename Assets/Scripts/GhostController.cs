using UnityEngine;
using System.Collections.Generic;

public class GhostController : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public float speed = 1.0f;
    public float chaseSpeed = 0.5f;
    public float chaseDistance = 1.5f;

    int current = 0;
    Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        if (patrolPoints.Count == 0) patrolPoints.Add(transform);
    }

    void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer < chaseDistance)
        {
            // chase
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * chaseSpeed * Time.deltaTime;
        }
        else
        {
            // patrol
            Transform target = patrolPoints[current];
            Vector3 dir = (target.position - transform.position);
            dir.y = 0;
            if (dir.magnitude < 0.5f)
            {
                current = (current + 1) % patrolPoints.Count;
            }
            else
            {
                transform.position += dir.normalized * speed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHitByGhost();
        }
    }
}



