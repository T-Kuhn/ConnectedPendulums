using System;

public class EulersMethod : INumericalSimulation
{
    public double Current_t => _current_t;
    public double Current_y => _current_y;
    public double Current_y_dot => _current_y_dot;
    
    private Func<double, double, double> _f_y_t;
    private double _epsilon, _current_t, _current_y, _current_y_dot;
    
    public void Setup(double t_0 = 0.0, double y_0 = 0.0, double epsilon = 0.001)
    {
        _current_t = t_0;
        _current_y = y_0;
        _current_y_dot = 0.0;
        _epsilon = epsilon;
    }

    public void SetupYDot(Func<double, double, double> f_y_t)
    {
        _f_y_t = f_y_t;
    }

    public void CalculateNext()
    {
        // calculate y dot
        _current_y_dot = _f_y_t(_current_y, _current_t);
        
        // advance the y
        _current_y = _current_y + _epsilon * _current_y_dot;
        
        // advance the time
        _current_t += _epsilon;
    }
}
