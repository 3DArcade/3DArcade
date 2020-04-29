using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Arcade
{
    public class FileManager
    {
        public enum FilePart
        {
            Path_Name_Extension,
            RelativePath_Name_Extension,
            Path,
            RelativePath,
            Name_Extension,
            Name,
            Extension
        }

        public enum ListType
        {
            Mame, Hyperspin, ATF, CatVer, History
        }

        // json
        public static T LoadJSONData<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                object resultValue = JsonUtility.FromJson<T>(dataAsJson);
                return (T)Convert.ChangeType(resultValue, typeof(T));
            }
            else
            {
                Debug.Log("Failed to load from " + filePath);
                return default;
            }
        }

        public static void SaveJSONData<T>(T data, string filePath, string fileName)
        {
            if (!Directory.Exists(filePath))
            {
                _ = Directory.CreateDirectory(filePath);
            }
            string file = Path.Combine(filePath, fileName);
            string dataAsJson = JsonUtility.ToJson(data, true);
            File.WriteAllText(file, dataAsJson);
        }

        // text
        public static string LoadTextFromFile(string fileFolder, string fileName, string filePath = null)
        {
            string path = filePath;
            if (path == null)
            {
                path = Path.Combine(fileFolder, fileName);
            }
            if (!File.Exists(path))
            { return null; }
            string text = File.ReadAllText(path);
            return text;
        }

        // path to video if exists
        public static string GetVideoPathFromFolder(string fileFolder, string fileName)
        {
            string path = DirectoryExists(fileFolder);
            if (path == null)
            { return null; }
            List<string> list = Directory.GetFiles(path).ToList();
            list = list.Where(file => Regex.IsMatch(Path.GetFileName(file).ToLower(), "^" + fileName.Trim().ToLower() + "\\.(mp4|m4v|mov|mpg|mpeg|ogv)$")).ToList();
            if (list.Count > 0)
            { return list[0]; }
            return null;
        }

        // paths to audio files if exists
        public static List<string> GetAudioPathsFromFolder(string fileFolder)
        {
            List<string> audioList = new List<string>();
            string path = DirectoryExists(fileFolder);
            if (path == null)
            { return audioList; }

            audioList = Directory.GetFiles(path).Where(file => Regex.IsMatch(Path.GetFileName(file).ToLower(), ".*\\.(wav|mp3|ogg|aif|aiff)$")).ToList();
            Debug.Log("audioyes " + audioList.Count);
            return audioList;
        }

        // textures from images
        public static List<Texture2D> LoadImagesFromFolder(string fileFolder, string fileName)
        {
            List<Texture2D> textureList = new List<Texture2D>();
            string path = DirectoryExists(fileFolder);
            if (path == null)
            { return textureList; }

            List<string> list = Directory.GetFiles(path).ToList();
            List<string> filteredList = list.Where(file => Regex.IsMatch(Path.GetFileName(file).ToLower(), "^" + fileName.Trim().ToLower() + "_.*\\.(png|jpg|jpeg)$")).ToList();
            if (filteredList.Count < 1)
            {
                filteredList = list.Where(file => Regex.IsMatch(Path.GetFileName(file).ToLower(), "^" + fileName.Trim().ToLower() + "\\.(png|jpg|jpeg)$")).ToList();
            }

            foreach (string file in filteredList)
            {
                Texture2D texture = LoadImageFromFile(null, null, file);
                if (texture != null)
                {
                    textureList.Add(texture);
                }
            }
            return textureList;
        }

        // texture from image
        public static Texture2D LoadImageFromFile(string fileFolder, string fileName, string filePath = null)
        {
            string url = filePath;
            if (url == null)
            {
                if (fileFolder == null || fileName == null)
                { return null; }
                url = FileExists(fileFolder, fileName);
            }
            if (url == null)
            { return null; }
            byte[] imgData;
            Texture2D tex = new Texture2D(1, 1);

            //Check if we should use UnityWebRequest or File.ReadAllBytes.
            if (url.Contains("://") || url.Contains(":///"))
            {
                UnityWebRequest www = UnityWebRequest.Get(url);
                _ = www.SendWebRequest();
                imgData = www.downloadHandler.data;
            }
            else
            {
                imgData = File.ReadAllBytes(url);
            }
            //Debug.Log("DataLength" + imgData.Length + url);
            if (imgData.Length > 0)
            {
                _ = tex.LoadImage(imgData);
                return tex;
            }
            return null;
        }

        // file handling
        public static List<FileInfo> FilesFromDirectory(string fileFolder, string extension, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            string path = DirectoryExists(fileFolder);
            if (path != null)
            {
                //Debug.Log("directory does exist " + path);
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FileInfo[] files;
                if (extension != null && extension.Trim() != "")
                {
                    files = directoryInfo.GetFiles(extension);
                }
                else
                {
                    files = directoryInfo.GetFiles();
                }
                List<FileInfo> list = files.ToList();
                list = (System.Collections.Generic.List<System.IO.FileInfo>)list.Where(x => x.Name.StartsWith("._", StringComparison.Ordinal) == false).ToList();
                list = (System.Collections.Generic.List<System.IO.FileInfo>)list.Where(x => x.Name.EndsWith(".meta", StringComparison.Ordinal) == false).ToList();
                list = (System.Collections.Generic.List<System.IO.FileInfo>)list.Where(x => x.Name.EndsWith(".manifest", StringComparison.Ordinal) == false).ToList();
                if (list.Count > 0)
                {
                    return list;
                }
                Debug.Log("No Files found");
                return null;
            }
            Debug.Log("Directory does not exist " + fileFolder);
            return null;
        }

        public static void CopyFile(string originalPath, string destinationFolder, string destinationFileName)
        {
            string destinationPath = Path.Combine(destinationFolder, destinationFileName);
            if (!Directory.Exists(destinationFolder))
            {
                _ = Directory.CreateDirectory(destinationFolder);
            }
            File.Copy(originalPath, destinationPath, true);
        }

        public static bool DeleteFile(string filePath, string fileName)
        {
            // TODO: Optionally first find file in subfolders too, needed for arcade configurations and emulator configurations
            string path = Path.Combine(filePath, fileName);
            Debug.Log("deletepath " + path);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (System.Exception ex)
                {
                    Debug.Log("Delete file error: " + ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool DeleteFilesInfolder(string path)
        {
            try
            {
                System.IO.DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in directoryInfo.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.Log("Delete file error: " + ex);
                return false;
            }
        }


        public static string FileExists(string fileFolder, string fileName, string filePath = null)
        {
            string path = filePath;
            if (path == null)
            {
                if (fileFolder == null || fileName == null)
                { return null; }
                path = Path.Combine(fileFolder, fileName);
            }
            if (!File.Exists(path))
            { return null; }
            return path;
        }

        public static string DirectoryExists(string fileFolder)
        {
            if (!Directory.Exists(fileFolder))
            { return null; }
            return fileFolder;
        }

        public static string CorrectFilePath(string path, bool ignoreEnd = false, bool ignoreStart = false)
        {
            path = path.Replace(@"\", "/");
            string filePath = path.Trim();
            if (filePath != "")
            {
                if (filePath.Substring(filePath.Length - 1, 1) != "/" && ignoreEnd == false)
                {
                    filePath += "/";
                }
                if (filePath.Substring(0, 1) != "/" && ignoreStart == false)
                {
                    filePath = "/" + filePath;
                }
            }
            return filePath;
        }

        public static string GetFilePart(FilePart filePart, string fileFolder, string fileName, string filePath = null)
        {
            string path = filePath;
            if (path == null)
            {
                if (fileFolder == null || fileName == null)
                { return null; }
                path = Path.Combine(fileFolder, fileName);
            }
            switch (filePart)
            {
                case FilePart.Path_Name_Extension:
                    return CorrectFilePath(path, true, true);
                case FilePart.RelativePath_Name_Extension:
                    path = CorrectFilePath(path, true, true);
                    path = StripPrefix(path, ArcadeManager.applicationPath);
                    return path == null ? null : CorrectFilePath(path, true);
                case FilePart.Path:
                    return CorrectFilePath(Path.GetDirectoryName(path), false, true);
                case FilePart.RelativePath:
                    path = CorrectFilePath(path, true, true);
                    path = StripPrefix(Path.GetDirectoryName(path), ArcadeManager.applicationPath);
                    return path == null ? null : CorrectFilePath(path);
                case FilePart.Name_Extension:
                    return Path.GetFileName(path);
                case FilePart.Name:
                    return Path.GetFileNameWithoutExtension(path);
                case FilePart.Extension:
                    return Path.GetExtension(path);
                default:
                    return null;
            }
        }

        public static string StripPrefix(string text, string prefix)
        {
            return text.StartsWith(prefix, StringComparison.CurrentCulture) ? text.Substring(prefix.Length) : null;
        }

        public static string FileMD5Changed(string MD5, string filePath, string fileName)
        {
            string path = FileExists(filePath, fileName);
            if (path == null)
            { return null; }
            string newMD5 = CalculateMD5(path);
            if (newMD5 != MD5)
            {
                return newMD5;
            }
            else
            {
                return null;
            }
        }

        private static string CalculateMD5(string filename)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        // file dialogs
        public static string DialogGetFolderPath(bool relativePath)
        {
            string[] paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", Path.Combine(ArcadeManager.applicationPath + "/3darcade~/"), false);
            if (paths != null && paths.Length > 0 && paths[0] != "")
            {
                string path = paths[0];
                path = CorrectFilePath(path, true, true);
                path = StripPrefix(path, ArcadeManager.applicationPath);
                return path == null ? null : CorrectFilePath(path);
            }
            return null;
        }

        public static string DialogGetFilePart(string fileName, string filePath, FilePart filePart, string fileExtension = "")
        {
            if (filePath == null)
            {
                filePath = Path.Combine(ArcadeManager.applicationPath + "/3darcade~/");
            }
            ExtensionFilter[] extensions = new[] {
                new SFB.ExtensionFilter("extensions", fileExtension.Split(','))
            };
            string[] paths = StandaloneFileBrowser.OpenFilePanel(fileName, filePath, extensions, false);
            string path = null;
            if (paths.Length < 1)
            { return path; }
            path = paths[0];
            return GetFilePart(filePart, null, null, path);
        }

        // mastergamelists
        public static Tuple<string, ListType> DialogGetMasterGamelist(string id, string extension)
        {
            string file = DialogGetFilePart(null, ArcadeManager.applicationPath + "/3darcade~/", FilePart.Path_Name_Extension, extension);
            return GetMasterGamelist(file, id);
        }

        public static Tuple<string, ListType> GetMasterGamelist(string filePath, string id)
        {
            if (filePath == null)
            { return null; }
            if (FileExists(null, null, filePath) == null)
            { return null; }
            string text = LoadTextFromFile(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
            if (text == null)
            { return null; }
            if (Path.GetExtension(filePath) == "ini")
            {
                if (text.Contains("[Category]"))
                {
                    CopyFile(filePath, ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/mame/", id + ".ini");
                    return new Tuple<string, ListType>(filePath, ListType.Mame);
                }
                else
                {
                    return null;
                }

            }
            if (text.Contains("<mame>") && text.Contains("<dipswitch"))
            {
                CopyFile(filePath, ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/mame/", id + ".xml");
                return new Tuple<string, ListType>(filePath, ListType.Mame);
            }
            if (text.Contains("<menu>") && text.Contains("<game"))
            {
                CopyFile(filePath, ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/hyperspin/", id + ".xml");
                return new Tuple<string, ListType>(filePath, ListType.Hyperspin);
            }
            string[] lines = text.Split(Environment.NewLine.ToCharArray());
            if (lines.Count() > 0)
            {
                string[] items = lines[0].Split("|".ToCharArray());
                if (items.Count() > 10)
                {
                    CopyFile(filePath, ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/atf/", id + ".atf");
                    return new Tuple<string, ListType>(filePath, ListType.ATF);
                }
            }
            return null;
        }

        // assets
        public static List<string> GetListOfAssetNames(string modelType, bool external)
        {
            string path;
            string extension;
            if (!external)
            {
                path = Application.dataPath + "/Resources/" + modelType + "s/";
                extension = "*.prefab";
            }
            else
            {
                path = ArcadeManager.applicationPath + "/3darcade~/Configuration/Assets/" + ArcadeManager.currentOS.ToString() + "/" + modelType + "s/";
                extension = "*.unity3d";
            }
            List<FileInfo> myGamesAssets = FilesFromDirectory(path, extension);
            List<string> myGamesAssetNames = new List<string>();
            if (myGamesAssets == null)
            { return new List<string>(); }
            foreach (FileInfo asset in myGamesAssets)
            {
                myGamesAssetNames.Add(Path.GetFileNameWithoutExtension(asset.Name));
            }
            return myGamesAssetNames;
        }
    }

    public static class UnityExtensions
    {
        public static void RunOnChildrenRecursive(this GameObject go, Action<GameObject> action)
        {
            if (go == null)
            {
                return;
            }

            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                action(trans.gameObject);
            }
        }

        public static List<GameObject> GetChildren(this GameObject gameObject)
        {
            List<GameObject> thisChildren = new List<GameObject>();
            if (gameObject == null)
            { return thisChildren; }
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject != null)
                {
                    thisChildren.Add(child.gameObject);
                }
            }
            return thisChildren;
        }
    }
}
