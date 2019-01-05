using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public int Damage;

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
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}
