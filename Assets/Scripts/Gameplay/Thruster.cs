using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    [SerializeField] private float flickerRate = 0.05f;
    [SerializeField] private float flickerMin = 1f;
    [SerializeField] private float flickerMax = 2f;
        
    private float flickerTime;
    
    // Update is called once per frame
    void Update()
    {
        flickerTime -= Time.deltaTime;
        if (flickerTime <= 0)
        {
            transform.localScale = new Vector3(Random.Range(flickerMin,flickerMax),1f,1f);
            flickerTime += flickerRate;
        }
    }
}
