using SkeletonTracing.Helper;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.DTW {
  public class Computation {
    private DTWResult result;

    public float ComputeDTW(Body[] template, Body[] sample, bool parallel = false) {
      return parallel ?
        ComputeParallelDTW(template, sample) :
        ComputeSequentialDTW(template, sample);
    }

    public DTWResult Result { get { return result; } set { result = value; } }


    private float ComputeSequentialDTW(Body[] template, Body[] sample) {
      result = new DTWResult(template.Length, sample.Length);
      
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        // set the bone name for the data in result
        result.Data[indexMap[boneName]].BoneName = boneName;

        ComputeDTWMatrix(template, sample, boneName);
        ComputeDTWCost(template.Length, sample.Length, boneName);
      }

      return 0;
    }

    // put infinity on row 0 and column 0 for easier computation of dtw path
    private void InitMatrix(ref float[,] Wmatrix, ref float[,] Xmatrix, ref float[,] Ymatrix, ref float[,] Zmatrix, int iLimit, int jLimit) {
      for (int i = 0; i < iLimit + 1; i++) {
        Wmatrix[i, 0] = 1F / 0F;
        Xmatrix[i, 0] = 1F / 0F;
        Ymatrix[i, 0] = 1F / 0F;
        Zmatrix[i, 0] = 1F / 0F;
      }

      for (int j = 0; j < jLimit + 1; j++) {
        Wmatrix[0, j] = 1F / 0F;
        Xmatrix[0, j] = 1F / 0F;
        Ymatrix[0, j] = 1F / 0F;
        Zmatrix[0, j] = 1F / 0F;
      }
    }

    private void ComputeDTWMatrix(Body[] template, Body[] sample, BoneName boneName) {
      for (int i = 0; i < template.Length; i++) {
        for (int j = 0; j < sample.Length; j++) {
          
          result.Data[indexMap[boneName]].TemplateSignal[0][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          result.Data[indexMap[boneName]].TemplateSignal[1][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          result.Data[indexMap[boneName]].TemplateSignal[2][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          result.Data[indexMap[boneName]].TemplateSignal[3][i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          result.Data[indexMap[boneName]].SampleSignal[0][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;
          result.Data[indexMap[boneName]].SampleSignal[1][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;
          result.Data[indexMap[boneName]].SampleSignal[2][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          result.Data[indexMap[boneName]].SampleSignal[3][j] = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          //// actually compute the matrix
          float templW = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          float samplW = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;

          float templX = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          float samplX = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;

          float templY = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          float samplY = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;

          float templZ = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;
          float samplZ = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;

          result.Data[indexMap[boneName]].Matrix[0][i][j] = Math.Abs(templW - samplW);
          result.Data[indexMap[boneName]].Matrix[1][i][j] = Math.Abs(templX - samplX);
          result.Data[indexMap[boneName]].Matrix[2][i][j] = Math.Abs(templY - samplY);
          result.Data[indexMap[boneName]].Matrix[3][i][j] = Math.Abs(templZ - samplZ);
        }
      }


    }

    private void ComputeDTWCost(int height, int width, BoneName boneName) {
      float[,] wMatrix = GetCostMatrix(result.Data[indexMap[boneName]].Matrix[0]);
      float[,] xMatrix = GetCostMatrix(result.Data[indexMap[boneName]].Matrix[1]);
      float[,] yMatrix = GetCostMatrix(result.Data[indexMap[boneName]].Matrix[2]);
      float[,] zMatrix = GetCostMatrix(result.Data[indexMap[boneName]].Matrix[3]);

      List<Tuple<int, int>> wShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> xShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> yShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> zShortestPath = new List<Tuple<int, int>>();

      GetDTWCost(wMatrix, height, width, ref wShortestPath);
      GetDTWCost(xMatrix, height, width, ref xShortestPath);
      GetDTWCost(yMatrix, height, width, ref yShortestPath);
      GetDTWCost(zMatrix, height, width, ref zShortestPath);

      result.Data[Mapper.indexMap[boneName]].ShortestPath[0] = wShortestPath;
      result.Data[Mapper.indexMap[boneName]].ShortestPath[1] = xShortestPath;
      result.Data[Mapper.indexMap[boneName]].ShortestPath[2] = yShortestPath;
      result.Data[Mapper.indexMap[boneName]].ShortestPath[3] = zShortestPath;
    }

    //create a copy of the matrix
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

    private float GetDTWCost(float[,] mat, int i, int j, ref List<Tuple<int, int>> shortestPath) {
      if (i == 0 && j == 0)
        return 0;

      // compare on diagonal the first time...
      int minI, minJ;
      /*
      if (mat[i - 1, j] < mat[i - 1, j - 1] && mat[i - 1, j] < mat[i, j - 1]) {
        minI = i - 1;
        minJ = j;
      } else if (mat[i, j - 1] < mat[i - 1, j - 1] && mat[i, j - 1] < mat[i - 1, j]) {
        minI = i;
        minJ = j - 1;
      } else {
        minI = i - 1;
        minJ = j - 1;
      }
       */

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

      float cost = mat[i, j] + GetDTWCost(mat, minI, minJ, ref shortestPath);

      return cost;
    }






    // Each quaternion is considered to be the value of a signal at a specific moment.
    // Get the maximum value for both template and sample and normalize both with the same value.
    // In this way the ratio between values of the signals will be kept, the difference matrix in
    // DTW will have values between -2 and 2. This will be useful later when DTW cost will be 
    // computer. For further info, check comments for GetDTWCost.
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

    private Dictionary<BoneName, int> indexMap = new Dictionary<BoneName, int>() {
      {BoneName.BodyCenter    , 0},
      {BoneName.LowerSpine    , 1},
      {BoneName.UpperSpine    , 2},
      {BoneName.Neck          , 3},
      {BoneName.ClavicleLeft  , 4},
      {BoneName.ArmLeft       , 5},
      {BoneName.ForearmLeft   , 6},
      {BoneName.ClavicleRight , 7},
      {BoneName.ArmRight      , 8},
      {BoneName.ForearmRight  , 9},
      {BoneName.HipLeft       , 10},
      {BoneName.FemurusLeft   , 11},
      {BoneName.TibiaLeft     , 12},
      {BoneName.HipRight      , 13},
      {BoneName.FemurusRight  , 14},
      {BoneName.TibiaRight    , 15},
    };
  }
}
