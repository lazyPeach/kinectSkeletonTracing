using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTimeWarping {
  public class DTWCost {
    public DTWCost() { }
    
    private List<Tuple<int, int>> shortestPath = new List<Tuple<int, int>>();
    private float cost;

    public List<Tuple<int, int>> ShortestPath { get { return shortestPath; } set { shortestPath = value; } }
    public float Cost { get { return cost; } set { cost = value; } }

  }
}
