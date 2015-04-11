using DynamicTimeWarping;
using Helper;
using SkeletonModel.Managers;
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

namespace DynamicTimeWarpingPlot.View {
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

    private void componentName_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      BoneName boneName = (BoneName)boneCombo.SelectedItem;
      int selectedIndex = boneComponentCombo.SelectedIndex;

      if (computation.Result != null) {
        float[] templateSignal = computation.Result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[selectedIndex];
        float[] sampleSignal = computation.Result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[selectedIndex];
        float[][] matrix = computation.Result.Data[Mapper.BoneIndexMap[boneName]].Matrix[selectedIndex];
        DTWCost cost = computation.Result.Data[Mapper.BoneIndexMap[boneName]].DTWCost[selectedIndex];
        
        signalPlot.Update(templateSignal, sampleSignal);
        matrixPlot.DrawSignals(templateSignal, sampleSignal);
        matrixPlot.DrawMatrix(matrix);
        costLbl.Content = cost.Cost.ToString();
        matrixPlot.DrawShortestPath(cost.ShortestPath);
         
      }
    }

    private void greedyShrotestPathBtn_Click(object sender, System.Windows.RoutedEventArgs e) {
      computation.ComputeGreedyDTWCost(bodyManager.BodyData.Length, bodyManager.SampleData.Length);
    }

    private void sequentialDTWBtn_Click(object sender, System.Windows.RoutedEventArgs e) {
      computation.ComputeSequentialDTWMatrix(bodyManager.BodyData, bodyManager.SampleData);
    }

    private void windowDTWBtn_Click(object sender, System.Windows.RoutedEventArgs e) {
      computation.ComputeWindowDTWMatrix(bodyManager.BodyData, bodyManager.SampleData);
    }
  }
}
