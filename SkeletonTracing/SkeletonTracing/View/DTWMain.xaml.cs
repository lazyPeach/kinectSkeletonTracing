using DynamicTimeWarping;
using Microsoft.Win32;
using SkeletonModel.Managers;
using System.Windows;
using System.Windows.Controls;

namespace SkeletonTracing.View {
  public partial class DTWMain : UserControl {
    //private KinectManager kinect;
    private BodyManager bodyManager;
    private Computation computation;

    public Computation Computation { get { return computation; } set { computation = value; } }

    /*
    public KinectManager Kinect {
      get { return kinect; }
      set { kinect = value; }
    }
    */

    public BodyManager BodyManager {
      get {
        return bodyManager;
      }

      set { 
        bodyManager = value;
        skeletonCanvas.BodyManager = bodyManager;
      }
    }

    public DTWMain() {
      InitializeComponent();
    }

    private void StartRecordingBtn_Click(object sender, RoutedEventArgs e) {
      //kinect.Start();
    }

    private void StopRecordingBtn_Click(object sender, RoutedEventArgs e) {
      //kinect.Stop();
    }

    private void SaveGestureBtn_Click(object sender, RoutedEventArgs e) {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "XML file|*.xml";
      saveFileDialog.ShowDialog();

      bodyManager.SaveCollection(saveFileDialog.OpenFile());
    }

    private void LoadGestureBtn_Click(object sender, RoutedEventArgs e) {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "XML file|*.xml";
      openFileDialog.ShowDialog();

      bodyManager.LoadCollection(openFileDialog.OpenFile());
    }

    private void LoadSampleBtn_Click(object sender, RoutedEventArgs e) {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "XML file|*.xml";
      openFileDialog.ShowDialog();

      bodyManager.LoadSample(openFileDialog.OpenFile());
    }

    private void ClearDataBtn_Click(object sender, RoutedEventArgs e) {
      bodyManager.ClearData();
    }
    
    private void PlayGestureBtn_Click(object sender, RoutedEventArgs e) {
      bodyManager.PlayGesture();
    }
  }
}
