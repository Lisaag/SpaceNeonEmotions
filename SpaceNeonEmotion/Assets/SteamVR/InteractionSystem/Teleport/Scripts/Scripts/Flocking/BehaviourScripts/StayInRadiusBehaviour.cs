using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay In Range")]
public class StayInRadiusBehaviour : FilteredFlockBehaviour
{
    public Vector3 centerpoint;
    public float radius = 15f;
    public float rangePercent = 90;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 centerOffset = centerpoint - agent.transform.position;
        float t = centerOffset.magnitude / radius;

        if (t < 0.9)
        {
            return Vector3.zero;
        }

        return centerOffset * t * t;
    }
}
