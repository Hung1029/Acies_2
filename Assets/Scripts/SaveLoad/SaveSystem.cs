using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveScene()
    {
        Debug.Log("in");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = "D:/GraduationProject/Acies/Assets/Scripts/SaveLoad/Save.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SceneData SceneName = new SceneData("StudyRoom");
        
        formatter.Serialize(stream, SceneName);

        stream.Close();
    }

    public static SceneData LoadScene()
    {
        string path = "D:/GraduationProject/Acies/Assets/Scripts/SaveLoad/Save.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SceneData SceneName = formatter.Deserialize(stream) as SceneData;
            stream.Close();

            return SceneName;
        }
        else
        {
            Debug.LogError("Not found");
            return null;
        }

    }
}
