using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class SkeletonManager {
    private KinectManager kinectManager;
    private Skeleton skeleton;

    public ObservableCollection<JointInfo> WristLeft { get; set; }
    public ObservableCollection<JointInfo> WristRight { get; set; }
    public ObservableCollection<JointInfo> ElbowLeft { get; set; }
    public ObservableCollection<JointInfo> ElbowRight { get; set; }
    public ObservableCollection<JointInfo> ShoulderLeft { get; set; }
    public ObservableCollection<JointInfo> ShoulderRight { get; set; }
    public ObservableCollection<JointInfo> ShoulderCenter { get; set; }
    public ObservableCollection<JointInfo> Spine { get; set; }
    public ObservableCollection<JointInfo> HipCenter { get; set; }
    public ObservableCollection<JointInfo> HipLeft { get; set; }
    public ObservableCollection<JointInfo> HipRight { get; set; }
    public ObservableCollection<JointInfo> KneeLeft { get; set; }
    public ObservableCollection<JointInfo> KneeRight { get; set; }
    public ObservableCollection<JointInfo> AnkleLeft { get; set; }
    public ObservableCollection<JointInfo> AnkleRight { get; set; }

    public SkeletonManager(KinectManager kinectManager) {
      this.kinectManager = kinectManager;
      kinectManager.KinectManagerEvent += new KinectManagerEventHandler(KinectManagerEventHandle);

      CreateCollections();
    }

    private void CreateCollections() {
      WristLeft = new ObservableCollection<JointInfo>();
      WristRight = new ObservableCollection<JointInfo>();
      ElbowLeft = new ObservableCollection<JointInfo>();
      ElbowRight = new ObservableCollection<JointInfo>();
      ShoulderLeft = new ObservableCollection<JointInfo>();
      ShoulderRight = new ObservableCollection<JointInfo>();
      ShoulderCenter = new ObservableCollection<JointInfo>();
      Spine = new ObservableCollection<JointInfo>();
      HipCenter = new ObservableCollection<JointInfo>();
      HipLeft = new ObservableCollection<JointInfo>();
      HipRight = new ObservableCollection<JointInfo>();
      KneeLeft = new ObservableCollection<JointInfo>();
      KneeRight = new ObservableCollection<JointInfo>();
      AnkleLeft = new ObservableCollection<JointInfo>();
      AnkleRight = new ObservableCollection<JointInfo>();
    }

    private void KinectManagerEventHandle(object sender, KinectManagerEventArgs e) {
      skeleton = e.Skeleton;
      ComputeJoints(e.Skeleton);
    }

    private void ComputeJoints(Skeleton skeleton) {
      foreach (JointType joint in Enum.GetValues(typeof(JointType))) {
        DepthImagePoint pt = kinectManager.GetJointInformation(skeleton, joint);
        AddJointToList(joint, pt);
      }
    }

    private void AddJointToList(JointType type, DepthImagePoint pt) {
      switch (type) {
        case JointType.WristLeft:
          WristLeft.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.WristRight:
          WristRight.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ElbowLeft:
          ElbowLeft.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ElbowRight:
          ElbowRight.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ShoulderLeft:
          ShoulderLeft.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ShoulderRight:
          ShoulderRight.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ShoulderCenter:
          ShoulderCenter.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.Spine:
          Spine.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.HipCenter:
          HipCenter.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.HipLeft:
          HipLeft.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.HipRight:
          HipRight.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.KneeLeft:
          KneeLeft.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.KneeRight:
          KneeRight.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.AnkleLeft:
          AnkleLeft.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.AnkleRight:
          AnkleRight.Add(new JointInfo() { XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        default:// other types not of interest
          break;
      }
    }

    public ObservableCollection<JointInfo> GetJointList(JointType jointType) {
      switch (jointType) {
        case JointType.WristLeft:
          return WristLeft;
        case JointType.WristRight:
          return WristRight;
        case JointType.ElbowLeft:
          return ElbowLeft;
        case JointType.ElbowRight:
          return ElbowRight;
        case JointType.ShoulderLeft:
          return ShoulderLeft;
        case JointType.ShoulderRight:
          return ShoulderRight;
        case JointType.ShoulderCenter:
          return ShoulderCenter;
        case JointType.Spine:
          return Spine;
        case JointType.HipCenter:
          return HipCenter;
        case JointType.HipLeft:
          return HipLeft;
        case JointType.HipRight:
          return HipRight;
        case JointType.KneeLeft:
          return KneeLeft;
        case JointType.KneeRight:
          return KneeRight;
        case JointType.AnkleLeft:
          return AnkleLeft;
        case JointType.AnkleRight:
          return AnkleRight;
        default:// other types not of interest
          return null;
      }
    }
  }
}
