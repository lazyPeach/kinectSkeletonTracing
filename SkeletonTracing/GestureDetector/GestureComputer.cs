using DynamicTimeWarping;
using Helper;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GestureDetector {
  public enum Gesture { RaiseBothHands, RaiseRightHand, RaiseLeftHand, Squat}

  public class GestureComputer {
    public bool IsBothHandsRaise(Body[] record) {
      Computation computation = new Computation();
      foreach (ObservableCollection<Body> bodyData in databaseData) {
        Console.WriteLine("pula mea asta-i combinatia");
        computation.ComputeOptimalDTW(bodyData.ToArray<Body>(), record);
        float sum = 0;
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          for (int i = 0; i < 4; i++) {
            sum += computation.OptimalResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost;
          }
        }

        Console.Write(sum.ToString());

        if (sum < 150)
          return true;
      }


      return false;
    }

    public void LoadGestureDB(Gesture gesture) {
      switch (gesture) {
        case Gesture.RaiseBothHands:
          LoadGesture("bothHandsRaise_db");
          break;
        case Gesture.RaiseLeftHand:
          LoadGesture("leftHandRaise_db");
          break;
        case Gesture.RaiseRightHand:
          LoadGesture("rightHandRaise_db");
          break;
        case Gesture.Squat:
          LoadGesture("squat_db");
          break;
      }
    }

    public void CleanGestureDB() {
      databaseData.Clear();
    }

    private void LoadGesture(string gesture) {
      List<string> files = new List<string>();

      foreach (string s in Directory.EnumerateFiles(@".\..\..\..\..\testData\")) {
        files.Add(s);
      }

      //iterate only through the first 5 files which we know are both hans rise serialization
      for (int sampleIndex = 0; sampleIndex < files.Count; sampleIndex++) {
        if (files[sampleIndex].Contains(gesture)) {
          FileStream sampleFileStream = new FileStream(files[sampleIndex], FileMode.Open, FileAccess.Read);
          LoadGesture(sampleFileStream);
        }
      }
    }

    private void LoadGesture(Stream file) {
      XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextReader textReader = new StreamReader(file);
      ObservableCollection<Body> bodyData = (ObservableCollection<Body>)deserializer.Deserialize(textReader);
      databaseData.Add(bodyData);
      textReader.Close();
    }

    private List<ObservableCollection<Body>> databaseData = new List<ObservableCollection<Body>>();


  }
}
