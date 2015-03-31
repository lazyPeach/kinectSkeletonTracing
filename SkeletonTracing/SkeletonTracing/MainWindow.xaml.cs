using DynamicTimeWarping;
using SkeletonModel.Managers;
using System.Windows;

namespace SkeletonTracing {
  public partial class MainWindow : Window {
    //private KinectManager kinect;
    private BodyManager bodyManager;
    private Computation computation;

    public MainWindow() {
      InitializeComponent();
      
      //kinect = new KinectManager();
      bodyManager = new BodyManager(/*kinect*/ null);
      computation = new Computation();

      //mainDTW.Kinect = kinect;
      mainDTW.BodyManager = bodyManager;
      mainDTW.Computation = computation;

      graphicDTW.BodyManager = bodyManager;
      graphicDTW.Computation = computation;
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
