using DynamicTimeWarping;
using Microsoft.Win32;
using SkeletonModel.Managers;
using System.Windows;
using System.Windows.Controls;

namespace DynamicTimeWarpingPlot.View {
  public partial class DTWMain : UserControl {
    public DTWMain() {
      InitializeComponent();
    }

    public Computation Computation { get { return computation; } set { computation = value; } }

    public BodyManager BodyManager {
      get { return bodyManager; }
      set {
        bodyManager = value;
        skeletonCanvas.BodyManager = bodyManager;
      }
    }


    private void LoadGestureBtn_Click(object sender, RoutedEventArgs e) {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "XML file|*.xml";
      openFileDialog.ShowDialog();

      bodyManager.LoadCollection(openFileDialog.OpenFile());
    }

    private void LoadSampleBtn_Click(object sender, RoutedEventArgs e) {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "XML file|*.xml";
      openFileDialog.ShowDialog();

      bodyManager.LoadSample(openFileDialog.OpenFile());
    }

    private void ClearDataBtn_Click(object sender, RoutedEventArgs e) {
      bodyManager.ClearData();
    }

    private void PlayGestureBtn_Click(object sender, RoutedEventArgs e) {
      bodyManager.PlayGesture();
    }

    private void computeResultBtn_Click(object sender, RoutedEventArgs e) {
      computation.ComputeDTW(bodyManager.BodyData, bodyManager.SampleData);
    }

    private void sumBodyCost_Click(object sender, RoutedEventArgs e) {
      MessageBox.Show("Sum body cost: " + computation.GetSumBodyCost().ToString());
    }

    private void avgBodyCost_Click(object sender, RoutedEventArgs e) {
      MessageBox.Show("Avg body cost: " + computation.GetAvgBodyCost().ToString());
    }


    private BodyManager bodyManager;
    private Computation computation;

    private void excellReportBtn_Click(object sender, RoutedEventArgs e) {
      Report report = new Report();
      report.CreateReport();
    }

    private void optimalResultBtn_Click(object sender, RoutedEventArgs e) {
      computation.ComputeOptimalDTW(bodyManager.BodyData, bodyManager.SampleData);
    }
  }
}
