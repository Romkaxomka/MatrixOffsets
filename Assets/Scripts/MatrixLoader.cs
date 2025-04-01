using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class MatrixLoader
{
    public static Matrix4x4[] LoadFromFile(string fileName)
    {
        return JsonConvert.DeserializeObject<Matrix4x4[]>(Resources.Load<TextAsset>(fileName).text);
    }

    public static void SaveToFile(string fileName, Matrix4x4[] matrices)
    {
        var savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + fileName;
        File.WriteAllText(savePath, JsonConvert.SerializeObject(matrices));
        Debug.Log("Save location: " + savePath);
    }
}