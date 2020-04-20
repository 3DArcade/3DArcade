using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class CopySomeFiles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //string history = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath + "/3darcade~/Configuration/MasterGamelists/", "history.csv"));
        //HistoryHelper csv = new HistoryHelper();
        //csv.saveHistory(history);
        //var filePath = "c:/history/";
   
        //GameObject[] myObjectsArray = Resources.LoadAll<GameObject>("games");
        //foreach (GameObject item in myObjectsArray)
        //{
        //    var name = item.name;
        //    if (System.IO.File.Exists(System.IO.Path.Combine(filePath, name + ".txt")))
        //    {
        //        System.IO.File.Copy(System.IO.Path.Combine(filePath, name + ".txt"), Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/info/" + name + ".txt", true);
        //    }
        //}
        //    return;
        //GameObject[] myObjectsArray = Resources.LoadAll<GameObject>("games");
        //foreach (GameObject item in myObjectsArray)
        //{
        //    var name = item.name;
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/snap/" + name + ".png"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/snap/" + name + ".png", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/snap/" + name + ".png", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/marquees/" + name + ".png"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/marquees/" + name + ".png", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/marquees/" + name + ".png", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/titles/" + name + ".png"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/titles/" + name + ".png", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/titles/" + name + ".png", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/cabinets/" + name + ".png"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/cabinets/" + name + ".png", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/cabinets/" + name + ".png", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/video/" + name + ".mp4"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/video/" + name + ".mp4", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/video/" + name + ".mp4", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/roms/" + name + ".zip"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/roms/" + name + ".zip", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/roms/" + name + ".zip", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/samples/" + name + ".zip"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/samples/" + name + ".zip", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/samples/" + name + ".zip", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/artwork/" + name + ".zip"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/artwork/" + name + ".zip", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/artwork/" + name + ".zip", true);
        //    }
        //    if (System.IO.File.Exists(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/flyers/" + name + ".png"))
        //    {
        //        System.IO.File.Copy(Application.dataPath + "/StreamingAssets/frontend/emulators/mame/flyers/" + name + ".png", Application.dataPath + "/StreamingAssets/3darcade~/emulators/mame/flyers/" + name + ".png", true);
        //    }





        //}
    }

    public class HistoryHelper
    {
        public void saveHistory(string csv)
        {
            var lines = csv.Split(Environment.NewLine.ToCharArray());
            int gameCount = 1;
            string game = gameCount.ToString() + ";";
            string info = "";
            string fileName = "";
            foreach (string line in lines)
            {
                if (line.StartsWith(game)) { //  print("line " + line);
                    if (fileName != "" && info != "")
                    {
                        var filePath = "c:/history/";
                        if (!System.IO.Directory.Exists(filePath))
                        {
                            System.IO.Directory.CreateDirectory(filePath);
                        }
                        string file = System.IO.Path.Combine(filePath, fileName + ".txt");

                        System.IO.File.WriteAllText(file, info);
                    }

                    var name = line.Split(';');
                    if (name.Length > 1)
                    {
                        fileName = name[1];
                    }
                    gameCount = gameCount + 1;
                    game = gameCount.ToString() + ";";
                    info = "";
                }
                else
                {
                    var tline = line;
                    if (tline.StartsWith("\";\""))
                    {
                        tline = tline.Remove(0, 3);
                    }
                    if (tline.StartsWith("\";"))
                    {
                        tline = tline.Remove(0, 2);
                    }
                    if (tline.StartsWith("\""))
                    {
                        tline = tline.Remove(0, 1);
                    }
                    if (tline.EndsWith("\""))
                    {
                        tline.Remove(tline.Length - 1);
                    }
                    info = info + tline + Environment.NewLine;
                }
  
            }
        }
    }
}
