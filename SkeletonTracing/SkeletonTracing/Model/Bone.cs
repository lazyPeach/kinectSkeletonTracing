using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Model {
  public enum BoneType {
    BodyCenter      = 0,
    LowerSpine      = 1,
    UpperSpine      = 2,
    Neck            = 3,
    ClavicleLeft    = 4,
    ArmLeft         = 5,
    ForearmLeft     = 6,
    ClavicleRight   = 7,
    ArmRight        = 8,
    ForearmRight    = 9,
    HipLeft         = 10,
    FemurusLeft     = 11,
    TibiaLeft       = 12,
    HipRight        = 13,
    FemurusRight    = 14,
    TibiaRight      = 15
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

  public class Bone {
    public Rotation Rotation { get; set; }
    public BoneType Type { get; set; }
    public Priority Priority { get; set; }

    public Bone() : this(new Rotation(), BoneType.BodyCenter, Priority.Low) { }

    public Bone(Rotation rotation, BoneType type, Priority priority = Priority.Low) {
      Rotation = rotation;
      Type = type;
      Priority = priority;
    }
  }
}
