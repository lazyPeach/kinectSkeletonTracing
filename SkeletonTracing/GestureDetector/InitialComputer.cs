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
      FileStream fileStream = new FileStream(@"..\..\..\..\testData\initialPosition1.xml", FileMode.Open); 
      bodyManager.LoadSample(fileStream);
      
      ComputeBounds();
      PrintBounds();
      TestBounds();
    }

    private void ComputeBounds() {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W = 1 / 0f;
        initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X = 1 / 0f;
        initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y = 1 / 0f;
        initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z = 1 / 0f;

        initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W = -1 / 0f;
        initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X = -1 / 0f;
        initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y = -1 / 0f;
        initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z = -1 / 0f;
      }
      
      foreach (Body sampleBody in bodyManager.SampleData) {
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W
            > initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W)
            initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W = 
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W;

          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X
            > initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X)
            initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X =
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X;

          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y
            > initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y)
            initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y =
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y;

          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z
            > initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z)
            initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z =
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z;

          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W
            < initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W)
            initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W =
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W;

          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X
            < initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X)
            initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X =
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X;

          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y
            < initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y)
            initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y =
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y;

          if (sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z
            < initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z)
            initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z =
              sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z;

        }
      }
    }

    private void PrintBounds() {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        Console.WriteLine(boneName.ToString());
        Console.WriteLine("w: min = " + initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W.ToString()
          + "; max = " + initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W);
        Console.WriteLine("w: min = " + initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X.ToString()
          + "; max = " + initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X);
        Console.WriteLine("w: min = " + initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y.ToString()
          + "; max = " + initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y);
        Console.WriteLine("w: min = " + initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z.ToString()
          + "; max = " + initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z);
      }
    }

    private void TestBounds() {
      Console.WriteLine("testing bounds");
      int i = 0;
      foreach (Body sampleBody in bodyManager.SampleData) {
        Console.WriteLine("sample " + i.ToString());
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          if (!(sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W
            || sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W)) {
              Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " W is " + sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W);
          }

          if (!(sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X
            || sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X)) {
            Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " X is " + sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X);
          }

          if (!(sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y
  || sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y)) {
            Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " Y is " + sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y);
          }

          if (!(sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z
  || sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z)) {
            Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " Z is " + sampleBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z);
          }
          

        }
        i++;


      }
    }

    private void TestInitialPosition() {
      BodyManager deviation = new BodyManager();
      int i = 0;
      foreach (Body sampleBody in bodyManager.SampleData) {
        Body newBody = new Body();
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W =
            Math.Abs(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W - 
              initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W);

          newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X =
            Math.Abs(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X -
              initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X);

          newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y =
            Math.Abs(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y -
              initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y);

          newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z =
            Math.Abs(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z -
              initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z);
          //THIS WILL SHOW THAT ALL SAMPLES HAVE AT LEAST ONE COMPONENT OUT OF THE DEVIATION
          //if (newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W >
          //  initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W) {
          //    Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " w is " + newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W.ToString());
          //}

          //if (newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X >
          //  initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X) {
          //  Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " X is " + newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X.ToString());
          //}

          //if (newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y >
          //  initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y) {
          //  Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " Y is " + newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y.ToString());
          //}

          //if (newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z >
          //  initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z) {
          //  Console.WriteLine("Sample " + i.ToString() + " bone " + boneName.ToString() + " w is " + newBody.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z.ToString());
          //}
        }
        i++;


      }
    }

    private void PrintMeanAndDeviation() {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        Console.WriteLine(boneName.ToString());
        Console.WriteLine("w: mean = " + initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W.ToString()
          + "; deviation = " + initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W);
        Console.WriteLine("w: mean = " + initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X.ToString()
          + "; deviation = " + initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X);
        Console.WriteLine("w: mean = " + initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y.ToString()
          + "; deviation = " + initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y);
        Console.WriteLine("w: mean = " + initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z.ToString()
          + "; deviation = " + initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z);
      }
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
          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W += 
            (float)Math.Pow(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W, 2);

          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X +=
            (float)Math.Pow(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X, 2);

          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y +=
            (float)Math.Pow(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y, 2);


          initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z +=
            (float)Math.Pow(sampleBody.Bones.Bones[Helper.Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z - initialPosition.Mean.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z, 2);
          
          }
      }

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W /= bodyManager.SampleData.Length;
        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X /= bodyManager.SampleData.Length;
        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y /= bodyManager.SampleData.Length;
        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z /= bodyManager.SampleData.Length;

        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W = 
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W);

        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X =
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X);

        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y =
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y);

        initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z =
          (float)Math.Sqrt(initialPosition.Deviation.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z);
      }
    }

    public bool IsInitialPosition(Body body) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        if (boneName != BoneName.BodyCenter &&
          boneName != BoneName.LowerSpine &&
          boneName != BoneName.UpperSpine &&
          boneName != BoneName.ClavicleRight &&
          boneName != BoneName.ClavicleLeft &&
          boneName != BoneName.Neck) {

            bool godCondition = 
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W &&
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W &&
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X &&
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X &&
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y &&
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y &&
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z > initialPosition.MinBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z &&
              body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z < initialPosition.MaxBound.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z;


          if (!godCondition) {
            Console.WriteLine("not ok on bone " + boneName.ToString()
              + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.W + " "
              + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.X + " "
              + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Y + " "
              + body.Bones.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Quaternion.Z);
            return false;
          }
        }
      }

      return true;
    }



    private BodyDeviation InitialPosition { get {return initialPosition;} set {initialPosition = value;} }

    private BodyDeviation initialPosition;
    BodyManager bodyManager;
  }
}
