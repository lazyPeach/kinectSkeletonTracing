using System;
using System.Collections.Generic;

namespace DynamicTimeWarping {
  public class DTWCost {
    public DTWCost() { }

    public DTWCost(List<Tuple<int, int>> shortestPath, float cost) {
      this.shortestPath = shortestPath;
      this.cost = cost;
    }

    public List<Tuple<int, int>> ShortestPath { get { return shortestPath; } set { shortestPath = value; } }
    public float Cost { get { return cost; } set { cost = value; } }

    private List<Tuple<int, int>> shortestPath = new List<Tuple<int, int>>();
    private float cost;
  }
}
