using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSelectedToggle : MonoBehaviour
{
    //private [SerializeField] 
    [SerializeField] private Toggle level1, level2, level3, level4;
    private ToggleGroup allowSwitch;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("ToggleSelected") == 1)
        {
            level1.isOn = true;
            level2.isOn = false;
            level3.isOn = false;
            level4.isOn = false;
        }
        else if (PlayerPrefs.GetInt("ToggleSelected") == 2)
        {
            level1.isOn = false;
            level2.isOn = true;
            level3.isOn = false;
            level4.isOn = false;
        }
        else if (PlayerPrefs.GetInt("ToggleSelected") == 3)
        {
            level1.isOn = false;
            level2.isOn = false;
            level3.isOn = true;
            level4.isOn = false;
        }
        if (PlayerPrefs.GetInt("ToggleSelected") == 4)
        {
            level1.isOn = false;
            level2.isOn = false;
            level3.isOn = false;
            level4.isOn = true;
        }
    }
    public void Level1Selected()
    {
        PlayerPrefs.SetInt("ToggleSelected", 1);
    }
    public void Level2Selected()
    {
        PlayerPrefs.SetInt("ToggleSelected", 2);
    }
    public void Level3Selected()
    {
        PlayerPrefs.SetInt("ToggleSelected", 3);
    }
    public void Level4Selected()
    {
        PlayerPrefs.SetInt("ToggleSelected", 4);
    }
}
