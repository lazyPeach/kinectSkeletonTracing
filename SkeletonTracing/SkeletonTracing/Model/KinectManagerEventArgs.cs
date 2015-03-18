using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class KinectManagerEventArgs : EventArgs {
    private Skeleton skeleton;
    
    public KinectManagerEventArgs(Skeleton skeleton) {
      this.skeleton = skeleton;
    }

    public Skeleton Skeleton { get { return skeleton; } }
  }
}
