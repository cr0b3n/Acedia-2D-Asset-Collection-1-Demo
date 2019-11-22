using UnityEngine;

[CreateAssetMenu(fileName = "New Demo Effect", menuName = "Acedia Demo/ Effects/ Effect")]
public class DemoEffectSO : ScriptableObject {

    public EffectType type;
    public GameObject effectPrefab;
    [Range(0.0f, 5.0f)]
    public float lifeTime = 2f;

    public void Show(Vector3 pos, Vector3 localScale) {

        GameObject obj = Instantiate(effectPrefab, pos, Quaternion.identity);

        obj.transform.localScale = localScale;
        Destroy(obj, lifeTime);
    }
}

public enum EffectType {
    Dash,
    Step,
    Jump,
    Land,
    Shoot
}