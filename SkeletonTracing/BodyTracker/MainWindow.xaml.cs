using GestureDetector;
using SkeletonModel.Events;
using SkeletonModel.Managers;
using SkeletonModel.Model;
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

namespace BodyTracker {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();

      kinect = new KinectManager();
      bodyManager = new BodyManager(kinect);
      initialComputer = new InitialComputer();
      bodyManagerExt = new BodyManagerExtended();
      record = new Queue<Body>();

      bodyManager.RealTimeEventHandler += RealTimeEventHandler;

      skeletonCanvas.BodyManager = bodyManager;
    }

    private void RealTimeEventHandler(object sender, BodyManagerEventArgs e) {
      if (initialComputer.InitialPosition == null) {
        initialComputer.DefineInitialPosition(e.Body);
      }

      Body body = e.Body;
      if (initialComputer.IsInitialPosition(body)) {
        stateRectangle.Fill = new SolidColorBrush(Colors.Green);
        if (record.Count > 30) { // consider each geesture with less than 30 samples incorrect
          //bodyManagerExt.Records.Enqueue(record);
          if (gestureComputer.IsBothHandsRaise(record.ToArray<Body>())) {
            count++;
            countLabel.Content = count.ToString();
          }
          record = new Queue<Body>();
        }
      } else {
        stateRectangle.Fill = new SolidColorBrush(Colors.Red);
        record.Enqueue(body);
      }
    }


    private Queue<Body> record;
    private KinectManager kinect;
    private BodyManager bodyManager;
    private InitialComputer initialComputer;
    private BodyManagerExtended bodyManagerExt;
    private GestureComputer gestureComputer = new GestureComputer();
    private int count = 0;

    private void startBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Start();
    }

    private void stopBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Stop();
    }

    private void raiseBothHandsRadio_Checked(object sender, RoutedEventArgs e) {
      gestureComputer.CleanGestureDB();
      gestureComputer.LoadGestureDB(Gesture.RaiseBothHands);
    }

    private void raiseRightHandRadio_Checked(object sender, RoutedEventArgs e) {
      gestureComputer.CleanGestureDB();
      gestureComputer.LoadGestureDB(Gesture.RaiseRightHand);
    }

    private void squatRadio_Checked(object sender, RoutedEventArgs e) {
      gestureComputer.CleanGestureDB();
      gestureComputer.LoadGestureDB(Gesture.Squat);
    }

    private void raiseLeftHandRadio_Checked(object sender, RoutedEventArgs e) {
      gestureComputer.CleanGestureDB();
      gestureComputer.LoadGestureDB(Gesture.RaiseLeftHand);
    }
  }
}
