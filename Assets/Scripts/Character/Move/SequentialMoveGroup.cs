using System;
using System.Collections.Generic;
using UnityEngine;

public class SequentialMoveGroup : Move, ITimedMove
{
    private List<IMoveClip> clips = new List<IMoveClip>();


    public IMove CurrentMove {
        get {
            float clipEndTime = 0;
            for (int i = 0; i < clips.Count; i++)
            {
                IMoveClip clip = clips[i];
                float clipStartTime = clipEndTime;
                clipEndTime += clip.Duration;

                if (TimeSinceStart >= clipStartTime && TimeSinceStart <= clipEndTime)
                {
                    return clip.Move;
                }
            }
            return null;
        }
    }

    public override bool InRightCondition {
        get {
            IMove current = CurrentMove;
            return current == null ? false : current.InRightCondition;
        }
    }

    public float TimeSinceStart { get; private set; }

    public float Duration {
        get {
            float duration = 0;
            for (int i = 0; i < clips.Count; i++)
            {
                duration += clips[i].Duration;
            }
            return duration;
        }
    }

    public SequentialMoveGroup(int id) : base(id) { }


    public void Register(ITimedMove move)
    {
        if (move == null)
        {
            throw new ArgumentNullException("move");
        }

        clips.Add(new TimedMoveClip(move));
    }

    public void Register(IMove move, float duration)
    {
        if (move == null)
        {
            throw new ArgumentNullException("move");
        }

        clips.Add(new MoveClip(move, duration));
    }


    protected override void Reset()
    {
        IMove current = CurrentMove;
        if (current != null)
        {
            current.Close();
        }
        TimeSinceStart = 0;
        
    }

    protected override void NextMove(float deltaTime)
    {
        float frameStartTime = TimeSinceStart;
        TimeSinceStart += deltaTime;
        float frameEndTime = TimeSinceStart;

        float clipEndTime = 0;
        for (int i = 0; i < clips.Count; i++)
        {
            IMoveClip clip = clips[i];
            float clipStartTime = clipEndTime;
            clipEndTime += clip.Duration;

            //if past clips 
            if (frameStartTime > clipEndTime)
            {
                clip.Move.Close();
            }
            //else if future clips
            else if (frameEndTime < clipStartTime)
            {
                return;
            }
            //else if current clip
            else if (!(frameEndTime < clipStartTime))
            {
                float updateStartTime = Mathf.Max(clipStartTime, frameStartTime);
                float updateEndTime = Mathf.Min(clipEndTime, frameEndTime);
                float updateLength = updateEndTime - updateStartTime;
                if (frameStartTime <= clipStartTime)
                {
                    clip.Move.Issue();
                }

                if (!clip.Move.Update(updateLength))
                {
                    Close();
                }
            }
        }

        //No more clips remaining
        CloseLastClip();
        Close();
    }


    private void CloseLastClip()
    {
        if (clips.Count > 0)
        {
            clips[clips.Count - 1].Move.Close();
        }
    }


    private interface IMoveClip
    {
        IMove Move { get; }
        float Duration { get; }
    }

    private struct MoveClip : IMoveClip
    {
        public IMove Move { get; set; }
        public float Duration { get; set; }

        public MoveClip(IMove move, float duration)
        {
            Move = move;
            Duration = duration;
        }
    }

    private struct TimedMoveClip : IMoveClip
    {
        public ITimedMove TimedMove { get; set; }
        public IMove Move {
            get { return TimedMove; }
        }
        public float Duration {
            get { return TimedMove.Duration; }
        }

        public TimedMoveClip(ITimedMove move)
        {
            TimedMove = move;
        }
    }
}