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

namespace SkeletonTracing.View {
  /// <summary>
  /// Interaction logic for DTWMatrixPlot.xaml
  /// </summary>
  public partial class DTWMatrixPlot : UserControl {
    private float horizontalUnit;

    public DTWMatrixPlot() {
      InitializeComponent();
    }

    public void Clear() {
      matrixCanvas.Children.Clear();
      templateCanvas.Children.Clear();
      sampleCanvas.Children.Clear();
    }

    public void Update(float[][] matrix, float[] templateSignal, float[] sampleSignal) {
      Clear();
      UpdateSignals(templateSignal, sampleSignal);
      PlotMatrix(matrix);
    }

    private void UpdateSignals(float[] templateSignal, float[] sampleSignal) {
      horizontalUnit = (float)templateCanvas.ActualWidth /
                              Math.Max(templateSignal.Length, sampleSignal.Length);

      for (int i = 1; i < templateSignal.Length; i++) {
        DrawPoint(i * horizontalUnit, 70 - templateSignal[i] * 30, templateCanvas);
      }

      for (int i = 1; i < sampleSignal.Length; i++) {
        DrawPoint(i * horizontalUnit, 75 - sampleSignal[i] * 30, sampleCanvas);
      }
    }

    private void DrawPoint(double x, double y, Canvas canvas) {
      Ellipse point = new Ellipse {
        Width = 2,
        Height = 2,
        Fill = new SolidColorBrush(Colors.Blue)
      };

      Canvas.SetLeft(point, x - point.Width / 2);
      Canvas.SetTop(point, y - point.Height / 2);
      canvas.Children.Add(point);
    }

    private void PlotMatrix(float[][] matrix) {
      int height = matrix.Length;
      int width = matrix[0].Length;

      double min = 1f / 0f, max = -1f / 0f;

      for (int i = 0; i < height; i++) {
        for (int j = 0; j < width; j++) {
          min = (matrix[i][j] < min) ? matrix[i][j] : min;
          max = (matrix[i][j] > max) ? matrix[i][j] : max;
        }
      }

      // normalize the matrix for show... divide everything by max => max element will be 1
      // put result as opacity
      for (int i = 0; i < height; i++) {
        for (int j = 0; j < width; j++) {
          Rectangle dtwSquare = new Rectangle {
            Width = horizontalUnit,
            Height = horizontalUnit,
            Fill = new SolidColorBrush(Colors.Red),
            Opacity = matrix[i][j] / max
          };

          Canvas.SetLeft(dtwSquare , j * horizontalUnit);
          Canvas.SetBottom(dtwSquare, i * horizontalUnit);
          matrixCanvas.Children.Add(dtwSquare);
        }
      }


      

      //Rectangle rect2 = new Rectangle {
      //  Width = horizontalUnit,
      //  Height = horizontalUnit,
      //  Fill = new SolidColorBrush(Colors.Blue),
      //};

      //Rectangle rect3 = new Rectangle {
      //  Width = horizontalUnit,
      //  Height = horizontalUnit,
      //  Fill = new SolidColorBrush(Colors.Black),
      //};



      //Canvas.SetLeft(rect2, 10);
      //Canvas.SetBottom(rect2, 0);
      //matrixCanvas.Children.Add(rect2);

      //Canvas.SetLeft(rect3, 20);
      //Canvas.SetBottom(rect3, 10);
      //matrixCanvas.Children.Add(rect3);

    }
  }
}
