using IniParser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Arcade
{
    public class UpdateMasterGamelists
    {
        public static void UpdateAll()
        {
            if (ArcadeManager.emulatorsConfigurationList.Count < 1)
            {
                _ = ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulatorsConfigurationList();
            }
            if (ArcadeManager.emulatorsConfigurationList.Count > 0)
            {
                for (int i = 0; i < ArcadeManager.emulatorsConfigurationList.Count; i++)
                {
                    EmulatorConfiguration emulatorConfiguration = ArcadeManager.emulatorsConfigurationList[i];
                    emulatorConfiguration = UpdateMasterGamelistFromEmulatorConfiguration(emulatorConfiguration);
                    ArcadeManager.emulatorsConfigurationList[i] = emulatorConfiguration;
                }
                ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorsConfigurationList();
            }
        }

        public static EmulatorConfiguration UpdateMasterGamelistFromEmulatorConfiguration(EmulatorConfiguration emulatorConfiguration, UnityEngine.UI.Slider slider = null)
        {
            List<ModelProperties> masterGamelist;
            Debug.Log("Updating emulator configuration " + emulatorConfiguration.emulator.descriptiveName + " with id " + emulatorConfiguration.emulator.id);
            // Mame xml?
            string fileName = emulatorConfiguration.emulator.id + ".xml";
            string filePath = ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/mame/";
            if (FileManager.FileExists(filePath, fileName) != null)
            {
                string md5 = FileManager.FileMD5Changed(emulatorConfiguration.md5MasterGamelist, filePath, fileName);
                if (md5 != null)
                {
                    masterGamelist = GetGamelistFromMameXML(filePath, fileName, emulatorConfiguration.emulator, slider);
                    if (masterGamelist != null)
                    {
                        emulatorConfiguration.lastMasterGamelistUpdate = DateTime.Now.ToString();
                        emulatorConfiguration.masterGamelist = masterGamelist;
                        emulatorConfiguration.md5MasterGamelist = md5;
                        Debug.Log("MasterGamelist updated from mame xml");
                    }
                    else
                    {
                        Debug.Log("MasterGamelist update failed from mame xml");
                    }
                }
                else
                {
                    Debug.Log("MasterGamelist is current, updated from mame xml is not needed");
                }
                return emulatorConfiguration;
            }

            // Hyperspin xml?
            fileName = emulatorConfiguration.emulator.id + ".xml";
            filePath = ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/hyperspin/";
            if (FileManager.FileExists(filePath, fileName) != null)
            {
                string md5 = FileManager.FileMD5Changed(emulatorConfiguration.md5MasterGamelist, filePath, fileName);
                if (md5 != null)
                {
                    masterGamelist = GetGamelistFromHyperspinXML(filePath, fileName, emulatorConfiguration.emulator, slider);
                    if (masterGamelist != null)
                    {
                        emulatorConfiguration.lastMasterGamelistUpdate = DateTime.Now.ToString();
                        emulatorConfiguration.masterGamelist = masterGamelist;
                        emulatorConfiguration.md5MasterGamelist = md5;
                        Debug.Log("MasterGamelist updated from atf");
                    }
                    else
                    {
                        Debug.Log("MasterGamelist update failed from hyperpsin xml");
                    }
                }
                else
                {
                    Debug.Log("MasterGamelist is current, updated from hyperspin xml is not needed");
                }
                return emulatorConfiguration;
            }

            // 3darcade atf?
            fileName = emulatorConfiguration.emulator.id + ".atf";
            filePath = ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/atf/";
            if (FileManager.FileExists(filePath, fileName) != null)
            {
                string md5 = FileManager.FileMD5Changed(emulatorConfiguration.md5MasterGamelist, filePath, fileName);
                if (md5 != null)
                {
                    masterGamelist = GetGamelistFrom3DArcadeATF(filePath, fileName, emulatorConfiguration.emulator, slider);
                    if (masterGamelist != null)
                    {
                        emulatorConfiguration.lastMasterGamelistUpdate = DateTime.Now.ToString();
                        emulatorConfiguration.masterGamelist = masterGamelist;
                        emulatorConfiguration.md5MasterGamelist = md5;
                        Debug.Log("MasterGamelist updated from 3darcade atf");
                    }
                    else
                    {
                        Debug.Log("MasterGamelist update failed from 3darcade atf");
                    }
                }
                else
                {
                    Debug.Log("MasterGamelist is current, updated from 3darcade atf is not needed");
                }
                return emulatorConfiguration;
            }

            // from folder?
            string gamePath = emulatorConfiguration.emulator.gamePath.Trim();
            filePath = ArcadeManager.applicationPath + gamePath;


            // if (FileManager.DirectoryExists(filePath) != null)
            //  {
            masterGamelist = GetGamelistFromGamePath(filePath, emulatorConfiguration.emulator, slider);
            if (masterGamelist != null)
            {
                emulatorConfiguration.lastMasterGamelistUpdate = DateTime.Now.ToString();
                emulatorConfiguration.masterGamelist = masterGamelist;
                emulatorConfiguration.md5MasterGamelist = "";
                Debug.Log("MasterGamelist updated from game path");
                return emulatorConfiguration;
            }
            else
            {
                Debug.Log("MasterGamelist update failed from game path");
            }
            //  }
            Debug.Log("MasterGamelist not found");
            return emulatorConfiguration;
        }

        private static List<ModelProperties> GetGamelistFromMameXML(string filePath, string fileName, EmulatorProperties emulatorProperties, UnityEngine.UI.Slider slider = null)
        {
            if (filePath == null)
            {
                filePath = ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/mame/";
            }
            if (fileName == null)
            {
                fileName = "mame.xml";
            }

            string emulatorExtension = emulatorProperties.extension;
            string emulatorID = emulatorProperties.id;
            string emulatorGamePath = ArcadeManager.applicationPath + emulatorProperties.gamePath;
            Mame2003XML.Mame gamesCollection = Mame2003XML.Mame.Load(Path.Combine(filePath, fileName));
            INIFile ini = new INIFile(Path.Combine(ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/mame/", emulatorID + ".ini"));
            //Debug.Log("mame " + gamesCollection.Game.Count);
            // Debug.Log("invaders is " + gamesCollection.Game.Where(x => x.Name.ToLower() == "invaders").ToList<mame2003XML.Game>()[0].Genre);
            List<ModelProperties> gamelist = new List<ModelProperties>();
            foreach (Mame2003XML.Game game in gamesCollection.Game)
            {
                ModelProperties model = new ModelProperties
                {
                    descriptiveName = game.Description,
                    id = game.Name,
                    idParent = game.Cloneof,
                    emulator = emulatorID.Replace(" ", string.Empty).ToLower(),
                    //model.animationType = "Never";
                    animatedTextureSpeed = 2.0f,
                    screen = (game.Video != null ? game.Video.Screen + " " : "") + (game.Video != null ? game.Video.Orientation : ""),
                    manufacturer = game.Manufacturer,
                    year = game.Year
                };
                model.genre = ini.GetValue("Category", model.id, "").Replace("* Mature *", "");
                model.mature = ini.GetValue("Category", model.id, "").Contains("Mature") ? true : false;
                model.runnable = game.Driver != null ? game.Driver.Status == "good" : false;
                model.available = IsGameAvailable(emulatorGamePath, model.id, emulatorExtension);
                gamelist.Add(model);
            }
            if (gamelist.Count > 0)
            {
                return gamelist;
            }
            return null;
        }

        private static List<ModelProperties> GetGamelistFromHyperspinXML(string filePath, string fileName, EmulatorProperties emulatorProperties, UnityEngine.UI.Slider slider = null)
        {
            string emulatorExtension = emulatorProperties.extension;
            string emulatorID = emulatorProperties.id;
            string emulatorGamePath = ArcadeManager.applicationPath + emulatorProperties.gamePath;

            HyperspinXML.Menu gamesCollection = HyperspinXML.Menu.Load(Path.Combine(filePath, fileName));
            //Debug.Log("invaders is " + gamesCollection.Game.Where(x => x.Name.ToLower() == "invaders").ToList<HyperspinXML.Game>()[0].Genre);
            List<ModelProperties> gamelist = new List<ModelProperties>();
            foreach (HyperspinXML.Game game in gamesCollection.Game)
            {
                ModelProperties model = new ModelProperties
                {
                    descriptiveName = game.Description,
                    id = game.Name,
                    idParent = game.Cloneof,
                    emulator = emulatorID.Replace(" ", string.Empty).ToLower(),
                    //model.animationType = "Never";
                    animatedTextureSpeed = 2.0f,
                    manufacturer = game.Manufacturer,
                    year = game.Year,
                    genre = game.Genre
                };
                model.available = IsGameAvailable(emulatorGamePath, model.id, emulatorExtension);
                // model.mature = game.Rating;
                gamelist.Add(model);
            }
            if (gamelist.Count > 0)
            {
                return gamelist;
            }
            return null;
        }

        private static List<ModelProperties> GetGamelistFrom3DArcadeATF(string filePath, string fileName, EmulatorProperties emulatorProperties, UnityEngine.UI.Slider slider = null)
        {
            string emulatorExtension = emulatorProperties.extension;
            string emulatorID = emulatorProperties.id;
            string emulatorGamePath = ArcadeManager.applicationPath + emulatorProperties.gamePath;

            string text = FileManager.LoadTextFromFile(filePath, fileName);
            if (text == null)
            { return null; }
            string[] lines = text.Split(Environment.NewLine.ToCharArray());
            //   | Description | Name | Year | Manufacturer | Clone | Romof | Category | VersionAdded | Available | Emulator | Type | Model | Favorites | Video | Orientation | Resolution | Aspect | Frequency | Depth | Stereo | Controltype | Buttons | Players | Coins | Driver | DriverStatus | SoundStatus | ColorStatus | HtmlLinks | TimesPlayed | DurationPlayed | Rating |||||
            List<ModelProperties> gamelist = new List<ModelProperties>();
            foreach (string line in lines)
            {
                string[] items = line.Split("|".ToCharArray());
                if (items.Count() < 35)
                { continue; }
                ModelProperties model = new ModelProperties
                {
                    descriptiveName = items[1],
                    id = items[2],
                    idParent = items[5] != "none" && items[5] != "" ? items[5] : "",
                    emulator = emulatorID.Replace(" ", string.Empty).ToLower(),
                    model = items[12] != "none" && items[12] != "" ? items[12] : "",
                    //model.animationType = "Never";
                    animatedTextureSpeed = 2.0f,
                    screen = (items[14] != "none" && items[14] != "" ? items[14] + " " : "") + items[15] != "none" && items[15] != "" ? items[15] : "",
                    manufacturer = items[4] != "none" && items[4] != "" ? items[4] : "",
                    year = items[3] != "none" && items[3] != "" ? items[3] : "",
                    genre = items[7] != "none" && items[7] != "" ? items[7].Replace("*Mature*", "") : "",
                    available = items[9].ToLower() != "no" ? true : false,
                    playCount = int.TryParse(items[30], out int number) ? number : 0,
                    mature = items[7].ToLower().Contains("mature") ? true : false
                };
                model.available = IsGameAvailable(emulatorGamePath, model.id, emulatorExtension);
                gamelist.Add(model);
            }
            if (gamelist.Count > 0)
            {
                return gamelist;
            }
            return null;
        }

        private static List<ModelProperties> GetGamelistFromGamePath(string filePath, EmulatorProperties emulatorProperties, UnityEngine.UI.Slider slider = null)
        {
            string emulatorExtension = emulatorProperties.extension;
            string emulatorID = emulatorProperties.id;

            if (!emulatorExtension.Contains(".") && emulatorExtension != "")
            { emulatorExtension = "." + emulatorExtension; }
            if (emulatorExtension == "")
            { emulatorExtension = "*.*"; }
            List<FileInfo> files = FileManager.FilesFromDirectory(filePath, emulatorExtension);
            if (files != null)
            {
                List<ModelProperties> gamelist = new List<ModelProperties>();
                foreach (FileInfo file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                    ModelProperties model = new ModelProperties
                    {
                        descriptiveName = fileName,
                        id = fileName,
                        idParent = "",
                        emulator = emulatorID.Replace(" ", string.Empty).ToLower(),
                        //model.animationType = "Never";
                        animatedTextureSpeed = 2.0f
                    };
                    gamelist.Add(model);
                }
                if (gamelist.Count > 0)
                {
                    return gamelist;
                }
            }
            return null;
        }

        private static bool IsGameAvailable(string filePath, string fileName, string extension)
        {
            string[] path = Directory.GetFiles(filePath, fileName + ".*");
            if (path.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}


namespace Mame2003XML
{
    [XmlRoot(ElementName = "rom")]
    public class Rom
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "size")]
        public string Size { get; set; }
        [XmlAttribute(AttributeName = "crc")]
        public string Crc { get; set; }
        [XmlAttribute(AttributeName = "sha1")]
        public string Sha1 { get; set; }
        [XmlAttribute(AttributeName = "region")]
        public string Region { get; set; }
        [XmlAttribute(AttributeName = "offset")]
        public string Offset { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "dispose")]
        public string Dispose { get; set; }
        [XmlAttribute(AttributeName = "soundonly")]
        public string Soundonly { get; set; }
        [XmlAttribute(AttributeName = "merge")]
        public string Merge { get; set; }
    }

    [XmlRoot(ElementName = "chip")]
    public class Chip
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "clock")]
        public string Clock { get; set; }
        [XmlAttribute(AttributeName = "soundonly")]
        public string Soundonly { get; set; }
    }

    [XmlRoot(ElementName = "video")]
    public class Video
    {
        [XmlAttribute(AttributeName = "screen")]
        public string Screen { get; set; }
        [XmlAttribute(AttributeName = "orientation")]
        public string Orientation { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "aspectx")]
        public string Aspectx { get; set; }
        [XmlAttribute(AttributeName = "aspecty")]
        public string Aspecty { get; set; }
        [XmlAttribute(AttributeName = "refresh")]
        public string Refresh { get; set; }
    }

    [XmlRoot(ElementName = "sound")]
    public class Sound
    {
        [XmlAttribute(AttributeName = "channels")]
        public string Channels { get; set; }
    }

    [XmlRoot(ElementName = "input")]
    public class Input
    {
        [XmlAttribute(AttributeName = "players")]
        public string Players { get; set; }
        [XmlAttribute(AttributeName = "control")]
        public string Control { get; set; }
        [XmlAttribute(AttributeName = "coins")]
        public string Coins { get; set; }
        [XmlAttribute(AttributeName = "service")]
        public string Service { get; set; }
        [XmlAttribute(AttributeName = "tilt")]
        public string Tilt { get; set; }
    }

    [XmlRoot(ElementName = "dipswitch")]
    public class Dipswitch
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "dipvalue")]
        public List<Dipvalue> Dipvalue { get; set; }
    }

    [XmlRoot(ElementName = "dipvalue")]
    public class Dipvalue
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "default")]
        public string Default { get; set; }
    }

    [XmlRoot(ElementName = "driver")]
    public class Driver
    {
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "color")]
        public string Color { get; set; }
        [XmlAttribute(AttributeName = "sound")]
        public string Sound { get; set; }
        [XmlAttribute(AttributeName = "palettesize")]
        public string Palettesize { get; set; }
    }

    [XmlRoot(ElementName = "game")]
    public class Game
    {
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "year")]
        public string Year { get; set; }
        [XmlElement(ElementName = "manufacturer")]
        public string Manufacturer { get; set; }
        [XmlElement(ElementName = "rom")]
        public List<Rom> Rom { get; set; }
        [XmlElement(ElementName = "chip")]
        public List<Chip> Chip { get; set; }
        [XmlElement(ElementName = "video")]
        public Video Video { get; set; }
        [XmlElement(ElementName = "sound")]
        public Sound Sound { get; set; }
        [XmlElement(ElementName = "input")]
        public Input Input { get; set; }
        [XmlElement(ElementName = "dipswitch")]
        public List<Dipswitch> Dipswitch { get; set; }
        [XmlElement(ElementName = "driver")]
        public Driver Driver { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "runnable")]
        public string Runnable { get; set; }
        [XmlAttribute(AttributeName = "cloneof")]
        public string Cloneof { get; set; }
        [XmlAttribute(AttributeName = "romof")]
        public string Romof { get; set; }
    }

    [XmlRoot(ElementName = "mame")]
    public class Mame
    {
        [XmlElement(ElementName = "game")]
        public List<Game> Game { get; set; }

        public static Mame Load(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Mame));
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as Mame;
            }
        }
    }

}


