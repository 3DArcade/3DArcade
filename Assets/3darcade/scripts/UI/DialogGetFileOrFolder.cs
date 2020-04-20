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

        [SerializeField]
        private DialogType dialogType;
        [SerializeField]
        private InputField Target;

        public void Show()
        {
            switch (dialogType)
            {
                case DialogType.Executable:
                    var executable = Arcade.FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Name_Extension, "exe");
                    if (executable != null)
                    {
                        Target.text = executable;
                    }
                    break;
                case DialogType.Dll:
                    var dll = Arcade.FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Name, "dll,dylib");
                    if (dll != null)
                    {
                        Target.text = dll;
                    }
                    break;
                case DialogType.Ini:
                    var ini = Arcade.FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Path_Name_Extension, "ini");
                    if (ini != null)
                    {
                        Target.text = ini;
                    }
                    break;
                case DialogType.MasterGamelist:
                    var masterGamelist = Arcade.FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Path_Name_Extension, "atf,xml");
                    if (masterGamelist != null)
                    {
                        Target.text = masterGamelist;
                    }
                    break;
                case DialogType.Image:
                    var image = Arcade.FileManager.DialogGetFilePart(null, ArcadeManager.applicationPath, FileManager.FilePart.Path_Name_Extension, "jpg,png");
                    if (image != null)
                    {
                        if (ArcadeManager.arcadeState == ArcadeStates.ArcadesConfigurationMenu)
                        {
                            string destinationFolder = ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath;
                            string destinationFilename = ArcadeManager.arcadeConfiguration.id + FileManager.getFilePart(FileManager.FilePart.Extension, null, null, image);
                            FileManager.DeleteFile(destinationFolder, ArcadeManager.arcadeConfiguration.id + ".jpg");
                            FileManager.DeleteFile(destinationFolder, ArcadeManager.arcadeConfiguration.id + ".png");
                            FileManager.CopyFile(image, destinationFolder, destinationFilename);
                            Target.text = FileManager.getFilePart(FileManager.FilePart.Name_Extension, destinationFolder, destinationFilename);
                        }
                    }
                    break;
                default:
                    var folder = Arcade.FileManager.DialogGetFolderPath(true);
                    if (folder != null)
                    {
                        Target.text = folder;
                    }
                    break;
            }
        }
    }
}
