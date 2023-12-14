using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject grid;
    public void gameStart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Update()
    {
        grid.transform.RotateAround(new Vector3(0f,0f,0f),Vector3.forward, 30f * Time.deltaTime);
    }
}