namespace HyperspinXML
{

    [XmlRoot(ElementName = "header")]
    public class Header
    {
        [XmlElement(ElementName = "listname")]
        public string Listname { get; set; }
        [XmlElement(ElementName = "lastlistupdate")]
        public string Lastlistupdate { get; set; }
        [XmlElement(ElementName = "listversion")]
        public string Listversion { get; set; }
        [XmlElement(ElementName = "exporterversion")]
        public string Exporterversion { get; set; }
    }

    [XmlRoot(ElementName = "game")]
    public class Game
    {
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "cloneof")]
        public string Cloneof { get; set; }
        [XmlElement(ElementName = "crc")]
        public string Crc { get; set; }
        [XmlElement(ElementName = "manufacturer")]
        public string Manufacturer { get; set; }
        [XmlElement(ElementName = "year")]
        public string Year { get; set; }
        [XmlElement(ElementName = "genre")]
        public string Genre { get; set; }
        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "index")]
        public string Index { get; set; }
        [XmlAttribute(AttributeName = "image")]
        public string Image { get; set; }
    }

    [XmlRoot(ElementName = "menu")]
    public class Menu
    {
        [XmlElement(ElementName = "header")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "game")]
        public List<Game> Game { get; set; }

        public static Menu Load(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Menu));
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as Menu;
            }
        }
    }


}

