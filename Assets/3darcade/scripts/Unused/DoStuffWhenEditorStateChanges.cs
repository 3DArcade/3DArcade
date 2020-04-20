using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DoStuffWhenEditorStateChanges : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("editorAwake");
        var editorModelcache = GameObject.Find("EditorModelCache");
        if (editorModelcache != null)
        {
            foreach (Transform child in editorModelcache.transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            GameObject.DestroyImmediate(editorModelcache);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("editorOnDestroy");
    }
}