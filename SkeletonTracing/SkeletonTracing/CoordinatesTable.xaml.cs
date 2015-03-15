using Microsoft.Kinect;
using SkeletonTracing.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace SkeletonTracing {
  public partial class CoordinatesTable : UserControl {

    private SkeletonManager skeletonManager;
    public SkeletonManager SkeletonManager {
      set {
        skeletonManager = value;
      }
    }

    public CoordinatesTable() {
      InitializeComponent();
      this.DataContext = this;

      InitializeJointsComboBox();
    }

    private void InitializeJointsComboBox() {
      JointList.ItemsSource = Enum.GetValues(typeof(JointType)).Cast<JointType>();
    }

    private void JointList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      JointType type = (JointType)JointList.SelectedValue;
      TableView.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = skeletonManager.GetJointList(type) });
    }
  }
}
