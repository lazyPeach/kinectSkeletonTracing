using SkeletonModel.Model;
using System;

namespace SkeletonModel.Events {
  public class BodyManagerPlayEventArgs : EventArgs {
    private Body templateBody;
    private Body sampleBody;

    public BodyManagerPlayEventArgs(Body template, Body sample) {
      templateBody = template;
      sampleBody = sample;
    }

    public Body TemplateBody { get { return templateBody; } }
    public Body SampleBody { get { return sampleBody; } }
  }
}
