using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    public class PrefabSetup : MonoBehaviour
    {
        void Reset()
        {
            if (gameObject.GetComponent<Arcade.ModelSetup>() != null)
            {
                Object.DestroyImmediate(gameObject.GetComponent<Arcade.ModelSetup>());
            }
            var comp = gameObject.GetComponentsInChildren<Arcade.ModelImageSetup>();
            if (comp.Length > 0)
            {
                Object.DestroyImmediate(comp[0]);
            }
            var comp2 = gameObject.GetComponentsInChildren<Arcade.ModelVideoSetup>();
            if (comp.Length > 0)
            {
                Object.DestroyImmediate(comp2[0]);
            }
            var comp3 = gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>();
            if (comp3.Length > 0)
            {
                Object.DestroyImmediate(comp3[0]);
            }

            List<GameObject> thisChildren = new List<GameObject>();
            foreach (Transform child in transform)
            {
                if (child.gameObject != null)
                {
                    thisChildren.Add(child.gameObject);
                }
            }

            // Rename if nescessary
            for (int i = 0; i < thisChildren.Count; i++)
            {
                if (!thisChildren[i].name.StartsWith(gameObject.name, System.StringComparison.Ordinal) && (!thisChildren[i].name.StartsWith("01", System.StringComparison.Ordinal) && !thisChildren[i].name.Contains(gameObject.name)) && (!thisChildren[i].name.StartsWith("02", System.StringComparison.Ordinal) && !thisChildren[i].name.Contains(gameObject.name)))
                {
                    if (i == 0)
                    {
                        thisChildren[i].name = "01_" + gameObject.name + "_" + thisChildren[i].name;
                    }
                    else if (i == 1)
                    {
                        thisChildren[i].name = "02_" + gameObject.name + "_" + thisChildren[i].name;
                    }
                    else
                    {
                        thisChildren[i].name = gameObject.name + "_" + thisChildren[i].name;
                    }
                }
            }

            // Add rigidbody to gameObject
            var rigid = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
            if (rigid == null)
            {
                rigid = gameObject.AddComponent<Rigidbody>();
                rigid.mass = 20;
            }

            // Setup collider
            var tMeshCollider = gameObject.GetComponent(typeof(MeshCollider)) as MeshCollider;
            var tChildrenMeshColliders = gameObject.GetComponentInChildren(typeof(MeshCollider)) as MeshCollider;
            var tChildrenBoxColliders = gameObject.GetComponentInChildren(typeof(BoxCollider)) as BoxCollider;
            if (tMeshCollider != null || tChildrenMeshColliders != null || tChildrenBoxColliders != null) { return; }
            var boxCol = gameObject.GetComponent(typeof(BoxCollider)) as BoxCollider;
            if (boxCol == null)
            {
                boxCol = gameObject.AddComponent<BoxCollider>();
            }
            Transform t = gameObject.transform;
            t.transform.position = new Vector3(0, 0, 0);
            Renderer[] rr = t.GetComponentsInChildren<Renderer>();
            Bounds b = rr[0].bounds;
            foreach (Renderer r in rr) { b.Encapsulate(r.bounds); }
            boxCol.center = new Vector3(0, b.size.y / 2, 0);
            boxCol.size = new Vector3(b.size.x, b.size.y, b.size.z);
        }
    }
}

