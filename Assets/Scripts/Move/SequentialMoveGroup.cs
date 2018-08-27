using System;
using System.Collections.Generic;
using UnityEngine;

public class SequentialMoveGroup : Move
{
    private List<MoveClip> clips = new List<MoveClip>();

    public override bool InRightCondition {
        get {
            IMove current = FindCurrentMove();
            return current == null ? false : current.InRightCondition;
        }
    }

    public float TimeSinceStart { get; private set; }


    public SequentialMoveGroup(string name) : base(name) { }


    public void Register(MoveClip clip)
    {
        if (clip == null)
        {
            throw new ArgumentNullException("clip");
        }

        if (clip.Move == null)
        {
            throw new ArgumentNullException("clip.Move");
        }

        clips.Add(clip);
    }


    protected override void Reset()
    {
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
            MoveClip clip = clips[i];
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
        //No more clips to be played so close
        Close();
    }


    private IMove FindCurrentMove()
    {
        float clipEndTime = 0;
        for (int i = 0; i < clips.Count; i++)
        {
            MoveClip clip = clips[i];
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