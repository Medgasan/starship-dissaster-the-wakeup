using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

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

        // Revisar puertas
        CheckDoor();

        // Estados
        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }
    }

    void ChasePlayer()
    {
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

        // Raycast puertas
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(
            transform.position + Vector3.up,
            transform.position + Vector3.up + transform.forward * doorCheckDistance
        );
    }
}
