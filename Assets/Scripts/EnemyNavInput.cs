using UnityEngine;
using UnityEngine.AI;
using Controller; // Make sure this namespace matches your CreatureMover

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CreatureMover))]
public class EnemyNavInput : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent agent;
    private CreatureMover mover;
    private Transform selfTransform;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mover = GetComponent<CreatureMover>();
        selfTransform = transform;

        // Disable NavMeshAgent's auto movement
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (player == null)
            return;

        // Set pathfinding destination
        agent.SetDestination(player.position);

        // Calculate local movement direction
        Vector3 worldMove = agent.desiredVelocity;
        Vector3 localMove = selfTransform.InverseTransformDirection(worldMove);
        Vector2 input = new Vector2(localMove.x, localMove.z);

        // Optional: run when close to player
        bool isRunning = worldMove.magnitude > 1.5f;

        // Set target look-at as the player
        Vector3 lookTarget = player.position;

        // Apply to CreatureMover
        mover.SetInput(in input, in lookTarget, isRunning, isJump: false);

        // Let NavMeshAgent know where the object is
        agent.nextPosition = selfTransform.position;
    }
}
