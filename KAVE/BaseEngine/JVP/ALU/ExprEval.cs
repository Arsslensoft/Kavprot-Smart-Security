using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;


namespace JVP.ALN
{
    /// <summary>
    /// This class will evaluate boolean and mathmatical expressions
    /// </summary>
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public class ExpressionEval : IExpression
    {
        #region Internal Structures and Classes

        /// <summary>
        /// This structure is used internally to order operations
        /// </summary>
        internal class BinaryOp
        {
            private string _strOp;
            private int _nPrecedence;

            public string Op { get { return _strOp; } }
            public int Precedence { get { return _nPrecedence; } }

            public BinaryOp(string strOp)
            { _strOp = strOp; _nPrecedence = ExpressionEval.OperatorPrecedence(strOp); }

            public override string ToString() { return Op; }
        }

        /// <summary>
        /// Queueing binary operations
        /// </summary>
        internal class BinaryOpQueue
        {
            private ArrayList _oplist = new ArrayList();

            public BinaryOpQueue(ArrayList expressionlist)
            {
                foreach (object item in expressionlist)
                    Enqueue(item as BinaryOp);
            }

            public void Enqueue(BinaryOp op)
            {
                if (op == null)
                    return;

                bool bQueued = false;
                for (int x = 0; x < _oplist.Count && !bQueued; x++)
                {
                    if (((BinaryOp)_oplist[x]).Precedence > op.Precedence)
                    {
                        _oplist.Insert(x, op);
                        bQueued = true;
                    }
                }
                if (!bQueued)
                    _oplist.Add(op);
            }

            public BinaryOp Dequeue()
            {
                if (_oplist.Count == 0)
                    return null;
                BinaryOp ret = (BinaryOp)_oplist[0];
                _oplist.RemoveAt(0);
                return ret;
            }

            public int Count
            {
                get { return _oplist.Count; }
            }
        }

        /// <summary>
        /// This structure is used internally to order operations
        /// </summary>
        internal class UnaryOp
        {
            private string _strOp;

            public string Op { get { return _strOp; } }

            public UnaryOp(string strOp)
            { _strOp = strOp; }

        }

        /// <summary>
        /// used to specify variables
        /// </summary>
        internal class Variable : IExpression
        {
            string _expr;
            Hashtable _vals;

            internal Variable(string expression, Hashtable vals)
            { _expr = expression; _vals = vals; }

            public string Expression
            { get { return _expr; } set { _expr = value; } }

            public object Evaluate()
            { return _vals[_expr]; }
        }

        #endregion

        #region Private Members

        ArrayList _expressionlist = new ArrayList();
        string _expression = "";
        bool _bParsed;
        internal Hashtable _variables = new Hashtable();

        #endregion

        #region Construction

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExpressionEval() { }

        /// <summary>
        /// Constructor with string
        /// </summary>
        /// <param name="expression">string of the Expression to evaluate</param>
        public ExpressionEval(string expression)
        { Expression = expression; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the expression to be evaluated.
        /// </summary>
        public string Expression
        {
            get { return _expression; }
            set
            {
                _expression = value.Trim();
                _bParsed = false;
                _expressionlist.Clear();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>object of the expression return value</returns>
        public object Evaluate()
        {
            if ("" + Expression == "")
                return 0;

            return ExecuteEvaluation();
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>bool value of the evaluated expression</returns>
        public bool EvaluateBool()
        { return Convert.ToBoolean(Evaluate(), CultureInfo.CurrentCulture); }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>integer value of the evaluated expression</returns>
        public int EvaluateInt()
        { return Convert.ToInt32(Evaluate(), CultureInfo.CurrentCulture); }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>double value of the evaluated expression</returns>
        public double EvaluateDouble()
        { return Convert.ToDouble(Evaluate(), CultureInfo.CurrentCulture); }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>long value of the evaluated expression</returns>
        public long EvaluateLong()
        { return Convert.ToInt64(Evaluate(), CultureInfo.CurrentCulture); }


        /// <summary>
        /// Static version of the Expression Evaluator
        /// </summary>
        /// <param name="expression">expression to be evaluated</param>
        /// <returns></returns>
        public static object Evaluate(string expressionString)
        {
            ExpressionEval expression = new ExpressionEval(expressionString);
            return expression.Evaluate();
        }

        /// <summary>
        /// Static version of the Expression Evaluator
        /// </summary>
        /// <param name="expression">expression to be evaluated</param>
        /// <param name="handler">attach a custom function handler</param>
        /// <returns></returns>
        public static object Evaluate(string expression, AdditionalFunctionEventHandler handler)
        {
            ExpressionEval expr = new ExpressionEval(expression);
            expr.AdditionalFunctionEventHandler += handler;
            return expr.Evaluate();
        }

        /// <summary>
        /// Sets a variable's value
        /// </summary>
        /// <param name="key">variable name</param>
        /// <param name="value">variable value</param>
        public void SetVariable(string key, object value)
        {
            ClearVariable(key);
            _variables.Add(key, value);
        }

        /// <summary>
        /// Clear's a variable's value
        /// </summary>
        /// <param name="key">variable name</param>
        public void ClearVariable(string key)
        {
            if (_variables.ContainsKey(key))
                _variables.Remove(key);
        }

        /// <summary>
        /// gets a string representation of this expression
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return "(" + Expression + ")"; }

        /// <summary>
        /// Sorts the mathmatical operations to be executed
        /// </summary>
        private object ExecuteEvaluation()
        {
            //Break Expression Apart into List
            if (!_bParsed)
                for (int x = 0; x < Expression.Length; x = NextToken(x)) ;
            _bParsed = true;

            //Perform Operations
            return EvaluateList();
        }

        /// <summary>
        /// This will search the expression for the next token (operand, operator, etc)
        /// </summary>
        /// <param name="nIdx">Start Position of Search</param>
        /// <returns>First character index after token.</returns>
        //[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private int NextToken(int nIdx)
        {
            Match mRet = null;
            int nRet = nIdx;
            object val = null;

            //Check for preceeding white space from last token index
            Match m = DefinedRegex.WhiteSpace.Match(Expression, nIdx);
            if (m.Success && m.Index == nIdx)
                return nIdx + m.Length;

            //Check Parenthesis
            m = DefinedRegex.Parenthesis.Match(Expression, nIdx);
            if (m.Success)
                mRet = m;

            //Check Function
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.Function.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                    mRet = m;
            }

            //Check Variable
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.Variable.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                { mRet = m; val = new Variable(m.Groups["Variable"].Value, _variables); }
            }

            //Check Unary Operator
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.UnaryOp.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                { mRet = m; val = new UnaryOp(m.Value); }
            }

            //Check Hexadecimal
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.Hexadecimal.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                { mRet = m; val = Convert.ToInt32(m.Value, 16); }
            }

            //Check Boolean
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.Boolean.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                { mRet = m; val = bool.Parse(m.Value); }
            }

