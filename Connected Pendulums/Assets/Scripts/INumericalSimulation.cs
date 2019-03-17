using System;

// Numerical Simulation Interface
public interface INumericalSimulation
{
	double Current_y_dot { get; }
	double Current_t { get; }
	double Current_y { get; }
	
	void Setup(double t_0, double y_0, double epsilon);
	void SetupYDot(Func<double, double, double> f_y_t);
	void CalculateNext();
}
