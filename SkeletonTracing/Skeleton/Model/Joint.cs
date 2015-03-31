using Helper;

namespace SkeletonModel.Model {
  public class Joint {
    public Joint() : this(0, 0, 0, JointName.HipCenter) { }

    public Joint(float xCoord, float yCoord, float zCoord, JointName jointName, Priority priority = Priority.Low) {
      this.xCoord = xCoord;
      this.yCoord = yCoord;
      this.zCoord = zCoord;
      this.jointName = jointName;
      this.priority = priority;
    }

    public float XCoord { get { return xCoord; } set { xCoord = value; } }
    public float YCoord { get { return yCoord; } set { yCoord = value; } }
    public float ZCoord { get { return zCoord; } set { zCoord = value; } }
    public JointName JointName { get { return jointName; } set { jointName = value; } }
    public Priority Priority { get { return priority; } set { priority = value; } }

    private float xCoord;
    private float yCoord;
    private float zCoord;
    private JointName jointName;
    private Priority priority;
  }
}
