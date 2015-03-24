﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using SkeletonTracing.Events;
using System;

namespace SkeletonTracing.Model {
  public delegate void BodyManagerRealTimeEventHandler(object sender, BodyManagerRealTimeEventArgs e);
  public delegate void BodyManagerPlayEventHandler(object sender, BodyManagerPlayEventArgs e);

  public class BodyManager {
    private KinectManager kinectManager;
    private ObservableCollection<Body> bodyData;
    private ObservableCollection<Body> sampleData;

    public Body[] BodyData { get { return bodyData.ToArray<Body>(); } }
    public Body[] SampleData { get { return sampleData.ToArray<Body>(); } }

    public event BodyManagerRealTimeEventHandler RealTimeEventHandler;
    public event BodyManagerPlayEventHandler PlayEventHandler;

    public BodyManager(KinectManager kinectManager) {
      this.kinectManager = kinectManager;
      kinectManager.KinectManagerEventHandl += KinectManagerEventHandler;

      bodyData = new ObservableCollection<Body>();
      sampleData = new ObservableCollection<Body>();
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

    private int bodyIndex;
    private System.Timers.Timer timer;

    public void PlayGesture() {
      bodyIndex = 0;
      timer = new System.Timers.Timer { Interval = 30 };
      timer.Elapsed += DelayTimerElapsed; // call this method every time the interval elapsed
      timer.Start();
    }

    private void DelayTimerElapsed(object sender, System.Timers.ElapsedEventArgs e) {
      if (bodyIndex == Math.Max(bodyData.Count, sampleData.Count)) { // stop the simulation when the biggest nr of samples was reached
        timer.Stop();
        return;
      }

      Body template = (bodyIndex < bodyData.Count) ? bodyData[bodyIndex] : bodyData[bodyData.Count - 1];
      Body sample = (bodyIndex < sampleData.Count) ? sampleData[bodyIndex] : sampleData[sampleData.Count - 1];
      BodyManagerPlayEventArgs ev = new BodyManagerPlayEventArgs(template, sample);
      OnPlayEvent(ev);
      bodyIndex++;
    }

    private void KinectManagerEventHandler(object sender, KinectManagerEventArgs e) {
      Body body = new Body(e.Skeleton);
      BodyManagerRealTimeEventArgs ev = new BodyManagerRealTimeEventArgs(body);
      OnRealTimeEvent(ev);
      bodyData.Add(body);
    }

    protected virtual void OnRealTimeEvent(BodyManagerRealTimeEventArgs e) {
      if (RealTimeEventHandler != null) {
        RealTimeEventHandler(this, e);
      }
    }

    protected virtual void OnPlayEvent(BodyManagerPlayEventArgs e) {
      if (PlayEventHandler != null) {
        PlayEventHandler(this, e);
      }
    }
  }
}