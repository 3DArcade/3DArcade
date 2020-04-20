using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade;

namespace Arade
{
    public class MainMenuButtonList : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The panel where new buttons will be added as children")]
        private RectTransform content;
        [SerializeField]
        [Tooltip("The button that will be used as a template to create new ones")]
        private GameObject Button_Template;


        public void UpdateMainMenuList()
        {
            //Clear menu buttons

            List<GameObject> thisChildren = new List<GameObject>();
            foreach (Transform child in content.transform)
            {
                if (child.gameObject != null)
                {
                    thisChildren.Add(child.gameObject);
                }
            }

            foreach (GameObject child in thisChildren)
            {
                //print("trydestroy " + child.name);
                if (child.name.Contains("lone"))
                {
                    //print("destroyclone");
                    Object.Destroy(child);
                }
                else
                {
                    //print("no clone");
                }
            }

            print("number of arcades is " + ArcadeManager.arcadesConfigurationList.Count);
            List<Texture2D> textureList = new List<Texture2D>();
            for (int i = 0; i < ArcadeManager.arcadesConfigurationList.Count; i++)
            {
                var arcade = ArcadeManager.arcadesConfigurationList[i];
                var file = FileManager.FileExists(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, arcade.id + ".jpg");
                if (file == null)
                {
                    file = FileManager.FileExists(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, arcade.id + ".png");
                }
                Texture2D texture = FileManager.LoadImageFromFile(null, null, file);

                GameObject go = Instantiate(Button_Template) as GameObject;
                go.transform.SetParent(content);
                go.SetActive(true);

                MainMenuButton TB = go.GetComponent<MainMenuButton>();
                TB.SetName(arcade.descriptiveName, texture == null ? arcade.descriptiveName : "");
                TB.SetIndex(i);
                if (texture != null)
                {
                    Image image = go.GetComponent<Image>();
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
                //go.transform.SetParent(Button_Template.transform.parent);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(go.gameObject);
            }
        }
    }
}

