using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SkeletonTracing.Model {
  public delegate void BodyManagerEventHandler(object sender, BodyManagerEventArgs e);
  
  public class BodyManager {
    public event BodyManagerEventHandler BodyManagerEventHandl;    
    private KinectManager kinectManager;
    public KinectManager KinectManagerProp { get { return kinectManager; } set { kinectManager = value; } }
    private Stopwatch stopwatch;

    public ObservableCollection<Body> BodyData { get; set; }


    public BodyManager(KinectManager kinectManager) {
      this.kinectManager = kinectManager;
      kinectManager.KinectManagerEventHandl += KinectManagerEventHandler;

      BodyData = new ObservableCollection<Body>();
      stopwatch = new Stopwatch();
    }

    public void Start() {
      stopwatch.Start();
    }

    public void Stop() {
      stopwatch.Stop();
    }

    public void SaveCollections(string path) {
      XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextWriter textWriter = new StreamWriter(@String.Format("{0}\\ninja.xml", path));
      serializer.Serialize(textWriter, BodyData);
      textWriter.Close();
    }

    public void LoadCollections(string path) {
      XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextReader textReader = new StreamReader(@String.Format("{0}\\ninja.xml", path));
      BodyData = (ObservableCollection<Body>)deserializer.Deserialize(textReader);
      textReader.Close();
    }

    private void KinectManagerEventHandler(object sender, KinectManagerEventArgs e) {
      Body body = new Body(e.Skeleton);
      BodyManagerEventArgs ev = new BodyManagerEventArgs(body);
      OnEvent(ev);
      BodyData.Add(body);
    }

    protected virtual void OnEvent(BodyManagerEventArgs e) {
      if (BodyManagerEventHandl != null) {
        BodyManagerEventHandl(this, e);
      }
    }













    /*
    private void ComputeJoints(Skeleton skeleton) {
      long aquireTime = stopwatch.ElapsedMilliseconds;  // must be called here such that all joints will have the same aquire time
      foreach (JointType jointType in Enum.GetValues(typeof(JointType))) {
        SkeletonPoint pt = skeleton.Joints[jointType].Position;
        AddJointToList(jointType, pt, aquireTime);
      }

      BoneOrientationCollection bones = skeleton.BoneOrientations;
      //bones[JointType.HipCenter]
      foreach (BoneOrientation bone in bones) {
        JointType join = bone.StartJoint;
      }
    }

    private void AddJointToList(JointType type, SkeletonPoint pt, long aquireTime) {
      JointInfo info = new JointInfo() { AquireTime = aquireTime, Type = type, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Z };
      
      switch (type) {
        case JointType.WristLeft:
          WristLeft.Add(info);
          break;
        case JointType.WristRight:
          WristRight.Add(info);
          break;
        case JointType.ElbowLeft:
          ElbowLeft.Add(info);
          break;
        case JointType.ElbowRight:
          ElbowRight.Add(info);
          break;
        case JointType.ShoulderLeft:
          ShoulderLeft.Add(info);
          break;
        case JointType.ShoulderRight:
          ShoulderRight.Add(info);
          break;
        case JointType.ShoulderCenter:
          ShoulderCenter.Add(info);
          break;
        case JointType.Spine:
          Spine.Add(info);
          break;
        case JointType.HipCenter:
          HipCenter.Add(info);
          break;
        case JointType.HipLeft:
          HipLeft.Add(info);
          break;
        case JointType.HipRight:
          HipRight.Add(info);
          break;
        case JointType.KneeLeft:
          KneeLeft.Add(info);
          break;
        case JointType.KneeRight:
          KneeRight.Add(info);
          break;
        case JointType.AnkleLeft:
          AnkleLeft.Add(info);
          break;
        case JointType.AnkleRight:
          AnkleRight.Add(info);
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
     */ 
  }
}
