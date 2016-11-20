using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    public GameObject MainTower_My;

    public GameObject MainTower_Enemy;

    bool state = false;

	// Update is called once per frame
	void Update ()
    {
	    if(!MainTower_My)
        {
            state = true;
            Time.timeScale = 0.0f;
        }

        if (!MainTower_Enemy)
        {
            state = true;
            Time.timeScale = 0.0f;
        }
    }

    void OnGUI()
    {
        if(state == true)
        {
            if (GUI.Button(new Rect(400, 250, 300, 40), "Restart"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1.0f;
            }
        }
    }
}
