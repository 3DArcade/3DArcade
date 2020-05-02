using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        private const string VIDEO_EXTENSIONS = ".mp4|.m4v|.avi|.mkv|.mov|.mpg|.mpeg|.ogv|.webm";
        private const string AUDIO_EXTENSIONS = ".wav|.mp3|.ogg|.aif|.aiff";
        private const string IMAGE_EXTENSIONS = ".png|.jpg|.jpeg";

        public enum ListType
        {
            Mame, Hyperspin, ATF, CatVer, History
        }

        // json
        public static T LoadJSONData<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }

        public static void SaveJSONData<T>(T data, string directoryPath, string fileName)
        {
            if (Directory.Exists(directoryPath))
            {
                _ = Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);
            string json     = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
        }

        // path to video if exists
        public static string GetVideoPathFromFolder(string directoryPath, string fileName)
        {
            if (!Directory.Exists(directoryPath))
            {
                return null;
            }

            fileName = fileName.Trim().ToLowerInvariant();
            return Directory.GetFiles(directoryPath)
                            .FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).Equals(fileName, StringComparison.OrdinalIgnoreCase)
                                                 && VIDEO_EXTENSIONS.Contains(Path.GetExtension(file).ToLowerInvariant()));
        }

        // paths to audio files if exists
        public static string[] GetAudioPathsFromFolder(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return new string[0];
            }

            return Directory.GetFiles(directoryPath)
                            .Where(file => AUDIO_EXTENSIONS.Contains(Path.GetExtension(file).ToLowerInvariant()))
                            .ToArray();
        }

        // textures from images
        public static List<Texture2D> LoadImagesFromFolder(string directoryPath, string fileName)
        {
            List<Texture2D> textureList = new List<Texture2D>();

            if (!Directory.Exists(directoryPath))
            {
                return textureList;
            }

            fileName = fileName.Trim().ToLowerInvariant();
            IEnumerable<string> files = Directory.GetFiles(directoryPath)
                                                 .Where(file => Path.GetFileNameWithoutExtension(file).ToLowerInvariant().Contains(fileName)
                                                             && IMAGE_EXTENSIONS.Contains(Path.GetExtension(file).ToLowerInvariant()));
            foreach (string file in files)
            {
                Texture2D texture = LoadImageFromFile(file);
                if (texture != null)
                {
                    textureList.Add(texture);
                }
            }

            return textureList;
        }

        // texture from image
        public static Texture2D LoadImageFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            //Check if we should use UnityWebRequest or File.ReadAllBytes.
            byte[] imgData;
            if (filePath.StartsWith("://"))
            {
                UnityWebRequest www = UnityWebRequest.Get(filePath);
                _ = www.SendWebRequest();
                imgData = www.downloadHandler.data;
            }
            else
            {
                imgData = File.ReadAllBytes(filePath);
            }

            //Debug.Log("DataLength" + imgData.Length + url);
            if (imgData.Length > 0)
            {
                Texture2D tex = new Texture2D(1, 1);
                if (tex.LoadImage(imgData))
                {
                    return tex;
                }
            }

            return null;
        }

        // file handling
        public static string[] FilesFromDirectory(string directoryPath, string fileExtension = "", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (directoryPath == null || !Directory.Exists(directoryPath))
            {
                return new string[0];
            }

            return Directory.GetFiles(directoryPath, fileExtension, searchOption)
                            .Where(file => !file.StartsWith("._", StringComparison.Ordinal)
                                        && !file.EndsWith(".meta", StringComparison.Ordinal)
                                        && !file.EndsWith(".manifest", StringComparison.Ordinal))
                            .ToArray();
        }

        public static void CopyFile(string sourcePath, string destinationDirectory, string destinationFileName)
        {
            string destinationPath = Path.Combine(destinationDirectory, destinationFileName);
            if (!Directory.Exists(destinationDirectory))
            {
                _ = Directory.CreateDirectory(destinationDirectory);
            }
            File.Copy(sourcePath, destinationPath, true);
        }

        public static bool DeleteFile(string directoryPath, string fileName)
        {
            // TODO: Optionally first find file in subfolders too, needed for arcade configurations and emulator configurations
            try
            {
                string filePath = Path.Combine(directoryPath, fileName);
                Debug.Log($"DeleteFile: {filePath}");
                File.Delete(filePath);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"DeleteFile exception: {e.Message}");
            }

            return false;
        }

        public static bool DeleteFiles(string directoryPath)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (DirectoryInfo directory in directoryInfo.EnumerateDirectories())
                {
                    directory.Delete(true);
                }
                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    file.Delete();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"DeleteFiles exception: {e.Message}");
                return false;
            }
        }

        public static string FileExists(string fileFolder, string fileName, string filePath = null)
        {
            string path = filePath;
            if (path == null)
            {
                if (fileFolder == null || fileName == null)
                {
                    return null;
                }
                path = Path.Combine(fileFolder, fileName);
            }

            return File.Exists(path) ? path : null;
        }

        public static string DirectoryExists(string directorypath) => Directory.Exists(directorypath) ? directorypath : null;

        public static string CorrectFilePath(string filePath, bool ignoreEnd = false, bool ignoreStart = false)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                filePath = filePath.Replace(@"\", "/").Trim();
                if (!ignoreEnd && filePath.Substring(filePath.Length - 1, 1) != "/")
                {
                    filePath += "/";
                }
                if (!ignoreStart && filePath.Substring(0, 1) != "/")
                {
                    filePath = "/" + filePath;
                }
            }

            return filePath;
        }

        public static string GetFilePart(FilePart filePart, string directoryPath, string fileName, string filePath = null)
        {
            string path = filePath;
            if (path == null)
            {
                if (directoryPath == null || fileName == null)
                {
                    return null;
                }
                path = Path.Combine(directoryPath, fileName);
            }

            switch (filePart)
            {
                case FilePart.Path_Name_Extension:
                    return CorrectFilePath(path, true, true);
                case FilePart.RelativePath_Name_Extension:
                    path = CorrectFilePath(path, true, true);
                    path = StripPrefix(path, ArcadeManager.applicationPath);
                    return CorrectFilePath(path, true);
                case FilePart.Path:
                    return CorrectFilePath(Path.GetDirectoryName(path), false, true);
                case FilePart.RelativePath:
                    path = CorrectFilePath(path, true, true);
                    path = StripPrefix(Path.GetDirectoryName(path), ArcadeManager.applicationPath);
                    return CorrectFilePath(path);
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

        public static string StripPrefix(string text, string prefix) => text.StartsWith(prefix, StringComparison.CurrentCulture) ? text.Substring(prefix.Length) : null;

        public static string FileMD5Changed(string md5, string directoryPath, string fileName)
        {
            string filePath = FileExists(directoryPath, fileName);
            if (filePath == null)
            {
                return null;
            }

            string md5File = CalculateMD5(filePath);
            return md5File != md5 ? md5File : null;
        }

        private static string CalculateMD5(string filename)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
                }
            }
        }

        // file dialogs
        public static string DialogGetFolderPath(bool relativePath)
        {
            string[] paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", relativePath ? Path.Combine(ArcadeManager.applicationPath, "3darcade~") : string.Empty, false);
            if (paths != null && paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                string path = paths[0];
                path = CorrectFilePath(path, true, true);
                path = StripPrefix(path, ArcadeManager.applicationPath);
                return CorrectFilePath(path);
            }

            return null;
        }

        public static string DialogGetFilePart(string fileName, string filePath, FilePart filePart, string fileExtension = "")
        {
            if (filePath == null)
            {
                filePath = Path.Combine(ArcadeManager.applicationPath, "3darcade~");
            }

            ExtensionFilter[] extensions = new[]
            {
                new ExtensionFilter("extensions", fileExtension.Split(','))
            };

            string[] paths = StandaloneFileBrowser.OpenFilePanel(fileName, filePath, extensions, false);
            if (paths.Length < 1)
            {
                return null;
            }

            return GetFilePart(filePart, null, null, paths[0]);
        }

        // mastergamelists
        public static Tuple<string, ListType> DialogGetMasterGamelist(string id, string extension)
        {
            string file = DialogGetFilePart(null, ArcadeManager.applicationPath + "/3darcade~/", FilePart.Path_Name_Extension, extension);
            return GetMasterGamelist(file, id);
        }

        public static Tuple<string, ListType> GetMasterGamelist(string filePath, string id)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            string text = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (Path.GetExtension(filePath).Equals(".ini", StringComparison.OrdinalIgnoreCase))
            {
                if (!text.Contains("[Category]"))
                {
                    return null;
                }
                CopyFile(filePath, ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/mame/", id + ".ini");
                return new Tuple<string, ListType>(filePath, ListType.Mame);
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
            if (lines.Length > 0)
            {
                string[] items = lines[0].Split('|');
                if (items.Length > 10)
                {
                    CopyFile(filePath, ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/atf/", id + ".atf");
                    return new Tuple<string, ListType>(filePath, ListType.ATF);
                }
            }

            return null;
        }

        // assets
        public static string[] GetListOfAssetNames(string modelType, bool external)
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

            string[] assetNames = FilesFromDirectory(path, extension);
            for (int i = 0; i < assetNames.Length; ++i)
            {
                assetNames[i] = Path.GetFileNameWithoutExtension(assetNames[i]);
            }

            return assetNames;
        }
    }
}
