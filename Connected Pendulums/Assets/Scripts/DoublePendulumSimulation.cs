using UnityEngine;
using System;

public class DoublePendulumSimulation : MonoBehaviour
{
	[SerializeField] private DoublePendulum _doublePendulum;
	
	private EulersMethod _eulersMethodLambda1;
	private EulersMethod _eulersMethodLambda2;
	private EulersMethod _eulersMethodTheta1;
	private EulersMethod _eulersMethodTheta2;

	private void Start()
	{
		var g = 9.81;

		var m_1 = 1.0;
		var m_2 = 5.0;
		var l_1 = 1.0;
		var l_2 = 1.0;

		var epsilon = 0.0001;

		_eulersMethodLambda1 = new EulersMethod(t_0: 0.0, y_0: 0.0, epsilon: epsilon);
		_eulersMethodLambda2 = new EulersMethod(t_0: 0.0, y_0: 0.0, epsilon: epsilon);
		_eulersMethodTheta1 = new EulersMethod(t_0: 0.0, y_0: Math.PI/2.0, epsilon: epsilon);
		_eulersMethodTheta2 = new EulersMethod(t_0: 0.0, y_0: Math.PI/2.0, epsilon: epsilon);

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
		_eulersMethodLambda1.SetupYdot((double lambda1, double t) =>
			(-m_2 * l_2 * _eulersMethodLambda2.Current_y_dot *
			 Math.Cos(_eulersMethodTheta1.Current_y - _eulersMethodTheta2.Current_y)
			 - m_2 * l_2 * _eulersMethodLambda2.Current_y * _eulersMethodLambda2.Current_y *
			 Math.Sin(_eulersMethodTheta1.Current_y - _eulersMethodTheta2.Current_y)
			 - g * (m_1 + m_2) * Math.Sin(_eulersMethodTheta1.Current_y))
			/
			((m_1 + m_2) * l_1));

		// label{eq:10}
		// \dot{\theta}_1 = \lambda_1 
		_eulersMethodTheta1.SetupYdot((double theta1, double t) => _eulersMethodLambda1.Current_y);

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
		_eulersMethodLambda2.SetupYdot((double lambda2, double t) =>
			(m_2 * l_1 * _eulersMethodLambda1.Current_y * _eulersMethodLambda1.Current_y *
			Math.Sin(_eulersMethodTheta1.Current_y - _eulersMethodTheta2.Current_y)
			- m_2 * l_1 * _eulersMethodLambda1.Current_y_dot * Math.Cos(_eulersMethodTheta1.Current_y - _eulersMethodTheta2.Current_y)
			- m_2 * g * Math.Sin(_eulersMethodTheta2.Current_y))
			/
			(m_2 * l_2));

		// no label (not written about this one yet)
		// \dot{\theta}_2 = \lambda_2 
		_eulersMethodTheta2.SetupYdot((double theta2, double t) => _eulersMethodLambda2.Current_y);
		
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < 150; i++)
		{
            _eulersMethodLambda1.CalculateNext();
            _eulersMethodLambda2.CalculateNext();
            
            _eulersMethodTheta1.CalculateNext();
            _eulersMethodTheta2.CalculateNext();	
		}

		_doublePendulum.Theta_1 = _eulersMethodTheta1.Current_y * 180.0/Math.PI;
		_doublePendulum.Theta_2 = _eulersMethodTheta2.Current_y * 180.0/Math.PI;
	}
}
