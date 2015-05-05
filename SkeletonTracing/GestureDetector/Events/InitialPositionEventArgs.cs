using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureDetector.Events {
  public enum State { Pause, Recording, Finish, Check }

  // TODO split in two -> one for state + timer, one for initial position events
  // holds the state of the InitialPositionComputer class, the timer in case it records and the position of the body
  public class InitialPositionEventArgs : EventArgs {
    public InitialPositionEventArgs(State state, int timer, bool isInitialPosition) {
      this.state = state;
      this.timer = timer;
      this.isInitialPosition = isInitialPosition;
    }

    public State State { get { return state; } }
    public int Timer { get { return timer; } }
    public bool IsInitialPosition { get { return isInitialPosition; } }

    private int timer;
    private State state;
    private bool isInitialPosition;
  }
}
