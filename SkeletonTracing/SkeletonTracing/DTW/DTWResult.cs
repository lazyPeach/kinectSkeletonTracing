using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.DTW {
  public class DTWResult {
    private int type; // 0 - normal, 1 - slow, 2 - fast;
    private float[][] cost = new float[16][];

    public int Type { get { return type; } set { type = value; } }
    public float[][] Cost { get { return cost; } set { cost = value; } }
  }
}
