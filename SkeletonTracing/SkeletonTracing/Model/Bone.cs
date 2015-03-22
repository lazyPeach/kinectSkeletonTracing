using SkeletonTracing.Helper;

namespace SkeletonTracing.Model {
  public class Bone {
    public Rotation Rotation { get; set; }
    public BoneName Type { get; set; }
    public Priority Priority { get; set; }

    public Bone() : this(new Rotation(), BoneName.BodyCenter, Priority.Low) { }

    public Bone(Rotation rotation, BoneName type, Priority priority = Priority.Low) {
      Rotation = rotation;
      Type = type;
      Priority = priority;
    }
  }

  public class Quaternion {
    public float W { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Quaternion()
      : this(0, 0, 0, 0) {
    }

    public Quaternion(float w, float x, float y, float z) {
      W = w;
      X = x;
      Y = y;
      Z = z;
    }
  }

  public class Rotation {
    public Quaternion Quaternion { get; set; }

    public Rotation() : this(0, 0, 0, 0) { }

    public Rotation(Quaternion quaternion) {
      Quaternion = quaternion;
    }

    public Rotation(float w, float x, float y, float z) {
      Quaternion = new Quaternion(w, x, y, z);
    }
  }
}
