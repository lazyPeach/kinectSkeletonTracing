using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class Joint {
    public float      XCoord    { get; set; }
    public float      YCoord    { get; set; }
    public float      ZCoord    { get; set; }
    public JointType  Type      { get; set; }
    public Priority   Priority  { get; set; }

    public Joint() : this(0, 0, 0, JointType.HipCenter) { }

    public Joint(float xCoord, float yCoord, float zCoord, JointType jointType, Priority priority = Priority.Low) {
      XCoord = xCoord;
      YCoord = yCoord;
      ZCoord = zCoord;
      Type = jointType;
      Priority = priority;
    }
  }
}
