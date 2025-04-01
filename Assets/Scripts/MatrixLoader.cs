using Newtonsoft.Json;
using System;
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
        var savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName;
        Matrix4x4_[] matrices_ = Array.ConvertAll(matrices, matrix => (Matrix4x4_)matrix);
        File.WriteAllText(savePath, JsonConvert.SerializeObject(matrices_, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }));
        Debug.Log("Save location: " + savePath);
    }
}

[Serializable]
struct Matrix4x4_
{
    public float m00, m10, m20, m30;
    public float m01, m11, m21, m31;
    public float m02, m12, m22, m32;
    public float m03, m13, m23, m33;

    public static explicit operator Matrix4x4_(Matrix4x4 b) => new Matrix4x4_()
    {
        m00 = b.m00,
        m10 = b.m10,
        m20 = b.m20,
        m30 = b.m30,
        m01 = b.m01,
        m11 = b.m11,
        m21 = b.m21,
        m31 = b.m31,
        m02 = b.m02,
        m12 = b.m12,
        m22 = b.m22,
        m32 = b.m32,
        m03 = b.m03,
        m13 = b.m13,
        m23 = b.m23,
        m33 = b.m33
    };
}
