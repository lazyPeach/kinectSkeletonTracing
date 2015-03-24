using SkeletonTracing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonTracing.Events {
  public class BodyManagerPlayEventArgs :EventArgs {
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
