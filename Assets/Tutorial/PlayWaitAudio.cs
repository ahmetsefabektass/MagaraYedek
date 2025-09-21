using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class PlayWaitAudio : Action
{
    public AudioSource audioSource;

    private bool isPlaying = false;

    public override void OnStart()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            isPlaying = true;
        }
        else
        {
            isPlaying = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!isPlaying)
            return TaskStatus.Failure;

        if (!audioSource.isPlaying)
            return TaskStatus.Success;

        return TaskStatus.Running;
    }
}
