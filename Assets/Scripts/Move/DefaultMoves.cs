using System;
using System.Collections.Generic;

/// <summary>
/// Always issued set of moves to act as a set of default moves
/// </summary>
public class DefaultMoves : IMove
{
    private IMove current;
    private List<IMove> moves = new List<IMove>();

    public IMove Current {
        get {
            if (current == null)
            {
                current = FindFirstMoveCandidate();
                if (current != null)
                {
                    current.Issue();
                }
            }
            else if (!current.InRightCondition || !current.Issued || CurrentIsAlwaysBest)
            {
                IMove next = FindFirstMoveCandidate();
                current.Close();
                if (next != null)
                {
                    next.Issue();
                }
                current = next;
            }

            if (current == null)
            {
                throw new InvalidOperationException("Found no default moves. Default moves should cover all edge cases");
            }

            return current;
        }
    }

    public bool CurrentIsAlwaysBest { get; set; }

    public bool Issued { get; private set; }

    public bool InRightCondition {
        get { return true; }
    }

    public string Name {
        get {
            return Current.Name;
        }
    }

    public void Close()
    {
        if (Issued)
        {
            Current.Close();
            Issued = false;
        }
    }

    public void Issue(bool doOverride = false)
    {
        if (doOverride)
        {
            Close();
        }

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

        Current.TryUpdate(deltaTime);
        return true;
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
