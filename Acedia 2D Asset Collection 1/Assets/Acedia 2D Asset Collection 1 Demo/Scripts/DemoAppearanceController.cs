using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DemoAppearanceController : MonoBehaviour {

    public DemoAppearance[] appearances;

    private void Start() {

        DemoController.instance.OnAppearanceButtonClick += ChangeAppearance;
        ChangeAppearance();        
    }

    private void OnDisable() {
        DemoController.instance.OnAppearanceButtonClick -= ChangeAppearance;
    }

    private void ChangeAppearance() {
        for (int i = 0; i < appearances.Length; i++)
            appearances[i].Change();
    }
}
