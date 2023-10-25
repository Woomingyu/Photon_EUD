using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InputName : MonoBehaviour
{
    public InputField InputPlayerName;
    public Button button;

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnName);
        button.onClick.AddListener(aaa);
    }

    public void OnName()
    {
        PlayerPrefs.SetString("PlayerName", InputPlayerName.text.ToString());

        SceneManager.LoadScene("SampleScene");
    }

    public void aaa()
    {
        Debug.Log("¾È³ç");
    }
}
