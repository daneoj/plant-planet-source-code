using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class lightScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float lifeCycle = 0.5f;
    Light2D thisLight;
    void Start()
    {
    	thisLight = GetComponent<Light2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        lifeCycle -= Time.deltaTime;
        if (lifeCycle > 0.25f) {
        	thisLight.intensity = (0.5f-lifeCycle)/0.25f;
        } else if (lifeCycle > 0f) {
        	thisLight.intensity = lifeCycle/0.25f;
        } else {
        	Destroy(gameObject);
        }
    }
}
