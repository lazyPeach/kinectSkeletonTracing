using Helper;
using Microsoft.Kinect;
using System;

namespace SkeletonModel.Model {
  public class BoneSkeleton {
    public static int BONES_NR = 16;

    public BoneSkeleton() {
      for (int i = 0; i < BONES_NR; i++) {
        bones[i] = new Bone();
      }
    }

    public BoneSkeleton(Skeleton skeleton) {
      foreach (BoneOrientation boneOrientation in skeleton.BoneOrientations) {
        if (!Mapper.JointTypeJointNameMap.ContainsKey(boneOrientation.StartJoint) ||
            !Mapper.JointTypeJointNameMap.ContainsKey(boneOrientation.EndJoint)) continue;

        Tuple<JointName, JointName> jointTuple = new Tuple<JointName, JointName>(
          Mapper.JointTypeJointNameMap[boneOrientation.StartJoint], 
          Mapper.JointTypeJointNameMap[boneOrientation.EndJoint]);

        if (!Mapper.JointBoneMap.ContainsKey(jointTuple)) continue;

        BoneName boneName = Mapper.JointBoneMap[jointTuple];

        Rotation rotation = new Rotation(boneOrientation.HierarchicalRotation.Quaternion.W
                                       , boneOrientation.HierarchicalRotation.Quaternion.X
                                       , boneOrientation.HierarchicalRotation.Quaternion.Y
                                       , boneOrientation.HierarchicalRotation.Quaternion.Z);

        Bone bone = new Bone(rotation, boneName);
        bones[Mapper.BoneIndexMap[boneName]] = bone;
      }
    }

    public Bone[] Bones { get { return bones; } set { bones = value; } }

    public Bone GetBone(BoneName type) {
      if (!Mapper.BoneIndexMap.ContainsKey(type))
        return new Bone();

      return bones[Mapper.BoneIndexMap[type]];
    }


    private Bone[] bones = new Bone[BONES_NR];
  }
}
