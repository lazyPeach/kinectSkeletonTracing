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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkeletonTracing {
  public partial class MainWindow : Window {
    private KinectManager kinect;
    private SkeletonManager skeletonManager;

    public MainWindow() {
      InitializeComponent();

      kinect = new KinectManager();
      skeletonManager = new SkeletonManager(kinect);
      TableView.SkeletonManager = skeletonManager;
    }

    private void StartRecordingBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Start();
    }
  }
}
