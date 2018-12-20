using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoadManager
{
    
    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.dataPath + "/GameData.dat", FileMode.Create);

        PlayerData data = new PlayerData();

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadGame()
    {


        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.dataPath + "/GameData.dat"))
        {
            FileStream stream = new FileStream(Application.dataPath + "/GameData.dat", FileMode.Open);

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
        if (File.Exists(Application.dataPath + "/GameData.dat"))
        {
            File.Delete(Application.dataPath + "/GameData.dat");
        }
    }
}

[Serializable]
public class PlayerData
{

    public int CurrentLevel;
    public int WinInaRow;

    public PlayerData()
        {
            CurrentLevel = GameManager.CurrentLevel;
            WinInaRow = GameManager.WinsInaRow;
            
        }

}

        