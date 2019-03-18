using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DoublePendulum : MonoBehaviour
{
    [SerializeField] private Transform _joint1;
    [SerializeField] private Transform _joint2;
    [SerializeField] private Transform _endpoint1;

    [SerializeField] private float theta1;
    [SerializeField] private float theta2;
    
    public double Theta_1
    {
        set { _joint1.localRotation = Quaternion.Euler(0f, 0f, (float) (value  * 180.0 / Math.PI)); }
        get { return _joint1.localRotation.eulerAngles.z * Math.PI / 180.0; }
    }
    
    public double Theta_2
    {
        set { _joint2.localRotation = Quaternion.Euler(0f, 0f, (float) (value  * 180.0 / Math.PI)); }
        get { return _joint2.localRotation.eulerAngles.z * Math.PI / 180.0; }
    }

    private void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            // Angle setting via inspector (Debug)
            Theta_1 = theta1;
            Theta_2 = theta2;    
        }
    }

    private void LateUpdate()
    {
        // joint2 is connected to the bottom of link1 (Endpoint1).
        _joint2.position = _endpoint1.position;
    }
}
