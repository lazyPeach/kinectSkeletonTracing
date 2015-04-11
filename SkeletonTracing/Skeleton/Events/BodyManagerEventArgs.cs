using SkeletonModel.Model;
using System;

namespace SkeletonModel.Events {
  public class BodyManagerEventArgs : EventArgs {
    private Body body;
    
    public BodyManagerEventArgs(Body body) {
      this.body = body;
    }

    public Body Body { get { return body; } }
  }
}
