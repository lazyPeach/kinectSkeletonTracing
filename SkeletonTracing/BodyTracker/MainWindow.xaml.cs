using GestureDetector;
using GestureDetector.Events;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BodyTracker {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();

      kinect = new KinectManager();
      bodyManager = new BodyManager(kinect);

      initialComputer = new InitialPositionComputer(bodyManager);
      initialComputer.InitialPositionEventHandler += InitialPositionEventHandler;

      gestureDatabase = new GestureDatabase();
      gestureDatabase.LoadDB();

      gestureComputer = new GestureComputer(bodyManager, initialComputer);
      gestureComputer.RecognizedGestureEventHandler += RecognizedGestureEventHandler;

      bodyManagerExt = new BodyManagerExtended();
      record = new Queue<Body>();

      skeletonCanvas.BodyManager = bodyManager;
      gesturesCombo.ItemsSource = gestureDatabase.GetAllGestures();
    }

    private int count = 0;

    private void RecognizedGestureEventHandler(object sender, RecognizedGestureEventArgs e) {
      count++;
      countLabel.Content = count.ToString();
    }

    // check for events from InitialPositionComputer
    private void InitialPositionEventHandler(object sender, InitialPositionEventArgs e) {
      switch (e.State) {
        case State.Pause:
          UpdateTimerBackground(Colors.Red);
          UpdateTimerLabel(e.Timer);
          break;
        case State.Finish:
          UpdateTimerBackground(Colors.White);
          UpdateTimerLabel(0);
          break;
        case State.Recording:
          UpdateTimerBackground(Colors.Green);
          UpdateTimerLabel(e.Timer);
          break;
        case State.Check:
          if (e.IsInitialPosition) {
            stateRectangle.Fill = new SolidColorBrush(Colors.Green);
          } else {
            stateRectangle.Fill = new SolidColorBrush(Colors.Red);
          }
          
          break;
      }
    }

    private void startTestInitialPositionBtn_Click(object sender, RoutedEventArgs e) {
      //initialComputer.InitialPositionEventHandler += InitialPositionEventHandler;
      initialComputer.TestInitialPosition();
      kinect.Start();
    }

    private void stopTestInitialPositionBtn_Click(object sender, RoutedEventArgs e) {
      //initialComputer.InitialPositionEventHandler -= InitialPositionEventHandler;
      kinect.Stop();
      stateRectangle.Fill = new SolidColorBrush(Colors.Gray);
    }

    private void UpdateTimerLabel(int time) {
      this.Dispatcher.Invoke((Action)(() => { // update label
        timerLbl.Content = time.ToString();
      }));
    }

    private void UpdateTimerBackground(Color color) {
      this.Dispatcher.Invoke((Action)(() => { // update label
        timerLbl.Background = new SolidColorBrush(color);
      }));
    }

    private void addNewGesture_Click(object sender, RoutedEventArgs e) {
      kinect.Start();
      
      gestureDatabase.AddGesture(newGestureNameTxt.Text);
      gestureDatabase.SaveDB();

      MessageBox.Show("You will have to perform the gesture 5 times in order to train the system");

      GestureRecorder gestureRecorder = new GestureRecorder(bodyManager, initialComputer, gestureDatabase.GestureDB[newGestureNameTxt.Text]);
      gestureRecorder.StartRecording();
    }

    private void gesturesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      string gestureName = (string)gesturesCombo.SelectedItem;
      gestureComputer.LoadGesture(gestureDatabase.GestureDB[gestureName]);
    }

    private void startGestureRecognitionBtn_Click(object sender, RoutedEventArgs e) {
      kinect.Start();
      gestureComputer.StartRecognition();
    }

    private void stopGestureRecognitionBtn_Click(object sender, RoutedEventArgs e) {

    }

    private GestureDatabase gestureDatabase;
    private Queue<Body> record;
    private KinectManager kinect;
    private BodyManager bodyManager;
    private InitialPositionComputer initialComputer;
    private BodyManagerExtended bodyManagerExt;
    private GestureComputer gestureComputer;

  }
}
