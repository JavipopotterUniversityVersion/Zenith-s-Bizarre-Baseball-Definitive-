using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LimitInstance : MonoBehaviour
{
    [SerializeField] Limit _instanceLimit;
    public Limit Limit => _instanceLimit;

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Limit.GetCenter(), Limit.GetSize());
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(LimitInstance))]
public class LimitInstanceCustomEditor : Editor
{
    GameObject limitContainer;
    public override void OnInspectorGUI()
    {
        LimitInstance limitInstance = (LimitInstance)target;

        if(limitContainer == null && GUILayout.Button("Create Limit"))
        {
            limitContainer = new GameObject("Limit");
            limitContainer.transform.position = limitInstance.transform.position;
            limitContainer.transform.parent = limitInstance.transform;

            GameObject min = new GameObject("Min");
            min.transform.parent = limitContainer.transform;
            min.transform.position = limitInstance.transform.position;

            GameObject max = new GameObject("Max");
            max.transform.parent = limitContainer.transform;
            max.transform.position = limitInstance.transform.position;

            limitInstance.Limit.SetLimits(min.transform, max.transform);
        }

        if(limitContainer == null) return;

        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("Select Min"))
        {
            ActiveEditorTracker.sharedTracker.isLocked = true;
            Selection.activeObject = limitInstance.Limit.Min;
        }

        if(GUILayout.Button("Select Max"))
        {
            ActiveEditorTracker.sharedTracker.isLocked = true;
            Selection.activeObject = limitInstance.Limit.Max;
        }

        EditorGUILayout.EndHorizontal();
    }
}
#endif