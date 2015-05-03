using GestureDetector;
using SkeletonModel.Events;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

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
      if (recordInitialPosition) {
        initialComputer.InitialPosition.Add(e.Body);
        return;
      }


      //if (initialComputer.InitialPosition == null) {
      //  initialComputer.DefineInitialPosition(e.Body);
      //}

      Body body = e.Body;
      if (initialComputer.IsInitialPosition(body)) {
        stateRectangle.Fill = new SolidColorBrush(Colors.Green);
        //if (record.Count > 50) { // consider each geesture with less than 30 samples incorrect
        //  //bodyManagerExt.Records.Enqueue(record);
        //  if (gestureComputer.IsBothHandsRaise(record.ToArray<Body>())) {
          //count++;
          //countLabel.Content = count.ToString();
        //  }
        //  record = new Queue<Body>();
        //}
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

    // during countdown record the initial position
    private void startBtn_Click(object sender, RoutedEventArgs e) {
      Thread.Sleep(5000);
      StartCountdownTimer(5);
      recordInitialPosition = true;
      kinect.Start();
    }

    private void stopBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Stop();
    }

    private void StartCountdownTimer(int sec) {
      countdownSec = sec;
      timer = new System.Timers.Timer { Interval = 1000 };
      timer.Elapsed += DelayTimerElapsed;
      timer.Start();
    }

    private void DelayTimerElapsed(object sender, System.Timers.ElapsedEventArgs e) {
      this.Dispatcher.Invoke((Action)(() => { // update label
        timerLbl.Content = countdownSec.ToString();
      }));
      
      if (countdownSec == 0) {
        recordInitialPosition = false;
        initialComputer.DefineInitialPosition();
        timer.Stop();
        return;
      }

      countdownSec--;
    }

    private System.Timers.Timer timer;
    private int countdownSec;
    private bool recordInitialPosition;
    private GestureDatabase gestureDatabase;

    private void addNewGesture_Click(object sender, RoutedEventArgs e) {
      Thread.Sleep(5000);
      StartCountdownTimer(5);
      recordInitialPosition = true;
      kinect.Start();
      
      gestureDatabase = new GestureDatabase();
      gestureDatabase.LoadDB();
      gestureDatabase.AddGesture(newGestureNameTxt.Text);
      gestureDatabase.SaveDB();

      //MessageBox.Show("You will have to perform the gesture 5 times in order to train the system");

      GestureRecorder gestureRecorder = new GestureRecorder(bodyManager, initialComputer, gestureDatabase.GestureDB[newGestureNameTxt.Text]);
      gestureRecorder.StartRecording();


    }
  }
}
