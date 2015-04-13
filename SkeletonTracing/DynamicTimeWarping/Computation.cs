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
      ComputeWindowDTWMatrix(template, sample);

      ComputeGreedyDTWCost(template.Length, sample.Length);
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
        result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix = ComputeDTWMatrixForBone(template, sample, boneName);
      }
    }

    private float[][][] ComputeDTWMatrixForBone(Body[] template, Body[] sample, BoneName boneName) {
      float[][][] res = new float[4][][];

      res[0] = new float[template.Length][];
      res[1] = new float[template.Length][];
      res[2] = new float[template.Length][];
      res[3] = new float[template.Length][];

      for (int i = 0; i < template.Length; i++) {
        res[0][i] = new float[sample.Length];
        res[1][i] = new float[sample.Length];
        res[2][i] = new float[sample.Length];
        res[3][i] = new float[sample.Length];
        
        for (int j = 0; j < sample.Length; j++) {
          res[0][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.W -
                                  sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W);
          res[1][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.X -
                                  sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X);
          res[2][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y -
                                  sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y);
          res[3][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z -
                                  sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z);
        }
      }

      return res;
    }

    public void ComputeWindowDTWMatrix(Body[] template, Body[] sample) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix = ComputeWindowDTWMatrixForBone(template, sample, boneName);
      }
    }

    // look on wikipedia for a faster way to compute only the slice needed http://en.wikipedia.org/wiki/Dynamic_time_warping
    private float[][][] ComputeWindowDTWMatrixForBone(Body[] template, Body[] sample, BoneName boneName) {
      float[][][] res = new float[4][][];

      res[0] = new float[template.Length][];
      res[1] = new float[template.Length][];
      res[2] = new float[template.Length][];
      res[3] = new float[template.Length][];

      float slope = (float)template.Length / (float)sample.Length;
      // r is the window size; if abs(i - j) > r put infinity on that cell
      int r = 10;

      for (int i = 0; i < template.Length; i++) {
        res[0][i] = new float[sample.Length];
        res[1][i] = new float[sample.Length];
        res[2][i] = new float[sample.Length];
        res[3][i] = new float[sample.Length];

        int lineJ = (int)((float)i / slope); // for each i, get the j on the line and compute the dtw only for j +- r

        for (int j = 0; j < sample.Length; j++) {
          if (Math.Abs(lineJ - j) <= r) {
            res[0][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.W -
                                  sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W);
            res[1][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.X -
                                    sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X);
            res[2][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y -
                                    sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y);
            res[3][i][j] = Math.Abs(template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z -
                                    sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z);
          } else {
            res[0][i][j] = 1f / 0f;
            res[1][i][j] = 1f / 0f;
            res[2][i][j] = 1f / 0f;
            res[3][i][j] = 1f / 0f;
          }
        }
      }

      return res;
    }

    public void ComputeGreedyDTWCost(int templateLength, int sampleLength) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        result.Data[Mapper.BoneIndexMap[boneName]].GreedyCost = ComputeGreedyDTWCostForBone(result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix, templateLength, sampleLength);
      }
    }

    public void ComputeGreedyDTWWindowCost(int templateLength, int sampleLength) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        result.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost = ComputeGreedyDTWCostForBone(result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix, templateLength, sampleLength);
      }
    }

    public DTWCost[] ComputeGreedyDTWCostForBone(float[][][] dtwMatrix, int templateLength, int sampleLength) {
      float[,] wMatrix = GetCostMatrix(dtwMatrix[0], templateLength, sampleLength);
      float[,] xMatrix = GetCostMatrix(dtwMatrix[1], templateLength, sampleLength);
      float[,] yMatrix = GetCostMatrix(dtwMatrix[2], templateLength, sampleLength);
      float[,] zMatrix = GetCostMatrix(dtwMatrix[3], templateLength, sampleLength);

      List<Tuple<int, int>> wShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> xShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> yShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> zShortestPath = new List<Tuple<int, int>>();

      float wCost = GetGreedyDTWCost(wMatrix, templateLength, sampleLength, ref wShortestPath);
      float xCost = GetGreedyDTWCost(xMatrix, templateLength, sampleLength, ref xShortestPath);
      float yCost = GetGreedyDTWCost(yMatrix, templateLength, sampleLength, ref yShortestPath);
      float zCost = GetGreedyDTWCost(zMatrix, templateLength, sampleLength, ref zShortestPath);

      DTWCost[] cost = new DTWCost[4];

      cost[0] = new DTWCost(wShortestPath, wCost);
      cost[1] = new DTWCost(xShortestPath, xCost);
      cost[2] = new DTWCost(yShortestPath, yCost);
      cost[3] = new DTWCost(zShortestPath, zCost);

      return cost;
    }

    //create a copy of the matrix having infinity on the first row and column
    private float[,] GetCostMatrix(float[][] matrix, int height, int width) {
      //int height = matrix.Length;
      //int width = matrix[0].Length;

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


    private DTWResult result;
  }
}
