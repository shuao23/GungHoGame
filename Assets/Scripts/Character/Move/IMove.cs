public interface IMove
{
    int Id { get; }
    bool Issued { get; }
    bool InRightCondition { get; }

    void Issue(bool doOverride = false);
    void Close();
    bool Update(float deltaTime);
}