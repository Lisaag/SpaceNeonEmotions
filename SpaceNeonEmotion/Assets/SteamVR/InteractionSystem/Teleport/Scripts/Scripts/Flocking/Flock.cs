using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    public int minimumSpeedChangeInterval = 3;
    public int maxSpeedChangeInterval = 10;

    [Range(1, 500)]
    public int startingCount = 250;
    const float agentDensity = 0.58f;

    [Range(0f, 25)]
    public float driveFactor = 10f;

    public bool changeSpeedOverTime = true;

    public int minDriveFactor = 1;
    public int maxDriveFactor = 25;

    [Range(0f, 100)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1.5f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * agentDensity + transform.position,
                Quaternion.Euler(Vector3.forward * Random.Range(0, 360)),
                transform);

            newAgent.name = "FlockingAgent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }

        if (changeSpeedOverTime)
            StartCoroutine(ChangeSpeed());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);

        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }

    public IEnumerator ChangeSpeed()
    {
        int waitTime = Random.Range(minimumSpeedChangeInterval, maxSpeedChangeInterval);
        yield return new WaitForSeconds(waitTime);
        driveFactor = Random.Range(minDriveFactor, maxDriveFactor);
        StartCoroutine(ChangeSpeed());
    }
}
