using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ShakeCamera : Action
{
    public Cinemachine.CinemachineImpulseSource impulse;
    public float shakeDuration = 3f;
    private float timer;

    public override void OnStart()
    {
        if (impulse == null)
        {
            impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
        }
        timer = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        if (timer < shakeDuration)
        {
            impulse.GenerateImpulse(0.2f);
            timer += Time.deltaTime;
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
