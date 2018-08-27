using System;
using System.Collections.Generic;

/// <summary>
/// A move made up of multiple moves. When issued, the best move is used. Does not close until none of the moves are able to.
/// </summary>
public class MoveGroup : IMove
{
    private List<IMove> moves = new List<IMove>();

    public IMove Current { get; private set; }

    public bool Issued { get; private set; }

    public bool InRightCondition {
        get {
            return FindFirstMoveCandidate() != null;
        }
    }

    public string Name {
        get {
            return Current == null ? string.Empty : Current.Name;
        }
    }

    public void Close()
    {
        if (Issued)
        {
            if(Current != null)
            {
                Current.Close();
            }
            Issued = false;
        }
    }

    public void Issue(bool doOverride = false)
    {
        if (doOverride)
        {
            Close();
        }

        Current = null;
        Issued = true;
    }

    public void Register(IMove move)
    {
        if (move == null)
        {
            throw new ArgumentNullException("move");
        }

        moves.Add(move);
    }

    public bool TryUpdate(float deltaTime)
    {
        if (!Issued)
        {
            return false;
        }
        
        if(Current == null)
        {
            Current = FindFirstMoveCandidate();
            if(Current == null)
            {
                Close();
                return false;
            }
            Current.Issue();
        }


        if (Current.TryUpdate(deltaTime))
        {
            return true;
        }
        else
        {
            Close();
            return false;
        }

    }

    private IMove FindFirstMoveCandidate()
    {
        for (int i = moves.Count - 1; i >= 0; i--)
        {
            if (moves[i].InRightCondition)
            {
                return moves[i];
            }
        }
        return null;
    }
}
