using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Helper {
  public class NotMappedException : Exception {
    public NotMappedException()
      : base("Not existing mapping") {
    }

    public NotMappedException(string message)
      : base(message) {
    }
  }
}
