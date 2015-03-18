using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class JointSkeleton {
    public static int JOINTS_NR = 16;

    private Joint[] joints = new Joint[JOINTS_NR];

    private Dictionary<JointType, int> indexMap = new Dictionary<JointType, int>() {
      {JointType.HipCenter, 0},
      {JointType.Spine, 1},
      {JointType.ShoulderCenter, 2},
      {JointType.Head, 3},
      {JointType.ShoulderLeft, 4},
      {JointType.ElbowLeft, 5},
      {JointType.WristLeft, 6},
      {JointType.ShoulderRight, 7},
      {JointType.ElbowRight, 8},
      {JointType.WristRight, 9},
      {JointType.HipLeft, 10},
      {JointType.KneeLeft, 11},
      {JointType.AnkleLeft, 12},
      {JointType.HipRight, 13},
      {JointType.KneeRight, 14},
      {JointType.AnkleRight, 15}
    };

    public JointSkeleton() {
      for (int i = 0; i < JOINTS_NR; i++) {
        joints[i] = new Joint();
      }
    }

    public JointSkeleton(Skeleton skeleton) {
      foreach (JointType jointType in Enum.GetValues(typeof(JointType))) {
        if (!indexMap.ContainsKey(jointType))
          continue;
        
        SkeletonPoint pt = skeleton.Joints[jointType].Position;
        Joint joint = new Joint(pt.X, pt.Y, pt.Z, jointType);
        joints[indexMap[jointType]] = joint;
      } 
    }

    public Joint GetJoint(JointType type) {
      if (!indexMap.ContainsKey(type))
        return null;// new Joint(); // or null?

      return joints[indexMap[type]];
    }


  }
}
