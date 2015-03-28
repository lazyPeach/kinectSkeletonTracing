using SkeletonTracing.Helper;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.DTW {
  public class Computation {
    private DTWResult result = new DTWResult();

    public DTWResult Result { get { return result; } set { result = value; } }

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

    public float ComputeDTW(Body[] template, Body[] sample, bool parallel = false) {
      return parallel ? 
        ComputeParallelDTW(template, sample) :
        ComputeSequentialDTW(template, sample);
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

    private float ComputeSequentialDTW(Body[] template, Body[] sample) {
      double ratio = (double)template.Length / (double)sample.Length;

      //if (ratio < 0.5) result.type = 1;
      //else if (ratio > 2) result.type = 2;
      //else result.type = 0;

      // pt fiecare bone calculeaza un DTW care va fi afisat la final
      float[,] WMatrix = new float[template.Length + 1, sample.Length + 1];
      float[,] XMatrix = new float[template.Length + 1, sample.Length + 1];
      float[,] YMatrix = new float[template.Length + 1, sample.Length + 1];
      float[,] ZMatrix = new float[template.Length + 1, sample.Length + 1];

      InitMatrix(ref WMatrix, ref XMatrix, ref YMatrix, ref ZMatrix, template.Length, sample.Length);

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        ComputeDTWMatrix(template, sample, boneName, ref WMatrix, ref XMatrix, ref YMatrix, ref ZMatrix);
        result.Cost[indexMap[boneName]] = new float[4];

        result.Cost[indexMap[boneName]][0] = GetDTWCost(WMatrix, template.Length, sample.Length);
        result.Cost[indexMap[boneName]][1] = GetDTWCost(XMatrix, template.Length, sample.Length);
        result.Cost[indexMap[boneName]][2] = GetDTWCost(YMatrix, template.Length, sample.Length);
        result.Cost[indexMap[boneName]][3] = GetDTWCost(ZMatrix, template.Length, sample.Length);
      }

      return 0;
    }

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

    private void ComputeDTWMatrix(Body[] template, Body[] sample, BoneName boneName, ref float[,] Wmatrix, ref float[,] Xmatrix, ref float[,] Ymatrix, ref float[,] Zmatrix) {
      for (int i = 0; i < template.Length; i++) {
        for (int j = 0; j < sample.Length; j++) {
          float templW = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
          float samplW = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.W;
          Wmatrix[i + 1, j + 1] = Math.Abs(templW - samplW);

          float templX = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
          float samplX = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.X;
          Xmatrix[i + 1, j + 1] = Math.Abs(templX - samplX);

          float templY = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          float samplY = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Y;
          Ymatrix[i + 1, j + 1] = Math.Abs(templY - samplY);

          float templZ = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;
          float samplZ = sample[j].Bones.GetBone(boneName).Rotation.Quaternion.Z;
          Zmatrix[i + 1, j + 1] = Math.Abs(templZ - samplZ);
        }
      }
    }

    private float GetDTWCost(float[,] mat, int i, int j) {
      if (i == 0 && j == 0)
        return 0;

      int minI, minJ;
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

      float cost = mat[i, j] + GetDTWCost(mat, minI, minJ);

      return cost;
    }



    private void ComputeXDTW(Body[] template, Body[] sample) {

    }

    private void ComputeYDTW(Body[] template, Body[] sample) {

    }

    private void ComputeZDTW(Body[] template, Body[] sample) {

    }
  }
}
