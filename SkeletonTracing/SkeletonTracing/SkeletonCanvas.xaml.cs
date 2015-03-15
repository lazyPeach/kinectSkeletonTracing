using SkeletonTracing.Model;
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

namespace SkeletonTracing {
  /// <summary>
  /// Interaction logic for SkeletonCanvas.xaml
  /// </summary>
  public partial class SkeletonCanvas : UserControl {
    private SkeletonManager skeletonManager;
    public SkeletonManager SkeletonManager {
      set {
        skeletonManager = value;
      }
    }

    public SkeletonCanvas() {
      InitializeComponent();
      DrawSimpleLine();
    }

    public void DrawSimpleLine() {
      Line line = new Line {
        X1 = 10,
        Y1 = 10,
        X2 = 30,
        Y2 = 30,
        StrokeThickness = 2,
        Stroke = new SolidColorBrush(Colors.GreenYellow)
      };

      canvas.Children.Add(line);
    }
  }
}
