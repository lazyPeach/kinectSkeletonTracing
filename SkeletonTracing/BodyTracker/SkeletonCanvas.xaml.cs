using GestureDetector;
using Helper;
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
  /// <summary>
  /// Interaction logic for SkeletonCanvas.xaml
  /// </summary>
  public partial class SkeletonCanvas : UserControl {
    public BodyManager BodyManager {
      set {
        bodyManager = value;
        bodyManager.RealTimeEventHandler += RealTimeEventHandler;
      }
    }

    public SkeletonCanvas() {
      InitializeComponent();
      DrawLine(1,1,1,1,templateCanvas);
    }

    public void Clear() {
      templateCanvas.Children.Clear();
    }

    private void RealTimeEventHandler(object sender, BodyManagerEventArgs e) {
      Body body = e.Body;

      this.Dispatcher.Invoke((Action)(() => { // needed in order to draw from any thread
        templateCanvas.Children.Clear();

        Joint centerJoint = body.JointSkeleton.GetJoint(JointName.HipCenter);
        centerJointX = centerJoint.XCoord;
        centerJointY = centerJoint.YCoord;

        DrawJoints(body.JointSkeleton, templateCanvas);
        DrawBones(body.JointSkeleton, templateCanvas);
      }));
    }

    private void DrawBones(JointSkeleton jointSkeleton, Canvas canvas) {
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName)).Cast<BoneName>()) {
        Tuple<JointName, JointName> boneExtremities =  Mapper.BoneJointMap[boneName];
        Joint startJoint = jointSkeleton.GetJoint(boneExtremities.Item1);
        Joint endJoint = jointSkeleton.GetJoint(boneExtremities.Item2);

        if (startJoint == null || endJoint == null) continue;

        double x1 = startJoint.XCoord - centerJointX;
        double y1 = startJoint.YCoord - centerJointY;
        double x2 = endJoint.XCoord - centerJointX;
        double y2 = endJoint.YCoord - centerJointY;

        DrawLine(centerX + x1 * 200, centerY - y1 * 200, centerX + x2 * 200, centerY - y2 * 200, canvas); // have a mirror display
      }
    }

    private void DrawJoints(JointSkeleton jointSkeleton, Canvas canvas) {
      foreach (JointName jointType in Enum.GetValues(typeof(JointName)).Cast<JointName>()) {
        Joint joint = jointSkeleton.GetJoint(jointType);

        if (joint == null) continue;

        double x = joint.XCoord - centerJointX;
        double y = joint.YCoord - centerJointY;

        DrawPoint(centerX + x * 200, centerY - y * 200, canvas); // have a mirror display
      }
    }

    private void DrawPoint(double x, double y, Canvas canvas) {
      Ellipse point = new Ellipse {
        Width = 10,
        Height = 10,
        Fill = new SolidColorBrush(Colors.Yellow)
      };

      Canvas.SetLeft(point, x - point.Width / 2);
      Canvas.SetTop(point, y - point.Height / 2);
      canvas.Children.Add(point);
    }

    private void DrawLine(double x1, double y1, double x2, double y2, Canvas canvas) {
      Line myLine = new Line();
      myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
      myLine.X1 = x1;
      myLine.X2 = x2;
      myLine.Y1 = y1;
      myLine.Y2 = y2;
      myLine.HorizontalAlignment = HorizontalAlignment.Left;
      myLine.VerticalAlignment = VerticalAlignment.Top;
      myLine.StrokeThickness = 2;
      canvas.Children.Add(myLine);
    }

    private int centerX = 210;
    private int centerY = 210;
    private float centerJointX;
    private float centerJointY;
    private BodyManager bodyManager;
  }
}
