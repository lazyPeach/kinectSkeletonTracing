using Microsoft.Kinect;
using SkeletonTracing.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class BoneSkeleton {
    public static int BONES_NR = 16;

    private Bone[] bones = new Bone[BONES_NR];

    private Dictionary<BoneName, int> indexMap = new Dictionary<BoneName, int>() {
      {BoneName.BodyCenter, 0},
      {BoneName.LowerSpine, 1},
      {BoneName.UpperSpine, 2},
      {BoneName.Neck, 3},
      {BoneName.ClavicleLeft, 4},
      {BoneName.ArmLeft, 5},
      {BoneName.ForearmLeft, 6},
      {BoneName.ClavicleRight, 7},
      {BoneName.ArmRight, 8},
      {BoneName.ForearmRight, 9},
      {BoneName.HipLeft, 10},
      {BoneName.FemurusLeft, 11},
      {BoneName.TibiaLeft, 12},
      {BoneName.HipRight, 13},
      {BoneName.FemurusRight, 14},
      {BoneName.TibiaRight, 15},
    };

    private Dictionary<Tuple<JointType, JointType>, BoneName> jointBoneMap = new Dictionary<Tuple<JointType, JointType>, BoneName>() {
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.HipCenter), BoneName.BodyCenter},
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.Spine), BoneName.LowerSpine},
      {new Tuple<JointType, JointType>(JointType.Spine, JointType.ShoulderCenter), BoneName.UpperSpine},
      {new Tuple<JointType, JointType>(JointType.ShoulderCenter, JointType.Head), BoneName.Neck},
      {new Tuple<JointType, JointType>(JointType.ShoulderCenter, JointType.ShoulderLeft), BoneName.ClavicleLeft},
      {new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft), BoneName.ArmLeft},
      {new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft), BoneName.ForearmLeft},
      {new Tuple<JointType, JointType>(JointType.ShoulderCenter, JointType.ShoulderRight), BoneName.ClavicleRight},
      {new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight), BoneName.ArmRight},
      {new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight), BoneName.ForearmRight},
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.HipLeft), BoneName.HipLeft},
      {new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft), BoneName.FemurusLeft},
      {new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft), BoneName.TibiaLeft},
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.HipRight), BoneName.HipRight},
      {new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight), BoneName.FemurusRight},
      {new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight), BoneName.TibiaRight}
    };

    public BoneSkeleton() {
      for (int i = 0; i < BONES_NR; i++) {
        bones[i] = new Bone();
      }
    }

    public BoneSkeleton(Skeleton skeleton) {
      BoneOrientationCollection boneOrientations = skeleton.BoneOrientations;
      foreach (BoneOrientation boneOrientation in boneOrientations) {
        Tuple<JointType, JointType> key = new Tuple<JointType, JointType>(boneOrientation.StartJoint, boneOrientation.EndJoint);
        if (!jointBoneMap.ContainsKey(key))
          continue;
        
        Rotation rotation = new Rotation(boneOrientation.HierarchicalRotation.Quaternion.W
          , boneOrientation.HierarchicalRotation.Quaternion.X
          , boneOrientation.HierarchicalRotation.Quaternion.Y
          , boneOrientation.HierarchicalRotation.Quaternion.Z);
        BoneName BoneName = jointBoneMap[key];

        Bone bone = new Bone(rotation, BoneName);
        bones[indexMap[BoneName]] = bone;
      }
    }

    public Bone GetBone(BoneName type) {
      if (!indexMap.ContainsKey(type))
        return new Bone();

      return bones[indexMap[type]];
    }

    public Bone GetBone(JointType startJoint, JointType endType) {
      if (!jointBoneMap.ContainsKey(new Tuple<JointType, JointType>(startJoint, endType)))
        return new Bone();

      return bones[indexMap[jointBoneMap[new Tuple<JointType, JointType>(startJoint, endType)]]];
    }
  }
}
