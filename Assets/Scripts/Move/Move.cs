using System;

public abstract class Move : IMove
{
    public bool Issued { get; private set; }

    public virtual bool InRightCondition {
        get { return OnInRightCondition == null ? false : OnInRightCondition(); }
    }

    public string Name { get; set; }

    public Func<bool> OnInRightCondition { get; set; }


    public Move()
    {
        Name = string.Empty;
    }


    public void Close()
    {
        if (Issued)
        {
            Reset();
            Issued = false;
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
    /// <returns>True if the update suceeds</returns>
    public bool TryUpdate(float deltaTime)
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
        else if (TryNextMove(deltaTime))
        {
            return true;
        }
        else
        {
            Close();
            return false;
        }
    }


    protected virtual void Reset() { }

    protected abstract bool TryNextMove(float deltaTime);
}