using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonModel.Managers {
  public class BodyManagerExtended : BodyManager{
    public BodyManagerExtended() {
      records = new Queue<Queue<Body>>();
    }
    
    public void StartRecording() {
      
    }

    public void StopRecording() {

    }

    public Queue<Queue<Body>> Records { get { return records; } set { records = value; } }

    // each sequence will be stored in a queue of Body instances. When a sequence is finished it will
    // be added into the queue.
    private Queue<Queue<Body>> records;
  }
}
