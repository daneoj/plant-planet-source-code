using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Vector3 PlanetPosition = new Vector3(0,0,1);
    public GameObject planet;
    public GameObject blast;
    public GameObject plant;
    public GameObject smoke;

    public Rigidbody2D rb2d;
    private float gravity = 30f;
    private float jetPack = 60f;
    private float moveSpd = 6.5f;
    public float pSpeed = 8.0f;
    public float projTimer;
    public float plantTimer;
    public float health;
    public Image healthBar;
    public Image ammoBar;
    public Image fuelBar;
    public int ammo;
    public int ammoMax;
    public int initAmmoMax;
    public float ammoTimer;
    private float ammoTimerMax = 0.15f;
    public float jetPackFuel;

    void Start()
    {
        projTimer = 0.3f;
        plantTimer = 0.3f;
        ammoMax = initAmmoMax;
        health = 1f;
        ammo = ammoMax;
        ammoTimer = ammoTimerMax;
        jetPackFuel = 2f;
    }


    void FixedUpdate()
    {
    	health -= 0.02f * Time.deltaTime;
    	UIupdate();
    	timerUpdate();

    	updateRotation();
    	rb2d.velocity = rb2d.velocity * 0.9f;
        if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow))
        {
        	rb2d.velocity = transform.right * -moveSpd;
        }

        if (Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow))
        {
        	rb2d.velocity = transform.right * moveSpd;
        }

        if (Input.GetKey(KeyCode.Space) && projTimer <= 0f && ammo > 0) {
        	Camera.main.GetComponent<gameManager>().playShoot();
        	projTimer = 0.15f;
        	ammo--;
        	var proj = Instantiate(blast, transform.position, transform.rotation);
        	proj.GetComponent<Rigidbody2D>().velocity = transform.up * pSpeed;
        	//proj.GetComponent<Rigidbody2D>().AddForce(transform.up * pSpeed);
        	Physics2D.IgnoreCollision(proj.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        }

        if ((Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)) && jetPackFuel > 0f) {
        	rb2d.AddForce(transform.up * jetPack * (1.0f + jetPackFuel/2f));
        	jetPackFuel -= Time.deltaTime;
        	//
        	var smokeI = Instantiate(smoke, transform.position, transform.rotation);
        	smokeI.GetComponent<Rigidbody2D>().velocity = transform.up * -10f;
        	Camera.main.GetComponent<gameManager>().playJetPack();
        }
        updateGravity();
    }

    void updateRotation() {
    	transform.up = -(planet.transform.position - transform.position);
    	//transform.Rotate(transform.forward * 90);
    }

    void updateGravity() {
    	updateRotation();
    	rb2d.AddForce(-transform.up * gravity);
    	//Vector3 gravity3d = (-transform.up * gravity);
    	//Vector2 gravity2d = gravity3d;

    	//rb2d.velocity = rb2d.velocity + gravity2d;
    }

    void OnCollisionStay2D(Collision2D col)
    {
    	
        if (col.gameObject.name=="Asteroid(Clone)") {
            Destroy(col.gameObject);
            health-=0.3f;
            Camera.main.GetComponent<gameManager>().playCollision();
            if (health < 0f)
    		{
    			healthBar.fillAmount = 0f;
    			die();
    		}
        }

        if (col.gameObject.name=="PB Asteroid(Clone)") {
            Destroy(col.gameObject);
            health-=0.6f;
            Camera.main.GetComponent<gameManager>().playCollision();
            if (health < 0f)
    		{
    			healthBar.fillAmount = 0f;
    			die();
    		}
        }

        if (col.gameObject.name=="Circle") {
        	if (jetPackFuel < 2f) {
        		jetPackFuel += Time.deltaTime;
        		if (jetPackFuel > 2f) {
        			jetPackFuel = 2f;
        		}
        	}
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
    	if (col.gameObject.name=="plant(Clone)") {
            Camera.main.GetComponent<gameManager>().addScore(col.gameObject);
            health+=0.2f + col.gameObject.transform.localScale.x*0.05f;
            jetPackFuel+=0.5f + col.gameObject.transform.localScale.x*0.15f;
            if (health > 1f) {
            	health = 1f;
            }
            if (jetPackFuel > 2f) {
            	jetPackFuel = 2f;
            }
        }
    }

    void die() {
    	Camera.main.GetComponent<gameManager>().gameOver = true;
    	Destroy(gameObject);
    }

    void UIupdate() {
        healthBar.fillAmount = health;
        ammoBar.fillAmount = (float)ammo/(float)ammoMax;
        fuelBar.fillAmount = jetPackFuel/2f; 
    }

    void timerUpdate() {
        if (health < 0f)
            {
                healthBar.fillAmount = 0f;
                die();
            }
        if (projTimer > 0f) {
            projTimer -= Time.deltaTime;
        }
        if (plantTimer > 0f) {
            plantTimer -= Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.Space)) {
            if (ammoTimer > 0f) {
                ammoTimer -= Time.deltaTime;
            }
            if (ammoTimer <= 0f) {
                ammoTimer = ammoTimerMax;
                ammo = ammoMax; // must release space to get back ammo
            }

        } else {
            ammoTimer = ammoTimerMax;
        }
    }
}

