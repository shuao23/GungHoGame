using System;
using System.Collections.Generic;

/// <summary>
/// A move made up of multiple moves. When issued, the best move is used. Does not close until none of the moves are able to.
/// </summary>
public class ParallelMoveGroup<T> : Move where T: class, IMove
{
    private T lastUsed;
    private List<T> moves = new List<T>();


    public T BestCandidate {
        get {
            for (int i = moves.Count - 1; i >= 0; i--)
            {
                if (moves[i].InRightCondition)
                { 
                    return moves[i];
                }
            }
            throw new NoMoveCandidatesException();
        }
    }

    public override bool InRightCondition {
        get { return true; }
    }

    public override string Name {
        get {
            return BestCandidate.Name;
        }
    }


    public void Register(T move)
    {
        if (move == null)
        {
            throw new ArgumentNullException("move");
        }

        moves.Add(move);
    }


    protected override void NextMove(float deltaTime)
    {
        for (int i = moves.Count - 1; i >= 0; i--)
        {
            T move = moves[i];
            move.Issue();
            if (move.Update(deltaTime))
            {
                if(lastUsed != null && move != lastUsed)
                {
                    lastUsed.Close();
                }
                lastUsed = move;
                return;
            }
        }
        throw new NoMoveCandidatesException();
    }

    protected override void Reset()
    {
        if(lastUsed != null)
        {
            lastUsed.Close();
        }
    }
}
