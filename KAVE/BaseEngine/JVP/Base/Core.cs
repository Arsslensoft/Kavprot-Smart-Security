using System;
using System.Collections.Generic;
using System.Text;

namespace JVP
{
    public delegate void JVPSignal(string Output, object data);
    public interface Core
    {
        string ProductID { get; }
        int Speed { get; }
        int Transfert { get; }
        // assembly runtime
        JVPAssembly JVPASM { get; }
        // execute single instruction and return value
        object ExecuteInstruction(object data, string Operand);
        // execute all assembly code and return value
        object ExecuteAssembly(string assembly, object data);
        // Send a signal
        event JVPSignal OnNewSignal;
        // send signal to output
        void SendSignal(string Output, object data);

        // initialize core
        void Initialize();
        // dispose core
        void Dispose();

        // random
        int MakeRandomDecision(int start, int end);
        double RandomPropability();


    }
}