namespace IniParser
{
    // *******************************
    // *** INIFile class V2.1      ***
    // *******************************
    // *** (C)2009-2013 S.T.A. snc ***
    // *******************************

    internal class INIFile
    {

        #region "Declarations"

        // *** Lock for thread-safe access to file and local cache ***
        private readonly object m_Lock = new object();

        internal string FileName { get; private set; } = null;

        // *** Lazy loading flag ***
        private bool m_Lazy = false;

        // *** Automatic flushing flag ***
        private bool m_AutoFlush = false;

        // *** Local cache ***
        private readonly Dictionary<string, Dictionary<string, string>> m_Sections = new Dictionary<string, Dictionary<string, string>>();
        private readonly Dictionary<string, Dictionary<string, string>> m_Modified = new Dictionary<string, Dictionary<string, string>>();

        // *** Local cache modified flag ***
        private bool m_CacheModified = false;

        #endregion

        #region "Methods"

        // *** Constructor ***
        public INIFile(string FileName)
        {
            Initialize(FileName, false, false);
        }

        public INIFile(string FileName, bool Lazy, bool AutoFlush)
        {
            Initialize(FileName, Lazy, AutoFlush);
        }

        // *** Initialization ***
        private void Initialize(string FileName, bool Lazy, bool AutoFlush)
        {
            this.FileName = FileName;
            m_Lazy = Lazy;
            m_AutoFlush = AutoFlush;
            if (!m_Lazy)
            {
                Refresh();
            }
        }

