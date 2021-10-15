using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject baseGo;
    [SerializeField] private SaveLoad saveLoad;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause) CallMenu();
            else CloseMenu();
        }
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        baseGo.SetActive(false);
        Time.timeScale = 1;
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        baseGo.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClickSave()
    {
        Debug.Log("세이브");
        saveLoad.SaveData();
    }
    
    public void ClickLoad()
    {
        Debug.Log("로드");
        saveLoad.LoadData();
    }
    
    public void ClickExit()
    {
        Debug.Log("종료");
        Application.Quit();
    }
}
