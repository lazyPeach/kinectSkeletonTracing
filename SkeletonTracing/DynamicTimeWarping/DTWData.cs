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
        dtwMatrix[i] = new float[templateLength][];
        dtwWindowMatrix[i] = new float[templateLength][];

        for (int j = 0; j < templateLength; j++) {
          dtwMatrix[i][j] = new float[sampleLength];
          dtwWindowMatrix[i][j] = new float[sampleLength];
        }
      }
    }

    public BoneName BoneName { get { return boneName; } set { boneName = value; } }
    public float[][] TemplateSignal { get { return templateSignal; } set { templateSignal = value; } }
    public float[][] SampleSignal { get { return sampleSignal; } set { sampleSignal = value; } }
    public float[][][] DTWMatrix { get { return dtwMatrix; } set { dtwMatrix = value; } }
    public DTWCost[] GreedyCost { get { return greedyCost; } set { greedyCost = value; } }
    public float[][][] DTWWindowMatrix { get { return dtwWindowMatrix; } set { dtwWindowMatrix = value; } }
    public DTWCost[] GreedyWindowCost { get { return greedyWindowCost; } set { greedyWindowCost = value; } }
    public DTWCost[] BestWindowCost { get { return bestWindowCost; } set { bestWindowCost = value; } }


    private BoneName boneName;
    private float[][] templateSignal = new float[4][];
    private float[][] sampleSignal = new float[4][];
    private float[][][] dtwMatrix = new float[4][][];
    private DTWCost[] greedyCost = new DTWCost[4];
    private float[][][] dtwWindowMatrix = new float[4][][];
    private DTWCost[] greedyWindowCost = new DTWCost[4];
    private DTWCost[] bestWindowCost = new DTWCost[4];
  }
}
