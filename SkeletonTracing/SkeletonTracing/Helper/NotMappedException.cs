using System;

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
