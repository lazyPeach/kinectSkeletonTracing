using Microsoft.Kinect;

namespace SkeletonModel.Model {
  public enum Priority {
    Low = 0,
    Medium = 1,
    High = 2
  }

  public class Body {
    public JointSkeleton Joints { get; set; }
    public BoneSkeleton Bones { get; set; }

    public Body(Skeleton skeleton) {
      Joints = new JointSkeleton(skeleton);
      Bones = new BoneSkeleton(skeleton);
    }

    public Body() {
      Joints = new JointSkeleton();
      Bones = new BoneSkeleton();
    }
  }
}
