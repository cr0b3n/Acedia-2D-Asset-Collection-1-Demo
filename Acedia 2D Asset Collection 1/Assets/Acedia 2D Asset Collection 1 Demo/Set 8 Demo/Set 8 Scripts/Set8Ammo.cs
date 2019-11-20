using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Set8Ammo : MonoBehaviour {

    public float speed = 15f;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    private void OnEnable() {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        if (rb != null)
            rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
    }
}
