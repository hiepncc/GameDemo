using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEfectSelect : MonoBehaviour
{
    [SerializeField] private Toggle on, off;
    // Start is called before the first frame update

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Muted"))
        {
            PlayerPrefs.SetInt("Muted", 0);
        }
    }
    public void onSelected()
    {
        AudioListener.pause = false;
        PlayerPrefs.SetInt("Muted", 0);
    }
    public void offSelected()
    {
        AudioListener.pause = true;
        PlayerPrefs.SetInt("Muted", 1);
    }
}
