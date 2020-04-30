using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(LoopScrollRect), true)]
public class LoopScrollRectInspector : Editor
{
    private int _index   = 0;
    private float _speed = 1000f;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        LoopScrollRect scroll = target as LoopScrollRect;

        GUI.enabled = Application.isPlaying;

        _ = EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear"))
        {
            scroll.ClearCells();
        }

        if (GUILayout.Button("Refresh"))
        {
            scroll.RefreshCells();
        }

        if (GUILayout.Button("Refill"))
        {
            scroll.RefillCells();
        }

        if (GUILayout.Button("RefillFromEnd"))
        {
            scroll.RefillCellsFromEnd();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = 45;
        float w = (EditorGUIUtility.currentViewWidth - 100f) / 2f;
        _ = EditorGUILayout.BeginHorizontal();
        _index = EditorGUILayout.IntField("Index", _index, GUILayout.Width(w));
        _speed = EditorGUILayout.FloatField("Speed", _speed, GUILayout.Width(w));
        if (GUILayout.Button("Scroll", GUILayout.Width(45)))
        {
            scroll.SrollToCell(_index, _speed);
        }
        EditorGUILayout.EndHorizontal();
    }
}
