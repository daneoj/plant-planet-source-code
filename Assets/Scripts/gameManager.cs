using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
	public GameObject player;
    public GameObject planet;
    public GameObject plant;
    public GameObject ship;
    public GameObject smoke;
    public GameObject boom;
	public float gravity = 1f;
    private float shipSpawnR = 8f;
	public float timer = 0f;
	public GameObject asteroid;
    public GameObject pbAsteroid;
	public float spawnDist = 6f;
    public float pDist = 3.5f;
    public float plantSpawnTimer;
    public float plantSpawnMax;
    public float astLimit;
    public float astGrowthRate;
    public float astMin;
    public GameObject restartScreen;
    public GameObject Light;
    public bool gameOver = false;
    public bool gameComplete = false;
    public float score = 0f;
    public Text t;
    public Text scoreText;
    public AudioSource pew;
    public AudioSource shoot;
    public AudioSource jetpack;
    public AudioSource collectPlant;
    public AudioSource collisionPlanet;
    public AudioSource collision;
    public AudioSource asteroidBoom;
    public float asteroidProb;
    public float pbAsteroidProb;
    public float shipProb;
    public string nextLevelName;
    // Start is called before the first frame update
    void Start()
    {
        //restartScreen.SetActive(false);
        astLimit = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (score < 0f) {
            score = 0f;
        }
        scoreText.text = "score: "  + (int) Mathf.Round(score);
        if (gameComplete) {
            gameCompleteHandler();
        } else if (gameOver) {
            gameOverHandler();
        } else {

            difficultyHandler();
            plantHandler();
            spawner();
            
        }
    }

    public void gameOverHandler() {
        scoreText.text = "";
            t.text = "press R to restart\n score: " + (int) Mathf.Round(score);
            if (Input.GetKey(KeyCode.R))
            {
                gameOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
    }

    public void gameCompleteHandler() {
        scoreText.text = "";
            t.text = "press R to continue";
            if (Input.GetKey(KeyCode.R))
            {
                gameOver = false;
                SceneManager.LoadScene(nextLevelName);
            }
    }

    public void spawner() {
        timer+=Time.deltaTime;
        if (timer > astLimit) {
                timer = 0f;
                float prob = Random.value;
                if (prob < asteroidProb) {
                    spawnAsteroid();
                } else if (prob < (1f-shipProb)) {
                    spawnPBasteroid();
                } else {
                    spawnShip();
                }
                
        }
    }

    public void difficultyHandler() {
        if (astLimit > astMin) {
                astLimit = 4f - score / astGrowthRate; // difficulty goes up with score.
        } else {
            astLimit = astMin;
        }

        if (score > 100) {
            gameComplete = true;
        }
    }

    public void plantHandler() {
            plantSpawnTimer += Time.deltaTime;
            if (plantSpawnTimer > plantSpawnMax) {
                plantSpawnTimer = 0f;
                float angle = Random.value * (2*Mathf.PI) - Mathf.PI;
                Vector3 spawnPos = new Vector3(Mathf.Cos(angle)*pDist, Mathf.Sin(angle)*pDist, 0);
                var pobj = Instantiate(plant, spawnPos, new Quaternion(0,0,0,0));
                pobj.transform.up = -(planet.transform.position - spawnPos);
            }
    }

    public void spawnAsteroid() {
        float angle = Random.value * (2*Mathf.PI) - Mathf.PI;
        Vector3 spawnPos = new Vector3(Mathf.Cos(angle)*spawnDist, Mathf.Sin(angle)*spawnDist, 0);
        Instantiate(asteroid, spawnPos, new Quaternion(0,0,0,0));
    }

    public void spawnPBasteroid() {
        float angle = Random.value * (2*Mathf.PI) - Mathf.PI;
        Vector3 spawnPos = new Vector3(Mathf.Cos(angle)*spawnDist, Mathf.Sin(angle)*spawnDist, 0);
        Instantiate(pbAsteroid, spawnPos, new Quaternion(0,0,0,0));
    }

    public void spawnShip() {
        float angle = Random.value * (2*Mathf.PI) - Mathf.PI;
        Vector3 spawnPos = new Vector3(Mathf.Cos(angle)*shipSpawnR, Mathf.Sin(angle)*shipSpawnR, 0);
        Instantiate(ship, spawnPos, new Quaternion(0,0,0,0));
    }


    public void doGravity(GameObject asteroid) {
    	Vector3 apos = asteroid.transform.position;
    	asteroid.transform.rotation = new Quaternion(0,0,0,0);
    	asteroid.transform.up = -(planet.transform.position - apos);
    	asteroid.GetComponent<Rigidbody2D>().velocity = asteroid.transform.up * -gravity;
    }

    public void doOrbit(GameObject ship, float angle) {
        ship.transform.position = new Vector3(Mathf.Cos(angle)*shipSpawnR, Mathf.Sin(angle)*shipSpawnR, 0);
        ship.transform.right = (planet.transform.position - ship.transform.position);
    }

    public void addScore(GameObject plant) {
        score += 1 + plant.transform.localScale.x*plant.transform.localScale.x;
        
        player.GetComponent<PlayerController>().ammoMax = player.GetComponent<PlayerController>().initAmmoMax  + (int) score / 10;
        
        playCollectPlant();
        Destroy(plant);
    }

    public void addScoreAsteroid() {
        score += 1;
        
    }

    public void addScoreShip() {
        score += 5;
        
    }

    public void subScoreAsteroid() {
        score -= 1;
    }

    public void playPew() {
        pew.Play();
    }

    public void playShoot() {
        shoot.Play();
    }

    public void playJetPack() {
        jetpack.Play();
    }

    public void playCollectPlant() {
        collectPlant.Play();
    }

    public void playPlanetDamage() {
        collisionPlanet.Play();
    }

    public void playCollision() {
        collision.Play();
    }

    public void playAsteroid() {
        asteroidBoom.Play();
    }


    public void spawnExplosion(Vector3 pos) {
        Instantiate(Light, pos, new Quaternion(0,0,0,0));

    }

    public void takeDamage() {
        player.GetComponent<PlayerController>().health -= 0.5f;
        playPlanetDamage();
    }

    public void takeDamage(float amount) {
        player.GetComponent<PlayerController>().health -= amount;
        //playPlanetDamage();
    }

    public void spawnSmoke(GameObject ship) {
        var smokeI = Instantiate(smoke, ship.transform.position, ship.transform.rotation);
        smokeI.GetComponent<Rigidbody2D>().velocity = ship.transform.right * 15f;
        playJetPack();
    }

    public void spawnBoom(GameObject spawner) {
        Instantiate(boom, spawner.transform.position, spawner.transform.rotation);
        playAsteroid();
    }
}
