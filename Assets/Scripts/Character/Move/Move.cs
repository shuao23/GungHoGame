using System;

public abstract class Move : IMove
{
    private readonly int id;

    public bool Issued { get; private set; }

    public virtual bool InRightCondition {
        get { return OnInRightCondition == null ? false : OnInRightCondition(); }
    }

    public virtual int Id { get { return id; } }

    public Func<bool> OnInRightCondition { get; set; }


    public Move()
    {
        id = -1;
    }

    public Move(int id)
    {
        this.id = id;
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
    public bool Update(float deltaTime)
    {
        if (!Issued)
        {
            return false;
        }
        else if (!InRightCondition)
        {
            Close();
            return false;
        }
        else
        {
            NextMove(deltaTime);
            return true;
        }
    }


    protected virtual void Reset() { }

    protected abstract void NextMove(float deltaTime);
}