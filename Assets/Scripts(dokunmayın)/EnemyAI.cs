using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Target & Ranges")]
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;

    [Header("Speeds")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    [Header("Attack")]
    public float attackCooldown = 1f;
    public float attackHitDelay = 0.5f;
    private bool alreadyAttacked;

    private NavMeshAgent agent;
    private Animator animator;
    private string currentState = "";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Player otomatik bulma
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        agent.speed = walkSpeed;
        if (patrolPoints.Length > 0) GotoNextPoint();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
            Attack();
        else if (dist <= chaseRange)
            Chase();
        else if (patrolPoints.Length > 0)
            Patrol();
        else
            Idle();
    }

    void Idle()
    {
        agent.isStopped = true;
        SafePlay("Idle");
    }

    void Patrol()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        SafePlay("Walk");

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        SafePlay("Run");
    }

    void Attack()
    {
        agent.isStopped = true;
        SafePlay("Attack");

        if (alreadyAttacked) return;
        alreadyAttacked = true;
        StartCoroutine(ResetAttack());
        StartCoroutine(DealDamageWithDelay());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        alreadyAttacked = false;
    }

    IEnumerator DealDamageWithDelay()
    {
        yield return new WaitForSeconds(attackHitDelay);
        if (player == null) yield break;
        if (Vector3.Distance(transform.position, player.position) <= attackRange + 0.5f)
        {
            var ph = player.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(10);
        }
    }

    void GotoNextPoint()
    {
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    // Sadece yeni state farklýysa oynatýr
    void SafePlay(string stateName)
    {
        if (currentState == stateName) return;
        animator.Play(stateName);
        currentState = stateName;
    }
}
