using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class BoneSkeleton {
    public static int BONES_NR = 16;

    private Bone[] bones = new Bone[BONES_NR];

    private Dictionary<BoneType, int> indexMap = new Dictionary<BoneType, int>() {
      {BoneType.BodyCenter, 0},
      {BoneType.LowerSpine, 1},
      {BoneType.UpperSpine, 2},
      {BoneType.Neck, 3},
      {BoneType.ClavicleLeft, 4},
      {BoneType.ArmLeft, 5},
      {BoneType.ForearmLeft, 6},
      {BoneType.ClavicleRight, 7},
      {BoneType.ArmRight, 8},
      {BoneType.ForearmRight, 9},
      {BoneType.HipLeft, 10},
      {BoneType.FemurusLeft, 11},
      {BoneType.TibiaLeft, 12},
      {BoneType.HipRight, 13},
      {BoneType.FemurusRight, 14},
      {BoneType.TibiaRight, 15},
    };

    private Dictionary<Tuple<JointType, JointType>, BoneType> jointBoneMap = new Dictionary<Tuple<JointType, JointType>, BoneType>() {
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.HipCenter), BoneType.BodyCenter},
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.Spine), BoneType.LowerSpine},
      {new Tuple<JointType, JointType>(JointType.Spine, JointType.ShoulderCenter), BoneType.UpperSpine},
      {new Tuple<JointType, JointType>(JointType.ShoulderCenter, JointType.Head), BoneType.Neck},
      {new Tuple<JointType, JointType>(JointType.ShoulderCenter, JointType.ShoulderLeft), BoneType.ClavicleLeft},
      {new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft), BoneType.ArmLeft},
      {new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft), BoneType.ForearmLeft},
      {new Tuple<JointType, JointType>(JointType.ShoulderCenter, JointType.ShoulderRight), BoneType.ClavicleRight},
      {new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight), BoneType.ArmRight},
      {new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight), BoneType.ForearmRight},
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.HipLeft), BoneType.HipLeft},
      {new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft), BoneType.FemurusLeft},
      {new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft), BoneType.TibiaLeft},
      {new Tuple<JointType, JointType>(JointType.HipCenter, JointType.HipRight), BoneType.HipRight},
      {new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight), BoneType.FemurusRight},
      {new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight), BoneType.TibiaRight}
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
        BoneType boneType = jointBoneMap[key];

        Bone bone = new Bone(rotation, boneType);
        bones[indexMap[boneType]] = bone;
      }
    }

    public Bone GetBone(BoneType type) {
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
