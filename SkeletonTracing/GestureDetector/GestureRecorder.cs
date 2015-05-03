using SkeletonModel.Events;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GestureDetector {
  public class GestureRecorder {
    public GestureRecorder(BodyManager bodyManager, InitialComputer initialComputer, string gestureFileName) {
      record = new Queue<Body>();
      this.bodyManager = bodyManager;
      this.initialComputer = initialComputer;
      this.gestureFileName = gestureFileName;
    }

    public void StartRecording() {
      bodyManager.RealTimeEventHandler += RealTimeEventHandler;
    }

    public void StopRecording() {
      bodyManager.RealTimeEventHandler -= RealTimeEventHandler;
      for (int i = 0; i < 5; i++) {
        XmlSerializer serializer = new XmlSerializer(typeof(Body[]));
        using (TextWriter textWriter = new StreamWriter(@"..\..\..\..\..\database\" + gestureFileName + i.ToString() + ".xml")) {
          serializer.Serialize(textWriter, records[i].ToArray());
          textWriter.Close();
        }
      }
    }

    private void RealTimeEventHandler(object sender, BodyManagerEventArgs e) {
      Body body = e.Body;
      
      if (initialComputer.IsInitialPosition(body)) {
        Console.WriteLine("back into initial position");
        if (record.Count > 50) { // consider each gesture with less than 50 samples incorrect
          Console.WriteLine("whoa we have a gesture");

          records[samplesCount] = record;
          samplesCount++;
          Console.WriteLine(samplesCount.ToString());
          if (IsFinished()) {
            StopRecording();
          }
          record = new Queue<Body>();
        }
      } else {
        record.Enqueue(body);
      }
    }

    public bool IsFinished() {
      return samplesCount == 5;
    }

    private int samplesCount = 0;
    private BodyManager bodyManager;
    private InitialComputer initialComputer;
    private string gestureFileName;
    private Queue<Body> record;
    private Queue<Body>[] records = new Queue<Body>[5];
  }
}
