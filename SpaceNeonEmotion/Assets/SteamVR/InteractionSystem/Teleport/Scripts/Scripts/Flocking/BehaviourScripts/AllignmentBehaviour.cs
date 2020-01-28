using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Allignment")]
public class AllignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0)
        {
            return agent.transform.forward;
        }

        Vector3 allignmentMove = Vector3.zero;

        List<Transform> filteredContext = (!filter) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        { 
            allignmentMove += item.transform.forward;
        }

        allignmentMove /= context.Count;

        return allignmentMove;
    }
}
