using SkeletonTracing.DTW;
using SkeletonTracing.Helper;
using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SkeletonTracing.View {
  public partial class DTWGraphic : UserControl {
    private BodyManager bodyManager;
    private Computation computation;

    public BodyManager BodyManager { get { return bodyManager; } set { bodyManager = value; } }
    public Computation Computation { get { return computation; } set { computation = value; } }

    public DTWGraphic() {
      InitializeComponent();

      boneCombo.ItemsSource = Enum.GetValues(typeof(BoneName));
      string[] boneComponents = { "W", "X", "Y", "Z" };
      boneComponentCombo.ItemsSource = boneComponents;
    }

    private void BoneCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      
    }

    private void componentName_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      BoneName boneName = (BoneName)boneCombo.SelectedItem;
      int selectedIndex = boneComponentCombo.SelectedIndex;

      if (computation.Result != null) {

        float[] templateSignal = computation.Result.Data[Mapper.indexMap[boneName]].TemplateSignal[selectedIndex];
        float[] sampleSignal = computation.Result.Data[Mapper.indexMap[boneName]].SampleSignal[selectedIndex];
        float[][] matrix = computation.Result.Data[Mapper.indexMap[boneName]].Matrix[selectedIndex];
        List<Tuple<int, int>> shortestPath = computation.Result.Data[Mapper.indexMap[boneName]].ShortestPath[selectedIndex];

        signalPlot.Update(templateSignal, sampleSignal);
        matrixPlot.Update(matrix, shortestPath, templateSignal, sampleSignal);
      }
    }

    private void showSignalsBtn_Click(object sender, System.Windows.RoutedEventArgs e) {
      signalPlot.Visibility = System.Windows.Visibility.Visible;
      matrixPlot.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void showMatrixBtn_Click(object sender, System.Windows.RoutedEventArgs e) {
      matrixPlot.Visibility = System.Windows.Visibility.Visible;
      signalPlot.Visibility = System.Windows.Visibility.Collapsed;
    }
  }
}
