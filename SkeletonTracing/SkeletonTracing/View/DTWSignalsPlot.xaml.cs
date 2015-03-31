using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SkeletonTracing.View {
  public partial class DTWPlot : UserControl {
    private float horizontalUnit;

    public DTWPlot() {
      InitializeComponent();
    }

    public void Clear() {
      templateCanvas.Children.Clear();
      sampleCanvas.Children.Clear();
    }

    public void Update(float[] templateSignal, float[] sampleSignal) {
      Clear();
      
      // Horizontal unit is the horizontal distance between 2 samples. The signal with more samples
      // will be stretched from left to right while the signal with less samples will be shown on a
      // certain ratio of the sample.
      horizontalUnit = (float)templateCanvas.ActualWidth /
                        Math.Max(templateSignal.Length, sampleSignal.Length);

      double templateMin = 1F/0F, templateMax = -1F/0F, sampleMin = 1F/0F, sampleMax = -1F/0F;

      for (int i = 1; i < templateSignal.Length; i++) {
        DrawPoint(i * horizontalUnit, 75 - templateSignal[i] * 30, templateCanvas);

        templateMin = (templateSignal[i] < templateMin) ? templateSignal[i] : templateMin;
        templateMax = (templateSignal[i] > templateMax) ? templateSignal[i] : templateMax;
      }

      for (int i = 1; i < sampleSignal.Length; i++) {
        DrawPoint(i * horizontalUnit, 75 - sampleSignal[i] * 30, sampleCanvas);

        sampleMin = (sampleSignal[i] < sampleMin) ? sampleSignal[i] : sampleMin;
        sampleMax = (sampleSignal[i] > sampleMax) ? sampleSignal[i] : sampleMax;
      }

      UpdateLimits(templateMax, templateMin, sampleMax, sampleMin);
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
