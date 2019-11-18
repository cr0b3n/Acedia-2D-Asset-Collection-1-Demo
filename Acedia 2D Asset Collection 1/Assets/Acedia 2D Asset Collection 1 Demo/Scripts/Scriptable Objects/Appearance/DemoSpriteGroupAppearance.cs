using UnityEngine;

[CreateAssetMenu(fileName = "New Demo Sprite Group Appearance", menuName = "Acedia Demo/ Appearance/ Sprite Group Appearance")]
public class DemoSpriteGroupAppearance : DemoAppearanceSO {

    public SpriteGroup[] spriteGroups;
    private int curIndex;


    [System.Serializable]
    public class SpriteGroup {
        public Sprite[] sprites;
    }

    public override void Change(SpriteRenderer[] renderers) {

        if (spriteGroups.Length <= 0) return;

        for (int i = 0; i < renderers.Length; i++) {

            if (i < spriteGroups[curIndex].sprites.Length)
                renderers[i].sprite = spriteGroups[curIndex].sprites[i];

        }

        curIndex = (curIndex + 1) % spriteGroups.Length;
    }
}
