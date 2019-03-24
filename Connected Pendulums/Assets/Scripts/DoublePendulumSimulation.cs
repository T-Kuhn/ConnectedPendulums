using UnityEngine;
using System;

public class DoublePendulumSimulation : MonoBehaviour
{
	[SerializeField] private DoublePendulum _doublePendulum;

	private double _m_1 = 1.0;
	private double _m_2 = 1.0;
	private double _l_1 = 0.5;
	private double _l_2 = 1.0;
	[SerializeField] private double _g = 9.81;

	private INumericalSimulation _NumericalSimLambda1;
	private INumericalSimulation _NumericalSimuLambda2;
	private INumericalSimulation _NumericalSimTheta1;
	private INumericalSimulation _NumericalSimTheta2;

	[SerializeField] double _epsilon;
	[SerializeField] int _iterations;

	[SerializeField] private double _kineticEnergy;
	[SerializeField] private double _potentialEnergy;
	[SerializeField] private double _totalEnergy;

	enum NumericalSimulationType
	{
		EulersMethod,
		RungeKutta
	}

	[SerializeField] NumericalSimulationType _numericalSimulationType;

	private void Start()
	{
		// get initial state
		var initialTheta1 = _doublePendulum.Theta_1;
		var initialTheta2 = _doublePendulum.Theta_2;

		_l_1 = _doublePendulum.Link1Length;
		_l_2 = _doublePendulum.Link2Length;

		_m_1 = _doublePendulum.Mass1;
		_m_2 = _doublePendulum.Mass2;

		_NumericalSimLambda1 = GetNumericalSimulator();
		_NumericalSimLambda1.Setup(t_0: 0.0, y_0: 0.0, epsilon: _epsilon);

		_NumericalSimuLambda2 = GetNumericalSimulator();
		_NumericalSimuLambda2.Setup(t_0: 0.0, y_0: 0.0, epsilon: _epsilon);

		_NumericalSimTheta1 = GetNumericalSimulator();
		_NumericalSimTheta1.Setup(t_0: 0.0, y_0: initialTheta1, epsilon: _epsilon);

		_NumericalSimTheta2 = GetNumericalSimulator();
		_NumericalSimTheta2.Setup(t_0: 0.0, y_0: initialTheta2, epsilon: _epsilon);

		// eq:9
		/*
		\dot{\lambda}_1 =
		\frac
		{
			- m_2 l_2 \dot{\lambda}_2 \cos(\theta_1 - \theta_2)
			- m_2 l_2 \lambda^2_2 \sin(\theta_1 - \theta_2)
			- g (m_1 + m_2) \sin \theta_1
		}
		{
			(m_1 + m_2) l_1
		}
		*/
		_NumericalSimLambda1.SetupYDot((double lambda1, double t) =>
			(-_m_2 * _l_2 * _NumericalSimuLambda2.Current_y_dot *
			 Math.Cos(_NumericalSimTheta1.Current_y - _NumericalSimTheta2.Current_y)
			 - _m_2 * _l_2 * _NumericalSimuLambda2.Current_y * _NumericalSimuLambda2.Current_y *
			 Math.Sin(_NumericalSimTheta1.Current_y - _NumericalSimTheta2.Current_y)
			 - _g * (_m_1 + _m_2) * Math.Sin(_NumericalSimTheta1.Current_y))
			/
			((_m_1 + _m_2) * _l_1));

		// label{eq:10}
		// \dot{\theta}_1 = \lambda_1 
		_NumericalSimTheta1.SetupYDot((double theta1, double t) => _NumericalSimLambda1.Current_y);

		// eq:11
		/*
		\dot{\lambda}_2 = 
		\frac
		{
			m_2 l_1 \lambda^2_1 sin(\theta_1 - \theta_2)
			- m_2 l_1 \dot{\lambda}_1 cos(\theta_1 - \theta_2)
			- m_2 g \sin \theta_2
		}
		{
			m_2 l_2
		} 
		
		*/
		_NumericalSimuLambda2.SetupYDot((double lambda2, double t) =>
			(_m_2 * _l_1 * _NumericalSimLambda1.Current_y * _NumericalSimLambda1.Current_y *
			 Math.Sin(_NumericalSimTheta1.Current_y - _NumericalSimTheta2.Current_y)
			 - _m_2 * _l_1 * _NumericalSimLambda1.Current_y_dot *
			 Math.Cos(_NumericalSimTheta1.Current_y - _NumericalSimTheta2.Current_y)
			 - _m_2 * _g * Math.Sin(_NumericalSimTheta2.Current_y))
			/
			(_m_2 * _l_2));

		// no label (not written about this one yet)
		// \dot{\theta}_2 = \lambda_2 
		_NumericalSimTheta2.SetupYDot((double theta2, double t) => _NumericalSimuLambda2.Current_y);

	}

	private INumericalSimulation GetNumericalSimulator()
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
			_NumericalSimTheta1.CalculateNext();
			_NumericalSimTheta2.CalculateNext();

			_NumericalSimLambda1.CalculateNext();
			_NumericalSimuLambda2.CalculateNext();
		}

		// potential energy is:
		// V_p = - (m_1 + m_2) * g * l_1 * cos(\theta_1) - m_2 * g * l_2 * cos(\theta2)
		_potentialEnergy = -(_m_1 + _m_2) * _g * _l_1 * Math.Cos(_NumericalSimTheta1.Current_y)
		                   - _m_2 * _g * _l_2 * Math.Cos(_NumericalSimTheta2.Current_y);

		// lambda is the speed of the rotation in 2PI/sec
		// V_k = 1/2 * m_1 * l_1^2 * Lamba_1^2 + 1/2 * m_2 * (l_1^2 * Lambda_1^2 + l_2^2 * Lambda_2^2 + 2* l_1 * l_2 * lambda1 * lambda2 * Math.Cos(theta1 - theta2))
		_kineticEnergy = 0.5 * _m_1 * _l_1 * _l_1 * _NumericalSimLambda1.Current_y * _NumericalSimLambda1.Current_y +
		                 0.5 * _m_2 * (_l_1 * _l_1 * _NumericalSimLambda1.Current_y * _NumericalSimLambda1.Current_y +
		                               _l_2 * _l_2 * _NumericalSimuLambda2.Current_y * _NumericalSimuLambda2.Current_y +
		                               2 * _l_1 * _l_2 * _NumericalSimLambda1.Current_y *
		                               _NumericalSimuLambda2.Current_y *
		                               Math.Cos(_NumericalSimTheta1.Current_y - _NumericalSimTheta2.Current_y));

		_totalEnergy = _kineticEnergy + _potentialEnergy;

		_doublePendulum.Theta_1 = _NumericalSimTheta1.Current_y;
		_doublePendulum.Theta_2 = _NumericalSimTheta2.Current_y;
	}
}
