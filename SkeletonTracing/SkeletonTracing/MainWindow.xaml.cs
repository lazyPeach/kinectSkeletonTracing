using Microsoft.Kinect;
using SkeletonTracing.DTW;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
