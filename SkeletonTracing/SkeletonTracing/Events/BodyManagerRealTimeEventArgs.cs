using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public class BodyManagerRealTimeEventArgs : EventArgs {
    private Body body;
    
    public BodyManagerRealTimeEventArgs(Body body) {
      this.body = body;
    }

    public Body Body { get { return body; } }
  }
}
