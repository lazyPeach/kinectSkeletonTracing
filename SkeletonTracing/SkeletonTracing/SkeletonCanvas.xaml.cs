using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SkeletonTracing {
  /// <summary>
  /// Interaction logic for SkeletonCanvas.xaml
  /// </summary>
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
      canvas.Children.Clear();
      DrawCoordinateAxisCenter();
      DrawJoints(body.Joints);
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
      Joint centerJoint = jointSkeleton.GetJoint(Microsoft.Kinect.JointType.HipCenter);
      DrawPoint(centerX, centerY);
      foreach (Microsoft.Kinect.JointType jointType in Enum.GetValues(typeof(Microsoft.Kinect.JointType)).Cast<Microsoft.Kinect.JointType>()) {
        Joint joint = jointSkeleton.GetJoint(jointType);

        if (joint == null) continue;
        
        double x = joint.XCoord - centerJoint.XCoord;
        double y = joint.YCoord - centerJoint.YCoord;

        DrawPoint(centerX - x * 200, centerY - y * 200);

      }

      //Console.WriteLine((int)(crtJoint.XCoord * 100) + " " + (int)(crtJoint.YCoord * 100));
      //DrawPoint((int)(Math.Abs(crtJoint.XCoord) * 100), (int)(Math.Abs(crtJoint.YCoord) * 100));
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
    }









    /*
    

    public SkeletonCanvas() {
      InitializeComponent();
      DrawCoordinateAxisCenter();
      //DrawLine();
      //DrawPoint();
    }

    private void KinectManagerEventHandle(object sender, KinectManagerEventArgs e) {
      Skeleton skeleton = e.Skeleton;
      NormalizeSkeleton(skeleton);

      //DrawSkeleton(skeleton);
    }

    // consider HipCenter as the center of the coordinate axis
    private void NormalizeSkeleton(Skeleton skeleton) {
      float xOffset = skeleton.Joints[JointType.HipCenter].Position.X;
      float yOffset = skeleton.Joints[JointType.HipCenter].Position.Y;
      float zOffset = skeleton.Joints[JointType.HipCenter].Position.Z;

      //skeleton.Joints[JointType.HipCenter].Position.X -= xOffset; 
    }

    private void DrawSkeleton(Skeleton skeleton) {
      foreach (JointType joint in Enum.GetValues(typeof(JointType))) {

      }
    }







    */


















    /*

    

    private void DrawPoint() {
      Ellipse point = new Ellipse {
        Width = 10,
        Height = 10,
        Fill = new SolidColorBrush(Colors.Red)
      };

      Canvas.SetLeft(point, 70/*joint.Position.X - ellipse.Width / 2*///);
      //Canvas.SetTop(point, /*joint.Position.Y - ellipse.Height / 2*/ 70);
    /*
      canvas.Children.Add(point);
    }

    */
  }
}
