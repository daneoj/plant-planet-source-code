using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class plantScript : MonoBehaviour
{
	public float growthRate = 0.1f;
	private Light2D thisLight;
    // Start is called before the first frame update
    void Start()
    {
        thisLight = GetComponent<Light2D>();  
    }

    // Update is called once per frame
    void Update()
    {
    	if (transform.localScale.x < 6f) {
    		Vector3 scale = transform.localScale;
        scale.x += growthRate * Time.deltaTime;
        scale.y += growthRate * Time.deltaTime;
        transform.localScale = scale;
        thisLight.pointLightOuterRadius = scale.x + 3f;
    	}
        
    }
}
