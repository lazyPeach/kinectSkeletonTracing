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
      bodyManager = new BodyManager();
      initialPosition = new BodyDeviation();
    }

    public void DefineInitialPosition() {
      FileStream fileStream = new FileStream(@"..\..\..\..\testData\initialPosition.xml", FileMode.Open); 
      bodyManager.LoadSample(fileStream);

      ComputeMean();
      ComputeDeviation();
    }

    private void ComputeMean() {
      foreach (Body sampleBody in bodyManager.SampleData) {
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W +=
            sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W;

          initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X +=
            sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X;

          initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y +=
            sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y;

          initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z +=
            sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z;
        }
      }

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W /= bodyManager.SampleData.Length;
        initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X /= bodyManager.SampleData.Length;
        initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y /= bodyManager.SampleData.Length;
        initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z /= bodyManager.SampleData.Length;
      }
    }

    private void ComputeDeviation() {
      foreach (Body sampleBody in bodyManager.SampleData) {
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W += (
            (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W)
            * (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W));

          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X += (
            (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X)
            * (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X));

          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y += (
            (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y)
            * (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y));

          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z += (
            (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z)
            * (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z));
        }
      }

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W = 
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W / bodyManager.SampleData.Length);

        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X =
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X / bodyManager.SampleData.Length);

        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y =
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y / bodyManager.SampleData.Length);

        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z =
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z / bodyManager.SampleData.Length);
      }
    }

    public bool IsInitialPosition(Body body) {
      return false;
    }



    private BodyDeviation InitialPosition { get {return initialPosition;} set {initialPosition = value;} }

    private BodyDeviation initialPosition;
    BodyManager bodyManager;
  }
}
