using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindow : MonoBehaviour
{

    // 200x300 px window will apear in the center of the screen.
    public Rect windowRect = new Rect((Screen.width - 200) / 2, (Screen.height - 300) / 2, 200, 300);
    // Only show it if needed.
    private bool show = false;


    void OnGUI()
    {

        print("showme!");
        if (show)
           
        print("showmenow!");
        windowRect = GUI.Window(0, windowRect, Dialog, "Hello there");
    }

    // This is the actual window.
    void Dialog(int windowID)
    {

        GUI.DragWindow(new Rect(0, 0, 10000, 20));
        GUILayout.BeginVertical();
        GUILayout.Label("Again?");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Restart"))
        {
            // Application.LoadLevel(0);
            //  show = false;
        }

        if (GUILayout.Button("Exit"))
        {
            //  Application.Quit();
            //   show = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
     

    }

    // To open the dialogue from outside of the script.
    public void Open()
    {
        print("showmefirst!");
        show = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
