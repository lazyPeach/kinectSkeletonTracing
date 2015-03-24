using Microsoft.Kinect;
using System;
using System.Collections.Generic;

namespace SkeletonTracing.Helper {

  public enum JointName {
    HipCenter       = 0,
    Spine           = 1,
    ShoulderCenter  = 2,
    Head            = 3,
    ShoulderLeft    = 4,
    ElbowLeft       = 5,
    WristLeft       = 6,
    ShoulderRight   = 7,
    ElbowRight      = 8,
    WristRight      = 9,
    HipLeft         = 10,
    KneeLeft        = 11,
    AnkleLeft       = 12,
    HipRight        = 13,
    KneeRight       = 14,
    AnkleRight      = 15// maybe add some undefined value or default
  }

  public enum BoneName {
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

  public class Mapper {
    // map elements that don't have a correspondent to its closest
    public static Dictionary<JointType, JointName> JointTypeJointNameMap = new Dictionary<JointType, JointName>() {
      {JointType.HipCenter      , JointName.HipCenter       },
      {JointType.Spine          , JointName.Spine           },
      {JointType.ShoulderCenter , JointName.ShoulderCenter  },
      {JointType.Head           , JointName.Head            },
      {JointType.ShoulderLeft   , JointName.ShoulderLeft    },
      {JointType.ElbowLeft      , JointName.ElbowLeft       },
      {JointType.WristLeft      , JointName.WristLeft       },
      {JointType.HandLeft       , JointName.WristLeft       },
      {JointType.ShoulderRight  , JointName.ShoulderRight   },
      {JointType.ElbowRight     , JointName.ElbowRight      },
      {JointType.WristRight     , JointName.WristRight      },
      {JointType.HandRight      , JointName.WristRight      },
      {JointType.HipLeft        , JointName.HipLeft         },
      {JointType.KneeLeft       , JointName.KneeLeft        },
      {JointType.AnkleLeft      , JointName.AnkleLeft       },
      {JointType.FootLeft       , JointName.AnkleLeft       },
      {JointType.HipRight       , JointName.HipRight        },
      {JointType.KneeRight      , JointName.KneeRight       },
      {JointType.AnkleRight     , JointName.AnkleRight      },
      {JointType.FootRight      , JointName.AnkleRight      }
    };

    public static Dictionary<JointName, JointType> JointNameJointTypeMap = new Dictionary<JointName, JointType>() {
      {JointName.HipCenter      , JointType.HipCenter       },
      {JointName.Spine          , JointType.Spine           },
      {JointName.ShoulderCenter , JointType.ShoulderCenter  },
      {JointName.Head           , JointType.Head            },
      {JointName.ShoulderLeft   , JointType.ShoulderLeft    },
      {JointName.ElbowLeft      , JointType.ElbowLeft       },
      {JointName.WristLeft      , JointType.WristLeft       },
      {JointName.ShoulderRight  , JointType.ShoulderRight   },
      {JointName.ElbowRight     , JointType.ElbowRight      },
      {JointName.WristRight     , JointType.WristRight      },
      {JointName.HipLeft        , JointType.HipLeft         },
      {JointName.KneeLeft       , JointType.KneeLeft        },
      {JointName.AnkleLeft      , JointType.AnkleLeft       },
      {JointName.HipRight       , JointType.HipRight        },
      {JointName.KneeRight      , JointType.KneeRight       },
      {JointName.AnkleRight     , JointType.AnkleRight      }
    };

