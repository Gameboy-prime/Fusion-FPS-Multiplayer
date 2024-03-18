using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public TMP_InputField inputField;

    public GameObject mainUi;

    public GameObject infoUI;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("nickName"))
        {
            inputField.text = PlayerPrefs.GetString("nickName");
        }
        
    }

    public void SetNickName()
    {
        PlayerPrefs.SetString("nickName", inputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void ActivateInfo()
    {
        infoUI.SetActive(true);
        mainUi.SetActive(false);
        

    }

    public void DeactivateInfo()
    {
        mainUi.SetActive(true);
        infoUI.SetActive(false);
    }

    
}
