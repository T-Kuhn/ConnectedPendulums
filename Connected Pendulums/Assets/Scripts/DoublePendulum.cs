using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class DoublePendulum : MonoBehaviour
{
    [SerializeField] private Transform _joint1;
    [SerializeField] private Transform _joint2;
    [SerializeField] private Transform _endpoint1;

    [SerializeField] private float _theta1;
    [SerializeField] private float _theta2;
    
    [SerializeField] private float _link1Length;
    [SerializeField] private float _link2Length;
    [SerializeField] private float _mass1;
    [SerializeField] private float _mass2;
    
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
    
    public float Link1Length
    {
        set { _joint1.localScale = new Vector3(1f, value, 1f); }
        get { return _joint1.localScale.y; }
    }
    
    public float Link2Length
    {
        set { _joint2.localScale = new Vector3(1f, value, 1f); }
        get { return _joint2.localScale.y; }
    }
    
    public float Mass1 
    {
        set { _mass1 = value; }
        get { return _mass1; }
    }
    
    public float Mass2
    {
        set { _mass2 = value; }
        get { return _mass2; }
    }
    

    private void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            // Angle setting via inspector
            Theta_1 = _theta1;
            Theta_2 = _theta2;    
            
            // Length setting via inspector
            Link1Length = _link1Length;
            Link2Length = _link2Length;
        }
    }

    private void LateUpdate()
    {
        // joint2 is connected to the bottom of link1 (Endpoint1).
        _joint2.position = _endpoint1.position;
    }
}