        // *** Parse section name ***
        private string ParseSectionName(string Line)
        {
            if (!Line.StartsWith("["))
            {
                return null;
            }

            if (!Line.EndsWith("]"))
            {
                return null;
            }

            if (Line.Length < 3)
            {
                return null;
            }

            return Line.Substring(1, Line.Length - 2);
        }

        // *** Parse key+value pair ***
        private bool ParseKeyValuePair(string Line, ref string Key, ref string Value)
        {
            // *** Check for key+value pair ***
            int i;
            if ((i = Line.IndexOf('=')) <= 0)
            {
                return false;
            }

            int j = Line.Length - i - 1;
            Key = Line.Substring(0, i).Trim();
            if (Key.Length <= 0)
            {
                return false;
            }

            Value = (j > 0) ? (Line.Substring(i + 1, j).Trim()) : ("");
            return true;
        }

        // *** Read file contents into local cache ***
        internal void Refresh()
        {
            lock (m_Lock)
            {
                StreamReader sr = null;
                try
                {
                    // *** Clear local cache ***
                    m_Sections.Clear();
                    m_Modified.Clear();

                    // *** Open the INI file ***
                    try
                    {
                        sr = new StreamReader(FileName);
                    }
                    catch (FileNotFoundException)
                    {
                        return;
                    }

                    // *** Read up the file content ***
                    Dictionary<string, string> CurrentSection = null;
                    string s;
                    string SectionName;
                    string Key = null;
                    string Value = null;
                    while ((s = sr.ReadLine()) != null)
                    {
                        s = s.Trim();

                        // *** Check for section names ***
                        SectionName = ParseSectionName(s);
                        if (SectionName != null)
                        {
                            // *** Only first occurrence of a section is loaded ***
                            if (m_Sections.ContainsKey(SectionName))
                            {
                                CurrentSection = null;
                            }
                            else
                            {
                                CurrentSection = new Dictionary<string, string>();
                                m_Sections.Add(SectionName, CurrentSection);
                            }
                        }
                        else if (CurrentSection != null)
                        {
                            // *** Check for key+value pair ***
                            if (ParseKeyValuePair(s, ref Key, ref Value))
                            {
                                // *** Only first occurrence of a key is loaded ***
                                if (!CurrentSection.ContainsKey(Key))
                                {
                                    CurrentSection.Add(Key, Value);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    // *** Cleanup: close file ***
                    if (sr != null)
                    {
                        sr.Close();
                    }

                    sr = null;
                }
            }
        }

        // *** Flush local cache content ***
        internal void Flush()
        {
            lock (m_Lock)
            {
                PerformFlush();
            }
        }

        private void PerformFlush()
        {
            // *** If local cache was not modified, exit ***
            if (!m_CacheModified)
            {
                return;
            }

            m_CacheModified = false;

            // *** Check if original file exists ***
            bool OriginalFileExists = File.Exists(FileName);

            // *** Get temporary file name ***
            string TmpFileName = Path.ChangeExtension(FileName, "$n$");

            // *** Copy content of original file to temporary file, replace modified values ***
            // *** Create the temporary file ***
            StreamWriter sw = new StreamWriter(TmpFileName);

            try
            {
                Dictionary<string, string> CurrentSection = null;
                if (OriginalFileExists)
                {
                    StreamReader sr = null;
                    try
                    {
                        // *** Open the original file ***
                        sr = new StreamReader(FileName);

                        // *** Read the file original content, replace changes with local cache values ***
                        string s;
                        string SectionName;
                        string Key = null;
                        string Value = null;
                        bool Unmodified;
                        bool Reading = true;
                        while (Reading)
                        {
                            s = sr.ReadLine();
                            Reading = (s != null);

                            // *** Check for end of file ***
                            if (Reading)
                            {
                                Unmodified = true;
                                s = s.Trim();
                                SectionName = ParseSectionName(s);
                            }
                            else
                            {
                                Unmodified = false;
                                SectionName = null;
                            }

                            // *** Check for section names ***
                            if ((SectionName != null) || (!Reading))
                            {
                                if (CurrentSection != null)
                                {
                                    // *** Write all remaining modified values before leaving a section ****
                                    if (CurrentSection.Count > 0)
                                    {
                                        foreach (string fkey in CurrentSection.Keys)
                                        {
                                            if (CurrentSection.TryGetValue(fkey, out Value))
                                            {
                                                sw.Write(fkey);
                                                sw.Write('=');
                                                sw.WriteLine(Value);
                                            }
                                        }
                                        sw.WriteLine();
                                        CurrentSection.Clear();
                                    }
                                }

                                if (Reading)
                                {
                                    // *** Check if current section is in local modified cache ***
                                    if (!m_Modified.TryGetValue(SectionName, out CurrentSection))
                                    {
                                        CurrentSection = null;
                                    }
                                }
                            }
                            else if (CurrentSection != null)
                            {
                                // *** Check for key+value pair ***
                                if (ParseKeyValuePair(s, ref Key, ref Value))
                                {
                                    if (CurrentSection.TryGetValue(Key, out Value))
                                    {
                                        // *** Write modified value to temporary file ***
                                        Unmodified = false;
                                        _ = CurrentSection.Remove(Key);

                                        sw.Write(Key);
                                        sw.Write('=');
                                        sw.WriteLine(Value);
                                    }
                                }
                            }

                            // *** Write unmodified lines from the original file ***
                            if (Unmodified)
                            {
                                sw.WriteLine(s);
                            }
                        }

                        // *** Close the original file ***
                        sr.Close();
                        sr = null;
                    }
                    finally
                    {
                        // *** Cleanup: close files ***
                        if (sr != null)
                        {
                            sr.Close();
                        }

                        sr = null;
                    }
                }

                // *** Cycle on all remaining modified values ***
                foreach (KeyValuePair<string, Dictionary<string, string>> SectionPair in m_Modified)
                {
                    CurrentSection = SectionPair.Value;
                    if (CurrentSection.Count > 0)
                    {
                        sw.WriteLine();

                        // *** Write the section name ***
                        sw.Write('[');
                        sw.Write(SectionPair.Key);
                        sw.WriteLine(']');

                        // *** Cycle on all key+value pairs in the section ***
                        foreach (KeyValuePair<string, string> ValuePair in CurrentSection)
                        {
                            // *** Write the key+value pair ***
                            sw.Write(ValuePair.Key);
                            sw.Write('=');
                            sw.WriteLine(ValuePair.Value);
                        }
                        CurrentSection.Clear();
                    }
                }
                m_Modified.Clear();

                // *** Close the temporary file ***
                sw.Close();
                sw = null;

                // *** Rename the temporary file ***
                File.Copy(TmpFileName, FileName, true);

                // *** Delete the temporary file ***
                File.Delete(TmpFileName);
            }
            finally
            {
                // *** Cleanup: close files ***
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        // *** Read a value from local cache ***
        internal string GetValue(string SectionName, string Key, string DefaultValue)
        {
            // *** Lazy loading ***
            if (m_Lazy)
            {
                m_Lazy = false;
                Refresh();
            }

            lock (m_Lock)
            {
                // *** Check if the section exists ***
                if (!m_Sections.TryGetValue(SectionName, out Dictionary<string, string> Section))
                {
                    return DefaultValue;
                }

                // *** Check if the key exists ***
                if (!Section.TryGetValue(Key, out string Value))
                {
                    return DefaultValue;
                }

                // *** Return the found value ***
                return Value;
            }
        }

        // *** Insert or modify a value in local cache ***
        internal void SetValue(string SectionName, string Key, string Value)
        {
            // *** Lazy loading ***
            if (m_Lazy)
            {
                m_Lazy = false;
                Refresh();
            }

            lock (m_Lock)
            {
                // *** Flag local cache modification ***
                m_CacheModified = true;

                // *** Check if the section exists ***
                if (!m_Sections.TryGetValue(SectionName, out Dictionary<string, string> Section))
                {
                    // *** If it doesn't, add it ***
                    Section = new Dictionary<string, string>();
                    m_Sections.Add(SectionName, Section);
                }

                // *** Modify the value ***
                if (Section.ContainsKey(Key))
                {
                    _ = Section.Remove(Key);
                }

                Section.Add(Key, Value);

                // *** Add the modified value to local modified values cache ***
                if (!m_Modified.TryGetValue(SectionName, out Section))
                {
                    Section = new Dictionary<string, string>();
                    m_Modified.Add(SectionName, Section);
                }

                if (Section.ContainsKey(Key))
                {
                    _ = Section.Remove(Key);
                }

                Section.Add(Key, Value);

                // *** Automatic flushing : immediately write any modification to the file ***
                if (m_AutoFlush)
                {
                    PerformFlush();
                }
            }
        }

        // *** Encode byte array ***
        private string EncodeByteArray(byte[] Value)
        {
            if (Value == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in Value)
            {
                string hex = Convert.ToString(b, 16);
                int l = hex.Length;
                if (l > 2)
                {
                    _ = sb.Append(hex.Substring(l - 2, 2));
                }
                else
                {
                    if (l < 2)
                    {
                        _ = sb.Append("0");
                    }

                    _ = sb.Append(hex);
                }
            }
            return sb.ToString();
        }

        // *** Decode byte array ***
        private byte[] DecodeByteArray(string Value)
        {
            if (Value == null)
            {
                return null;
            }

            int l = Value.Length;
            if (l < 2)
            {
                return new byte[] { };
            }

            l /= 2;
            byte[] Result = new byte[l];
            for (int i = 0; i < l; i++)
            {
                Result[i] = Convert.ToByte(Value.Substring(i * 2, 2), 16);
            }

            return Result;
        }

        // *** Getters for various types ***
        internal bool GetValue(string SectionName, string Key, bool DefaultValue)
        {
            string StringValue = GetValue(SectionName, Key, DefaultValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (int.TryParse(StringValue, out int Value))
            {
                return (Value != 0);
            }

            return DefaultValue;
        }

        internal int GetValue(string SectionName, string Key, int DefaultValue)
        {
            string StringValue = GetValue(SectionName, Key, DefaultValue.ToString(CultureInfo.InvariantCulture));
            if (int.TryParse(StringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out int Value))
            {
                return Value;
            }

            return DefaultValue;
        }

        internal long GetValue(string SectionName, string Key, long DefaultValue)
        {
            string StringValue = GetValue(SectionName, Key, DefaultValue.ToString(CultureInfo.InvariantCulture));
            if (long.TryParse(StringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out long Value))
            {
                return Value;
            }

            return DefaultValue;
        }

        internal double GetValue(string SectionName, string Key, double DefaultValue)
        {
            string StringValue = GetValue(SectionName, Key, DefaultValue.ToString(CultureInfo.InvariantCulture));
            if (double.TryParse(StringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double Value))
            {
                return Value;
            }

            return DefaultValue;
        }

        internal byte[] GetValue(string SectionName, string Key, byte[] DefaultValue)
        {
            string StringValue = GetValue(SectionName, Key, EncodeByteArray(DefaultValue));
            try
            {
                return DecodeByteArray(StringValue);
            }
            catch (FormatException)
            {
                return DefaultValue;
            }
        }

        internal DateTime GetValue(string SectionName, string Key, DateTime DefaultValue)
        {
            string StringValue = GetValue(SectionName, Key, DefaultValue.ToString(CultureInfo.InvariantCulture));
            if (DateTime.TryParse(StringValue, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeLocal, out DateTime Value))
            {
                return Value;
            }

            return DefaultValue;
        }

        // *** Setters for various types ***
        internal void SetValue(string SectionName, string Key, bool Value)
        {
            SetValue(SectionName, Key, (Value) ? ("1") : ("0"));
        }

        internal void SetValue(string SectionName, string Key, int Value)
        {
            SetValue(SectionName, Key, Value.ToString(CultureInfo.InvariantCulture));
        }

        internal void SetValue(string SectionName, string Key, long Value)
        {
            SetValue(SectionName, Key, Value.ToString(CultureInfo.InvariantCulture));
        }

        internal void SetValue(string SectionName, string Key, double Value)
        {
            SetValue(SectionName, Key, Value.ToString(CultureInfo.InvariantCulture));
        }

        internal void SetValue(string SectionName, string Key, byte[] Value)
        {
            SetValue(SectionName, Key, EncodeByteArray(Value));
        }

        internal void SetValue(string SectionName, string Key, DateTime Value)
        {
            SetValue(SectionName, Key, Value.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

    }

}
