using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgLight : MonoBehaviour
{
    // Start is called before the first frame update
    private float randx;
    private float randy;
    void Start()
    {
    	randx = Random.Range(-30,30);
    	randy = Random.Range(-30,30);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.PingPong(randx+3*Time.time, 40)-20f, Mathf.PingPong(randy+3*Time.time, 25)-12.5f, 1);
    }
}
