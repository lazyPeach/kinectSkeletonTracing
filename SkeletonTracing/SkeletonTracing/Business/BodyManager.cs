using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace SkeletonTracing.Model {
  public delegate void BodyManagerEventHandler(object sender, BodyManagerEventArgs e);

  public class BodyManager {
    public event BodyManagerEventHandler BodyManagerEventHandl;
    public KinectManager KinectManagerProp { get { return kinectManager; } set { kinectManager = value; } }
    public ObservableCollection<Body> BodyData { get; set; }

    private KinectManager kinectManager;
    private Stopwatch stopwatch;

    public BodyManager(KinectManager kinectManager) {
      this.kinectManager = kinectManager;
      kinectManager.KinectManagerEventHandl += KinectManagerEventHandler;

      BodyData = new ObservableCollection<Body>();
      stopwatch = new Stopwatch();
    }

    public void Start() {
      stopwatch.Start();
    }

    public void Stop() {
      stopwatch.Stop();
    }

    public void SaveCollection(Stream file) {
      XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextWriter textWriter = new StreamWriter(file);
      serializer.Serialize(textWriter, BodyData);
      textWriter.Close();
    }

    public void LoadCollection(Stream file) {
      XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextReader textReader = new StreamReader(file);
      BodyData = (ObservableCollection<Body>)deserializer.Deserialize(textReader);
      textReader.Close();
    }

    public void ClearBodyData() {
      BodyData.Clear();
    }

    private int bodyIndex = 0;
    private System.Timers.Timer t;

    public void PlayGesture() {
      bodyIndex = 0;
      t = new System.Timers.Timer { Interval = 30 };
      t.Elapsed += DelayTimerElapsed;
      t.Start();
    }

    private void DelayTimerElapsed(object sender, System.Timers.ElapsedEventArgs e) {
      if (bodyIndex == BodyData.Count) {
        t.Enabled = false;
        t.Stop();
        return;
      }
      
      Body body = BodyData[bodyIndex];
      BodyManagerEventArgs ev = new BodyManagerEventArgs(body);
      OnEvent(ev);
      bodyIndex++;
    }

    private void KinectManagerEventHandler(object sender, KinectManagerEventArgs e) {
      Body body = new Body(e.Skeleton);
      BodyManagerEventArgs ev = new BodyManagerEventArgs(body);
      OnEvent(ev);
      BodyData.Add(body);
    }

    protected virtual void OnEvent(BodyManagerEventArgs e) {
      if (BodyManagerEventHandl != null) {
        BodyManagerEventHandl(this, e);
      }
    }
  }
}
