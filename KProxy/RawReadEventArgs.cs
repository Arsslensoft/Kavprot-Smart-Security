namespace KProxy
{
    using System;
    using System.Runtime.CompilerServices;

    public class RawReadEventArgs : EventArgs
    {
        private readonly byte[] _arrData;
        private readonly int _iCountBytes;
        private readonly Session _oS;
        [CompilerGenerated]
        private bool _AbortReading;

        internal RawReadEventArgs(Session oS, byte[] arrData, int iCountBytes)
        {
            this._arrData = arrData;
            this._iCountBytes = iCountBytes;
            this._oS = oS;
        }

        public bool AbortReading
        {
            [CompilerGenerated]
            get
            {
                return this._AbortReading;
            }
            [CompilerGenerated]
            set
            {
                this._AbortReading = value;
            }
        }

        public byte[] arrDataBuffer
        {
            get
            {
                return this._arrData;
            }
        }

        public int iCountOfBytes
        {
            get
            {
                return this._iCountBytes;
            }
        }

        public Session sessionOwner
        {
            get
            {
                return this._oS;
            }
        }
    }
}

