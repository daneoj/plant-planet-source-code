using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class jetsmokeScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float life = 0.5f;
    Light2D light;
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life < 0f) {
        	Destroy(gameObject);
        }
        light.intensity = life/2f;
        float x = transform.localScale.x * life/0.5f;
        float y = transform.localScale.y * life/0.5f;
        transform.localScale = new Vector3(x,y,1f);
    }
}
