﻿using Microsoft.Kinect;

namespace SkeletonModel.Model {
  public class Body {
    public Body() {
      jointSkeleton = new JointSkeleton();
      boneSkeleton  = new BoneSkeleton();
    }

    public Body(Skeleton skeleton) {
      jointSkeleton = new JointSkeleton(skeleton);
      boneSkeleton  = new BoneSkeleton(skeleton);
    }

    public JointSkeleton JointSkeleton { get { return jointSkeleton; } set { jointSkeleton = value; } }
    public BoneSkeleton BoneSkeleton { get { return boneSkeleton; } set { boneSkeleton = value; } }


    private JointSkeleton jointSkeleton;
    private BoneSkeleton boneSkeleton;
  }
}
