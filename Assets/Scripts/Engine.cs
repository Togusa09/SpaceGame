using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class Engine : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool _engineState;

    public void StartEngine()
    {
        if (!_engineState)
        {
            _engineState = true;
            this.gameObject.SetActive(true);
        }
    }

    public void StopEngine()
    {
        if (_engineState)
        {
            _engineState = false;
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private float _particleLength = 2.0f;


    public void OnDrawGizmos()
    {
        var origColor = Gizmos.color;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, transform.localScale.magnitude / 2);

        Gizmos.color = origColor;
    }
}
