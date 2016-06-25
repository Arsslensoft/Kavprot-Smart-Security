using System;

namespace JVP.ALN
{
    #region "Event Handling Objects"

    /// <summary>
    /// Event Handling for Additional Functions
    /// </summary>
    public class AdditionalFunctionEventArgs : EventArgs
    {
        private string      _name = "";
        private object[]    _parameters;
        private object      _return;

        /// <summary>
        /// This is the only constructor
        /// </summary>
        /// <param name="name">the Name of the function</param>
        /// <param name="a_params"></param>
        public AdditionalFunctionEventArgs(string name, object[] parameters)
        {
            _name = name;
            _parameters = parameters;
        }

        /// <summary>
        /// This is the name of the additional function
        /// </summary>
        public string Name { get { return _name; } }

        public object ReturnValue
        {
            get { return _return; }
            set { _return = value; }
        }

        /// <summary>
        /// This method will return an array of objects that are parameters.
        /// </summary>
        /// <returns>object array of function parameters</returns>
        public object[] GetParameters() 
        {
            if (_parameters == null)
                return null;

            object[] ret = new object[_parameters.Length];
            Array.Copy(_parameters, ret, ret.Length);
            return ret;
        }
    }
    #endregion

}
