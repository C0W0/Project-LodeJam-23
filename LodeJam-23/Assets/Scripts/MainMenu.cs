using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Load the main scene when the start button is pressed
    public static void OnStartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
