using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckRangeAction : Action
{
    public SharedTransform vender;
    public SharedTransform player;
    public float distance;

    public override TaskStatus OnUpdate()
    {
        if (vender.Value != null && player.Value != null)
        {
            float currentDistance = Vector3.Distance(vender.Value.position, player.Value.position);
            if (currentDistance < distance)
            {
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Running;
    }
}
