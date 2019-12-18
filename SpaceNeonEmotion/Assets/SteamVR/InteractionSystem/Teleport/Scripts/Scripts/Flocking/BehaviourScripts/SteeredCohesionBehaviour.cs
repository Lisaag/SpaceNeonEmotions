using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Steered Cohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 cohesionMove = Vector3.zero;

        List<Transform> filteredContext = (!filter) ? context : filter.Filter(agent, context); 
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }

        cohesionMove /= context.Count;

        cohesionMove -= agent.transform.position;

        if (float.IsNaN(currentVelocity.x) || float.IsNaN(currentVelocity.y)) currentVelocity = Vector2.zero;

        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}
