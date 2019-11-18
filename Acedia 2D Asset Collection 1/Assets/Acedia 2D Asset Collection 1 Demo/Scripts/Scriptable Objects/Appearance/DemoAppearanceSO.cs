using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemoAppearanceSO : ScriptableObject {
    public abstract void Change(SpriteRenderer[] renderers);
}

[System.Serializable]
public class DemoAppearance {

    public SpriteRenderer[] renderers;
    public DemoAppearanceSO appearance;

    public void Change() {

        if (appearance == null) return;

        appearance.Change(renderers);
    }
}