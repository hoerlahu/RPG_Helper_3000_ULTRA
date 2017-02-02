using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour, IKeyCodeReceiver {

    public GameObject optionsPrefab;

    private GameObject shownOptions;

    private bool showsOptions;

    private void Start()
    {
        KeyManager.GetInstance().registerListener(this, KeyCode.Escape, KeyManager.KeyReceivingImportance.low);
    }

    private void showOptions()
    {
        shownOptions = Instantiate(optionsPrefab);
        showsOptions = true;
    }

    private void removeOptions()
    {
        Destroy(shownOptions);
        showsOptions = false;
    }

    public bool HandlesKeyDown(KeyCode key)
    {
        if (key == KeyCode.Escape) {
            if (showsOptions == true)
            {
                removeOptions();
            }
            else
            {
                showOptions();
            }
            return true;
        }
        return false;
    }
    

}
