using System;

public class StandardMove<T, S> : Move 
    where T : Motor<S> 
    where S : class
{
    public bool Continous { get; set; }
    public T Motor { get; private set; }
    public S Stats { get; private set; }
    public Action<T, S> OnMotorSetup { get; set; }
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
        Continous = true;
    }

    protected override void NextMove(float deltaTime)
    {
        if(OnMotorSetup != null)
        {
            OnMotorSetup(Motor, Stats);
        }
        Motor.Update(deltaTime);
        if(OnPostMotorUpdate != null)
        {
            OnPostMotorUpdate(Motor, Stats);
        }
        if (!Continous)
        {
            Close();
        }
    }
}