using DynamicTimeWarping;
using Helper;
using SkeletonModel.Managers;
using System;
using System.Windows.Controls;

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
        float[] filteredTemplateSignal = computation.Result.Data[Mapper.BoneIndexMap[boneName]].FilteredTemplateSignal[selectedIndex];
        float[] filteredSampleSignal = computation.Result.Data[Mapper.BoneIndexMap[boneName]].FilteredSampleSignal[selectedIndex];
        float[][] dtwMatrix = computation.Result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[selectedIndex];
        float[][] dtwWindowMatrix = computation.Result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[selectedIndex];
        DTWCost cost = computation.Result.Data[Mapper.BoneIndexMap[boneName]].GreedyCost[selectedIndex];
        DTWCost windowCost = computation.Result.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost[selectedIndex];
        DTWCost bestCost = computation.Result.Data[Mapper.BoneIndexMap[boneName]].BestCost[selectedIndex];

        signalPlot.Update(templateSignal, sampleSignal);
        signalPlot.PlotFilteredSignals(filteredTemplateSignal, filteredSampleSignal);
        matrixPlot.DrawSignals(templateSignal, sampleSignal);
        matrixPlot.DrawMatrix(dtwMatrix);
        matrixPlot.DrawShortestPath(cost.ShortestPath);
        matrixPlot.UpdateCost(cost.Cost);

        windowMatrixPlot.DrawSignals(templateSignal, sampleSignal);
        windowMatrixPlot.DrawMatrix(dtwWindowMatrix);
        windowMatrixPlot.DrawShortestPath(windowCost.ShortestPath);
        windowMatrixPlot.UpdateCost(windowCost.Cost);

        bestMatrixPlot.DrawSignals(templateSignal, sampleSignal);
        bestMatrixPlot.DrawMatrix(dtwWindowMatrix);
        bestMatrixPlot.DrawShortestPath(bestCost.ShortestPath);
        bestMatrixPlot.UpdateCost(bestCost.Cost);
      }
    }
  }
}
