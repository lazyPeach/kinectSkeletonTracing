using Microsoft.Kinect;
using System;
using System.Linq;

namespace SkeletonTracing.Model {
  public delegate void KinectManagerEventHandler(object sender, KinectManagerEventArgs e);

  public class KinectManager {
    public event KinectManagerEventHandler KinectManagerEvent;

    private KinectSensor kinectSensor;
    private Skeleton[] skeletonData;

    public KinectManager() {
      InitializeKinect();
    }

    public void Start() {
      kinectSensor.Start();
    }

    public void Stop() {
      kinectSensor.Stop();
    }


    private void InitializeKinect() {
      kinectSensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
      kinectSensor.SkeletonStream.Enable();

      skeletonData = new Skeleton[kinectSensor.SkeletonStream.FrameSkeletonArrayLength];
      kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);
    }

    // listen to SkeletonFrameReady events
    private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e) {
      SkeletonFrame skeletonFrame = e.OpenSkeletonFrame();                                        // Open the Skeleton frame

      if (skeletonFrame != null && skeletonData != null) {
        skeletonFrame.CopySkeletonDataTo(skeletonData);                                           // get the skeletal information in this frame

        foreach (Skeleton skeleton in skeletonData) {                                             // iterate through the 6 skeletons that sensor is able to track
          if (skeleton.TrackingState == SkeletonTrackingState.Tracked) {
            KinectManagerEventArgs newEvent = new KinectManagerEventArgs(skeleton);
            OnEvent(newEvent);
            break;                                                                                // once you find a skeleton that is tracked don't care about others
          }
        }
      }


    }

    protected virtual void OnEvent(KinectManagerEventArgs e) {
      if (KinectManagerEvent != null) {
        KinectManagerEvent(this, e);
      }
    }

    public DepthImagePoint GetJointInformation(Skeleton skeleton, JointType jointType) {
      return kinectSensor.CoordinateMapper.MapSkeletonPointToDepthPoint(
        skeleton.Joints[jointType].Position, DepthImageFormat.Resolution640x480Fps30);
    }
  }
}
