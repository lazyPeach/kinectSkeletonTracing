using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class KinectManagerEventArgs :EventArgs {
    private Skeleton _skeleton;
    
    public KinectManagerEventArgs(Skeleton skeleton) {
      _skeleton = skeleton;
    }

    public Skeleton Skeleton { get { return _skeleton; } }

  }
}