    // maps a bone to its extremity joints
    public static Dictionary<BoneName, Tuple<JointName, JointName>> BoneJointMap = new Dictionary<BoneName, Tuple<JointName, JointName>>() {
      {BoneName.BodyCenter    , new Tuple<JointName, JointName>(JointName.HipCenter, JointName.HipCenter)           },
      {BoneName.LowerSpine    , new Tuple<JointName, JointName>(JointName.HipCenter, JointName.Spine)               },
      {BoneName.UpperSpine    , new Tuple<JointName, JointName>(JointName.Spine, JointName.ShoulderCenter)          },
      {BoneName.Neck          , new Tuple<JointName, JointName>(JointName.ShoulderCenter, JointName.Head)           },
      {BoneName.ClavicleLeft  , new Tuple<JointName, JointName>(JointName.ShoulderCenter, JointName.ShoulderLeft)   },
      {BoneName.ArmLeft       , new Tuple<JointName, JointName>(JointName.ShoulderLeft, JointName.ElbowLeft)        },
      {BoneName.ForearmLeft   , new Tuple<JointName, JointName>(JointName.ElbowLeft, JointName.WristLeft)           },
      {BoneName.ClavicleRight , new Tuple<JointName, JointName>(JointName.ShoulderCenter, JointName.ShoulderRight)  },
      {BoneName.ArmRight      , new Tuple<JointName, JointName>(JointName.ShoulderRight, JointName.ElbowRight)      },
      {BoneName.ForearmRight  , new Tuple<JointName, JointName>(JointName.ElbowRight, JointName.WristRight)         },
      {BoneName.HipLeft       , new Tuple<JointName, JointName>(JointName.HipCenter, JointName.HipLeft)             },
      {BoneName.FemurusLeft   , new Tuple<JointName, JointName>(JointName.HipLeft, JointName.KneeLeft)              },
      {BoneName.TibiaLeft     , new Tuple<JointName, JointName>(JointName.KneeLeft, JointName.AnkleLeft)            },
      {BoneName.HipRight      , new Tuple<JointName, JointName>(JointName.HipCenter, JointName.HipRight)            },
      {BoneName.FemurusRight  , new Tuple<JointName, JointName>(JointName.HipRight, JointName.KneeRight)            },
      {BoneName.TibiaRight    , new Tuple<JointName, JointName>(JointName.KneeRight, JointName.AnkleRight)          }
    };
    
    
    public static Dictionary<Tuple<JointName, JointName>, BoneName> JointBoneMap = new Dictionary<Tuple<JointName, JointName>, BoneName>() {
      {new Tuple<JointName, JointName>(JointName.HipCenter, JointName.HipCenter)          , BoneName.BodyCenter     },
      {new Tuple<JointName, JointName>(JointName.HipCenter, JointName.Spine)              , BoneName.LowerSpine     },
      {new Tuple<JointName, JointName>(JointName.Spine, JointName.ShoulderCenter)         , BoneName.UpperSpine     },
      {new Tuple<JointName, JointName>(JointName.ShoulderCenter, JointName.Head)          , BoneName.Neck           },
      {new Tuple<JointName, JointName>(JointName.ShoulderCenter, JointName.ShoulderLeft)  , BoneName.ClavicleLeft   },
      {new Tuple<JointName, JointName>(JointName.ShoulderLeft, JointName.ElbowLeft)       , BoneName.ArmLeft        },
      {new Tuple<JointName, JointName>(JointName.ElbowLeft, JointName.WristLeft)          , BoneName.ForearmLeft    },
      {new Tuple<JointName, JointName>(JointName.ShoulderCenter, JointName.ShoulderRight) , BoneName.ClavicleRight  },
      {new Tuple<JointName, JointName>(JointName.ShoulderRight, JointName.ElbowRight)     , BoneName.ArmRight       },
      {new Tuple<JointName, JointName>(JointName.ElbowRight, JointName.WristRight)        , BoneName.ForearmRight   },
      {new Tuple<JointName, JointName>(JointName.HipCenter, JointName.HipLeft)            , BoneName.HipLeft        },
      {new Tuple<JointName, JointName>(JointName.HipLeft, JointName.KneeLeft)             , BoneName.FemurusLeft    },
      {new Tuple<JointName, JointName>(JointName.KneeLeft, JointName.AnkleLeft)           , BoneName.TibiaLeft      },
      {new Tuple<JointName, JointName>(JointName.HipCenter, JointName.HipRight)           , BoneName.HipRight       },
      {new Tuple<JointName, JointName>(JointName.HipRight, JointName.KneeRight)           , BoneName.FemurusRight   },
      {new Tuple<JointName, JointName>(JointName.KneeRight, JointName.AnkleRight)         , BoneName.TibiaRight     }
    };
  }
}
