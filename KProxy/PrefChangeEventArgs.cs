namespace KProxy
{
    using System;

    public class PrefChangeEventArgs : EventArgs
    {
        private readonly string _prefName;
        private readonly string _prefValueString;

        internal PrefChangeEventArgs(string prefName, string prefValueString)
        {
            this._prefName = prefName;
            this._prefValueString = prefValueString;
        }

        public string PrefName
        {
            get
            {
                return this._prefName;
            }
        }

        public bool ValueBool
        {
            get
            {
                if (this._prefValueString == null)
                {
                    return false;
                }
                return "True".Equals(this._prefValueString, StringComparison.OrdinalIgnoreCase);
            }
        }

        public string ValueString
        {
            get
            {
                return this._prefValueString;
            }
        }
    }
}

