using System;

public class StandardMove<T, S> : Move, ITimedMove
    where T : Motor<S> 
    where S : class
{
    public float Duration { get; set; }
    public float TimeSinceStart { get; private set; }

    public T Motor { get; private set; }
    public S Stats { get; private set; }

    public Action<T, S> OnMotorSetup { get; set; }
    public Action<T, S> DefaultOnMotorSetup { get; private set; }
    public Action<T, S> OnPostMotorUpdate { get; set; }

    public StandardMove(string name, T motor, S stats) : base(name)
    {
        if (motor == null)
        {
            throw new ArgumentNullException("motor");
        }

        if(stats == null)
        {
            throw new ArgumentNullException("stats");
        }

        Motor = motor;
        Stats = stats;
        Duration = float.PositiveInfinity;

        OnMotorSetup = DefaultOnMotorSetup = (T m, S s) =>
        {
            m.Stats = s;
        };

    }


    protected override void Reset()
    {
        TimeSinceStart = 0;
    }

    protected override void NextMove(float deltaTime)
    {
        UnityEngine.Debug.Log(InRightCondition);
        float frameStartTime = TimeSinceStart;
        TimeSinceStart += deltaTime;

        bool lastUpdate = TimeSinceStart >= Duration;

        if(OnMotorSetup != null)
        {
            OnMotorSetup(Motor, Stats);
        }

        float updateTime = deltaTime;
        if (lastUpdate)
        {
            updateTime = Duration - frameStartTime;
        }
        Motor.Update(updateTime);

        if(OnPostMotorUpdate != null)
        {
            OnPostMotorUpdate(Motor, Stats);
        }

        if (lastUpdate)
        {
            Close();
        }
    }
}