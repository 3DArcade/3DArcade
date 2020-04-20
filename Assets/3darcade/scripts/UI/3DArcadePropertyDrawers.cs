using System;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Arcade
{
    public class ArcadePopUp : PropertyAttribute
    {
        public ArcadePopUp(params string[] list)
        {
            List = list;
        }
        
        public ArcadePopUp(Type type)
        {
            if (type.IsEnum)
            {
                List = Enum.GetNames(type) as string[];
            }
            else
            {
                try
                {
                    List = type.GetProperties().Select(x => x.Name).ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception : {0} ", e.Message);
                }
                if (List.Length < 1)
                {
                    try
                    {
                        List = type.GetFields().Select(x => x.Name).ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception : {0} ", e.Message);
                    }
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

        public string[] List
        {
            get;
            private set;
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

