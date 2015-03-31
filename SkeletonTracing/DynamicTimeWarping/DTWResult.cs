using SkeletonModel.Model;

namespace DynamicTimeWarping {
  public enum ExerciseSpeed {
    Slow,
    Normal,
    Fast
  }

  public class DTWResult {
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


    private DTWData[] data = new DTWData[JointSkeleton.JOINTS_NR];
    private ExerciseSpeed speed;
  }
}
