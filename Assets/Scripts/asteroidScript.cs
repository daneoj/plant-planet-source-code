using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class asteroidScript : MonoBehaviour
{
	//Rigidbody2D rb2d;
	public float gravity = 1f;
	public int startingHealth = 5;
    private int health;
    public int planetDamage = 0;
    Light2D thisLight;
    
    // Start is called before the first frame update
    void Start()
    {
        health = startingHealth;
        thisLight = GetComponent<Light2D>();
        //Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.GetComponent<gameManager>().doGravity(gameObject);
    }

    void OnCollisionStay2D(Collision2D col)
    {
    	if (col.gameObject.name=="WaterBlast(Clone)")
    	{
    		Destroy(col.gameObject);
    		health--;
            Camera.main.GetComponent<gameManager>().playPew();
            thisLight.intensity = 0.25f + (float) health / (float) startingHealth;
    	}
    	if (health < 1)
    	{
            Camera.main.GetComponent<gameManager>().addScoreAsteroid();
            Camera.main.GetComponent<gameManager>().spawnBoom(gameObject);
    		Destroy(gameObject);


    	}
        if (col.gameObject.name=="Circle") {
            //Camera.main.GetComponent<gameManager>().subScoreAsteroid();
            // only have planetBusters subtract score, perhaps.
            Camera.main.GetComponent<gameManager>().spawnExplosion(transform.position);
            if (planetDamage == 1) {
                Camera.main.GetComponent<gameManager>().takeDamage();
            }
            Camera.main.GetComponent<gameManager>().spawnBoom(gameObject);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        //Debug.Log("TRIGGER!");
        if (col.gameObject.name == "plant(Clone)") {
            Destroy(col.gameObject);
            //Destroy(gameObject);
        }
    }
}
