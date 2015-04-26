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
          offset = 0.2f;
        } else {
          offset = 0.1f;
        }

        initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W - offset;
        initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X - offset;
        initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y - offset;
        initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z - offset;

        initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W + offset;
        initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X + offset;
        initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y + offset;
        initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z =
          initialPosition.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z + offset;
      }
    }

    public bool IsInitialPosition(Body body) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {

        bool godCondition =
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W > initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W &&
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W < initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W &&
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X > initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X &&
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X < initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X &&
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y > initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y &&
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y < initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y &&
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z > initialPositionDeviation.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z &&
          body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z < initialPositionDeviation.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z;

        if (!godCondition) {
          //Console.WriteLine("not ok on bone " + boneName.ToString()
          //  + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W + " "
          //  + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X + " "
          //  + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y + " "
          //  + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z);
          return false;
        }
      }

      return true;
    }


    public BodyDeviation InitialPositionDeviation { get { return initialPositionDeviation; } set { initialPositionDeviation = value; } }
    public Body InitialPosition { get { return initialPosition; } set { initialPosition = value; } }

    private BodyDeviation initialPositionDeviation;
    private Body initialPosition;
  }
}
