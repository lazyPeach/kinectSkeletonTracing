using Helper;

namespace SkeletonModel.Model {
  public class Bone {
    public Bone() : this(new Rotation(), BoneName.BodyCenter, Priority.Low) { }

    public Bone(Rotation rotation, BoneName boneName, Priority priority = Priority.Low) {
      this.rotation = rotation;
      this.boneName = boneName;
      this.priority = priority;
    }

    public Rotation Rotation { get { return rotation; } set { rotation = value; } }
    public BoneName BoneName { get { return boneName; } set { boneName = value; } }
    public Priority Priority { get { return priority; } set { priority = value; } }

    private Rotation rotation;
    private BoneName boneName;
    private Priority priority;

  }


  public class Rotation {
    public Rotation() : this(0, 0, 0, 0) { }

    public Rotation(Quaternion quaternion) {
      this.quaternion = quaternion;
    }

    public Rotation(float w, float x, float y, float z) {
      quaternion = new Quaternion(w, x, y, z);
    }

    public Quaternion Quaternion { get { return quaternion; } set { quaternion = value; } }

    private Quaternion quaternion;
  }

  public class Quaternion {
    public Quaternion()
      : this(0, 0, 0, 0) {
    }

    public Quaternion(float w, float x, float y, float z) {
      W = w;
      X = x;
      Y = y;
      Z = z;
    }

    public float W { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
  }
}
