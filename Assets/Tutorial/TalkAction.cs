using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TalkAction : Action
{
    public List<SharedString> sentences;
    public List<float> sentenceWaitTimes;
    private float waitTime;
    private float timer;
    private bool sentenceShown;

    public override void OnStart()
    {
        int index = Random.Range(0, sentenceWaitTimes.Count);
        waitTime = sentenceWaitTimes[index];
        timer = 0f;
        sentenceShown = false;
    }

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;

        if (!sentenceShown && timer >= waitTime)
        {
            int index = Random.Range(0, sentences.Count);
            string sentence = sentences[index].Value;

            GeneralUI.Instance.SetInfoText(sentence, waitTime);
            sentenceShown = true;
        }

        return TaskStatus.Running;
    }
}
