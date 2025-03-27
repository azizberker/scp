using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // oyuncunun transformu
    public float chaseRange = 15f; // kovalama mesafesi
    public float attackRange = 2f; // sald�rma mesafesi
    public float moveSpeed = 3.5f; // d��man�n hareket h�z�
    public float attackCooldown = 1f; // sald�r� s�kl��� (saniye)
    private float lastAttackTime = 0f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange && distance > attackRange)
        {
            // Oyuncuyu kovala
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else if (distance <= attackRange)
        {
            // Oyuncuya sald�r
            Attack();
        }
        else
        {
            // Durdur
            agent.isStopped = true;
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // hasar miktar�
                Debug.Log("Enemy sald�rd�!");
            }
        }
    }

    // Kovalamaya ve sald�r�ya ne kadar yakla�t���n� sahnede g�rsel olarak g�rmek i�in:
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}