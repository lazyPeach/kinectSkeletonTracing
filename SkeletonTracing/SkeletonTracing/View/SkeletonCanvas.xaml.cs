using SkeletonTracing.Events;
using SkeletonTracing.Helper;
using SkeletonTracing.Model;
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
        bodyManager.PlayEventHandler += PlayEventHandler;
      }
    }

    public SkeletonCanvas() {
      InitializeComponent();
    }
    
    private void RealTimeEventHandler(object sender, BodyManagerRealTimeEventArgs e) {
      Body body = e.Body;

      this.Dispatcher.Invoke((Action)(() => { // needed in order to draw from any thread
        templateCanvas.Children.Clear();
        DrawJoints(body.Joints, templateCanvas);
      }));
    }

    private void PlayEventHandler(object sender, BodyManagerPlayEventArgs e) {
      Body template = e.TemplateBody;
      Body sample = e.SampleBody;

      this.Dispatcher.Invoke((Action)(() => { // needed in order to draw from any thread
        templateCanvas.Children.Clear();
        sampleCanvas.Children.Clear();
        DrawJoints(template.Joints, templateCanvas);
        DrawJoints(sample.Joints, sampleCanvas);
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
        Fill = new SolidColorBrush(Colors.Red)
      };

      Canvas.SetLeft(point, x - point.Width / 2);
      Canvas.SetTop(point, y - point.Height / 2);
      canvas.Children.Add(point);
    }
  }
}
