using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DynamicTimeWarpingPlot.View {
  public partial class DTWSignalsPlot : UserControl {
    public DTWSignalsPlot() {
      InitializeComponent();
    }

    public void PlotSignals(float[] templateSignal, float[] sampleSignal) {
      ClearPlots();

      // Horizontal unit is the horizontal distance between 2 samples. The signal with more samples
      // will be stretched from left to right while the signal with less samples will be shown on a
      // certain ratio of the sample.
      float horizontalUnit = (float)templateCanvas.ActualWidth /
                        Math.Max(templateSignal.Length, sampleSignal.Length);

      PlotSignal(templateSignal, horizontalUnit, templateCanvas, new SolidColorBrush(Colors.Red));
      PlotSignal(sampleSignal, horizontalUnit, sampleCanvas, new SolidColorBrush(Colors.Red));

      UpdateLimits(templateSignal, sampleSignal);
    }

    public void PlotFilteredSignals(float[] templateSignal, float[] sampleSignal) {
      float horizontalUnit = (float)templateCanvas.ActualWidth /
                        Math.Max(templateSignal.Length, sampleSignal.Length);

      PlotSignal(templateSignal, horizontalUnit, templateCanvas, new SolidColorBrush(Colors.Yellow));
      PlotSignal(sampleSignal, horizontalUnit, sampleCanvas, new SolidColorBrush(Colors.Yellow));
    }


    private void ClearPlots() {
      templateCanvas.Children.Clear();
      sampleCanvas.Children.Clear();
    }

    private void PlotSignal(float[] signal, float sampleDistance, Canvas canvas, SolidColorBrush color) {
      for (int i = 1; i < signal.Length; i++) {
        DrawPoint(i * sampleDistance, 75 - signal[i] * 30, canvas, color);
      }
    }

    private Tuple<double, double> GetExtremities(float[] signal) {
      double min =  1f / 0f
           , max = -1f / 0f;

      for (int i = 1; i < signal.Length; i++) {
        min = (signal[i] < min) ? signal[i] : min;
        max = (signal[i] > max) ? signal[i] : max;
      }

      return new Tuple<double, double>(min, max);
    }

    private void DrawPoint(double x, double y, Canvas canvas, SolidColorBrush color) {
      Ellipse point = new Ellipse {
        Width = 2,
        Height = 2,
        Fill = color
      };

      Canvas.SetLeft(point, x - point.Width / 2);
      Canvas.SetTop(point, y - point.Height / 2);
      canvas.Children.Add(point);
    }

    private void UpdateLimits(float[] template, float[] sample) {
      Tuple<double, double> templateTup = GetExtremities(template);
      Tuple<double, double> sampleTup = GetExtremities(sample);

      templateUpperLimitLbl.Content = templateTup.Item2.ToString();
      templateLowerLimitLbl.Content = templateTup.Item1.ToString();
      sampleUpperLimitLbl.Content = sampleTup.Item2.ToString();
      sampleLowerLimitLbl.Content = sampleTup.Item1.ToString();
    }
  }
}
