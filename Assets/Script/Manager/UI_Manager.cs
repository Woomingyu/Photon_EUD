using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager I;
    public GameObject hpimage;
    public Image image;
    public GameObject player;
    public GameObject Retrybtn;
    public float realTime;

    public float HP_full;
    public float HP;

    

    void Awake()
    {
        Time.timeScale = 1.0f;
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
    }

    void Start()
    {
        image = hpimage.transform.GetComponent<Image>();
        /*
        if (SceneManager.GetActiveScene().name == "TestScene")
        {
            Instantiate(player);
        }
        */
    }

    void Update()
    {
        realTime += Time.deltaTime;
        HP -= Time.deltaTime + (realTime * 0.0001f);
        image.fillAmount = (HP / HP_full);

        if (HP <= 0)
        {
            gameOver();
        }

        
    }
    /*
    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadedsceneEvent;
    }
    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "TestScene")
        {
            Instantiate(player);
        }
    }
    */

    public void HP_Add(int XP)
    {
        HP += XP;
        if (HP_full < HP)
        {
            HP = HP_full;
        }
    }

    public void HP_minus(int XP)
    {
        HP -= XP;
    }

    public void gameOver()
    {
        Retrybtn.SetActive(true);
        Time.timeScale = 0f;    
    }
}
