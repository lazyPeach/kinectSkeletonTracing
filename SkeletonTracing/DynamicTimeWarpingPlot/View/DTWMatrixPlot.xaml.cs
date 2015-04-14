using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DynamicTimeWarpingPlot.View {
  public partial class DTWMatrixPlot : UserControl {
    public DTWMatrixPlot() {
      InitializeComponent();
    }

    public void DrawSignals(float[] templateSignal, float[] sampleSignal) {
      ClearSignalCanvas();

      horizontalUnit = (float)templateCanvas.ActualWidth /
                       Math.Max(templateSignal.Length, sampleSignal.Length);

      DrawSignal(templateSignal, templateCanvas);
      DrawSignal(sampleSignal, sampleCanvas);
    }

    public void DrawMatrix(float[][] matrix) {
      DrawingVisual drawingVisual = new DrawingVisual();
      DrawingContext drawingContext = drawingVisual.RenderOpen();

      int height = matrix.Length;
      int width = matrix[0].Length;

      double min = 1f / 0f, max = -1f / 0f;

      for (int i = 0; i < height; i++) {
        for (int j = 0; j < width; j++) {
          if (matrix[i][j] != 1f / 0f) {
            min = (matrix[i][j] < min) ? matrix[i][j] : min;
            max = (matrix[i][j] > max) ? matrix[i][j] : max;
          }
        }
      }

      double opacity = max - min / (double)256;

      // Alpha in argb should be between 0 and 255 => map [min, max] to [0, 255]
      for (int i = 0; i < height; i++) {
        for (int j = 0; j < width; j++) {
          Rect rect = new Rect(i * horizontalUnit, j * horizontalUnit, horizontalUnit, horizontalUnit);
          if (matrix[i][j] != 1f / 0f) {
            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb((byte)(matrix[i][j] * 255 / max), 255, 255, 0)), null, rect);
          } else {
            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)), null, rect);
          }
        }
      }

      drawingContext.Close();
      RenderTargetBitmap renderBmp = new RenderTargetBitmap(400, 400, 96d, 96d, PixelFormats.Pbgra32);
      renderBmp.Render(drawingVisual);
      plotImage.Source = renderBmp;
    }

    public void DrawShortestPath(List<Tuple<int, int>> shortestPath) {
      DrawingVisual drawingVisual = new DrawingVisual();
      DrawingContext drawingContext = drawingVisual.RenderOpen();

      foreach (Tuple<int, int> elem in shortestPath) {
        Rect rect = new Rect(elem.Item1 * horizontalUnit, elem.Item2 * horizontalUnit, horizontalUnit, horizontalUnit);
        drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)), null, rect);
      }

      drawingContext.Close();

      RenderTargetBitmap renderBmp = new RenderTargetBitmap(400, 400, 96d, 96d, PixelFormats.Pbgra32);
      renderBmp.Render(drawingVisual);
      shortestPathImage.Source = renderBmp;
    }

    public void UpdateCost(double cost) {
      costLbl.Content = cost.ToString();
    }

    private void ClearSignalCanvas() {
      templateCanvas.Children.Clear();
      sampleCanvas.Children.Clear();
    }

    private void DrawSignal(float[] signal, Canvas canvas) {
      for (int i = 1; i < signal.Length; i++) {
        DrawPoint(i * horizontalUnit, 70 - signal[i] * 30, canvas);
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


    private float horizontalUnit;
  }
}
