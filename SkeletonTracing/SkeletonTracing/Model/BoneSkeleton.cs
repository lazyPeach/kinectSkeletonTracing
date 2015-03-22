using Microsoft.Kinect;
using SkeletonTracing.Helper;
using System;
using System.Collections.Generic;

namespace SkeletonTracing.Model {
  public class BoneSkeleton {
    public static int BONES_NR = 16;
    private Bone[] bones = new Bone[BONES_NR];
    private Dictionary<BoneName, int> indexMap = new Dictionary<BoneName, int>() {
      {BoneName.BodyCenter    , 0},
      {BoneName.LowerSpine    , 1},
      {BoneName.UpperSpine    , 2},
      {BoneName.Neck          , 3},
      {BoneName.ClavicleLeft  , 4},
      {BoneName.ArmLeft       , 5},
      {BoneName.ForearmLeft   , 6},
      {BoneName.ClavicleRight , 7},
      {BoneName.ArmRight      , 8},
      {BoneName.ForearmRight  , 9},
      {BoneName.HipLeft       , 10},
      {BoneName.FemurusLeft   , 11},
      {BoneName.TibiaLeft     , 12},
      {BoneName.HipRight      , 13},
      {BoneName.FemurusRight  , 14},
      {BoneName.TibiaRight    , 15},
    };

    public Bone[] Bones { get { return bones; } set { bones = value; } }

    public BoneSkeleton() {
      for (int i = 0; i < BONES_NR; i++) {
        bones[i] = new Bone();
      }
    }

    public BoneSkeleton(Skeleton skeleton) {
      foreach (BoneOrientation boneOrientation in skeleton.BoneOrientations) {
        if (!Mapper.JointTypeJointNameMap.ContainsKey(boneOrientation.StartJoint) ||
          !Mapper.JointTypeJointNameMap.ContainsKey(boneOrientation.EndJoint)) continue;

        Tuple<JointName, JointName> tup = new Tuple<JointName, JointName>(
          Mapper.JointTypeJointNameMap[boneOrientation.StartJoint], Mapper.JointTypeJointNameMap[boneOrientation.EndJoint]);

        if (!Mapper.JointBoneMap.ContainsKey(tup)) continue;


        BoneName boneName = Mapper.JointBoneMap[tup];

        Rotation rotation = new Rotation(boneOrientation.HierarchicalRotation.Quaternion.W
          , boneOrientation.HierarchicalRotation.Quaternion.X
          , boneOrientation.HierarchicalRotation.Quaternion.Y
          , boneOrientation.HierarchicalRotation.Quaternion.Z);

        Bone bone = new Bone(rotation, boneName);
        bones[indexMap[boneName]] = bone;
      }
    }

    public Bone GetBone(BoneName type) {
      if (!indexMap.ContainsKey(type))
        return new Bone();

      return bones[indexMap[type]];
    }

    /*
    public Bone GetBone(JointName startJoint, JointName endJoint) {
      Mapper mapper = new Mapper();
      //return bones[indexMap[mapper.GetBoneName(startJoint, endJoint)]];
    }

    public Bone GetBone(JointType startJoint, JointType endJoint) {
      Mapper mapper = new Mapper();
      //return GetBone(mapper.GetJointName(startJoint), mapper.GetJointName(endJoint));
    }
     */
  }
}
