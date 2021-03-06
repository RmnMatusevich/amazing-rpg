using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Control;
using RPG.Attributes;
using GameDevTV.Utils;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 3f;
    [SerializeField] float agroCooldownTime = 3f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waypointDwellTime = 3f;
    [Range(0, 1)]
    [SerializeField] float patrolSpeedFraction = 0.2f;
    [SerializeField] float shoutDistance = 5f;

    Fighter fighter;
    Health health;
    Mover mover;
    GameObject player;

    LazyValue<Vector3> guardPosition;
    float timeSinceLastSawPlayer = Mathf.Infinity;
    float timeSinceArrivedAtWaypoint = Mathf.Infinity;
    float timeSinceAggrevated = Mathf.Infinity;
    int currentWaypointIndex = 0;

    private void Awake()
    {
        fighter = GetComponent<Fighter>();
        player = GameObject.FindWithTag("Player");
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();
        guardPosition = new LazyValue<Vector3>(GetGuardPosition);
    }

    private Vector3 GetGuardPosition()
    {
        return transform.position;
    }

    private void Start()
    {
        guardPosition.ForceInit();
    }

    private void Update()
    {
        if (health.IsDead()) return;

        if (IsAggrevated() && fighter.CanAttack(player))
        {
            AttackBehaviour();
        }
        else if (timeSinceLastSawPlayer < suspicionTime)
        {
            SuspicionBehavoiur();
        }
        else
        {
            PatrolBehaviour();
        }

        UpdateTimers();
    }

    public void Aggrevate()
    {
        timeSinceAggrevated = 0;
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        timeSinceAggrevated += Time.deltaTime;
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition.value;

        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
        }

        if (timeSinceArrivedAtWaypoint > waypointDwellTime)
        {
            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

        return distanceToWaypoint < waypointTolerance;
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private void SuspicionBehavoiur()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void AttackBehaviour()
    {
        timeSinceLastSawPlayer = 0;
        fighter.Attack(player);

        AggrevateNearbyEnemies();
    }

    private void AggrevateNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            AIController enemyAI = hit.collider.GetComponent<AIController>();
            if (enemyAI != null)
            {
                enemyAI.Aggrevate();
            }

        }
    }

    private bool IsAggrevated()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        return distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCooldownTime;
    }

    // Called by Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
