using System;

public class RungeKutta : INumericalSimulation
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
        // advance the time
        _current_t += _epsilon;
        
        var k_1 = _epsilon * _f_y_t(_current_y, _current_t);
        var k_2 = _epsilon * _f_y_t(_current_y + k_1 / 2.0, _current_t + _epsilon / 2.0);
        var k_3 = _epsilon * _f_y_t(_current_y + k_2 / 2.0, _current_t + _epsilon / 2.0);
        var k_4 = _epsilon * _f_y_t(_current_y + k_3, _current_t + _epsilon);
        
        // calculate y dot
        _current_y_dot = 1.0 / 6.0 * (k_1 + 2 * k_2 + 2 * k_3 + k_4) / _epsilon;

        // advance the y
        _current_y = _current_y + 1.0 / 6.0 * (k_1 + 2 * k_2 + 2 * k_3 + k_4);
    }
}
