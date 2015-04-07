using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    [Serializable]
    public class PickleException : Exception
    {
        public PickleException() { }
        public PickleException(string message) : base(message) { }
        public PickleException(string message, Exception inner) : base(message, inner) { }
        protected PickleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class CriticalPickleException : Exception
    {
        public CriticalPickleException() { }
        public CriticalPickleException(string message) : base(message + " This should never happen!") { }
        public CriticalPickleException(string message, Exception inner) : base(message + " This should never happen!", inner) { }
        protected CriticalPickleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
