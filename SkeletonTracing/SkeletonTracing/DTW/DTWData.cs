using SkeletonTracing.Helper;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.DTW {

  // Signals represent the array of values for each component describing a bone. As a result a bone
  // generates 4 signals: w, x, y, z. A signal will be stored in a list and the 4 lists representing
  // the cumulative signal for a bone will be stored in another list of 4 elements.

  public class DTWData {
    private BoneName boneName;
    private float[][] templateSignal = new float[4][];
    private float[][] sampleSignal = new float[4][];
    private float[][][] matrix = new float[4][][];
    //private float[] cost = new float[4]; // for each of the quaternions we keep a cost

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
  //  public float[] Cost { get { return cost; } set { cost = value; } }
  }
}
