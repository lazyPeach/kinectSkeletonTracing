using System;
using System.Windows.Controls;

namespace SkeletonTracing.View {
  public partial class DTWGraphic : UserControl {
    public DTWGraphic() {
      InitializeComponent();

      bodyCenterPlot.BoneName = "Body Center";
      neckPlot.BoneName = "Neck";
      upperSpinePlot.BoneName = "Upper Spine";
      lowerSpinePlot.BoneName = "Lower Spine";
      clavicleLeftPlot.BoneName = "Left Clavicle";
      clavicleRightPlot.BoneName = "Right Clavicle";
      armLeftPlot.BoneName = "Left arm";
      armRightPlot.BoneName = "Right arm";
      forearmLeftPlot.BoneName = "Left forearm";
      forearmRightPlot.BoneName = "Right forearm";
      hipLeftPlot.BoneName = "Left hip";
      hipRightPlot.BoneName = "Right hip";
      femurusLeftPlot.BoneName = "Left femurus";
      femurusRightPlot.BoneName = "Right femurus";
      tibiaLeftPlot.BoneName = "Left tibia";
      tibiaRightPlot.BoneName = "Right tibia";
    }
  }
}
