using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DoublePendulum : MonoBehaviour
{
    // the two doubles below are for debug only.
    public double theta1;
    public double theta2;
    
    [SerializeField] private Transform _joint1;
    [SerializeField] private Transform _joint2;
    [SerializeField] private Transform _endpoint1;
    
    public double Theta_1
    {
        set { _joint1.localRotation = Quaternion.Euler(0f, 0f, (float) value); }
    }
    
    public double Theta_2
    {
        set { _joint2.localRotation = Quaternion.Euler(0f, 0f, (float) value); }
    }

    private void LateUpdate()
    {
        // Angle setting via inspector (Debug)
        Theta_1 = theta1;
        Theta_2 = theta2;
        
        // joint2 is connected to the bottom of link1 (Endpoint1).
        _joint2.position = _endpoint1.position;
    }
}
