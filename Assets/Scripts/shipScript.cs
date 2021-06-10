using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class shipScript : MonoBehaviour
{
	// make transform.right point toward planet, move left and right radially around the planet,
	// shoot down laser balls periodically.
	private float scaleGoal = 1.2f;
    private float growthRate = 0.05f;
    private float timer = 0f;
    private float orbitMax = 3f;
    private float angle;
    private float angRate;
    private float angMax = 0.3f;
    private float shootingTime = 1f;
    private float stunTime = 1f;
    private float dps = 0.1f;
    private SpriteRenderer sprite;
    private int health = 8;
    Light2D thisLight;

    // Start is called before the first frame update

    enum shipState {
        starting,
        orbiting,
        shooting,
        damage
    }

    shipState state = shipState.starting;

    void Start()
    {
        thisLight = GetComponent<Light2D>(); 
        sprite = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        angle = -100f;
        angRate = 0f;
        orbit();
    }

    // Update is called once per frame
    void Update()
    {
    	if (angle == -100f) angle = Random.value * (2*Mathf.PI) - Mathf.PI;
        doAction();
    }

    void doAction() {
        switch (state) {
            case shipState.starting:
                if (transform.localScale.x <= scaleGoal) {
                        Vector3 scale = transform.localScale;
                        transform.localScale = scale + new Vector3(growthRate, growthRate, 0f);
                    } else {
                        transform.localScale = new Vector3(scaleGoal,scaleGoal,1f);
                        startOrbit();
                    }
                break;
            case shipState.orbiting:
                if (timer > 0f) {
                    timer -= Time.deltaTime;
                    orbit();
                } else {
                    state = shipState.shooting;
                    timer = shootingTime;
                }
                break;
            case shipState.shooting:
                if (timer > 0f) {
                    timer -= Time.deltaTime;
                    Camera.main.GetComponent<gameManager>().spawnSmoke(gameObject);
                    Camera.main.GetComponent<gameManager>().takeDamage(dps*Time.deltaTime);
                } else {
                    startOrbit();
                }
                break;
            case shipState.damage:
                if (timer > 0f) {
                    timer -= Time.deltaTime;
                    thisLight.intensity = 0.3f;
                } else {
                    thisLight.intensity = 1f;
                    startOrbit();
                }
                break;
            default:
                break;
        }
    }

    void orbit() {
        //update angle
        angle += angRate * Time.deltaTime;
        if (angle > 2*Mathf.PI) {
            angle = 0f;
        } else if (angle < 0f) {
            angle = 2*Mathf.PI;
        }
        //
        Camera.main.GetComponent<gameManager>().doOrbit(gameObject, angle);
    }

    void startOrbit() {
        state = shipState.orbiting;
        timer = 1.5f + Random.value * orbitMax; // min = 1.5s
        angRate = Random.value * 2 * angMax - angMax;
    }

    void startDamage() {
        timer = stunTime;
        if (state != shipState.starting) {
        	state = shipState.damage;
        }
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name=="WaterBlast(Clone)") {
            Destroy(col.gameObject);
            Camera.main.GetComponent<gameManager>().playPew();
            health -= 1;
            if (health == 0) {
                doDeath();
            }
            startDamage();
        }
    }

    void doDeath() {
        Camera.main.GetComponent<gameManager>().addScoreShip();
        Camera.main.GetComponent<gameManager>().spawnBoom(gameObject);
        Destroy(gameObject);
    }

}
