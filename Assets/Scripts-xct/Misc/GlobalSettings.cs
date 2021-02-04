using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class GlobalSettings : MonoBehaviour {

    private static bool inited = false;
    public static GlobalSettings ins = null;
    public static Settings settings = null;
    public string SETTINGS_FILE_PATH;

    public class Settings
    {
        //-------Settings--------
        public bool debug = false;
        public bool noHit = false;

#if UNITY_ANDROID
        public bool useDPad = true;
#else
    public bool useDPad = false;
#endif
        public bool useVibrator = true;
        public bool useCButton = false;

        //-------InBattle--------
        public int hits = 0;
        public int totalHurtHP = 0;
        public float inBattleTime = 0;
    }

	void Awake () {
        if (inited)
        {
            Destroy(gameObject);
            return;
        }

        SETTINGS_FILE_PATH = FileLoader.DefaultDataPath + Path.DirectorySeparatorChar + "UserData" + Path.DirectorySeparatorChar + "settings.xml";
        ins = this;
        inited = true;

        if (!File.Exists(SETTINGS_FILE_PATH))
        {
            Directory.CreateDirectory(FileLoader.DefaultDataPath + Path.DirectorySeparatorChar + "UserData");
            settings = new Settings();
            Save();
        }
        else
            ReadXML(SETTINGS_FILE_PATH);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RestInBattleStats()
    {
        settings.hits = 0;
        settings.totalHurtHP = 0;
        settings.inBattleTime = 0;
    }

    public void Save()
    {
        SaveXML(SETTINGS_FILE_PATH);
    }

    private void SaveXML(string path)
    {
        XmlSerializer writer = new XmlSerializer(typeof(GlobalSettings.Settings));
        StreamWriter ioWriter = new StreamWriter(path);
        writer.Serialize(ioWriter, settings);
        ioWriter.Close();
    }

    public void ReadXML(string path)
    {
        XmlSerializer reader = new XmlSerializer(typeof(GlobalSettings.Settings));
        StreamReader ioReader = new StreamReader(path);
        settings = (Settings)reader.Deserialize(ioReader);
        ioReader.Close();
    }
}
