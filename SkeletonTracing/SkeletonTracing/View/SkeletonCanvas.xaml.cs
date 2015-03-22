using SkeletonTracing.Helper;
using SkeletonTracing.Model;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SkeletonTracing {
  public partial class SkeletonCanvas : UserControl {
    private int centerX = 250;
    private int centerY = 250;

    private BodyManager bodyManager;
    public BodyManager BodyManager {
      set {
        bodyManager = value;
        bodyManager.BodyManagerEventHandl += BodyManagerEventHandler;
      }
    }

    public SkeletonCanvas() {
      InitializeComponent();
      DrawCoordinateAxisCenter();
    }
    
    private void BodyManagerEventHandler(object sender, BodyManagerEventArgs e) {
      Body body = e.Body;

      this.Dispatcher.Invoke((Action)(() => {
        canvas.Children.Clear();
        DrawCoordinateAxisCenter();
        DrawJoints(body.Joints);
      }));
    }

    private void DrawCoordinateAxisCenter() {
      Line xAxis = new Line {
        X1 = centerX,
        Y1 = centerY,
        X2 = centerX + 30,
        Y2 = centerY,
        StrokeThickness = 1,
        Stroke = new SolidColorBrush(Colors.Yellow)
      };

      Line yAxis = new Line {
        X1 = centerX,
        Y1 = centerY,
        X2 = centerX,
        Y2 = centerY - 30,
        StrokeThickness = 1,
        Stroke = new SolidColorBrush(Colors.Blue)
      };

      canvas.Children.Add(xAxis);
      canvas.Children.Add(yAxis);
    }

    private void DrawJoints(JointSkeleton jointSkeleton) {
      Joint centerJoint = jointSkeleton.GetJoint(JointName.HipCenter);
      DrawPoint(centerX, centerY);

      foreach (JointName jointType in Enum.GetValues(typeof(JointName)).Cast<JointName>()) {
        Joint joint = jointSkeleton.GetJoint(jointType);

        if (joint == null) continue;
        
        double x = joint.XCoord - centerJoint.XCoord;
        double y = joint.YCoord - centerJoint.YCoord;

        DrawPoint(centerX - x * 200, centerY - y * 200);
      }
    }

    private void DrawLine(double x1, double y1, double x2, double y2) {
      Line line = new Line {
        X1 = x1,
        Y1 = y1,
        X2 = x2,
        Y2 = y2,
        StrokeThickness = 2,
        Stroke = new SolidColorBrush(Colors.GreenYellow)
      };
      canvas.Children.Add(line);
    }

    private void DrawPoint(double x, double y) {
      Ellipse point = new Ellipse {
        Width = 10,
        Height = 10,
        Fill = new SolidColorBrush(Colors.Red)
      };

      Canvas.SetLeft(point, x - point.Width / 2);
      Canvas.SetTop(point, y - point.Height / 2 );
    
      canvas.Children.Add(point);
      canvas.InvalidateVisual();
    }
  }
}
