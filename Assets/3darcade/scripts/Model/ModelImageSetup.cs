using UnityEngine;

namespace Arcade
{
    public class ModelImageSetup : MonoBehaviour
    {
        // Save current material, when material is set to RenderTextureMenu.
        private Material savedMaterial = null;

        public void Setup(Texture2D tex, RenderSettings renderSettings, ModelProperties modelProperties, ModelComponentType modelComponentType)
        {
            if (!Application.isPlaying) { return; }
            Renderer thisRenderer = GetComponent<Renderer>();
            if (thisRenderer == null || modelProperties == null) { return; }

            // Generate a Marquee texture when there is none available.
            if (tex == null && transform.parent.CompareTag("gamemodel") && (modelComponentType == ModelComponentType.Marquee))
            {
                GameObject obj = GameObject.Find("RenderCanvasToTexture");
                if (obj != null)
                {
                    RenderCanvasToTexture renderCanvasToTexture = obj.GetComponent<RenderCanvasToTexture>();
                    if (renderCanvasToTexture != null)
                    {
                        tex = renderCanvasToTexture.RenderToTexture((thisRenderer.bounds.size.y / thisRenderer.bounds.size.x), modelProperties);
                    }
                }
            }

            // Create a new material from the currentr material, so we have an unique one.
            var tempMaterial = new Material(thisRenderer.sharedMaterial);
            var shaderName = tempMaterial.shader.name;
            var newShader = Shader.Find(shaderName);

            // TODO: This fixes black textures when some prefabs are loaded from an assetbundle, still nescessary?
            if (newShader != null)
            {
                tempMaterial.shader = newShader;
            }
            else
            {
                Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + tempMaterial.name);
            }

            tempMaterial.mainTexture = tex != null ? tex : thisRenderer.material.mainTexture;

            // Also put it on the emissive map. 
            if (modelComponentType == ModelComponentType.Marquee)
            {
                Texture myTexture = tempMaterial.GetTexture("_MainTex");
                tempMaterial.SetTexture("_EmissionMap", myTexture);
                tempMaterial.color = Color.black;
                tempMaterial.SetColor("_EmissionColor", Color.white);
                tempMaterial.SetVector("_EmissionColor", Color.white * renderSettings.marqueeIntensity);
                tempMaterial.EnableKeyword("_EMISSION");
                tempMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }

            // Put the new texture on all the model components that use this material, for magic pixels!
            if (tex != null && modelComponentType != ModelComponentType.Screen)
            {
                var r = transform.parent.GetComponentsInChildren(typeof(Renderer));
                foreach (Renderer i in r)
                {
                    if (thisRenderer.sharedMaterial.mainTexture == null || i.sharedMaterial.mainTexture == null) { continue; }
                    int id1 = thisRenderer.sharedMaterial.mainTexture.GetInstanceID();
                    int id2 = i.sharedMaterial.mainTexture.GetInstanceID();
                    if (id1 == id2)
                    {
                        if (i.transform.gameObject.name != transform.gameObject.name)
                        {
                            var tempMaterial2 = new Material(i.material);
                            bool isEmissive = i.material.IsKeywordEnabled("_Emission");
                            i.material = tempMaterial2;
                            i.material.mainTexture = tempMaterial.mainTexture;
                            if (isEmissive)
                            {
                                Texture myTexture = tempMaterial.GetTexture("_MainTex");
                                i.material.SetTexture("_EmissionMap", myTexture);
                            }
                        }
                    }
                }
            }
            thisRenderer.material = tempMaterial;
            savedMaterial = tempMaterial;
        }

        public void SetMenuTexture()
        {
            Renderer thisRenderer = GetComponent<Renderer>();
            if (thisRenderer == null) { return; }
            Material renderTextureMenu = Resources.Load("cfg/RenderTextureMenu", typeof(Material)) as Material;
            thisRenderer.material = renderTextureMenu;
        }

        public void ResetArcadeTexture()
        {
            if (savedMaterial == null) { return; }
            Renderer thisRenderer = GetComponent<Renderer>();
            if (thisRenderer == null) { return; }
            thisRenderer.material = savedMaterial;
            savedMaterial = null;
        }
    }
}
