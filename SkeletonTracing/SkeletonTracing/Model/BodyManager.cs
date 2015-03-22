using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace SkeletonTracing.Model {
  public delegate void BodyManagerEventHandler(object sender, BodyManagerEventArgs e);

  public class BodyManager {
    public event BodyManagerEventHandler BodyManagerEventHandl;
    private KinectManager kinectManager;
    public KinectManager KinectManagerProp { get { return kinectManager; } set { kinectManager = value; } }
    private Stopwatch stopwatch;

    public ObservableCollection<Body> BodyData { get; set; }


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

    public int count = 0;

    public void PlayGesture() {
      //foreach (Body body in BodyData) {
      //  BodyManagerEventArgs ev = new BodyManagerEventArgs(body);
      //  OnEvent(ev);
      //  Thread.Sleep(30);
      //}
    }

    public void LoadCollection(Stream file) {
      XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<Body>));
      TextReader textReader = new StreamReader(file);
      BodyData = (ObservableCollection<Body>)deserializer.Deserialize(textReader);
      textReader.Close();
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
