using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera playerCamera;
    public NavMeshAgent agent;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float detectionRange = 30f;
    public float killRange = 1.5f;
    public float checkInterval = 0.1f;
    public LayerMask obstacleLayer;

    private bool isVisible = false;
    private float nextCheckTime;

    void Start()
    {
        // Find player if not assigned
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        // Find player camera if not assigned
        if (playerCamera == null && player != null)
        {
            playerCamera = player.GetComponentInChildren<Camera>();
        }

        // Get NavMeshAgent component
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (player == null || playerCamera == null) return;

        // Check if it's time to update visibility
        if (Time.time >= nextCheckTime)
        {
            CheckVisibility();
            nextCheckTime = Time.time + checkInterval;
        }

        // Check kill range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= killRange)
        {
            KillPlayer();
            return;
        }

        // Move only if not visible and within detection range
        if (!isVisible && distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    void CheckVisibility()
    {
        if (playerCamera == null)
        {
            isVisible = false;
            return;
        }

        // 1. Quick frustum check using viewport coordinates
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);

        // If behind the camera or outside viewport => not visible
        if (viewportPoint.z < 0 || viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            isVisible = false;
            return;
        }

        // 2. Raycast to detect occlusion (any collider between camera and enemy blocks visibility)
        Vector3 dir = transform.position - playerCamera.transform.position;
        float dist = dir.magnitude;
        RaycastHit hit;

        // Cast against everything except the player
        if (Physics.Raycast(playerCamera.transform.position, dir.normalized, out hit, dist))
        {
            // If the first thing hit is the enemy itself, it's visible. Otherwise, it's occluded.
            isVisible = hit.transform == transform;
        }
        else
        {
            // Nothing hit – technically unobstructed, but if this occurs treat as visible to be safe
            isVisible = true;
        }
    }

    void MoveTowardsPlayer()
    {
        // Predict if moving to the next position would make the enemy visible
        Vector3 nextPos = Vector3.MoveTowards(transform.position, player.position, agent.speed * Time.deltaTime);
        if (WouldBeVisible(nextPos))
        {
            StopMoving();
            return;
        }
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    bool WouldBeVisible(Vector3 position)
    {
        if (playerCamera == null)
            return false;

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(position);
        if (viewportPoint.z < 0 || viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
            return false;

        Vector3 dir = position - playerCamera.transform.position;
        float dist = dir.magnitude;
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, dir.normalized, out hit, dist))
        {
            return hit.transform == transform;
        }
        return true;
    }

    void StopMoving()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    void KillPlayer()
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1000); // Instant kill
        }
    }
} 