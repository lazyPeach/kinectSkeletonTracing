using Helper;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;

// change result into rawResult

namespace DynamicTimeWarping {
  public class Computation {
    public DTWResult RawResult { get { return rawResult; } set { rawResult = value; } }
    public DTWResult FilteredResult { get { return filteredResult; } set { filteredResult = value; } }
    public DTWResult OptimalResult { get { return optimalResult; } set { optimalResult = value; } }

    // some of the further actions can be done in the same loop but for the sake of this prject they
    // will be done in separate loops just to measure how well does DTW for recognizing body gestures.
    public void ComputeDTWForGraphic(Body[] template, Body[] sample) {
      rawResult = new DTWResult(template.Length, sample.Length);
      filteredResult = new DTWResult(template.Length, sample.Length);

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        rawResult.Data[Mapper.BoneIndexMap[boneName]].BoneName = boneName;
        filteredResult.Data[Mapper.BoneIndexMap[boneName]].BoneName = boneName;

        rawResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal = GetRawSignal(template, boneName);
        rawResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal = GetRawSignal(sample, boneName);

        FilterSignalsForBone(boneName);
      }


      ComputeDTWForRawSignals(template, sample);
      ComputeDTWForFilteredSignals(template, sample);
    }

    private float[][] GetRawSignal(Body[] bodySignal, BoneName boneName) {
      float[][] res = new float[4][];
      for (int i = 0; i < 4; i++) {
        res[i] = new float[bodySignal.Length];
      }

      for (int i = 0; i < bodySignal.Length; i++) {
        res[0][i] = bodySignal[i].BoneSkeleton.GetBone(boneName).Rotation.W;
        res[1][i] = bodySignal[i].BoneSkeleton.GetBone(boneName).Rotation.X;
        res[2][i] = bodySignal[i].BoneSkeleton.GetBone(boneName).Rotation.Y;
        res[3][i] = bodySignal[i].BoneSkeleton.GetBone(boneName).Rotation.Z;
      }

      return res;
    }

    // Apply low pass filter to remove spikes and anomalies in recorded data
    private void FilterSignalsForBone(BoneName boneName) {
      for (int i = 0; i < 4; i++) {
        filteredResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[i] =
          FilterSignal(rawResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[i]);

        filteredResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[i] =
          FilterSignal(rawResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[i]);
      }
    }

    private float[] FilterSignal(float[] signal) {
      float[] filteredSignal = new float[signal.Length];
      float cutoff = 0.1f;

      filteredSignal[0] = signal[0];
      for (int i = 1; i < signal.Length; i++) {
        filteredSignal[i] = cutoff * signal[i] + (1 - cutoff) * filteredSignal[i - 1];
      }

      return filteredSignal;
    }



