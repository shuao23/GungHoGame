using System;

public class NoMoveCandidatesException : Exception
{
    public NoMoveCandidatesException() { }
    public NoMoveCandidatesException(string message) : base(message) { }
    public NoMoveCandidatesException(string message, Exception inner) : base(message, inner) { }
}
