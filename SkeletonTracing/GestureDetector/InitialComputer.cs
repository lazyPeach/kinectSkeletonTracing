using Helper;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureDetector {
  public class InitialComputer {

    public InitialComputer() {
      initialPositionDeviation = new BodyDeviation();
    }

    public void DefineInitialPosition(Body initialPosition) {
      this.initialPosition = initialPosition;
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        // TODO lower the offset
        float offset;
        if (boneName == BoneName.BodyCenter || boneName == BoneName.Neck) {
          offset = 0.5f;
        } else if (boneName == BoneName.ArmLeft || boneName == BoneName.ArmRight ||
                   boneName == BoneName.ForearmLeft || boneName == BoneName.ForearmRight ||
                   boneName == BoneName.FemurusLeft || boneName == BoneName.FemurusRight ||
                   boneName == BoneName.TibiaLeft || boneName == BoneName.TibiaRight) {
          offset = 0.1f;
        } else {
          offset = 0.1f;
        }

        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W - offset;
        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X - offset;
        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y - offset;
        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z - offset;

        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W + offset;
        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X + offset;
        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y + offset;
        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z =
          initialPosition.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z + offset;
      }
    }

    public bool IsInitialPosition(Body body) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        bool godCondition =
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W > initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W &&
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W < initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W &&
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X > initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X &&
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X < initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X &&
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y > initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y &&
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y < initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y &&
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z > initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z &&
          body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z < initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z;

        if (!godCondition) return false;
      }

      return true;
    }

    public BodyDeviation InitialPositionDeviation { get { return initialPositionDeviation; } set { initialPositionDeviation = value; } }
    public Body InitialPosition { get { return initialPosition; } set { initialPosition = value; } }


    private BodyDeviation initialPositionDeviation;
    private Body initialPosition;
  }
}
