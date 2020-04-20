using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Arcade;
using System.Linq;
using System.Collections.Generic;

public class ScrollIndexCallbackEmulatorsConfiguration : MonoBehaviour
{
    public Image image;
    public Text text;
    private int index = -1;
    private List<string> listString = new List<string>();
    private void Start()
    {
        //if (listString.Count < 1)
        //{
        //    print("start with scroll");
        //    var list = ArcadeManager.masterGamelist;
        //    list = list.Where(x => x.idParent.Trim() == "").ToList();
        //    list = list.Where(x => x.runnable == true).ToList();
        //    list = list.Where(x => x.mature == false).ToList();
        //    list = list.OrderBy(x => x.descriptiveName).ToList();
        //    foreach (ModelProperties item in list)
        //    {
        //        listString.Add(item.descriptiveName);
        //    }
        //}

    }

    void ScrollCellIndex(int idx)
    {
        // if (idx > listString.Count - 1) { return; }
        index = idx;
        string name = "Cell " + idx.ToString();
        if (text != null)
        {
            // print("game " + ArcadeManager.masterGamelist[idx].id);
            text.text = EmulatorsConfigurationEmulatorProperties.filteredSelectedModelList[idx].descriptiveName;
        }
        //if (image != null)
        //{
        //    image.color = Rainbow(idx / 50.0f);
        //}
        gameObject.name = name;
    }

    public void ButtonClicked()
    {
        print("clicked nr " + index);
        GameObject obj = GameObject.Find("EmulatorsConfigurationUI");
        EmulatorsConfigurationEmulatorProperties filter = obj.GetComponent<EmulatorsConfigurationEmulatorProperties>();
        filter.SetSelectedModel(index);
    }

    // http://stackoverflow.com/questions/2288498/how-do-i-get-a-rainbow-color-gradient-in-c
    public static Color Rainbow(float progress)
    {
        progress = Mathf.Clamp01(progress);
        float r = 0.0f;
        float g = 0.0f;
        float b = 0.0f;
        int i = (int)(progress * 6);
        float f = progress * 6.0f - i;
        float q = 1 - f;

        switch (i % 6)
        {
            case 0:
                r = 1;
                g = f;
                b = 0;
                break;
            case 1:
                r = q;
                g = 1;
                b = 0;
                break;
            case 2:
                r = 0;
                g = 1;
                b = f;
                break;
            case 3:
                r = 0;
                g = q;
                b = 1;
                break;
            case 4:
                r = f;
                g = 0;
                b = 1;
                break;
            case 5:
                r = 1;
                g = 0;
                b = q;
                break;
        }
        return new Color(r, g, b);
    }
}
