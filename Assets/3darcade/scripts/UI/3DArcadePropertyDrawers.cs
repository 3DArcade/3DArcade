using System;
using System.Linq;
using UnityEngine;

namespace Arcade
{
    public class ArcadePopUp : PropertyAttribute
    {
        public readonly string[] List;

        public ArcadePopUp(params string[] list) => List = list;

        public ArcadePopUp(Type type)
        {
            if (type.IsEnum)
            {
                List = Enum.GetNames(type);
            }
            else
            {
                try
                {
                    List = type.GetProperties().Select(x => x.Name).ToArray();
                    if (List.Length < 1)
                    {
                        List = type.GetFields().Select(x => x.Name).ToArray();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public ArcadePopUp(string name)
        {
            if (name == "Arcade")
            {
                List = ArcadeManager.arcadesConfigurationList.Select(x => x.id).ToList().ToArray();
            }
        }
    }

    //[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ArcadeAttribute : PropertyAttribute
    {
        public string label;
        public GUIStyle labelStyle;
        public float width;
        public ModelSetup modelSetup;

        public ArcadeAttribute(string label)
        {
            this.label = label;
            //   this.modelSetup = modelSetup;
            labelStyle = GUI.skin.GetStyle("miniLabel");
            width = labelStyle.CalcSize(new GUIContent(label)).x;
        }
    }
}
