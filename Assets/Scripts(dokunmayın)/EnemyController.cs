using System.Collections;
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

    [Header("Delay")]
    public float animPauseDelay = 0.2f;

    private bool isVisible;
    private float nextCheckTime;
    public Animator animator;

    private Coroutine animPauseCoroutine;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerCamera == null && player != null)
            playerCamera = player.GetComponentInChildren<Camera>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (player == null || playerCamera == null) return;

        if (Time.time >= nextCheckTime)
        {
            CheckVisibility();
            nextCheckTime = Time.time + checkInterval;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        // Oyuncu düşmanı görebiliyorsa (isVisible == true) yakın mesafede olsa bile hasar verilmez.
        if (dist <= killRange && !isVisible)
        {
            KillPlayer();
            return;
        }

        if (!isVisible && dist <= detectionRange)
            ResumeMovementAndAnimation();
        else
            PauseMovementImmediate_AnimationDelayed();
    }

    void CheckVisibility()
    {
        Vector3 vp = playerCamera.WorldToViewportPoint(transform.position);
        if (vp.z < 0 || vp.x < 0 || vp.x > 1 || vp.y < 0 || vp.y > 1)
        {
            isVisible = false;
            return;
        }
        Vector3 dir = transform.position - playerCamera.transform.position;
        if (Physics.Raycast(playerCamera.transform.position, dir.normalized, out RaycastHit hit, dir.magnitude))
            isVisible = (hit.transform == transform);
        else
            isVisible = true;
    }

    void ResumeMovementAndAnimation()
    {
        // Eğer animasyon durdurma coroutine'i varsa iptal et, animasyon 1 olsun
        if (animPauseCoroutine != null)
        {
            StopCoroutine(animPauseCoroutine);
            animPauseCoroutine = null;
        }

        agent.isStopped = false;
        animator.speed = 1f;
        agent.SetDestination(player.position);
    }

    void PauseMovementImmediate_AnimationDelayed()
    {
        // Hareketi hemen durdur
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // Eğer zaten bir anim pause coroutine'i yoksa, başlat
        if (animPauseCoroutine == null)
            animPauseCoroutine = StartCoroutine(DelayedAnimPause());
    }

    private IEnumerator DelayedAnimPause()
    {
        yield return new WaitForSeconds(animPauseDelay);
        animator.speed = 0f;
        animPauseCoroutine = null;
    }

    void KillPlayer()
    {
        player.GetComponent<PlayerHealth>()?.TakeDamage(1000);
    }
}
