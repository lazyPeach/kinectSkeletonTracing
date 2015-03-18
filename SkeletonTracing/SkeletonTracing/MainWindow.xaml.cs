using Microsoft.Kinect;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

    private void StartRecordingBtn_Click(object sender, RoutedEventArgs e) {
      //skeletonManager.Start();
      kinect.Start();
    }

    private void StopRecordingBtn_Click(object sender, RoutedEventArgs e) {
      //skeletonManager.Stop();
      kinect.Stop();
    }

    private void SaveGestureBtn_Click(object sender, RoutedEventArgs e) {
      FolderBrowserDialog fileBrowser = new FolderBrowserDialog();
      fileBrowser.ShowDialog();
      //skeletonManager.SaveCollections(fileBrowser.SelectedPath);
    }

    private void LoadGestureBtn_Click(object sender, RoutedEventArgs e) {
      FolderBrowserDialog fileBrowser = new FolderBrowserDialog();
      fileBrowser.ShowDialog();
      //skeletonManager.LoadCollections(fileBrowser.SelectedPath);
    }
  }
}
