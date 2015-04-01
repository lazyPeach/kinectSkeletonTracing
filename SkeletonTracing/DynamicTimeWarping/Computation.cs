using Helper;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;

namespace DynamicTimeWarping {
  public class Computation {
    public DTWResult Result { get { return result; } set { result = value; } }

    public void ComputeSequentialDTWMatrix(Body[] template, Body[] sample) {
      result = new DTWResult(template.Length, sample.Length);

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        // set the bone name for the data in result
        result.Data[Mapper.BoneIndexMap[boneName]].BoneName = boneName;

        ComputeDTWMatrixForBone(template, sample, boneName);
        //ComputeDTWCost(template.Length, sample.Length, boneName);
      }
    }

    private void ComputeDTWMatrixForBone(Body[] template, Body[] sample, BoneName boneName) {
      for (int i = 0; i < template.Length; i++) {
        for (int j = 0; j < sample.Length; j++) {

          // construct the signals
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[0][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[1][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[2][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[3][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[0][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;
          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[1][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;
          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[2][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[3][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          //// actually compute the matrix
          float templW = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          float samplW = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;

          float templX = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          float samplX = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;

          float templY = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          float samplY = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;

          float templZ = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;
          float samplZ = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          result.Data[Mapper.BoneIndexMap[boneName]].Matrix[0][i][j] = Math.Abs(templW - samplW);
          result.Data[Mapper.BoneIndexMap[boneName]].Matrix[1][i][j] = Math.Abs(templX - samplX);
          result.Data[Mapper.BoneIndexMap[boneName]].Matrix[2][i][j] = Math.Abs(templY - samplY);
          result.Data[Mapper.BoneIndexMap[boneName]].Matrix[3][i][j] = Math.Abs(templZ - samplZ);
        }
      }
    }

    public void ComputeGreedyDTWCost(int templateLength, int sampleLength) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        ComputeGreedyDTWCostForBone(templateLength, sampleLength, boneName);
      }
    }

    public void ComputeGreedyDTWCostForBone(int templateLength, int sampleLength, BoneName boneName) {
      float[,] wMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[0]);
      float[,] xMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[1]);
      float[,] yMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[2]);
      float[,] zMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[3]);

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

      result.Data[Mapper.BoneIndexMap[boneName]].DTWCost[0] = wDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].DTWCost[1] = xDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].DTWCost[2] = yDTWCost;
      result.Data[Mapper.BoneIndexMap[boneName]].DTWCost[3] = zDTWCost;
    }

    public DTWCost ComputeGreedyDTWCost(int templateLength, int sampleLength, BoneName boneName, int quaternionComponent) {
      float[,] matrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[quaternionComponent]);

      //float[,] wMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[0]);
      //float[,] xMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[1]);
      //float[,] yMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[2]);
      //float[,] zMatrix = GetCostMatrix(result.Data[Mapper.BoneIndexMap[boneName]].Matrix[3]);
      DTWCost retCost = new DTWCost();
      List<Tuple<int, int>> shortestPath = new List<Tuple<int, int>>();
      

      //List<Tuple<int, int>> wShortestPath = new List<Tuple<int, int>>();
      //List<Tuple<int, int>> xShortestPath = new List<Tuple<int, int>>();
      //List<Tuple<int, int>> yShortestPath = new List<Tuple<int, int>>();
      //List<Tuple<int, int>> zShortestPath = new List<Tuple<int, int>>();

      retCost.Cost = GetGreedyDTWCost(matrix, templateLength, sampleLength, ref shortestPath);
      retCost.ShortestPath = shortestPath;

      return retCost;

      //GetGreedyDTWCost(wMatrix, templateLength, sampleLength, ref wShortestPath);
      //GetGreedyDTWCost(xMatrix, templateLength, sampleLength, ref xShortestPath);
      //GetGreedyDTWCost(yMatrix, templateLength, sampleLength, ref yShortestPath);
      //GetGreedyDTWCost(zMatrix, templateLength, sampleLength, ref zShortestPath);

      //result.Data[Mapper.BoneIndexMap[boneName]].ShortestPath[0] = wShortestPath;
      //result.Data[Mapper.BoneIndexMap[boneName]].ShortestPath[1] = xShortestPath;
      //result.Data[Mapper.BoneIndexMap[boneName]].ShortestPath[2] = yShortestPath;
      //result.Data[Mapper.BoneIndexMap[boneName]].ShortestPath[3] = zShortestPath;
    }

    //create a copy of the matrix having infinity on the first row and column
    private float[,] GetCostMatrix(float[][] matrix) {
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

    // Each quaternion is considered to be the value of a signal at a specific moment.
    // Get the maximum value for both template and sample and normalize both with the same value.
    // In this way the ratio between values of the signals will be kept, the difference matrix in
    // DTW will have values between -2 and 2. This will be useful later when DTW cost will be 
    // computer. For further info, check comments for GetDTWCost.
    /*
    public void NormalizeArraysOfBones(Body[] template, Body[] sample, out Body[] templateOut, out Body[] sampleOut) {
      templateOut = template;
      sampleOut = sample;

      float maxTemplateW = 0, maxSampleW = 0
          , maxTemplateX = 0, maxSampleX = 0
          , maxTemplateY = 0, maxSampleY = 0
          , maxTemplateZ = 0, maxSampleZ = 0;

      // get normalized arrays with values for each quaternion
      for (int i = 0; i < template.Length; i++) {
        maxTemplateW = (template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.W > maxTemplateW) ?
          template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.W : maxTemplateW;
        maxTemplateX = (template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.X > maxTemplateX) ?
          template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.X : maxTemplateX;
        maxTemplateY = (template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Y > maxTemplateY) ?
          template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Y : maxTemplateY;
        maxTemplateZ = (template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Z > maxTemplateZ) ?
          template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Z : maxTemplateZ;
      }

      for (int i = 0; i < sample.Length; i++) {

        maxSampleW = (sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.W > maxSampleW) ?
          sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.W : maxSampleW;
        maxSampleX = (sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.X > maxSampleX) ?
          sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.X : maxSampleX;
        maxSampleY = (sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Y > maxSampleY) ?
          sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Y : maxSampleY;
        maxSampleZ = (sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Z > maxSampleZ) ?
          sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Z : maxSampleZ;
      }

      // samples should be normalized with the maximum between maxTemplate and maxSample
      // in this way we keep the ratio between their values

      float maxW = Math.Max(maxTemplateW, maxSampleW);
      float maxX = Math.Max(maxTemplateX, maxSampleX);
      float maxY = Math.Max(maxTemplateY, maxSampleY);
      float maxZ = Math.Max(maxTemplateZ, maxSampleZ);

      //normalize samples
      for (int i = 0; i < template.Length; i++) {
        template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.W /= maxW;
        template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.X /= maxX;
        template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Y /= maxY;
        template[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Z /= maxZ;
      }

      for (int i = 0; i < sample.Length; i++) {
        sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.W /= maxW;
        sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.X /= maxX;
        sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Y /= maxY;
        sample[i].Bones.GetBone(BoneName.BodyCenter).Rotation.Quaternion.Z /= maxZ;
      }
    }



    private float ComputeParallelDTW(Body[] template, Body[] sample) {
      return 0;
    }
    */
    private DTWResult result;

  }
}
