using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Talk : Action
{
    public SharedString message;
    public SharedFloat duration;
    public SharedBool waitUntilFinished;

    private float startTime;

    public override void OnStart()
    {
        GeneralUI.Instance.SetInfoText(message.Value, duration.Value);
        startTime = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if (waitUntilFinished.Value)
        {
            if (Time.time - startTime < duration.Value)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Success;
    }
}
