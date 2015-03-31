using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace SkeletonModel.Events {
  public class KinectManagerEventArgs : EventArgs {
    private Skeleton skeleton;

    public KinectManagerEventArgs(Skeleton skeleton) {
      this.skeleton = skeleton;
    }

    public Skeleton Skeleton { get { return skeleton; } }
  }
}
