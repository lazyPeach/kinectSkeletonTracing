using DynamicTimeWarping;
using Helper;
using SkeletonModel.Managers;
using System.Windows.Controls;

namespace DynamicTimeWarpingPlot.View {
  public partial class MultipleSamples : UserControl {
    public MultipleSamples() {
      InitializeComponent();
    }

    public Computation Computation { get { return computation; } set { computation = value; } }
    public BodyManager BodyManager { get { return bodyManager; } set { bodyManager = value; } }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      BoneName boneName = (BoneName)boneCombo.SelectedItem;
      int selectedIndex = boneComponentCombo.SelectedIndex;

      //if (computation.Result != null) {

      //  float[] templateSignal = computation.Result.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[selectedIndex];
      //  float[] sampleSignal = computation.Result.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[selectedIndex];
      //  float[][] dtwMatrix = computation.Result.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[selectedIndex];
      //  float[][] dtwWindowMatrix = computation.Result.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[selectedIndex];
      //  DTWCost greedyCost = computation.Result.Data[Mapper.BoneIndexMap[boneName]].GreedyCost[selectedIndex];
      //  DTWCost windowCost = computation.Result.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost[selectedIndex];
      //  DTWCost bestCost = computation.Result.Data[Mapper.BoneIndexMap[boneName]].BestCost[selectedIndex];

      //  float[] filteredTemplateSignal = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].TemplateSignal[selectedIndex];
      //  float[] filteredSampleSignal = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].SampleSignal[selectedIndex];
      //  float[][] filteredDTWMatrix = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].DTWMatrix[selectedIndex];
      //  float[][] filteredDTWWindowMatrix = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].DTWWindowMatrix[selectedIndex];
      //  DTWCost filteredGreedyCost = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].GreedyCost[selectedIndex];
      //  DTWCost filteredWindowCost = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].GreedyWindowCost[selectedIndex];
      //  DTWCost filteredBestCost = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[selectedIndex];

      //  signalPlot.PlotSignals(templateSignal, sampleSignal);
      //  signalPlot.PlotFilteredSignals(filteredTemplateSignal, filteredSampleSignal);

      //  matrixPlot.DrawSignals(templateSignal, sampleSignal);
      //  matrixPlot.DrawMatrix(dtwMatrix);
      //  matrixPlot.DrawShortestPath(greedyCost.ShortestPath);
      //  matrixPlot.UpdateCost(greedyCost.Cost);

      //  filteredMatrixPlot.DrawSignals(filteredTemplateSignal, filteredSampleSignal);
      //  filteredMatrixPlot.DrawMatrix(filteredDTWMatrix);
      //  filteredMatrixPlot.DrawShortestPath(filteredGreedyCost.ShortestPath);
      //  filteredMatrixPlot.UpdateCost(filteredGreedyCost.Cost);

      //  windowMatrixPlot.DrawSignals(templateSignal, sampleSignal);
      //  windowMatrixPlot.DrawMatrix(dtwWindowMatrix);
      //  windowMatrixPlot.DrawShortestPath(windowCost.ShortestPath);
      //  windowMatrixPlot.UpdateCost(windowCost.Cost);

      //  filteredWindowMatrixPlot.DrawSignals(filteredTemplateSignal, filteredSampleSignal);
      //  filteredWindowMatrixPlot.DrawMatrix(filteredDTWWindowMatrix);
      //  filteredWindowMatrixPlot.DrawShortestPath(filteredWindowCost.ShortestPath);
      //  filteredWindowMatrixPlot.UpdateCost(filteredWindowCost.Cost);

      //  bestMatrixPlot.DrawSignals(templateSignal, sampleSignal);
      //  bestMatrixPlot.DrawMatrix(dtwWindowMatrix);
      //  bestMatrixPlot.DrawShortestPath(bestCost.ShortestPath);
      //  bestMatrixPlot.UpdateCost(bestCost.Cost);

      //  filteredBestMatrixPlot.DrawSignals(filteredTemplateSignal, filteredSampleSignal);
      //  filteredBestMatrixPlot.DrawMatrix(filteredDTWWindowMatrix);
      //  filteredBestMatrixPlot.DrawShortestPath(filteredBestCost.ShortestPath);
      //  filteredBestMatrixPlot.UpdateCost(filteredBestCost.Cost);
      //}
    }

    private BodyManager bodyManager;
    private Computation computation;
  }
}
