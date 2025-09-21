using UnityEngine.SceneManagement;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TutorialFinish : Action
{
    public override void OnStart()
    {
        SceneManager.LoadScene("Clock"); 
    }
}
