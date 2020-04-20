using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{

    public class RenderCanvasToTexture : MonoBehaviour
    {
        public TextMeshPro defaultMarqueeText;
        public Image defaultMarqueeImage;
        private int i = 0;

        public Camera renderCanvasCamera;

        public Sprite defaultMarqueeImage_1;
        public Sprite defaultMarqueeImage_2;
        public Sprite defaultMarqueeImage_3;
        public Sprite defaultMarqueeImage_4;
        public Sprite defaultMarqueeImage_5;
        public Sprite defaultMarqueeImage_6;
        public Sprite defaultMarqueeImage_7;
        public Sprite defaultMarqueeImage_8;

        // Take a "screenshot" of a camera's Render Texture.
        public Texture2D RenderToTexture(float aspectRatio, ModelProperties modelProperties)
        {
            var defaultMarqueeImages = new Sprite[] { defaultMarqueeImage_1, defaultMarqueeImage_2, defaultMarqueeImage_3, defaultMarqueeImage_4, defaultMarqueeImage_5, defaultMarqueeImage_6, defaultMarqueeImage_7, defaultMarqueeImage_8 };
            System.Random rnd = new System.Random();

            var defaultMarqueeTextColors = new Color[] { Color.green, Color.yellow, Color.white, Color.magenta, Color.blue, Color.cyan };

            // The Render Texture in RenderTexture.active is the one that will be read by ReadPixels.
            var currentRT = RenderTexture.active;
            RenderTexture.active = renderCanvasCamera.targetTexture;

            if (aspectRatio > 0.6) { aspectRatio = 0.6f; }
            if (aspectRatio < 0.2) { aspectRatio = 0.2f; }
            if (ArcadeManager.currentOS == OS.MacOS)
            {
                renderCanvasCamera.aspect = 1 / aspectRatio;
            }
            else
            {
                renderCanvasCamera.rect = new Rect(0, 0, 1, aspectRatio); // windows? test
            }
            var descriptiveName = modelProperties.descriptiveName;
            int index = descriptiveName.IndexOf("(", StringComparison.Ordinal);
            if (index > 0)
            {
                descriptiveName = descriptiveName.Substring(0, index);
            }
            defaultMarqueeText.text = descriptiveName;
            defaultMarqueeText.outlineColor = new Color32((byte)rnd.Next(), (byte)rnd.Next(), (byte)rnd.Next(), (byte)(rnd.Next(100) + 155));
            defaultMarqueeText.overrideColorTags = true;
            defaultMarqueeText.faceColor = defaultMarqueeTextColors[rnd.Next(defaultMarqueeTextColors.Length)];
            defaultMarqueeText.ForceMeshUpdate();
            if (defaultMarqueeImage != null)
            {
                defaultMarqueeImage.sprite = defaultMarqueeImages[rnd.Next(defaultMarqueeImages.Length)];
            }

            // Render the camera's view.
            renderCanvasCamera.gameObject.SetActive(true);
            renderCanvasCamera.Render();

            int height = (int)(renderCanvasCamera.targetTexture.height * aspectRatio);
            Texture2D image = new Texture2D(renderCanvasCamera.targetTexture.width, height);
            if (ArcadeManager.currentOS == OS.MacOS)
            {
                image = new Texture2D(renderCanvasCamera.targetTexture.width, 512);
                image.ReadPixels(new Rect(0, 0, renderCanvasCamera.targetTexture.width, 512), 0, 0);
            }
            else
            {
                image.ReadPixels(new Rect(0, renderCanvasCamera.targetTexture.height - height, renderCanvasCamera.targetTexture.width, height), 0, 0);
            }
            image.Apply();

            renderCanvasCamera.gameObject.SetActive(false);
            // Replace the original active Render Texture.
            RenderTexture.active = currentRT;
            return image;
        }
    }
}