using System;

public abstract class Move : IMove
{
    private string name;

    public bool Issued { get; private set; }

    public virtual bool InRightCondition {
        get { return OnInRightCondition == null ? false : OnInRightCondition(); }
    }

    public virtual string Name { get { return name; } }

    public Func<bool> OnInRightCondition { get; set; }


    public Move()
    {
        name = string.Empty;
    }

    public Move(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }

        this.name = name;
    }


    public void Close()
    {
        if (Issued)
        {
            Issued = false;
            Reset();
        }
    }

    public void Issue(bool doOverride = false)
    {
        if (doOverride && Issued)
        {
            Close();
        }

        Issued = true;
    }

    /// <summary>
    /// Update move
    /// </summary>
    /// <param name="deltaTime">The change in time since last update</param>
    public void Update(float deltaTime)
    {
        if (!Issued)
        {
            return;
        }
        else if (!InRightCondition)
        {
            Close();
        }
        NextMove(deltaTime);
    }


    protected virtual void Reset() { }

    protected abstract void NextMove(float deltaTime);
}