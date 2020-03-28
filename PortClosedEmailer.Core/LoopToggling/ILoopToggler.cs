using System;

namespace PortClosedEmailer.Core.LoopToggling
{
    public interface ILoopToggler
    {
        event EventHandler<string>       FoundOpen;
        event EventHandler<string>       FoundClosed;

        void StartAllLoops ();
        void  StopOneLoop   (int loopIndex);
    }
}
