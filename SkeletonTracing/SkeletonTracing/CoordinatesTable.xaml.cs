using Microsoft.Kinect;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkeletonTracing {
  public partial class CoordinatesTable : UserControl {

    public ObservableCollection<JointInfo> LeftWrist { get; set; }
    public ObservableCollection<JointInfo> LeftElbow { get; set; }
    public ObservableCollection<JointInfo> LeftShoulder { get; set; }
    public ObservableCollection<JointInfo> CenterShoulder { get; set; }
    public ObservableCollection<JointInfo> RightShoulder { get; set; }
    public ObservableCollection<JointInfo> RightElbow { get; set; }
    public ObservableCollection<JointInfo> RightWrist { get; set; }
    public ObservableCollection<JointInfo> Spine { get; set; }
    public ObservableCollection<JointInfo> HipCenter { get; set; }
    public ObservableCollection<JointInfo> HipLeft { get; set; }
    public ObservableCollection<JointInfo> HipRight { get; set; }
    public ObservableCollection<JointInfo> KneeLeft { get; set; }
    public ObservableCollection<JointInfo> KneeRight { get; set; }
    public ObservableCollection<JointInfo> AnkleLeft { get; set; }
    public ObservableCollection<JointInfo> AnkleRight { get; set; }
    
    public ObservableCollection<JointInfo> Data { get; set; }
    
    private KinectSensor kinectSensor;
    private Skeleton[] skeletonData;
    private Stopwatch sw; // used for getting time of joint aquizition


    public CoordinatesTable() {
      InitializeComponent();
      this.DataContext = this;

      InitializeJointsList();
      CreateCollections();
      InitializeKinect();
    }

    private void InitializeKinect() {
      kinectSensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
      kinectSensor.SkeletonStream.Enable();

      skeletonData = new Skeleton[kinectSensor.SkeletonStream.FrameSkeletonArrayLength];
      kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);
    }

    private void InitializeJointsList() {
      JointList.ItemsSource = Enum.GetValues(typeof(JointType)).Cast<JointType>();
      JointList.SelectedIndex = 0;
    }

    private void CreateCollections() {
      Data = new ObservableCollection<JointInfo>();

      LeftWrist = new ObservableCollection<JointInfo>();
      LeftElbow = new ObservableCollection<JointInfo>();
      LeftShoulder = new ObservableCollection<JointInfo>();
      CenterShoulder = new ObservableCollection<JointInfo>();
      RightWrist = new ObservableCollection<JointInfo>();
      RightElbow = new ObservableCollection<JointInfo>();
      RightShoulder = new ObservableCollection<JointInfo>();
      Spine = new ObservableCollection<JointInfo>();
      HipCenter = new ObservableCollection<JointInfo>();
      HipLeft = new ObservableCollection<JointInfo>();
      HipRight = new ObservableCollection<JointInfo>();
      KneeLeft = new ObservableCollection<JointInfo>();
      KneeRight = new ObservableCollection<JointInfo>();
      AnkleLeft = new ObservableCollection<JointInfo>();
      AnkleRight = new ObservableCollection<JointInfo>();
    }

    private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e) {
      using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame()) { // Open the Skeleton frame
        if (skeletonFrame != null && skeletonData != null) {
          skeletonFrame.CopySkeletonDataTo(skeletonData); // get the skeletal information in this frame
          ComputeSkeletonData();
        }
      }
    }

    private void ComputeSkeletonData() {
      foreach (Skeleton skeleton in skeletonData) {
        if (skeleton.TrackingState == SkeletonTrackingState.Tracked) { // find a way to take only one skeleton and ignore the rest
          ComputeJoints(skeleton);
        }
      }
    }

    private void ComputeJoints(Skeleton skeleton) {
      foreach (JointType joint in Enum.GetValues(typeof(JointType))) {
        AquireJointInformation(skeleton, joint);
      }
    }

    private void AquireJointInformation(Skeleton skeleton, JointType type) {
      DepthImagePoint pt = kinectSensor.CoordinateMapper.MapSkeletonPointToDepthPoint(
        skeleton.Joints[type].Position, DepthImageFormat.Resolution640x480Fps30);
      AddJointToList(type, pt);
    }

    private void AddJointToList(JointType type, DepthImagePoint pt) {
      switch (type) {
        case JointType.WristLeft:
          LeftWrist.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.WristRight:
          RightWrist.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ElbowLeft:
          LeftElbow.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ElbowRight:
          RightElbow.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ShoulderLeft:
          LeftShoulder.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ShoulderRight:
          RightShoulder.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.ShoulderCenter:
          CenterShoulder.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.Spine:
          Spine.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.HipCenter:
          HipCenter.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.HipLeft:
          HipLeft.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.HipRight:
          HipRight.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.KneeLeft:
          KneeLeft.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.KneeRight:
          KneeRight.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.AnkleLeft:
          AnkleLeft.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        case JointType.AnkleRight:
          AnkleRight.Add(new JointInfo() { AquireTime = sw.ElapsedMilliseconds, XCoord = pt.X, YCoord = pt.Y, ZCoord = pt.Depth });
          break;
        default:// other types not of interest
          break;
      }
    }

    public void Start() {
      sw = Stopwatch.StartNew();
      kinectSensor.Start();
    }

    private void JointList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      JointType type = (JointType)JointList.SelectedValue;
      switch (type) {
        case JointType.WristLeft:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = LeftWrist });
          break;
        case JointType.WristRight:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = RightWrist });
          break;
        case JointType.ElbowLeft:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = LeftElbow });
          break;
        case JointType.ElbowRight:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = RightElbow });
          break;
        case JointType.ShoulderLeft:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = LeftShoulder });
          break;
        case JointType.ShoulderRight:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = RightShoulder });
          break;
        case JointType.ShoulderCenter:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = CenterShoulder });
          break;
        case JointType.Spine:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = Spine });
          break;
        case JointType.HipCenter:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = HipCenter });
          break;
        case JointType.HipLeft:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = HipLeft });
          break;
        case JointType.HipRight:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = HipRight });
          break;
        case JointType.KneeLeft:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = KneeLeft });
          break;
        case JointType.KneeRight:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = KneeRight });
          break;
        case JointType.AnkleLeft:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = AnkleLeft });
          break;
        case JointType.AnkleRight:
          TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = AnkleRight });
          break;
        default:// other types not of interest
          break; 
      }


    }
  }
}
