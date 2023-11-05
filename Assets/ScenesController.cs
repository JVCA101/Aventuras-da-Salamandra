using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(bool player2)
    {
        SceneManager.LoadScene("GameScene");
        PlayerPrefs.SetInt("player2", player2 ? 1 : 0);
    }

    public void MainTitle()
    {
        SceneManager.LoadScene("Main Title");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
