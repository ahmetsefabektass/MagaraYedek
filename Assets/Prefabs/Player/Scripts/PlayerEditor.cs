using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerController playerController = (PlayerController)target;

        if (playerController != null)
        {
            SceneView.RepaintAll();
        }
    }
    private void OnSceneGUI()
    {
        PlayerController playerController = (PlayerController)target;
        if (playerController == null) return;
        float range = playerController.interactionRange;
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(playerController.transform.position, Vector3.up, range);
        Handles.DrawWireDisc(playerController.transform.position, Vector3.forward, range);
    }
}