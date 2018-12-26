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
            //_particleSystem.Play(true);
            this.gameObject.SetActive(true);
        }
        

     
       // StartCoroutine(FadeIn());
    }

    public void StopEngine()
    {
        if (_engineState)
        {
            _engineState = false;
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        
        //_particleSystem.Stop();
        //StartCoroutine(FadeOut());
    }

    private float _particleLength = 2.0f;

    IEnumerator FadeIn()
    {
        var aTime = _particleLength;
        
        //_particleSystem.gameObject.SetActive(true);

        var main = _particleSystem.main;
        main.startLifetimeMultiplier = 0;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            if (!_engineState) yield break;
            main.startLifetimeMultiplier = t;
            yield return null;
        }

        yield return null;
    }

    IEnumerator FadeOut()
    {
        var aTime = _particleLength;


        var main = _particleSystem.main;
        main.startLifetimeMultiplier = aTime;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            if (_engineState) yield break;
            main.startLifetimeMultiplier = aTime - t;
            yield return null;
        }

        //_particleSystem.Stop();
        yield return null;
    }

    public void OnDrawGizmos()
    {
        var origColor = Gizmos.color;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, transform.localScale.magnitude / 2);

        Gizmos.color = origColor;
    }
}