            //Check DateTime
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.DateTime.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                { mRet = m; val = Convert.ToDateTime(m.Groups["DateString"].Value, CultureInfo.CurrentCulture); }
            }

            //Check Timespan
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.TimeSpan.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                {
                    mRet = m;
                    val = new TimeSpan(
                        int.Parse("0" + m.Groups["Days"].Value),
                        int.Parse(m.Groups["Hours"].Value),
                        int.Parse(m.Groups["Minutes"].Value),
                        int.Parse("0" + m.Groups["Seconds"].Value),
                        int.Parse("0" + m.Groups["Milliseconds"].Value)
                    );
                }
            }

            //Check Numeric
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.Numeric.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                {
                    while (m.Success && ("" + m.Value == ""))
                        m = m.NextMatch();
                    if (m.Success)
                    {
                        mRet = m;
                        val = double.Parse(m.Value, CultureInfo.CurrentCulture);
                    }
                }
            }

            if (mRet == null || mRet.Index > nIdx)
            {
                //Check String
                m = DefinedRegex.String.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                { mRet = m; val = m.Groups["String"].Value.Replace("\\\"", "\""); }
            }

            //Check Binary Operator
            if (mRet == null || mRet.Index > nIdx)
            {
                m = DefinedRegex.BinaryOp.Match(Expression, nIdx);
                if (m.Success && (mRet == null || m.Index < mRet.Index))
                { mRet = m; val = new BinaryOp(m.Value); }
            }

            if (mRet == null)
                throw new ArgumentException("Invalid expression construction: \"" + Expression + "\".");

            if (mRet.Index != nIdx)
            {
                throw new ArgumentException(
                    "Invalid token in expression: [" +
                        Expression.Substring(nIdx, mRet.Index - nIdx).Trim() + "]"
                );
            }

            if (mRet.Value == "(" || mRet.Value.StartsWith("$"))
            {
                nRet = mRet.Index + mRet.Length;
                int nDepth = 1;
                bool bInQuotes = false;
                while (nDepth > 0)
                {
                    if (nRet >= Expression.Length)
                        throw new ArgumentException("Missing " + (bInQuotes ? "\"" : ")") + " in Expression");
                    if (!bInQuotes && Expression[nRet] == ')')
                        nDepth--;
                    if (!bInQuotes && Expression[nRet] == '(')
                        nDepth++;

                    if (Expression[nRet] == '"' && (nRet == 0 || Expression[nRet - 1] != '\\'))
                        bInQuotes = !bInQuotes;

                    nRet++;
                }
                if (mRet.Value == "(")
                {
                    ExpressionEval expr = new ExpressionEval(
                        Expression.Substring(mRet.Index + 1, nRet - mRet.Index - 2)
                    );
                    if (this.AdditionalFunctionEventHandler != null)
                        expr.AdditionalFunctionEventHandler += this.AdditionalFunctionEventHandler;
                    expr._variables = this._variables;
                    _expressionlist.Add(expr);
                }
                else
                {
                    FunctionEval func = new FunctionEval(
                        Expression.Substring(mRet.Index, (nRet) - mRet.Index)
                    );
                    if (this.AdditionalFunctionEventHandler != null)
                        func.AdditionalFunctionEventHandler += this.AdditionalFunctionEventHandler;
                    func._variables = this._variables;
                    _expressionlist.Add(func);
                }
            }
            else
            {
                nRet = mRet.Index + mRet.Length;
                _expressionlist.Add(val);
            }

            return nRet;
        }

        /// <summary>
        /// Traverses the list to perform operations on items according to operator precedence
        /// </summary>
        /// <returns>final evaluated expression of Expression string</returns>
        private object EvaluateList()
        {
            ArrayList list = (ArrayList)_expressionlist.Clone();

            //Do the unary operators first
            for (int x = 0; x < list.Count; x++)
            {
                if (list[x] is UnaryOp)
                {
                    list[x] = PerformUnaryOp(
                        (UnaryOp)list[x],
                        list[x + 1]
                    );
                    list.RemoveAt(x + 1);
                }
            }

            //Get the queued binary operations
            BinaryOpQueue opqueue = new BinaryOpQueue(list);

            string msg = "";
            for (int x = 1; x < list.Count; x += 2)
            {
                 if (list[x] is BinaryOp)
                {
                    if (x + 1 == list.Count)
                    {
                        throw new ArgumentException(
                            "Expression cannot end in a binary operation: [" + list[x].ToString() + "]"
                        );
                    }
                }
                else
                {
                    msg += string.Format(
                        "\n{0} [?] {1}",
                        (list[x - 1] is string) ? "\"" + list[x - 1] + "\"" : list[x - 1],
                        (list[x] is string) ? "\"" + list[x] + "\"" : list[x]
                    );
                    x--;
                }
            }
            if (msg != "")
                throw new ArgumentException("Missing binary operator: " + msg);

            BinaryOp op = opqueue.Dequeue();
            while (op != null)
            {
                int nIdx = list.IndexOf(op);
                list[nIdx - 1] = PerformBinaryOp(
                    (BinaryOp)list[nIdx],
                    list[nIdx - 1],
                    list[nIdx + 1]
                );
                list.RemoveAt(nIdx);
                list.RemoveAt(nIdx);
                op = opqueue.Dequeue();
            }

            object ret = null;
            if (list[0] is IExpression)
                ret = ((IExpression)list[0]).Evaluate();
            else
                ret = list[0];
            return ret;
        }

        /// <summary>
        /// This method gets the precedence of a binary operator
        /// </summary>
        /// <param name="strOp"></param>
        /// <returns></returns>
        private static int OperatorPrecedence(string strOp)
        {
            switch (strOp)
            {
                case "MUL":
                case "DIV":
                case "MOD": return 0;
                case "ADD":
                case "SUB": return 1;
                case "RSH":
                case "LSH": return 2;
                case "CLT":
                case "CLE":
                case "CGT":
                case "CGE": return 3;
                case "LEQ":
                case "AEQ":
                case "NEQ": return 4;
                case "BAND": return 5;
                case "XOR": return 6;
                case "BOR": return 7;
                case "AND": return 8;
                case "OR": return 9;
            }
            throw new ArgumentException("Operator " + strOp + "not defined.");
        }

        /// <summary>
        /// This routine will actually execute an operation and return its value
        /// </summary>
        /// <param name="op">Operator Information</param>
        /// <param name="v1">left operand</param>
        /// <param name="v2">right operand</param>
        /// <returns>v1 (op) v2</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static object PerformBinaryOp(BinaryOp op, object v1, object v2)
        {
            IExpression tv = v1 as IExpression;
            if (tv != null)
                v1 = tv.Evaluate();
            tv = v2 as IExpression;
            if (tv != null)
                v2 = tv.Evaluate();

            switch (op.Op)
            {
                case "MUL": return (Convert.ToDouble(v1, CultureInfo.CurrentCulture) *
                                  Convert.ToDouble(v2, CultureInfo.CurrentCulture));
                case "DIV": return (Convert.ToDouble(v1, CultureInfo.CurrentCulture) /
                                  Convert.ToDouble(v2, CultureInfo.CurrentCulture));
                case "MOD": return (Convert.ToInt64(v1, CultureInfo.CurrentCulture) %
                                  Convert.ToInt64(v2, CultureInfo.CurrentCulture));
                case "LSH": return (Convert.ToInt64(v1, CultureInfo.CurrentCulture) <<
                                   Convert.ToInt32(v2, CultureInfo.CurrentCulture));
                case "RSH": return (Convert.ToInt64(v1, CultureInfo.CurrentCulture) >>
                                   Convert.ToInt32(v2, CultureInfo.CurrentCulture));
                case "ADD":
                case "SUB":
                case "CLT":
                case "CLE":
                case "CGT":
                case "CGE":
                case "LEQ":
                case "AEQ":
                case "NEQ": return DoSpecialOperator(op, v1, v2);
                case "BAND": return (Convert.ToUInt64(v1, CultureInfo.CurrentCulture) &
                                  Convert.ToUInt64(v2, CultureInfo.CurrentCulture));
                case "XOR": return (Convert.ToUInt64(v1, CultureInfo.CurrentCulture) ^
                                  Convert.ToUInt64(v2, CultureInfo.CurrentCulture));
                case "BOR": return (Convert.ToUInt64(v1, CultureInfo.CurrentCulture) |
                                  Convert.ToUInt64(v2, CultureInfo.CurrentCulture));
                case "AND": return (Convert.ToBoolean(v1, CultureInfo.CurrentCulture) &&
                                   Convert.ToBoolean(v2, CultureInfo.CurrentCulture));
                case "OR": return (Convert.ToBoolean(v1, CultureInfo.CurrentCulture) ||
                                   Convert.ToBoolean(v2, CultureInfo.CurrentCulture));
            }
            throw new ArgumentException("Binary Operator " + op.Op + "not defined.");
        }

        /// <summary>
        /// This will perform comparison operations based upon data type of value
        /// </summary>
        /// <param name="op">binary operator</param>
        /// <param name="v1">left operand</param>
        /// <param name="v2">right operand</param>
        /// <returns>return result of operator</returns>
        //[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static object DoSpecialOperator(BinaryOp op, object v1, object v2)
        {
            if (v1 is string || v2 is string)
            {
                string str1 = "" + v1,
                        str2 = "" + v2;

                switch (op.Op)
                {
                    case "ADD": return str1 + str2;
                    case "SUB": throw new ArgumentException("Operator 'SUB' invalid for strings.");
                    case "CLT": return str1.CompareTo(str2) < 0;
                    case "CLE": return str1.CompareTo(str2) < 0 || str1 == str2;
                    case "CGT": return str1.CompareTo(str2) > 0;
                    case "CGE": return str1.CompareTo(str2) > 0 || str1 == str2; ;
                    case "LEQ":
                    case "AEQ": return str1 == str2;
                    case "NEQ": return str1 != str2;
                }
            }
            if (v1 is DateTime || v2 is DateTime)
            {
                DateTime
                    d1 = Convert.ToDateTime(v1, CultureInfo.CurrentCulture),
                    d2 = Convert.ToDateTime(v2, CultureInfo.CurrentCulture);

                switch (op.Op)
                {
                    case "ADD": throw new ArgumentException("Operator 'ADD' invalid for dates.");
                    case "SUB": return d1 - d2;
                    case "CLT": return d1 < d2;
                    case "CLE": return d1 <= d2;
                    case "CGT": return d1 > d2;
                    case "CGE": return d1 >= d2;
                    case "LEQ":
                    case "AEQ": return d1 == d2;
                    case "NEQ": return d1 != d2;
                }
            }

            double
                f1 = Convert.ToDouble(v1, CultureInfo.CurrentCulture),
                f2 = Convert.ToDouble(v2, CultureInfo.CurrentCulture);
            switch (op.Op)
            {
                case "ADD": return f1 + f2;
                case "SUB": return f1 - f2;
                case "CLT": return f1 < f2;
                case "CLE": return f1 <= f2;
                case "CGT": return f1 > f2;
                case "CGE": return f1 >= f2;
                case "LEQ":
                case "AEQ": return f1 == f2;
                case "NEQ": return f1 != f2;
            }

            throw new ArgumentException("Operator '" + op.Op + "' not specified.");
        }

        /// <summary>
        /// This routine will actually execute an operation and return its value
        /// </summary>
        /// <param name="op">Operator Information</param>
        /// <param name="v">right operand</param>
        /// <returns>(op)v</returns>
        private static object PerformUnaryOp(UnaryOp op, object v)
        {
            IExpression tempv = v as IExpression;
            if (tempv != null)
                v = tempv.Evaluate();

            switch (op.Op)
            {
                case "ADD": return (Convert.ToDouble(v, CultureInfo.CurrentCulture));
                case "SUB": return (-Convert.ToDouble(v, CultureInfo.CurrentCulture));
                case "NOT": return (!Convert.ToBoolean(v, CultureInfo.CurrentCulture));
                case "BWC": return (~Convert.ToUInt64(v, CultureInfo.CurrentCulture));
            }
            throw new ArgumentException("Unary Operator " + op.Op + "not defined.");
        }

        #endregion

        #region Events

        /// <summary>
        /// This event will trigger for every function that is not intercepted internally
        /// </summary>
        public event AdditionalFunctionEventHandler AdditionalFunctionEventHandler;

        #endregion
    }
}
