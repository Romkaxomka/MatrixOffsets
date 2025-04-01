
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneCore : MonoBehaviour
{
    public ComputeShader computeShader;
    public GameObject cubePrefab;

    MatrixGPU matrixGPU;

    Matrix4x4[] model;
    Matrix4x4[] space;

    Dictionary<Matrix4x4, CubeColor> modelCubes = new();
    Dictionary<Matrix4x4, CubeColor> spaceCubes = new();

    public void LoadMatrixs()
    {
        model = MatrixLoader.LoadFromFile("model");
        space = MatrixLoader.LoadFromFile("space");
    }

    private void Awake()
    {
        if (!SystemInfo.supportsComputeShaders)
        {
            Debug.LogError("Compute Shaders не поддерживаются!");
        }
    }

    //dolgaya pupa
    //void CheckOffsets()
    //{
    //    foreach (Matrix4x4 modelMatrix in model)
    //    {
    //        foreach (Matrix4x4 spaceMatrix in space)
    //        {
    //            Matrix4x4 offset = spaceMatrix * modelMatrix.inverse;

    //            foreach (Matrix4x4 m in model)
    //            {
    //                Matrix4x4 transformed = offset * m;
    //                if (space.Contains(transformed))
    //                {
    //                    Debug.Log($"Fisetis: \n{offset}");
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //}

    List<Matrix4x4> offsets = new List<Matrix4x4>();

    public void FindOffests()
    {
        LoadMatrixs();

        for (int i = 0; i < model.Length; i++)
        {
            modelCubes[model[i]] = CreateCube(model[i]);
            //modelCubes[model[i]].SetColor(Color.black);
        }

        for (int i = 0; i < space.Length; i++)
        {
            spaceCubes[space[i]] = CreateCube(space[i]);
        }

        //CheckOffsets();

        matrixGPU = new MatrixGPU(computeShader, model.Length, space.Length);
        offsets = matrixGPU.Compute(model, space);

        foreach (var item in offsets)
        {
            CreateCube(item).SetColor(Color.blue);
        }

        Debug.Log($"Offsets count: {offsets.Count}");

        MatrixLoader.SaveToFile("Offsets.json", offsets.ToArray());
    }

    public CubeColor CreateCube(Matrix4x4 matrix4X4)
    {
        GameObject cube = Instantiate(
            cubePrefab,
            matrix4X4.GetPosition(),
            matrix4X4.rotation
        );

        cube.transform.localScale = matrix4X4.lossyScale;

        return cube.GetComponent<CubeColor>();
    }
}