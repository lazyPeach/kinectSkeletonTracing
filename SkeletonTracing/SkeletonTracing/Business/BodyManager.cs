using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace SkeletonTracing.Model {
  public delegate void BodyManagerEventHandler(object sender, BodyManagerEventArgs e);

  public class BodyManager {
    public event BodyManagerEventHandler BodyManagerEventHandl;
    public KinectManager KinectManagerProp { get { return kinectManager; } set { kinectManager = value; } }
    private ObservableCollection<Body> bodyData;
    private ObservableCollection<Body> sampleData;

    public Body[] BodyData { get { return bodyData.ToArray<Body>(); } }
    public Body[] SampleData { get { return sampleData.ToArray<Body>(); } }

    private KinectManager kinectManager;
    private Stopwatch stopwatch;

    public BodyManager(KinectManager kinectManager) {
      this.kinectManager = kinectManager;
      kinectManager.KinectManagerEventHandl += KinectManagerEventHandler;

      bodyData = new ObservableCollection<Body>();
      sampleData = new ObservableCollection<Body>();
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
      serializer.Serialize(textWriter, bodyData);
      textWriter.Close();
    }

    public void LoadCollection(Stream file) {
      XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextReader textReader = new StreamReader(file);
      bodyData = (ObservableCollection<Body>)deserializer.Deserialize(textReader);
      textReader.Close();
    }

    public void LoadSample(Stream file) {
      XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextReader textReader = new StreamReader(file);
      sampleData = (ObservableCollection<Body>)deserializer.Deserialize(textReader);
      textReader.Close();
    }

    public void ClearData() {
      bodyData.Clear();
      sampleData.Clear();
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
      if (bodyIndex == bodyData.Count) {
        t.Enabled = false;
        t.Stop();
        return;
      }
      
      Body body = bodyData[bodyIndex];
      BodyManagerEventArgs ev = new BodyManagerEventArgs(body);
      OnEvent(ev);
      bodyIndex++;
    }

    private void KinectManagerEventHandler(object sender, KinectManagerEventArgs e) {
      Body body = new Body(e.Skeleton);
      BodyManagerEventArgs ev = new BodyManagerEventArgs(body);
      OnEvent(ev);
      bodyData.Add(body);
    }

    protected virtual void OnEvent(BodyManagerEventArgs e) {
      if (BodyManagerEventHandl != null) {
        BodyManagerEventHandl(this, e);
      }
    }
  }
}
