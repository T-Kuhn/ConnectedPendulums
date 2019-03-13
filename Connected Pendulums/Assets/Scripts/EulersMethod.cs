using System;

public class EulersMethod
{
    private Func<double, double, double> _f_y_t;
    private double _epsilon;
    public double Current_t, Current_y;
    
    public EulersMethod(double t_0 = 0.0, double y_0 = 0.0, double epsilon = 0.001)
    {
        Current_t = t_0;
        Current_y = y_0;
        _epsilon = epsilon;
    }

    public void SetupYdot(Func<double, double, double> f_y_t)
    {
        _f_y_t = f_y_t;
    }

    public void CalculateNext()
    {
        // advance the time.
        Current_t += _epsilon;
        
        // advance the y.
        Current_y = Current_y + _epsilon * _f_y_t(Current_y, Current_t);
    }
}
