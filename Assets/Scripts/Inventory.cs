using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static DefaultNamespace.Constants;

public class Inventory : MonoBehaviour
{
    private bool _yellowGem;
    private bool _blueGem;
    private bool _greenGem;
    public Image yellowGemImg, blueGemImg, greenGemImg, keyImg;
    private int levelNumber;
    private bool areAllGemsFoundBefore;

    private void Start()
    {
        _yellowGem = false;
        _blueGem = false;
        _greenGem = false;
        levelNumber = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.HasKey(GemYellow + levelNumber))
        { 
            AddYellowGem();
        }
        if (PlayerPrefs.HasKey(GemBlue + levelNumber))
        {
            AddBlueGem();
        }
        if (PlayerPrefs.HasKey(GemGreen + levelNumber))
        { 
            AddGreenGem();
        }

        areAllGemsFoundBefore = _blueGem && _greenGem && _yellowGem;
    }

    public void AddYellowGem()
    {
        yellowGemImg.color = new Color(1,1,1);
        _yellowGem = true;  
    }
    
    public void AddBlueGem()
    {
        blueGemImg.color = new Color(1,1,1);
        _blueGem = true; 
    }
    
    public void AddGreenGem()
    {
        greenGemImg.color = new Color(1,1,1);
        _greenGem = true;  
    }
    
    public void AddKey()
    {
        keyImg.color = new Color(1,1,1);
    }
    
    public void RecountItems()
    {
        if (_yellowGem)
        {
            PlayerPrefs.SetInt(GemYellow + levelNumber, 1);     
        }

        if (_blueGem)
        {
            PlayerPrefs.SetInt(GemBlue + levelNumber, 1);    
        }

        if (_greenGem)
        {
            PlayerPrefs.SetInt(GemGreen + levelNumber, 1);   
        }
    }

    public bool AreAllGemsCollected()
    {
        if (areAllGemsFoundBefore)
        {
            return false;
        }

        return _blueGem && _greenGem && _yellowGem;
    }
}
