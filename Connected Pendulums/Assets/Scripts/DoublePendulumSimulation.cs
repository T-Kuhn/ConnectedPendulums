using UnityEngine;

public class DoublePendulumSimulation : MonoBehaviour
{
	private EulersMethod _eulersMethod;

	private void Start()
	{
		var g = 9.81;
		var k = 0.000007757;
		var m = 0.00007;
		var epsilon = 0.01;

		_eulersMethod = new EulersMethod(
			f_y_t: (double y, double t) =>  (g - k / m * y * y),
			epsilon: epsilon
			);
	}

	private void FixedUpdate()
	{
		_eulersMethod.CalculateNext();
		
		Debug.Log("t: " + _eulersMethod.Current_t);
		Debug.Log("y: " + _eulersMethod.Current_y);
	}
}
