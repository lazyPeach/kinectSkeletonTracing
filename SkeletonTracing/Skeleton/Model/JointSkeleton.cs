using Helper;
using Microsoft.Kinect;
using System;

namespace SkeletonModel.Model {
  public class JointSkeleton {
    public static int JOINTS_NR = 16;

    public JointSkeleton() {
      for (int i = 0; i < JOINTS_NR; i++) {
        joints[i] = new Joint();
      }
    }

    public JointSkeleton(Skeleton skeleton) {
      foreach (JointType jointType in Enum.GetValues(typeof(JointType))) {
        if (!Mapper.JointTypeJointNameMap.ContainsKey(jointType)) continue;

        SkeletonPoint pt = skeleton.Joints[jointType].Position;
        JointName jointName = Mapper.JointTypeJointNameMap[jointType];
        Joint joint = new Joint(pt.X, pt.Y, pt.Z, jointName);

        joints[Mapper.JointIndexMap[jointName]] = joint;
      }
    }

    public Joint[] Joints { get { return joints; } set { joints = value; } }

    public Joint GetJoint(JointName type) {
      return joints[Mapper.JointIndexMap[type]];
    }

    public Joint GetJoint(JointType type) {
      if (!Mapper.JointTypeJointNameMap.ContainsKey(type)) return null;

      return GetJoint(Mapper.JointTypeJointNameMap[type]);
    }


    private Joint[] joints = new Joint[JOINTS_NR];
  }
}
