using UnityEngine;

public class TestSimulation : MonoBehaviour
{
	private INumericalSimulation _numericalSimulation;

	[SerializeField] double _epsilon;
	[SerializeField] int _iterations;

	enum NumericalSimulationType
	{
		EulersMethod,
		RungeKutta
	}

	[SerializeField] NumericalSimulationType _numericalSimulationType;

	private void Start()
	{
		_numericalSimulation = GetSimulation();
		_numericalSimulation.Setup(0.0, 0.5, _epsilon);
		_numericalSimulation.SetupYDot(f_y_t: (double y, double t) => 2 * y / (1 + t));
	}

	private INumericalSimulation GetSimulation()
	{
		switch (_numericalSimulationType)
		{
			case NumericalSimulationType.EulersMethod:
				return new EulersMethod();

			case NumericalSimulationType.RungeKutta:
				return new RungeKutta();

			default:
				return new EulersMethod();
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < _iterations; i++)
		{
			_numericalSimulation.CalculateNext();
		}

		Debug.Log("t: " + _numericalSimulation.Current_t);
		Debug.Log("y: " + _numericalSimulation.Current_y);
	}
}
