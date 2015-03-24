using SkeletonTracing.Helper;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.DTW {
  struct DTWResult {
    public int result;
    public int type; // 0 - normal, 1 - slow, 2 - fast;
  }
  
  public class Computation {
    private float[,] WMatrix;
    private float[,] XMatrix;
    private float[,] YMatrix;
    private float[,] ZMatrix;

    public float ComputeDTW(Body[] template, Body[] sample) {
      DTWResult result;
      double report = (double)template.Length / (double)sample.Length;

      if (report < 0.5) result.type = 1;
      else if (report > 2) result.type = 2;
      else result.type = 0;

      WMatrix = new float[template.Length + 1, sample.Length + 1];
      XMatrix = new float[template.Length + 1, sample.Length + 1];
      YMatrix = new float[template.Length + 1, sample.Length + 1];
      ZMatrix = new float[template.Length + 1, sample.Length + 1];

      for (int i = 0; i < template.Length + 1; i++) {
        WMatrix[i, 0] = 1F/0F;
        XMatrix[i, 0] = 1F / 0F;
        YMatrix[i, 0] = 1F / 0F;
        ZMatrix[i, 0] = 1F / 0F;
      }

      for (int j = 0; j < sample.Length + 1; j++) {
        WMatrix[0, j] = 1F / 0F;
        XMatrix[0, j] = 1F / 0F;
        YMatrix[0, j] = 1F / 0F;
        ZMatrix[0, j] = 1F / 0F;
      }

      ComputeWDTW(template, sample);
      float cost = GetDTWCost(WMatrix, template.Length, sample.Length);

      return cost;
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

    private void ComputeWDTW(Body[] template, Body[] sample) {
      for (int i = 0; i < template.Length; i++) {
        for (int j = 0; j < sample.Length; j++) {
          // trebuie sa fac ceva pt fiecare bone si pot face un dtw cumulat la final
          // momentan calc doar elbowLeft
          float templW = template[i].Bones.GetBone(BoneName.FemurusLeft/*ForearmLeft*/).Rotation.Quaternion.W;
          float samplW = sample[j].Bones.GetBone(BoneName.FemurusLeft/*ForearmLeft*/).Rotation.Quaternion.W;
          WMatrix[i + 1, j + 1] = Math.Abs(templW - samplW);
        }
      }
    }

    private void ComputeXDTW(Body[] template, Body[] sample) {

    }

    private void ComputeYDTW(Body[] template, Body[] sample) {

    }

    private void ComputeZDTW(Body[] template, Body[] sample) {

    }
  }
}
