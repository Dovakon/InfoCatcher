using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoadManager
{
    
    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/GameData.txt", FileMode.Create);

        PlayerData data = new PlayerData();

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadGame()
    {


        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/GameData.txt"))
        {
            FileStream stream = new FileStream(Application.persistentDataPath + "/GameData.txt", FileMode.Open);

            PlayerData data = bf.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }

        else
        {
            return null;
        }

    }

    public static void DeleteSavedGame()
    {
        if (File.Exists(Application.persistentDataPath + "/GameData.txt"))
        {
            File.Delete(Application.persistentDataPath + "/GameData.txt");
            Debug.Log("Delete");
        }
    }
}

[Serializable]
public class PlayerData
{

    public int CurrentLevel;
    public int WinInaRow;
    public bool SoundsOn;

    public PlayerData()
        {
            CurrentLevel = GameManager.CurrentLevel;
            WinInaRow = GameManager.WinsInaRow;
            SoundsOn = true;
            
        }

}

        