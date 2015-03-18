using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public enum Priority {
    Low     = 0,
    Medium  = 1,
    High    = 2
  }

  public class Body {
    public JointSkeleton Joints { get; set; }
    public BoneSkeleton Bones { get; set; }

    public Body(Skeleton skeleton) {
      Joints = new JointSkeleton(skeleton);
      Bones = new BoneSkeleton(skeleton);
    }
  }
}
