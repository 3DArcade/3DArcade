using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{
    public class MainMenuButton : MonoBehaviour
    {
        private int arcadeIndex;
        private string arcadeName;
        public Text ButtonText;
        public GameObject menu;

        public void SetName(string name, string label)
        {
            arcadeName = name;
            GetComponentInChildren<Text>().text = label;
        }
        public void SetIndex(int index)
        {
            arcadeIndex = index;
        }
        public void Button_Click()
        {
            // Start Arcade
            Debug.Log("Clicked!!!");
            // ArcadeManager.loadSaveArcade.StartArcadeWithIndex(arcadeIndex);
        }

        public void Button_Hover_Enter()
        {
            ButtonText.text = arcadeName;
        }

        public void Button_Hover_Leave()
        {
            ButtonText.text = "---";
        }
    }
}



