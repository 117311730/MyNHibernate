using System;
using System.Runtime.Serialization;

namespace NHibernate.Indexer {
    [Serializable]
    public class IndexException : Exception {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public IndexException() {}
        public IndexException(string message) : base(message) {}
        public IndexException(string message, Exception inner) : base(message, inner) {}

        protected IndexException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}