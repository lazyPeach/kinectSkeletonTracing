using SkeletonTracing.Helper;

namespace SkeletonTracing.Model {
  public class Joint {
    public float      XCoord    { get; set; }
    public float      YCoord    { get; set; }
    public float      ZCoord    { get; set; }
    public JointName  Name      { get; set; }
    public Priority   Priority  { get; set; }

    public Joint() : this(0, 0, 0, JointName.HipCenter) { }

    public Joint(float xCoord, float yCoord, float zCoord, JointName name, Priority priority = Priority.Low) {
      XCoord = xCoord;
      YCoord = yCoord;
      ZCoord = zCoord;
      Name = name;
      Priority = priority;
    }
  }
}
