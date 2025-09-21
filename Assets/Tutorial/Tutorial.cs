using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
    BehaviorTree behaviorTree;
    private void Awake()
    {
        behaviorTree = GetComponent<BehaviorTree>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public BehaviorTree GetBehaviorTree()
    {
        return behaviorTree;
    }
}
