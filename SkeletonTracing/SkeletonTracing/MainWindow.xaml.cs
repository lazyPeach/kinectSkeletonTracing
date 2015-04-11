using SkeletonModel.Managers;
using System.Windows;
using System.Windows.Forms;

namespace SkeletonTracing {
  public partial class MainWindow : Window {
    private KinectManager kinect;
    private BodyManager bodyManager;

    public MainWindow() {
      InitializeComponent();
      
      kinect = new KinectManager();
      bodyManager = new BodyManager(kinect);

      skeletonCanvas.BodyManager = bodyManager;
    }

    private void startRecordingBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Start();
    }

    private void stopRecordingBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Stop();
    }

    private void saveGestureBtn_Click(object sender, RoutedEventArgs e) {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "XML file|*.xml";
      saveFileDialog.ShowDialog();

      bodyManager.SaveCollection(saveFileDialog.OpenFile());
    }

    private void clearBtn_Click(object sender, RoutedEventArgs e) {
      bodyManager.ClearData();
      skeletonCanvas.Clear();
    }

  }
}
