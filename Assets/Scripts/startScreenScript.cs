using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class startScreenScript : MonoBehaviour
{

	public GameObject MainMenu;
	public GameObject Credits;
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
        Credits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
        	//SceneManager.LoadScene("Level 1");
        }
    }

    public void clickPlay() {
    	SceneManager.LoadScene("Level 1");
    }

    public void clickCredits() {
    	MainMenu.SetActive(false);
        Credits.SetActive(true);
    }

    public void clickBack() {
    	MainMenu.SetActive(true);
        Credits.SetActive(false);
    }
}
