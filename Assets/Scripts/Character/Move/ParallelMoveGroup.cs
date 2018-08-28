using System;
using System.Collections.Generic;

/// <summary>
/// A move made up of multiple moves. When issued, the best move is used. Does not close until none of the moves are able to.
/// </summary>
public class ParallelMoveGroup : Move
{
    private IMove lastUsed;
    private List<IMove> moves = new List<IMove>();


    public IMove BestCandidate {
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

    public override int Id {
        get {
            return BestCandidate.Id;
        }
    }


    public void Register(IMove move)
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
            IMove move = moves[i];
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
