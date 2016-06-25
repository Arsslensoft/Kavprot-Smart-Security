namespace KProxy
{
    using System;

    public class StateChangeEventArgs : EventArgs
    {
        public readonly SessionStates newState;
        public readonly SessionStates oldState;

        internal StateChangeEventArgs(SessionStates ssOld, SessionStates ssNew)
        {
            this.oldState = ssOld;
            this.newState = ssNew;
        }
    }
}

