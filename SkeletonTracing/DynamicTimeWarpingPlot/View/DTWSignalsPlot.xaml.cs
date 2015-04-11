using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DynamicTimeWarpingPlot.View {
  public partial class DTWSignalsPlot : UserControl {
    public DTWSignalsPlot() {
      InitializeComponent();
    }

    public void Update(float[] templateSignal, float[] sampleSignal) {
      ClearPlots();

      // Horizontal unit is the horizontal distance between 2 samples. The signal with more samples
      // will be stretched from left to right while the signal with less samples will be shown on a
      // certain ratio of the sample.
      float horizontalUnit = (float)templateCanvas.ActualWidth /
                        Math.Max(templateSignal.Length, sampleSignal.Length);

      Tuple<double, double> templateTup = PlotSignalAndGetExtremities(templateSignal, horizontalUnit, templateCanvas);
      Tuple<double, double> sampleTup = PlotSignalAndGetExtremities(sampleSignal, horizontalUnit, sampleCanvas);

      double templateMin = templateTup.Item1
           , templateMax = templateTup.Item2
           , sampleMin = sampleTup.Item1
           , sampleMax = sampleTup.Item2;

      UpdateLimits(templateMax, templateMin, sampleMax, sampleMin);
    }


    private void ClearPlots() {
      templateCanvas.Children.Clear();
      sampleCanvas.Children.Clear();
    }

    private Tuple<double, double> PlotSignalAndGetExtremities(float[] signal, float sampleDistance, Canvas canvas) {
      double min =  1f / 0f
           , max = -1f / 0f;

      for (int i = 1; i < signal.Length; i++) {
        DrawPoint(i * sampleDistance, 75 - signal[i] * 30, canvas);

        min = (signal[i] < min) ? signal[i] : min;
        max = (signal[i] > max) ? signal[i] : max;
      }

      return new Tuple<double, double>(min, max);
    }

    private void DrawPoint(double x, double y, Canvas canvas) {
      Ellipse point = new Ellipse {
        Width = 2,
        Height = 2,
        Fill = new SolidColorBrush(Colors.Red)
      };

      Canvas.SetLeft(point, x - point.Width / 2);
      Canvas.SetTop(point, y - point.Height / 2);
      canvas.Children.Add(point);
    }

    private void UpdateLimits(double templateUpperLimit, double templateLowerLimit
                            , double sampleUpperLimit, double sampleLowerLimit) {
      templateUpperLimitLbl.Content = templateUpperLimit.ToString();
      templateLowerLimitLbl.Content = templateLowerLimit.ToString();
      sampleUpperLimitLbl.Content = sampleUpperLimit.ToString();
      sampleLowerLimitLbl.Content = sampleLowerLimit.ToString();
    }
  }
}
