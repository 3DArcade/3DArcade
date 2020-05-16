using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{
    public class DialogGetFileOrFolder : MonoBehaviour
    {
        private enum DialogType
        {
            Folder,
            Executable,
            Image,
            Dll,
            Ini,
            MasterGamelist
        }

        [SerializeField] private DialogType dialogType = default;
        [SerializeField] private InputField Target     = default;

        public void Show()
        {
            switch (dialogType)
            {
                case DialogType.Executable:
                    string executable = FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Name_Extension, "exe");
                    if (executable != null)
                    {
                        Target.text = executable;
                    }
                    break;
                case DialogType.Dll:
                    string dll = FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Name, "dll,dylib");
                    if (dll != null)
                    {
                        Target.text = dll;
                    }
                    break;
                case DialogType.Ini:
                    string ini = FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Path_Name_Extension, "ini");
                    if (ini != null)
                    {
                        Target.text = ini;
                    }
                    break;
                case DialogType.MasterGamelist:
                    string masterGamelist = FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Path_Name_Extension, "atf,xml");
                    if (masterGamelist != null)
                    {
                        Target.text = masterGamelist;
                    }
                    break;
                case DialogType.Image:
                    string image = FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Path_Name_Extension, "jpg,png");
                    if (image != null)
                    {
                        if (ArcadeManager.arcadeState == ArcadeStates.ArcadesConfigurationMenu)
                        {
                            string destinationFolder = ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath;
                            string destinationFilename = ArcadeManager.arcadeConfiguration.id + FileManager.GetFilePart(FileManager.FilePart.Extension, null, null, image);
                            _ = FileManager.DeleteFile(destinationFolder, ArcadeManager.arcadeConfiguration.id + ".jpg");
                            _ = FileManager.DeleteFile(destinationFolder, ArcadeManager.arcadeConfiguration.id + ".png");
                            FileManager.CopyFile(image, destinationFolder, destinationFilename);
                            Target.text = FileManager.GetFilePart(FileManager.FilePart.Name_Extension, destinationFolder, destinationFilename);
                        }
                    }
                    break;
                default:
                    string folder = FileManager.DialogGetFolderPath(true);
                    if (folder != null)
                    {
                        Target.text = folder;
                    }
                    break;
            }
        }
    }
}
