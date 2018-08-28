public interface ITimedMove : IMove
{
    float Duration { get; }
    float TimeSinceStart { get; }
}