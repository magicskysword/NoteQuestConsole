using System;
using System.Runtime.Serialization;

namespace NoteQuest.Script;

public class EvaluateException : Exception
{
    public EvaluateException()
    {
    }

    public EvaluateException(string message) : base(message)
    {
    }

    public EvaluateException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected EvaluateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}