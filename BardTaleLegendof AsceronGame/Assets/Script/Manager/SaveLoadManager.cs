using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour {

    private static SaveLoadManager _instance;
    private const string FILE_EXTENSION = ".sav";
    private readonly string[] saveType = { "Autosave_", "ManualSave_" };
    private string savePath;
    //private int saveNumber = 0;

    private readonly int resWidth = 426;
    private readonly int resHeight = 240;
    public static SaveLoadManager Instance
    {
        get { return _instance; }
    }   

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
            savePath = Application.persistentDataPath + "/saves/";
        }
    }

    public void Save(bool isAutosave)
    {
        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        string saveTime = System.DateTime.Now.ToString("yyyyMMddHHmmss_");

        int sec = PlayerManager.Instance.PlayTime % 60;
        int min = (PlayerManager.Instance.PlayTime / 60) % 60;
        int hour = (PlayerManager.Instance.PlayTime / 60) / 60;
        string playTime = hour.ToString("D4") + min.ToString("D2") + sec.ToString("D2") +"_";

        //int changedSaveNumber = saveNumber + 1000;
        //string hexValue = changedSaveNumber.ToString("X");
        //int hexNumber = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
        //string hexNumToString = hexNumber.ToString("X") + "_";
        //saveNumber++;

        string fileName = ((isAutosave) ? saveType[0] : saveType[1]) + playTime + saveTime; //+ hexNumToString;
        string path = savePath + fileName;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path + FILE_EXTENSION, FileMode.Create);
        PlayerData data = new PlayerData();
        bf.Serialize(stream, data);
        stream.Close();
        SaveScreenshot(path);
    }

    public void Continue()
    {
        FileInfo[] files = LoadAllSavefile();
        if (files != null)
        {
            Load(files[files.Length - 1].FullName);
        }
    }

    public void Load(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
            stream.Close();

            PlayerManager.Instance.Difficulty = data.difficulty;
            PlayerManager.Instance.Currency = data.currency;
            PlayerManager.Instance.PlayTime = data.playTime;
        }
    }

    public FileInfo[] LoadAllSavefile()
    {
        /*string[] savePaths = Directory.GetFiles(savePath, "*" + FILE_EXTENSION);
        for(int i=0;i<savePaths.Length;i++)
        {
            Debug.Log(savePaths[i]);
        }*/

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            return null;
        }
        else
        {
            DirectoryInfo di = new DirectoryInfo(savePath);
            FileInfo[] files = di.GetFiles("*" + FILE_EXTENSION).OrderBy(p => p.CreationTime).ToArray();
            //for(int i=0;i<files.Length;i++)
            //{
            //    Debug.Log(files[i].FullName);
            //}
            if (files.Length > 0)
            {
                return files;
            }
            else
            {
                return null;
            }
        }
    }

    void SaveScreenshot(string path)
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = path + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);
    }
}

[Serializable]
public class PlayerData
{
    public int playTime;
    public int difficulty;
    public List<Character> characters;
    public int currency;

    public PlayerData()
    {
        playTime = PlayerManager.Instance.PlayTime;
        difficulty = PlayerManager.Instance.Difficulty;
        currency = PlayerManager.Instance.Currency;
    }
}

[Serializable]
public class Character
{
    public int level;
    public int exp;
    public int maxHP;
    public int hp;
    public int maxMP;
    public int mp;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int vitality;
    public int endurance;
    public int wisdom;
    public int fireRes;
    public int lightningRes;
    public int iceRes;
    public int cosmosRes;
    public int chaosRes;
    public List<int> skillIDs;
    public List<Equipment> equipments;
}

[Serializable]
public class Equipment
{
    public Equipment()
    {

    }
}