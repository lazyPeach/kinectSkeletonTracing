using Helper;
using System;
using System.Collections.Generic;

namespace DynamicTimeWarping {
  // Signals represent the array of values for each component describing a bone. As a result a bone
  // generates 4 signals: w, x, y, z. A signal will be stored in a list and the 4 lists representing
  // the cumulative signal for a bone will be stored in another list of 4 elements.
  // Difference matrix, cost and shortest path is also stored for each of the four elements separately.
  public class DTWData {
    public DTWData(int templateLength, int sampleLength) {
      for (int i = 0; i < 4; i++) {
        templateSignal[i] = new float[templateLength];
        sampleSignal[i] = new float[sampleLength];
      }

      for (int i = 0; i < 4; i++) {
        matrix[i] = new float[templateLength][];

        for (int j = 0; j < templateLength; j++) {
          matrix[i][j] = new float[sampleLength];
        }
      }
    }

    public float[][] TemplateSignal { get { return templateSignal; } set { templateSignal = value; } }
    public float[][] SampleSignal { get { return sampleSignal; } set { sampleSignal = value; } }
    public BoneName BoneName { get { return boneName; } set { boneName = value; } }
    public float[][][] Matrix { get { return matrix; } set { matrix = value; } }
    public DTWCost[] DTWCost { get { return dtwCost; } set { dtwCost = value; } }
    
//    public List<Tuple<int, int>>[] ShortestPath { get { return shortestPath; } set { shortestPath = value; } }
//    public float[] Cost { get { return cost; } set { cost = value; } }


    private BoneName boneName;
    private float[][] templateSignal = new float[4][];
    private float[][] sampleSignal = new float[4][];
    private float[][][] matrix = new float[4][][];
    private DTWCost[] dtwCost = new DTWCost[4];
//    private List<Tuple<int, int>>[] shortestPath = new List<Tuple<int, int>>[4];
//    private float[] cost = new float[4];

  }
}
