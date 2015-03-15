using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class JointInfo {
    public long       AquireTime  { get; set; }
    public int        XCoord      { get; set; }
    public int        YCoord      { get; set; }
    public int        ZCoord      { get; set; }
    public JointType  Type        { get; set; }
  }
}
