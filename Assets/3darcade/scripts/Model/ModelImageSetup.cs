using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcade
{
    public class ModelImageSetup : MonoBehaviour
    {
        // Save current material, when material is set to RenderTextureMenu.
        private Material _savedMaterial = null;

        public void Setup(Texture2D tex, RenderSettings renderSettings, ModelProperties modelProperties, ModelComponentType modelComponentType)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (modelProperties == null)
            {
                return;
            }

            if (!TryGetComponent(out Renderer renderer))
            {
                return;
            }

            _savedMaterial = renderer.sharedMaterial;

            // Generate a Marquee texture when there is none available.
            if (tex == null && transform.parent.CompareTag("gamemodel") && modelComponentType == ModelComponentType.Marquee)
            {
                GameObject obj = GameObject.Find("RenderCanvasToTexture");
                if (obj != null)
                {
                    if (obj.TryGetComponent(out RenderCanvasToTexture renderCanvasToTexture))
                    {
                        tex = renderCanvasToTexture.RenderToTexture(renderer.bounds.size.y / renderer.bounds.size.x, modelProperties);
                    }
                }
            }

            if (tex != null)
            {
                renderer.material.SetTexture("_MainTex", tex);
            }

            Texture mainTexture = renderer.material.GetTexture("_MainTex");
            if (mainTexture == null)
            {
                return;
            }

            // Also put it on the emissive map.
            if (modelComponentType == ModelComponentType.Marquee)
            {
                renderer.material.color = Color.black;
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetTexture("_EmissionMap", mainTexture);
                renderer.material.SetColor("_EmissionColor", Color.white * renderSettings.marqueeIntensity);
                renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
            }
            else if (modelComponentType != ModelComponentType.Screen && tex != null)
            {
                // Put the new texture on all the child renderers that uses this material, for magic pixels!
                IEnumerable<Renderer> childRenderers = transform.parent.GetComponentsInChildren<Renderer>()
                                                                       .Where(r => r.gameObject != gameObject
                                                                                && r.sharedMaterial == _savedMaterial
                                                                                && r.sharedMaterial.GetTexture("_MainTex") != null);
                foreach (Renderer childRenderer in childRenderers)
                {
                    childRenderer.material.SetTexture("_MainTex", mainTexture);
                    if (childRenderer.material.IsKeywordEnabled("_Emission"))
                    {
                        childRenderer.material.SetTexture("_EmissionMap", mainTexture);
                    }
                }
            }
        }

        public void SetMenuTexture()
        {
            if (TryGetComponent(out Renderer renderer))
            {
                Material renderTextureMenu = Resources.Load<Material>("cfg/RenderTextureMenu");
                renderer.material          = renderTextureMenu;
            }
        }

        public void ResetArcadeTexture()
        {
            if (_savedMaterial == null)
            {
                return;
            }

            if (TryGetComponent(out Renderer renderer))
            {
                renderer.material = _savedMaterial;
            }

            _savedMaterial = null;
        }
    }
}
