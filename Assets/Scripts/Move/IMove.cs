public interface IMove
{
    string Name { get; }
    bool Issued { get; }
    bool InRightCondition { get; }

    void Issue(bool doOverride = false);
    void Close();
    bool Update(float deltaTime);
}