using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureDetector {
  // this class holds the mean and standard deviation for body bones
  public class BodyDeviation {
    public BodyDeviation() {
      mean = new Body();
      deviation = new Body();
    }

    public Body Mean { get { return mean; } set { mean = value; } }
    public Body Deviation { get { return deviation; } set { deviation = value; } }

    private Body mean;
    private Body deviation;
  }
}
