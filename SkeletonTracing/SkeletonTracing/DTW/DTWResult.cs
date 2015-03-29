using SkeletonTracing.Helper;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.DTW {
  public enum ExerciseSpeed {
    Slow,
    Normal,
    Fast
  }

  public class DTWResult {
    private Dictionary<JointName, int> indexMap = new Dictionary<JointName, int>() {
      {JointName.HipCenter      , 0},
      {JointName.Spine          , 1},
      {JointName.ShoulderCenter , 2},
      {JointName.Head           , 3},
      {JointName.ShoulderLeft   , 4},
      {JointName.ElbowLeft      , 5},
      {JointName.WristLeft      , 6},
      {JointName.ShoulderRight  , 7},
      {JointName.ElbowRight     , 8},
      {JointName.WristRight     , 9},
      {JointName.HipLeft        , 10},
      {JointName.KneeLeft       , 11},
      {JointName.AnkleLeft      , 12},
      {JointName.HipRight       , 13},
      {JointName.KneeRight      , 14},
      {JointName.AnkleRight     , 15}
    };

    private DTWData[] data = new DTWData[JointSkeleton.JOINTS_NR];
    private ExerciseSpeed speed;

    public DTWResult(int templateLength, int sampleLength) {
      for (int i = 0; i < JointSkeleton.JOINTS_NR; i++) {
        data[i] = new DTWData(templateLength, sampleLength);
      }

      ComputeExerciseSpeed(templateLength, sampleLength);
    }

    public DTWData[] Data { get { return data; } set { data = value; } }
    public ExerciseSpeed Speed { get { return speed; } set { speed = value; } }

    // If the template length is much larger than sample => it took too much to user to make the exercise
    // If the template length is much smaller than sample => user made the exercise too fast
    private void ComputeExerciseSpeed(int templateLength, int sampleLength) {
      double ratio = (double)templateLength / (double)sampleLength;

      if (ratio < 0.5) speed = ExerciseSpeed.Slow;
      else if (ratio > 2) speed = ExerciseSpeed.Fast;
      else speed = ExerciseSpeed.Normal;
    }

    

  }
}
