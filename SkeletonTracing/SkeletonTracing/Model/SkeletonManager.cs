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
  public class SkeletonManager {
    private KinectManager kinectManager;
    private Skeleton skeleton;
    private Stopwatch stopwatch;

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

      stopwatch = new Stopwatch();

      CreateCollections();
    }

    public void Start() {
      stopwatch.Start();
    }

    public void Stop() {
      stopwatch.Stop();
    }

    public void SaveCollections(string path) {
      XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<JointInfo>));

      // make a file for each joint
      TextWriter wristLeftWriter = new StreamWriter(@String.Format("{0}\\wristLeft.xml", path));
      TextWriter wristRightWriter = new StreamWriter(@String.Format("{0}\\wristRight.xml", path));
      TextWriter elbowLeftWriter = new StreamWriter(@String.Format("{0}\\elbowLeft.xml", path));
      TextWriter elbowRightWriter = new StreamWriter(@String.Format("{0}\\elbowRight.xml", path));
      TextWriter shoulderLeftWriter = new StreamWriter(@String.Format("{0}\\shoulderLeft.xml", path));
      TextWriter shoulderRightWriter = new StreamWriter(@String.Format("{0}\\shoulderRight.xml", path));
      TextWriter shoulderCenterWriter = new StreamWriter(@String.Format("{0}\\shoulderCenter.xml", path));
      TextWriter spineWriter = new StreamWriter(@String.Format("{0}\\spine.xml", path));
      TextWriter hipCenterWriter = new StreamWriter(@String.Format("{0}\\hipCenter.xml", path));
      TextWriter hipLeftWriter = new StreamWriter(@String.Format("{0}\\hipLeft.xml", path));
      TextWriter hipRightWriter = new StreamWriter(@String.Format("{0}\\hipRight.xml", path));
      TextWriter kneeLeftWriter = new StreamWriter(@String.Format("{0}\\kneeLeft.xml", path));
      TextWriter kneeRightWriter = new StreamWriter(@String.Format("{0}\\kneeRight.xml", path));
      TextWriter ankleLeftWriter = new StreamWriter(@String.Format("{0}\\ankleLeft.xml", path));
      TextWriter ankleRightWriter = new StreamWriter(@String.Format("{0}\\ankleRight.xml", path));

      serializer.Serialize(wristLeftWriter, WristLeft);
      serializer.Serialize(wristRightWriter, WristRight);
      serializer.Serialize(elbowLeftWriter, ElbowLeft);
      serializer.Serialize(elbowRightWriter, ElbowRight);
      serializer.Serialize(shoulderLeftWriter, ShoulderLeft);
      serializer.Serialize(shoulderRightWriter, ShoulderRight);
      serializer.Serialize(shoulderCenterWriter, ShoulderCenter);
      serializer.Serialize(spineWriter, Spine);
      serializer.Serialize(hipCenterWriter, HipCenter);
      serializer.Serialize(hipLeftWriter, HipLeft);
      serializer.Serialize(hipRightWriter, HipRight);
      serializer.Serialize(kneeLeftWriter, KneeLeft);
      serializer.Serialize(kneeRightWriter, KneeRight);
      serializer.Serialize(ankleLeftWriter, AnkleLeft);
      serializer.Serialize(ankleRightWriter, AnkleRight);

      wristLeftWriter.Close();
      wristRightWriter.Close();
      elbowLeftWriter.Close();
      elbowRightWriter.Close();
      shoulderLeftWriter.Close();
      shoulderRightWriter.Close();
      shoulderCenterWriter.Close();
      spineWriter.Close();
      hipCenterWriter.Close();
      hipLeftWriter.Close();
      hipRightWriter.Close();
      kneeLeftWriter.Close();
      kneeRightWriter.Close();
      ankleLeftWriter.Close();
      ankleRightWriter.Close();
    }

    public void LoadCollections(string path) {
      XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<JointInfo>));

      TextReader wristLeftReader = new StreamReader(@String.Format("{0}\\wristLeft.xml", path));
      TextReader wristRightReader = new StreamReader(@String.Format("{0}\\wristRight.xml", path));
      TextReader elbowLeftReader = new StreamReader(@String.Format("{0}\\elbowLeft.xml", path));
      TextReader elbowRightReader = new StreamReader(@String.Format("{0}\\elbowRight.xml", path));
      TextReader shoulderLeftReader = new StreamReader(@String.Format("{0}\\shoulderLeft.xml", path));
      TextReader shoulderRightReader = new StreamReader(@String.Format("{0}\\shoulderRight.xml", path));
      TextReader shoulderCenterReader = new StreamReader(@String.Format("{0}\\shoulderCenter.xml", path));
      TextReader spineReader = new StreamReader(@String.Format("{0}\\spine.xml", path));
      TextReader hipCenterReader = new StreamReader(@String.Format("{0}\\hipCenter.xml", path));
      TextReader hipLeftReader = new StreamReader(@String.Format("{0}\\hipLeft.xml", path));
      TextReader hipRightReader = new StreamReader(@String.Format("{0}\\hipRight.xml", path));
      TextReader kneeLeftReader = new StreamReader(@String.Format("{0}\\kneeLeft.xml", path));
      TextReader kneeRightReader = new StreamReader(@String.Format("{0}\\kneeRight.xml", path));
      TextReader ankleLeftReader = new StreamReader(@String.Format("{0}\\ankleLeft.xml", path));
      TextReader ankleRightReader = new StreamReader(@String.Format("{0}\\ankleRight.xml", path));

      WristLeft = (ObservableCollection<JointInfo>)deserializer.Deserialize(wristLeftReader);
      WristRight = (ObservableCollection<JointInfo>)deserializer.Deserialize(wristRightReader);
      ElbowLeft = (ObservableCollection<JointInfo>)deserializer.Deserialize(elbowLeftReader);
      ElbowRight = (ObservableCollection<JointInfo>)deserializer.Deserialize(elbowRightReader);
      ShoulderLeft = (ObservableCollection<JointInfo>)deserializer.Deserialize(shoulderLeftReader);
      ShoulderRight = (ObservableCollection<JointInfo>)deserializer.Deserialize(shoulderRightReader);
      ShoulderCenter = (ObservableCollection<JointInfo>)deserializer.Deserialize(shoulderCenterReader);
      Spine = (ObservableCollection<JointInfo>)deserializer.Deserialize(spineReader);
      HipCenter = (ObservableCollection<JointInfo>)deserializer.Deserialize(hipCenterReader);
      HipLeft = (ObservableCollection<JointInfo>)deserializer.Deserialize(hipLeftReader);
      HipRight = (ObservableCollection<JointInfo>)deserializer.Deserialize(hipRightReader);
      KneeLeft = (ObservableCollection<JointInfo>)deserializer.Deserialize(kneeLeftReader);
      KneeRight = (ObservableCollection<JointInfo>)deserializer.Deserialize(kneeRightReader);
      AnkleLeft = (ObservableCollection<JointInfo>)deserializer.Deserialize(ankleLeftReader);
      AnkleRight = (ObservableCollection<JointInfo>)deserializer.Deserialize(ankleRightReader);

      wristLeftReader.Close();
      wristRightReader.Close();
      elbowLeftReader.Close();
      elbowRightReader.Close();
      shoulderLeftReader.Close();
      shoulderRightReader.Close();
      shoulderCenterReader.Close();
      spineReader.Close();
      hipCenterReader.Close();
      hipLeftReader.Close();
      hipRightReader.Close();
      kneeLeftReader.Close();
      kneeRightReader.Close();
      ankleLeftReader.Close();
      ankleRightReader.Close();
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
      long aquireTime = stopwatch.ElapsedMilliseconds;  // must be called here such that all joints will have the same aquire time
      
      foreach (JointType joint in Enum.GetValues(typeof(JointType))) {
        DepthImagePoint pt = kinectManager.GetJointInformation(skeleton, joint);
        AddJointToList(joint, pt, aquireTime);
      }
    }

    private void AddJointToList(JointType type, DepthImagePoint pt, long aquireTime) {
      JointInfo info = new JointInfo() { AquireTime = aquireTime, Type = type, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth };
      
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
  }
}
