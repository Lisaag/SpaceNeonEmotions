using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 avoidanceMove = Vector3.zero;
        int toAvoidCount = 0;

        List<Transform> filteredContext = (!filter) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        { 
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                toAvoidCount++;
                avoidanceMove += agent.transform.position - item.position;
            }
        }

        if (toAvoidCount > 0)
            avoidanceMove /= toAvoidCount;

        return avoidanceMove;
    }
}
