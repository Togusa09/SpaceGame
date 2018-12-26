using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LimitLifespan());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LimitLifespan()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
