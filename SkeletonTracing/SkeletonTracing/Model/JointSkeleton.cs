using Microsoft.Kinect;
using SkeletonTracing.Helper;
using System;
using System.Collections.Generic;

namespace SkeletonTracing.Model {
  public class JointSkeleton {
    public static int JOINTS_NR = 16;
    private Joint[] joints = new Joint[JOINTS_NR];
    private Dictionary<JointName, int> indexMap = new Dictionary<JointName, int>() {
      {JointName.HipCenter      , 0},
      {JointName.Spine          , 1},
      {JointName.ShoulderCenter , 2},
      {JointName.Head           , 3},
      {JointName.ShoulderLeft   , 4},
      {JointName.ElbowLeft      , 5},
      {JointName.WristLeft      , 6},
      {JointName.ShoulderRight  , 7},
      {JointName.ElbowRight     , 8},
      {JointName.WristRight     , 9},
      {JointName.HipLeft        , 10},
      {JointName.KneeLeft       , 11},
      {JointName.AnkleLeft      , 12},
      {JointName.HipRight       , 13},
      {JointName.KneeRight      , 14},
      {JointName.AnkleRight     , 15}
    };

    public Joint[] Joints { get { return joints; } set { joints = value; } }

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
        
        joints[indexMap[jointName]] = joint;
      } 
    }

    public Joint GetJoint(JointName type) {
      return joints[indexMap[type]];
    }

    public Joint GetJoint(JointType type) {
      if (!Mapper.JointTypeJointNameMap.ContainsKey(type)) return null;

      return GetJoint(Mapper.JointTypeJointNameMap[type]);
    }
  }
}
