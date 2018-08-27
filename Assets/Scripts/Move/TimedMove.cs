using System;
using System.Collections.Generic;
using UnityEngine;

public class TimedMove : Move
{
    private List<MoveClip> clips = new List<MoveClip>();
    private IMove lastMove;

    public override bool InRightCondition {
        get {
            IMove current = FindCurrentMove(); 
            return current == null ? false : current.InRightCondition;
        }
    }

    public float TimeSinceStart { get; private set; }


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
        Debug.Log("reset");
        TimeSinceStart = 0;
    }

    protected override bool TryNextMove(float deltaTime)
    {
        float frameStartTime = TimeSinceStart;
        TimeSinceStart += deltaTime;
        float frameEndTime = TimeSinceStart;

        float clipEndTime = 0;
        for(int i = 0; i < clips.Count; i++)
        {
            MoveClip clip = clips[i];
            float clipStartTime = clipEndTime;
            clipEndTime += clip.Duration;

            //if past clips 
            if (frameStartTime > clipEndTime)
            {
                clip.Move.Close();
            }
            //else if current frames
            else if (!(frameEndTime < clipStartTime))
            {
                float updateStartTime = Mathf.Max(clipStartTime, frameStartTime);
                float updateEndTime = Mathf.Min(clipEndTime, frameEndTime);
                float updateLength = updateEndTime - updateStartTime;
                if (frameStartTime <= clipStartTime)
                {
                    clip.Move.Issue();
                }
                Debug.Log("doing curent");
                clip.Move.TryUpdate(updateLength); return true;
            }
        }
        return false; //reached the end of timeline
    }


    private IMove FindCurrentMove()
    {
        float clipEndTime = 0;
        for (int i = 0; i < clips.Count; i++) {
            MoveClip clip = clips[i];
            float clipStartTime = clipEndTime;
            clipEndTime += clip.Duration;

            if (TimeSinceStart >= clipStartTime && TimeSinceStart <= clipEndTime)
            {
                Debug.Log("foundgood");
                return clip.Move;
            }
        }
        Debug.Log("foundnone");
        return null;
    }
}