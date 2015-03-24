using Microsoft.Win32;
using SkeletonTracing.DTW;
using SkeletonTracing.Model;
using System.Windows;
using System.Windows.Controls;

namespace SkeletonTracing.View {
  public partial class DTWMain : UserControl {
    private KinectManager kinect;
    private BodyManager bodyManager;

    public KinectManager Kinect { set { kinect = value; } }
    public BodyManager BodyManager { set { 
        bodyManager = value;
        skeletonCanvas.BodyManager = bodyManager;
      }
    }

    public DTWMain() {
      InitializeComponent();
    }

    private void StartRecordingBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Start();
    }

    private void StopRecordingBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Stop();
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

    private void DTWBtn_Click(object sender, RoutedEventArgs e) {
      Computation computation = new Computation();
      float cost = computation.ComputeDTW(bodyManager.BodyData, bodyManager.SampleData);
      System.Windows.MessageBox.Show(cost.ToString());
    }
  }
}
