using DynamicTimeWarping;
using GestureDetector.Events;
using Helper;
using SkeletonModel.Events;
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

  public delegate void RecognizedGestureEventHandler(object sender, RecognizedGestureEventArgs e);

  public class GestureComputer {
    public event RecognizedGestureEventHandler RecognizedGestureEventHandler;

    public GestureComputer(BodyManager bodyManager, InitialPositionComputer initialPositionComputer) {
      this.bodyManager = bodyManager;
      this.initialPositionComputer = initialPositionComputer;
    }

    public bool IsCorrectGesture(Body[] record) {
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

        if (sum < 100)
          return true;
      }

      return false;
    }

    public void CleanGestureDB() {
      databaseData.Clear();
    }

    public void StartRecognition() {
      record = new Queue<Body>();
      bodyManager.RealTimeEventHandler += RealTimeEventHandler;
      initialPositionComputer.InitialPositionEventHandler += InitialPositionEventHandler;
      initialPositionComputer.RecordInitialPosition();
    }

    private void InitialPositionEventHandler(object sented, InitialPositionEventArgs e) {
      if (e.State == State.Finish) {
        record = new Queue<Body>();// empty the record in order to get rid of the body samples taken during initial positon acquisition
      }
    }

    private void RealTimeEventHandler(object sender, BodyManagerEventArgs e) {
      Body body = e.Body;

      if (initialPositionComputer.IsInitialPosition(body)) { // move this in an event raised by initial computer
        if (record.Count > 50) { // consider each gesture with less than 50 samples incorrect
          if (IsCorrectGesture(record.ToArray())) {
            OnEvent(new RecognizedGestureEventArgs());
          }

          record = new Queue<Body>();
        }
      } else {
        record.Enqueue(body);
      }
    }

    protected virtual void OnEvent(RecognizedGestureEventArgs e) {
      if (RecognizedGestureEventHandler != null) {
        RecognizedGestureEventHandler(this, e);
      }
    }

    public void LoadGesture(string gesture) {
      List<string> files = new List<string>();

      foreach (string s in Directory.EnumerateFiles(@"..\..\..\..\..\database\")) {
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

    private Queue<Body> record;
    private List<ObservableCollection<Body>> databaseData = new List<ObservableCollection<Body>>();
    private BodyManager bodyManager;
    private InitialPositionComputer initialPositionComputer;
  }
}
