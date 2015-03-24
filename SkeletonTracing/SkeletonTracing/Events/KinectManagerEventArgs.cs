using Microsoft.Kinect;
using System;

namespace SkeletonTracing.Model {
  public class KinectManagerEventArgs : EventArgs {
    private Skeleton skeleton;
    
    public KinectManagerEventArgs(Skeleton skeleton) {
      this.skeleton = skeleton;
    }

    public Skeleton Skeleton { get { return skeleton; } }
  }
}