    public void ComputeOptimalDTW(Body[] template, Body[] sample) {
      optimalResult = new DTWResult(template.Length, sample.Length);
      //todo get rid of this
      rawResult = new DTWResult(template.Length, sample.Length);
      filteredResult = new DTWResult(template.Length, sample.Length);

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        optimalResult.Data[Mapper.BoneIndexMap[boneName]].BoneName = boneName;
        rawResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal = GetRawSignal(template, boneName);
        rawResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal = GetRawSignal(sample, boneName);

        FilterSignalsForBone(boneName);
        
        optimalResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix = 
          ComputeWindowDTWMatrix(filteredResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal,
          filteredResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal);
        
        // this could be removed... the best cost is the cell[n][m]... in real case we don't care
        // about optimal path only its final cost
        optimalResult.Data[Mapper.BoneIndexMap[boneName]].BestCost = 
          ComputeBestCostForBone(optimalResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix,
          template.Length, sample.Length);
      }
    }

    public float GetSumBodyCost() {
      float sum = 0;
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        for (int i = 0; i < 4; i++) {
          sum += filteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost;
        }
      }

      return sum;
    }

    public float GetAvgBodyCost() {
      float sum = 0;
      float nr = 0;
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        for (int i = 0; i < 4; i++) {
          nr++;
          sum += filteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost;
        }
      }

      return sum/nr;
    }


    private void ComputeDTWForRawSignals(Body[] template, Body[] sample) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        rawResult.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix = 
          ComputeDTWMatrix(rawResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal,
            rawResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal);
        
        rawResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix =
          ComputeWindowDTWMatrix(rawResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal,
            rawResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal);
        
        rawResult.Data[Mapper.BoneIndexMap[boneName]].GreedyCost =
          ComputeGreedyDTWCostForBone(rawResult.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix,
            template.Length, sample.Length);

        rawResult.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost =
          ComputeGreedyDTWCostForBone(rawResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix,
            template.Length, sample.Length);

        rawResult.Data[Mapper.BoneIndexMap[boneName]].BestCost =
          ComputeBestCostForBone(rawResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix,
          template.Length, sample.Length);
      }
    }

    private void ComputeDTWForFilteredSignals(Body[] template, Body[] sample) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        filteredResult.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix = 
          ComputeDTWMatrix(filteredResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal,
          filteredResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal);
        filteredResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix =
          ComputeWindowDTWMatrix(filteredResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal,
          filteredResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal);
        filteredResult.Data[Mapper.BoneIndexMap[boneName]].GreedyCost =
          ComputeGreedyDTWCostForBone(filteredResult.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix,
          template.Length, sample.Length);
        filteredResult.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost =
          ComputeGreedyDTWCostForBone(filteredResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix,
          template.Length, sample.Length);
        filteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost =
          ComputeBestCostForBone(filteredResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix,
          template.Length, sample.Length);
      }
    }

    private float[][][] ComputeDTWMatrix(float[][] template, float[][] sample) {
      float[][][] res = new float[4][][];

      for (int i = 0; i < 4; i++) {
        res[i] = new float[template[i].Length][];

        for (int j = 0; j < template[i].Length; j++) {
          res[i][j] = new float[sample[i].Length];

          for (int k = 0; k < sample[i].Length; k++) {
            res[i][j][k] = Math.Abs(template[i][j] - sample[i][k]);
          }
        }
      }
      return res;
    }

    // look on wikipedia for a faster way to compute only the slice needed http://en.wikipedia.org/wiki/Dynamic_time_warping
    private float[][][] ComputeWindowDTWMatrix(float[][] template, float[][] sample) {
      float[][][] res = new float[4][][];
      float slope = (float)template[0].Length / (float)sample[0].Length;
      // r is the window size; if abs(i - j) > r put infinity on that cell
      int r = (int)(0.1 * Math.Max(template[0].Length, sample[0].Length));

      for (int i = 0; i < 4; i++) {
        res[i] = new float[template[i].Length][];

        for (int j = 0; j < template[i].Length; j++) {
          res[i][j] = new float[sample[i].Length];
          int lineJ = (int)((float)j / slope); // for each i, get the j on the line and compute the dtw only for j +- r

          for (int k = 0; k < sample[i].Length; k++) {
            if (Math.Abs(lineJ - k) <= r) {
              res[i][j][k] = Math.Abs(template[i][j] - sample[i][k]);
            } else {
              res[i][j][k] = 1f / 0f;
            }
          }
        }
      }

      return res;
    }

    private DTWCost[] ComputeGreedyDTWCostForBone(float[][][] dtwMatrix, int templateLength, int sampleLength) {
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

    private DTWCost[] ComputeBestCostForBone(float[][][] dtwMatrix, int templateLength, int sampleLength) {
      float[,] wMatrix = GetBestCostMatrix(dtwMatrix[0], templateLength, sampleLength);
      float[,] xMatrix = GetBestCostMatrix(dtwMatrix[1], templateLength, sampleLength);
      float[,] yMatrix = GetBestCostMatrix(dtwMatrix[2], templateLength, sampleLength);
      float[,] zMatrix = GetBestCostMatrix(dtwMatrix[3], templateLength, sampleLength);

      List<Tuple<int, int>> wShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> xShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> yShortestPath = new List<Tuple<int, int>>();
      List<Tuple<int, int>> zShortestPath = new List<Tuple<int, int>>();

      GetGreedyDTWCost(wMatrix, templateLength, sampleLength, ref wShortestPath);
      GetGreedyDTWCost(xMatrix, templateLength, sampleLength, ref xShortestPath);
      GetGreedyDTWCost(yMatrix, templateLength, sampleLength, ref yShortestPath);
      GetGreedyDTWCost(zMatrix, templateLength, sampleLength, ref zShortestPath);

      DTWCost[] cost = new DTWCost[4];
      cost[0] = new DTWCost(wShortestPath, wMatrix[templateLength, sampleLength]);
      cost[1] = new DTWCost(xShortestPath, xMatrix[templateLength, sampleLength]);
      cost[2] = new DTWCost(yShortestPath, yMatrix[templateLength, sampleLength]);
      cost[3] = new DTWCost(zShortestPath, zMatrix[templateLength, sampleLength]);

      return cost;
    }
    
    //create a copy of the matrix having infinity on the first row and column
    private float[,] GetCostMatrix(float[][] matrix, int height, int width) {
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

    // Best cost matrix contains on each cell the best cost from [0,0] to [i,j]
    // TODO refactor it
    private float[,] GetBestCostMatrix(float[][] matrix, int height, int width) {
      float[,] ret = new float[height + 1, width + 1];

      for (int i = 0; i <= height; i++) {
        ret[i, 0] = 1f / 0f;
      }

      for (int j = 0; j <= width; j++) {
        ret[0, j] = 1f / 0f;
      }

      ret[1, 1] = matrix[0][0];

      for (int i = 2; i <= height; i++) {
        ret[i,1] = matrix[i-1][0] + matrix[i-2][0];
      }

      for (int j = 2; j < width; j++) {
        ret[1,j] = matrix[0][j-1] + matrix[0][j-2];
      }

      for (int j = 2; j <= width; j++) {
        for (int i = 2; i <= height; i++) {
          float min = GetMin(ret[i-1,j], ret[i,j-1], ret[i-1,j-1]);
          ret[i, j] = matrix[i-1][j-1] + min;
        }
      }

      return ret;
    }

    private float GetMin(float x, float y, float z) {
      if (x <= y && x <= z) return x;
      if (y <= x && y <= z) return y;
      if (z <= x && z <= y) return z;
      return 0;
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

    private DTWResult rawResult;
    private DTWResult filteredResult;
    private DTWResult optimalResult;
  }
}
