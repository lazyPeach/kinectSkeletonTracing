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

      //List<string> bones = new List<string>();
      //foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
      //  bones.Add(boneName.ToString());
      //}

      //boneCombo.ItemsSource = bones;
      //bodyCenterPlot.BoneName = "Body Center";
      //neckPlot.BoneName = "Neck";
      //upperSpinePlot.BoneName = "Upper Spine";
      //lowerSpinePlot.BoneName = "Lower Spine";
      //clavicleLeftPlot.BoneName = "Left Clavicle";
      //clavicleRightPlot.BoneName = "Right Clavicle";
      //armLeftPlot.BoneName = "Left arm";
      //armRightPlot.BoneName = "Right arm";
      //forearmLeftPlot.BoneName = "Left forearm";
      //forearmRightPlot.BoneName = "Right forearm";
      //hipLeftPlot.BoneName = "Left hip";
      //hipRightPlot.BoneName = "Right hip";
      //femurusLeftPlot.BoneName = "Left femurus";
      //femurusRightPlot.BoneName = "Right femurus";
      //tibiaLeftPlot.BoneName = "Left tibia";
      //tibiaRightPlot.BoneName = "Right tibia";
    }

    public void UpdateResults() {
      //bodyCenterPlot.UpdateResult(computation.Result.Cost[0]);
      //lowerSpinePlot.UpdateResult(computation.Result.Cost[1]);
      //upperSpinePlot.UpdateResult(computation.Result.Cost[2]);
      //neckPlot.UpdateResult(computation.Result.Cost[3]);

      //clavicleLeftPlot.UpdateResult(computation.Result.Cost[4]);
      //armLeftPlot.UpdateResult(computation.Result.Cost[5]);
      //forearmLeftPlot.UpdateResult(computation.Result.Cost[6]);

      //clavicleRightPlot.UpdateResult(computation.Result.Cost[7]);
      //armRightPlot.UpdateResult(computation.Result.Cost[8]);
      //forearmRightPlot.UpdateResult(computation.Result.Cost[9]);

      //hipLeftPlot.UpdateResult(computation.Result.Cost[10]);
      //femurusLeftPlot.UpdateResult(computation.Result.Cost[11]);
      //tibiaLeftPlot.UpdateResult(computation.Result.Cost[12]);

      //hipRightPlot.UpdateResult(computation.Result.Cost[13]);
      //femurusRightPlot.UpdateResult(computation.Result.Cost[14]);
      //tibiaRightPlot.UpdateResult(computation.Result.Cost[15]);

    }
    /*
    public void Clear() {
      bodyCenterPlot.Clear();
      lowerSpinePlot.Clear();
      upperSpinePlot.Clear();
      neckPlot.Clear();

      clavicleLeftPlot.Clear();
      armLeftPlot.Clear();
      forearmLeftPlot.Clear();

      clavicleRightPlot.Clear();
      armRightPlot.Clear();
      forearmRightPlot.Clear();

      hipLeftPlot.Clear();
      femurusLeftPlot.Clear();
      tibiaLeftPlot.Clear();

      hipRightPlot.Clear();
      femurusRightPlot.Clear();
      tibiaRightPlot.Clear();
    }

    public void Plot() {
      if (bodyManager.BodyData == null || bodyManager.SampleData == null) return;

      computation.NormalizeArraysOfBones(bodyManager.BodyData, bodyManager.SampleData, out template, out sample);

      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        PlotSignals(boneName);
      }
    }

    private void PlotSignals(BoneName boneName) {
      float[] templateW = new float[template.Length];
      float[] templateX = new float[template.Length];
      float[] templateY = new float[template.Length];
      float[] templateZ = new float[template.Length];

      float[] sampleW = new float[sample.Length];
      float[] sampleX = new float[sample.Length];
      float[] sampleY = new float[sample.Length];
      float[] sampleZ = new float[sample.Length];

      for (int i = 0; i < template.Length; i++) {
        templateW[i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
        templateX[i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
        templateY[i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
        templateZ[i] = template[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;
      }

      for (int i = 0; i < sample.Length; i++) {
        sampleW[i] = sample[i].Bones.GetBone(boneName).Rotation.Quaternion.W;
        sampleX[i] = sample[i].Bones.GetBone(boneName).Rotation.Quaternion.X;
        sampleY[i] = sample[i].Bones.GetBone(boneName).Rotation.Quaternion.Y;
        sampleZ[i] = sample[i].Bones.GetBone(boneName).Rotation.Quaternion.Z;
      }

      switch (boneName) {
        case BoneName.BodyCenter:
          bodyCenterPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.LowerSpine:
          lowerSpinePlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.UpperSpine:
          upperSpinePlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.Neck:
          neckPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.ClavicleLeft:
          clavicleLeftPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.ArmLeft:
          armLeftPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.ForearmLeft:
          forearmLeftPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.ClavicleRight:
          clavicleRightPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.ArmRight:
          armRightPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.ForearmRight:
          forearmRightPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.HipLeft:
          hipLeftPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.FemurusLeft:
          femurusLeftPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.TibiaLeft:
          tibiaLeftPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.HipRight:
          hipRightPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.FemurusRight:
          femurusRightPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        case BoneName.TibiaRight:
          tibiaRightPlot.PlotSignals(templateW, sampleW, templateX, sampleX
                             , templateY, sampleY, templateZ, sampleZ);
          break;
        default:
          break;
      }

    }
    */

    private void BoneCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      
    }

    private void componentName_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      BoneName boneName = (BoneName)boneCombo.SelectedItem;
      int selectedIndex = boneComponentCombo.SelectedIndex;

      if (computation.Result != null) {

        float[] templateSignal = computation.Result.Data[Mapper.indexMap[boneName]].TemplateSignal[selectedIndex];
        float[] sampleSignal = computation.Result.Data[Mapper.indexMap[boneName]].SampleSignal[selectedIndex];
        float[][] matrix = computation.Result.Data[Mapper.indexMap[boneName]].Matrix[selectedIndex];

        signalPlot.Update(templateSignal, sampleSignal);
        matrixPlot.Update(matrix, templateSignal, sampleSignal);
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
