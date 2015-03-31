using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonModel.Events {
  public class BodyManagerEventArgs : EventArgs {
    private Body body;
    
    public BodyManagerEventArgs(Body body) {
      this.body = body;
    }

    public Body Body { get { return body; } }
  }
}
