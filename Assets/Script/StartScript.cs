using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScript : MonoBehaviour
{
    public void GameStart()
    {
        StartCoroutine("LoadLevel");
    }

    IEnumerator LoadLevel()
    {
        float fadeTime = GameObject.Find("SceneManager").GetComponent<SceneChange>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene("GNP");
    }
}
