using DynamicTimeWarping;
using SkeletonModel.Managers;
using System.Windows;

namespace DynamicTimeWarpingPlot {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();

      bodyManager = new BodyManager();
      computation = new Computation();

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

    private BodyManager bodyManager;
    private Computation computation;
  }
}
