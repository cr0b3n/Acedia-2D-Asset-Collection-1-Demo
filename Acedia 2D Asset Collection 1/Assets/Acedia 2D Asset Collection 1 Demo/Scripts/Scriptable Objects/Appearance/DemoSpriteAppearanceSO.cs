using UnityEngine;

[CreateAssetMenu(fileName = "New Demo Sprite Appearance", menuName = "Acedia Demo/ Appearance/ Sprite Appearance")]
public class DemoSpriteAppearanceSO : DemoAppearanceSO {

    public Sprite[] sprites;

    public override void Change(SpriteRenderer[] renderers) {

        if (sprites.Length <= 0) return;

        int index = Random.Range(0, sprites.Length);

        for (int i = 0; i < renderers.Length; i++) 
            renderers[i].sprite = sprites[index];
      
    }
}
