using Helper;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;

namespace DynamicTimeWarping {
  public class Computation {
    public DTWResult Result { get { return result; } set { result = value; } }

    // some of the further actions can be done in the same loop but for the sake of this prject they
    // will be done in separate loops just to measure how well does DTW for recognizing body gestures.
    public void ComputeDTW(Body[] template, Body[] sample) {
      result = new DTWResult(template.Length, sample.Length);

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        result.Data[Mapper.BoneIndexMap[boneName]].BoneName = boneName;
        ConstructSignals(template, sample, boneName);
      }

      ComputeDTWMatrix(template, sample);
      ComputeGreedyDTWCost(template.Length, sample.Length);
      ComputeWindowDTWMatrix(template, sample);
      ComputeGreedyDTWWindowCost(template.Length, sample.Length);
    }


    private void ConstructSignals(Body[] template, Body[] sample, BoneName boneName) {
      for (int i = 0; i < template.Length; i++) {
        for (int j = 0; j < sample.Length; j++) {
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[0][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[1][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[2][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[3][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[0][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;
          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[1][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;
          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[2][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[3][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;
        }
      }
    }

    private void ComputeDTWMatrix(Body[] template, Body[] sample) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        ComputeDTWMatrixForBone(template, sample, boneName);
      }
    }

    private void ComputeDTWMatrixForBone(Body[] template, Body[] sample, BoneName boneName) {
      for (int i = 0; i < template.Length; i++) {
        for (int j = 0; j < sample.Length; j++) {
          float templW = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          float samplW = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;

          float templX = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          float samplX = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;

          float templY = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          float samplY = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;

          float templZ = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;
          float samplZ = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[0][i][j] = Math.Abs(templW - samplW);
          result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[1][i][j] = Math.Abs(templX - samplX);
          result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[2][i][j] = Math.Abs(templY - samplY);
          result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[3][i][j] = Math.Abs(templZ - samplZ);
        }
      }
    }

    public void ComputeGreedyDTWCost(int templateLength, int sampleLength) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        ComputeGreedyDTWCostForBone(templateLength, sampleLength, boneName);
      }
    }

    public void ComputeGreedyDTWCostForBone(int templateLength, int sampleLength, BoneName boneName) {
      float[,] wMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[0]/*, templateLength, sampleLength*/);
      float[,] xMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[1]);
      float[,] yMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[2]);
      float[,] zMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[3]);

      List<Tuple<int, int>> wShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> xShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> yShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> zShortestPath = new List<Tuple<int, int>>();

      float wCost = GetGreedyDTWCost(wMatrix, templateLength, sampleLength, ref wShortestPath);
      float xCost = GetGreedyDTWCost(xMatrix, templateLength, sampleLength, ref xShortestPath);
      float yCost = GetGreedyDTWCost(yMatrix, templateLength, sampleLength, ref yShortestPath);
      float zCost = GetGreedyDTWCost(zMatrix, templateLength, sampleLength, ref zShortestPath);

      DTWCost wDTWCost = new DTWCost(wShortestPath, wCost);
      DTWCost xDTWCost = new DTWCost(xShortestPath, xCost);
      DTWCost yDTWCost = new DTWCost(yShortestPath, yCost);
      DTWCost zDTWCost = new DTWCost(zShortestPath, zCost);

      result.Data[Mapper.BoneIndexMap[boneName]].GreedyCost[0] = wDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].GreedyCost[1] = xDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].GreedyCost[2] = yDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].GreedyCost[3] = zDTWCost;
    }

    //create a copy of the matrix having infinity on the first row and column
    private float[,] GetCostMatrix(float[][] matrix/*, int height, int length*/) {
      int height = matrix.Length;
      int width = matrix[0].Length;

      float[,] ret = new float[height + 1, width + 1];

      for (int i = 0; i <= height; i++) {
        ret[i, 0] = 1f / 0f;
      }

      for (int j = 0; j <= width; j++) {
        ret[0, j] = 1f / 0f;
      }

      for (int i = 1; i <= height; i++) {
        for (int j = 1; j <= width; j++) {
          ret[i, j] = matrix[i - 1][j - 1];
        }
      }

      return ret;
    }

    private float GetGreedyDTWCost(float[,] mat, int i, int j, ref List<Tuple<int, int>> shortestPath) {
      if (i == 0 && j == 0)
        return 0;

      int minI, minJ;

      if (mat[i - 1, j - 1] <= mat[i, j - 1] && mat[i - 1, j - 1] <= mat[i - 1, j]) {
        minI = i - 1;
        minJ = j - 1;
      } else if (mat[i, j - 1] <= mat[i - 1, j - 1] && mat[i, j - 1] <= mat[i - 1, j]) {
        minI = i;
        minJ = j - 1;
      } else {
        minI = i - 1;
        minJ = j;
      }

      Tuple<int, int> elem = new Tuple<int, int>(i - 1, j - 1); // add current element from the real matrix in data
      shortestPath.Add(elem);

      float cost = mat[i, j] + GetGreedyDTWCost(mat, minI, minJ, ref shortestPath);

      return cost;
    }

    public void ComputeWindowDTWMatrix(Body[] template, Body[] sample) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        ComputeWindowDTWMatrixForBone(template, sample, boneName);
      }
    }

    private void ComputeWindowDTWMatrixForBone(Body[] template, Body[] sample, BoneName boneName) {
      float slope = (float)template.Length / (float)sample.Length;
      // r is the window size; if abs(i - j) > r put infinity on that cell
      int r = 10;

      for (int i = 0; i < template.Length; i++) {
        int lineJ = (int)((float)i / slope); // for each i, get the j on the line and compute the dtw only for j +- r

        for (int j = 0; j < sample.Length; j++) {
          float templW = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          float samplW = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;

          float templX = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          float samplX = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;

          float templY = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          float samplY = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;

          float templZ = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;
          float samplZ = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          if (Math.Abs(lineJ - j) <= r) {
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[0][i][j] = Math.Abs(templW - samplW);
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[1][i][j] = Math.Abs(templX - samplX);
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[2][i][j] = Math.Abs(templY - samplY);
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[3][i][j] = Math.Abs(templZ - samplZ);
          } else {
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[0][i][j] = 1f / 0f;
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[1][i][j] = 1f / 0f;
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[2][i][j] = 1f / 0f;
            result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[3][i][j] = 1f / 0f;
          }
        }
      }
    }

    public void ComputeGreedyDTWWindowCost(int templateLength, int sampleLength) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        ComputeGreedyDTWWindowCostForBone(templateLength, sampleLength, boneName);
      }
    }

    public void ComputeGreedyDTWWindowCostForBone(int templateLength, int sampleLength, BoneName boneName) {
      float[,] wMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[0]/*, templateLength, sampleLength*/);
      float[,] xMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[1]);
      float[,] yMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[2]);
      float[,] zMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[3]);

      List<Tuple<int, int>> wShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> xShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> yShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> zShortestPath = new List<Tuple<int, int>>();

      float wCost = GetGreedyDTWCost(wMatrix, templateLength, sampleLength, ref wShortestPath);
      float xCost = GetGreedyDTWCost(xMatrix, templateLength, sampleLength, ref xShortestPath);
      float yCost = GetGreedyDTWCost(yMatrix, templateLength, sampleLength, ref yShortestPath);
      float zCost = GetGreedyDTWCost(zMatrix, templateLength, sampleLength, ref zShortestPath);

      DTWCost wDTWCost = new DTWCost(wShortestPath, wCost);
      DTWCost xDTWCost = new DTWCost(xShortestPath, xCost);
      DTWCost yDTWCost = new DTWCost(yShortestPath, yCost);
      DTWCost zDTWCost = new DTWCost(zShortestPath, zCost);

      result.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost[0] = wDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost[1] = xDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost[2] = yDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost[3] = zDTWCost;
    }


    private DTWResult result;
  }
}
