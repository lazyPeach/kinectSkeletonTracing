using Helper;
using SkeletonModel.Events;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SkeletonTracing {
  public partial class SkeletonCanvas : UserControl {
    private int centerX = 210;
    private int centerY = 210;

    private BodyManager bodyManager;
    public BodyManager BodyManager {
      set {
        bodyManager = value;
        bodyManager.RealTimeEventHandler += RealTimeEventHandler;
      }
    }

    public SkeletonCanvas() {
      InitializeComponent();
    }

    public void Clear() {
      templateCanvas.Children.Clear();
    }

    private void RealTimeEventHandler(object sender, BodyManagerEventArgs e) {
      Body body = e.Body;

      this.Dispatcher.Invoke((Action)(() => { // needed in order to draw from any thread
        templateCanvas.Children.Clear();
        DrawJoints(body.Joints, templateCanvas);
      }));
    }

    private void DrawJoints(JointSkeleton jointSkeleton, Canvas canvas) {
      Joint centerJoint = jointSkeleton.GetJoint(JointName.HipCenter);
      DrawPoint(centerX, centerY, canvas);

      foreach (JointName jointType in Enum.GetValues(typeof(JointName)).Cast<JointName>()) {
        Joint joint = jointSkeleton.GetJoint(jointType);

        if (joint == null) continue;
        
        double x = joint.XCoord - centerJoint.XCoord;
        double y = joint.YCoord - centerJoint.YCoord;

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
  }
}
