using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadSim()
    {
        SceneManager.LoadScene("Sim");
    }
    
    public void ExitSim()
    {
        Application.Quit();
    }
}
