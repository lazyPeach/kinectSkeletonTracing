using SkeletonTracing.Model;
using System.Windows;

namespace SkeletonTracing {
  public partial class MainWindow : Window {
    private KinectManager kinect;
    private BodyManager bodyManager;

    public MainWindow() {
      InitializeComponent();
      
      kinect = new KinectManager();
      bodyManager = new BodyManager(kinect);

      mainDTW.Kinect = kinect;
      mainDTW.BodyManager = bodyManager;
    }

    private void MainDTWMenu_Click(object sender, RoutedEventArgs e) {
      mainDTW.Visibility = System.Windows.Visibility.Visible;
      graphicDTW.Visibility = System.Windows.Visibility.Hidden;
    }

    private void GraphicsDTWMenu_Click(object sender, RoutedEventArgs e) {
      mainDTW.Visibility = System.Windows.Visibility.Hidden;
      graphicDTW.Visibility = System.Windows.Visibility.Visible;
    }

  }
}
