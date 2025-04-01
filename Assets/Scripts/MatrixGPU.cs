using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MatrixGPU
{
    private ComputeShader computeShader;
    private ComputeBuffer modelBuffer, spaceBuffer, offsetBuffer;

    int modelsCount;
    int spacesCount;

    int matrixSize = sizeof(float) * 16;

    public MatrixGPU(ComputeShader computeShader, int modelsCount, int spacesCount)
    {
        this.computeShader = computeShader;

        this.modelsCount = modelsCount;
        this.spacesCount = spacesCount;
    }

    public List<Matrix4x4> Compute(Matrix4x4[] models, Matrix4x4[] spaces)
    {
        modelBuffer = new ComputeBuffer(modelsCount, matrixSize);
        spaceBuffer = new ComputeBuffer(spacesCount, matrixSize);
        offsetBuffer = new ComputeBuffer(modelsCount * spacesCount, matrixSize, ComputeBufferType.Append);

        modelBuffer.SetData(models);
        spaceBuffer.SetData(spaces);
        offsetBuffer.SetCounterValue(0);

        int kernelHandle = computeShader.FindKernel("SolveMatrix4");

        computeShader.SetBuffer(kernelHandle, "modelMatrices", modelBuffer);
        computeShader.SetBuffer(kernelHandle, "spaceMatrices", spaceBuffer);
        computeShader.SetBuffer(kernelHandle, "offsetResults", offsetBuffer);
        computeShader.SetInt("modelsCount", modelsCount);
        computeShader.SetInt("spacesCount", spacesCount);

        computeShader.GetKernelThreadGroupSizes(kernelHandle, out var _threadX, out var _threadY, out var _threadZ);

        computeShader.Dispatch(kernelHandle, Mathf.CeilToInt(modelsCount / _threadX) + 1, Mathf.CeilToInt((float)spacesCount / _threadY) + 1, 1);

        Matrix4x4[] offsetResults = new Matrix4x4[offsetBuffer.count];
        offsetBuffer.GetData(offsetResults);

        List<Matrix4x4> offsets = new List<Matrix4x4>();
        foreach (var matrix in offsetResults)
        {
            if (matrix != Matrix4x4.zero) 
                offsets.Add(matrix);
        }

        Debug.Log(offsetResults[0]);

        modelBuffer.Release();
        spaceBuffer.Release();
        offsetBuffer.Release();

        return offsets;
    }
}