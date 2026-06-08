using GLTFast.Schema;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;



public class EnemyAI : MonoBehaviour
{
    private Animator animator;

    [Header("Jugador")]
    public Transform player;

    [Header("Ataque")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    private bool attacking = false;
    private float attackTimer = 0f;

    [Header("Movimiento libre")]
    public float roamRadius = 20f;
    public float roamDelay = 4f;

    [Header("Detección")]
    public float detectionRange = 10f;
    public float loseRange = 15f;

    [Header("Puertas")]
    public float doorCheckDistance = 2f;
    public LayerMask doorLayer;

    private NavMeshAgent agent;

    private bool chasing = false;
    private float roamTimer;

    private Vector3 startPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        // Buscar layer "Puerta"
        doorLayer = LayerMask.GetMask("Puerta");

        SetRandomDestination();
    }

    void Update()
    {
        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        // Detectar jugador
        if (distanceToPlayer <= detectionRange)
        {
            chasing = true;
        }
        // Perder jugador
        else if (distanceToPlayer >= loseRange)
        {
            chasing = false;
        }
        animator.SetBool("Chasing", chasing);

        // Revisar puertas
        CheckDoor();

        attackTimer -= Time.deltaTime;

        if (chasing)
        {
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                
                ChasePlayer();
            }
        }
        else
        {
            
            Roam();
        }
    }

    void AttackPlayer()
    {
        agent.isStopped = true;

        transform.LookAt(new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z));

        if (attackTimer <= 0)
        {
            animator.SetTrigger("Attack");

            attackTimer = attackCooldown;
        }
    }

    public void HitPlayer()
    {
        float distance =
            Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            PlayerHealth health =
            player.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.Die();
            }
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.destination = player.position;
    }

    void Roam()
    {
        roamTimer += Time.deltaTime;

        // Si llegó o pasó tiempo
        if ((!agent.pathPending && agent.remainingDistance < 1f)
            || roamTimer >= roamDelay)
        {
            SetRandomDestination();
            roamTimer = 0f;
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * roamRadius;

        randomDirection += startPosition;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            randomDirection,
            out hit,
            roamRadius,
            NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
    }

    void CheckDoor()
    {
        RaycastHit hit;

        Vector3 origin = transform.position + Vector3.up;

        if (Physics.Raycast(
            origin,
            transform.forward,
            out hit,
            doorCheckDistance,
            doorLayer))
        {
            PuertaBehavior puerta =
    hit.collider.GetComponent<PuertaBehavior>();

            if (puerta != null)
            {
                puerta.Interact();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Pérdida
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);

        // Movimiento libre
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRadius);

        // Ataque
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Raycast puertas
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(
            transform.position + Vector3.up,
            transform.position + Vector3.up + transform.forward * doorCheckDistance
        );
    }
}
