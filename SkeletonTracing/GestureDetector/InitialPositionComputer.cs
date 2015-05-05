using GestureDetector.Events;
using Helper;
using SkeletonModel.Events;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GestureDetector {
  public delegate void InitialPositionEventHandler(object sender, InitialPositionEventArgs e);

  public class InitialPositionComputer {

    public event InitialPositionEventHandler InitialPositionEventHandler;

    private BodyManager bodyManager;

    public InitialPositionComputer(BodyManager bodyManager) {
      initialPositionDeviation = new BodyDeviation();
      initialPosition = new List<Body>();
      this.bodyManager = bodyManager;
      bodyManager.RealTimeEventHandler += RealTimeEventHandler;
    }

    // when recording just adds the current body to the list of body samples
    // after recording returns the state of the body (initial position or not)
    private void RealTimeEventHandler(object sender, BodyManagerEventArgs e) {
      if (shouldRecord) {
        initialPosition.Add(e.Body);
        return;
      }

      OnEvent(new InitialPositionEventArgs(State.Check, 0, IsInitialPosition(e.Body)));
    }

    public void TestInitialPosition() {
      RecordInitialPosition();
    }

    public void RecordInitialPosition() {
      CountDown();
    }

    private int countdownSec;
    private Timer timer;
    private bool shouldRecord = false;

    private void CountDown() {
      countdownSec = 0;
      timer = new System.Timers.Timer { Interval = 1000 };
      timer.Elapsed += PauseSystem;
      timer.Start();
    }

    // make time for the user to get in front of the sensor (5 sec should be enough)
    private void PauseSystem(object sender, System.Timers.ElapsedEventArgs e) {
      OnEvent(new InitialPositionEventArgs(State.Pause, ++countdownSec, false));

      if (countdownSec == 5) {
        timer.Stop();

        timer = new System.Timers.Timer { Interval = 1000 };
        timer.Elapsed += RecordInitialPosition;
        shouldRecord = true;
        timer.Start();
        return;
      }
    }

    // record the position of the user for 5 sec
    // in the end, define the initial position
    private void RecordInitialPosition(object sender, System.Timers.ElapsedEventArgs e) {
      OnEvent(new InitialPositionEventArgs(State.Recording, --countdownSec, false));

      if (countdownSec == 0) {
        shouldRecord = false;
        DefineInitialPosition();
        timer.Stop();
        OnEvent(new InitialPositionEventArgs(State.Finish, 0, false));
        return;
      }
    }

    protected virtual void OnEvent(InitialPositionEventArgs e) {
      if (InitialPositionEventHandler != null) {
        InitialPositionEventHandler(this, e);
      }
    }

    public void DefineInitialPosition() {
      foreach (Body body in initialPosition) {
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          // refactor with min
          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W < initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W) {
            initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W;
          }

          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X < initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X) {
            initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X;
          }

          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y < initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y) {
            initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y;
          }

          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z < initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z) {
            initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z;
          }

          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W > initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W) {
            initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W;
          }

          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X > initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X) {
            initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X;
          }

          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y > initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y) {
            initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y;
          }

          if (body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z > initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z) {
            initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z = body.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z;
          }
        }
      }

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
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

        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W -= offset;
        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X -= offset;
        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y -= offset;
        initialPositionDeviation.MinBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z -= offset;

        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.W += offset;
        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.X += offset;
        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Y += offset;
        initialPositionDeviation.MaxBound.BoneSkeleton.Bones[Mapper.BoneIndexMap[boneName]].Rotation.Z += offset;
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
    public List<Body> InitialPosition { get { return initialPosition; } set { initialPosition = value; } }


    private BodyDeviation initialPositionDeviation;
    private List<Body> initialPosition;
  }
}
