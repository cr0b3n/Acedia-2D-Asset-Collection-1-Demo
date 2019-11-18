using UnityEngine;

[CreateAssetMenu(fileName = "New Demo Color Appearance", menuName = "Acedia Demo/ Appearance/ Color Appearance")]
public class DemoColorAppearanceSO : DemoAppearanceSO {

    public override void Change(SpriteRenderer[] renderers) {

        Color c = new Color(Random.value, Random.value, Random.value);

        for (int i = 0; i < renderers.Length; i++) 
            renderers[i].color = c;

    }
}
