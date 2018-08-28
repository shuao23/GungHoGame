using System;

public interface IMove
{
    int Id { get; }
    bool Issued { get; }
    bool InRightCondition { get; }

    event EventHandler OnMoveStart;
    event EventHandler OnMoveEnd;

    void Issue(bool doOverride = false);
    void Close();
    bool Update(float deltaTime);
}