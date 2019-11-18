using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class DemoController : MonoBehaviour {

    #region /Singleton

    public static DemoController instance;

    private void Awake() {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    #endregion

    public event Action OnAppearanceButtonClick;

    public void ChangeAppearance() {
        OnAppearanceButtonClick?.Invoke();
    }
}
