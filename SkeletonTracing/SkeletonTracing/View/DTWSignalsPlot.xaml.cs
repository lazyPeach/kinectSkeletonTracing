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
      
      // horizontal unit is the horizontal distance between 2 samples. The signal with more samples
      // will be stretched from left to right while the signal with less samples will be shown on
      // a certain ratio of the sample.
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

    public void PlotSignals( float[] templateW, float[] sampleW
                           , float[] templateX, float[] sampleX
                           , float[] templateY, float[] sampleY
                           , float[] templateZ, float[] sampleZ ) {
      
      //double plotOffset = wCanvas.ActualWidth / (double)Math.Max(templateW.Length, sampleW.Length);

      //// plot w signal
      //for (int i = 1; i < templateW.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Red;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 60 - templateW[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 60 - templateW[i] * 20;
      //  line.StrokeThickness = 1;
      //  wCanvas.Children.Add(line);
      //}

      //for (int i = 1; i < sampleW.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Yellow;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 20 - sampleW[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 20 - sampleW[i] * 20;
      //  line.StrokeThickness = 1;
      //  wCanvas.Children.Add(line);
      //}

      //// plot x signal
      //for (int i = 1; i < templateX.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Red;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 60 - templateX[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 60 - templateX[i] * 20;
      //  line.StrokeThickness = 1;
      //  xCanvas.Children.Add(line);
      //}

      //for (int i = 1; i < sampleX.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Yellow;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 20 - sampleX[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 20 - sampleX[i] * 20;
      //  line.StrokeThickness = 1;
      //  xCanvas.Children.Add(line);
      //}

      //// plot y signal
      //for (int i = 1; i < templateY.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Red;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 60 - templateY[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 60 - templateY[i] * 20;
      //  line.StrokeThickness = 1;
      //  yCanvas.Children.Add(line);
      //}

      //for (int i = 1; i < sampleY.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Yellow;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 20 - sampleY[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 20 - sampleY[i] * 20;
      //  line.StrokeThickness = 1;
      //  yCanvas.Children.Add(line);
      //}

      //// plot z signal
      //for (int i = 1; i < templateZ.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Red;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 60 - templateZ[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 60 - templateZ[i] * 20;
      //  line.StrokeThickness = 1;
      //  zCanvas.Children.Add(line);
      //}

      //for (int i = 1; i < sampleZ.Length; i++) {
      //  Line line = new Line();
      //  line.Stroke = System.Windows.Media.Brushes.Yellow;
      //  line.X1 = plotOffset * (i - 1);
      //  line.Y1 = 20 - sampleZ[i - 1] * 20;
      //  line.X2 = plotOffset * i;
      //  line.Y2 = 20 - sampleZ[i] * 20;
      //  line.StrokeThickness = 1;
      //  zCanvas.Children.Add(line);
      //}

    }



  }
}
