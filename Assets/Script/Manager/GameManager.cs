using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class GameManager : MonoBehaviour
{
    public GameObject Score;
    public Text timeTxt;
    float alive = 0f;
    void Start()
    {
    }

    void Update()
    {


    }
    public void GameOver()
    {
        alive += Time.deltaTime;
        timeTxt.text = alive.ToString("N2");
        Time.timeScale = 0f;
        Score.SetActive(true);
    }
}
