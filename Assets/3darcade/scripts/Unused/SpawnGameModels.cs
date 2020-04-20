//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Arcade;
//using System.Linq;

//public class SpawnGameModels : MonoBehaviour
//{
//    private Object gameModels;
//    public static List<GameObject> myObjects;
//    private GameObject[] myObjectsArray;

//    void Start()
//    {

//        var list = ArcadeManager.masterGamelist;
//        list = list.Where(x => x.idParent.Trim() == "").ToList();
//        list = list.Where(x => x.runnable == true).ToList();
//        list = list.Where(x => x.mature == false).ToList();
//        list = list.OrderBy(x => x.descriptiveName).ToList();
//        if (list.Count > 0)
//        {
//            Dropdown dropdown = gameObject.GetComponent<Dropdown>();
//            dropdown.options.Clear();
//            dropdown.options.Add(new Dropdown.OptionData("none"));
//            foreach (ModelProperties item in list)
//            {
//                //GameObject myObj = Instantiate(myObject);
//                dropdown.options.Add(new Dropdown.OptionData(item.descriptiveName));
//            }
//            dropdown.onValueChanged.AddListener(delegate {
//                DropdownValueChangedHandler(dropdown);
//            });
//        }

//       // gameModels = Resources.LoadAll("arcades/gamesSetup",typeof(pref));
//        //myObjectsArray = Resources.LoadAll<GameObject>("games");
//        //myObjects = new List<GameObject>(myObjectsArray);
//        ////Debug.Log(myObjects[0]);
//        //if (myObjects.Count > 0)
//        //{
//        //    Dropdown dropdown = gameObject.GetComponent<Dropdown>();
//        //    dropdown.options.Clear();
//        //    dropdown.options.Add(new Dropdown.OptionData("none"));
//        //    foreach (GameObject myObject in myObjects) 
//        //    {
//        //        //GameObject myObj = Instantiate(myObject);
//        //        dropdown.options.Add(new Dropdown.OptionData(myObject.name));
//        //    }
//        //    dropdown.onValueChanged.AddListener(delegate {
//        //        DropdownValueChangedHandler(dropdown);
//        //    });
//        //}
//    }

//    public void Spawn() 
//    {
//        Debug.Log("spawn");
//    }

//    private void DropdownValueChangedHandler(Dropdown target)
//    {
//        //Debug.Log("selected: " + target.value);
//        if (target.value > 0)
//        {
//            GameObject myobj = Instantiate(myObjects[target.value - 1]);
//            myobj.transform.parent = GameObject.Find("GameModels").transform;
//            myobj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 8;
//            myobj.transform.LookAt(Camera.main.transform.parent.transform);
//            myobj.transform.rotation = Quaternion.Euler(-90,myobj.transform.eulerAngles.y, myobj.transform.eulerAngles.z);
//        }
//    }
//}
