using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureDetector {
  public class BodyDeviation {
    public BodyDeviation() {
      minBound = new Body();
      maxBound = new Body();
    }

    public Body MinBound { get { return minBound; } set { maxBound = value; } }
    public Body MaxBound { get { return maxBound; } set { maxBound = value; } }

    private Body minBound;
    private Body maxBound;
  }
}
