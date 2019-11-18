using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DemoBGScroller : MonoBehaviour {

    public float speed = 2.0f;
    public int totalBG = 2;
    public float backgroundWidth;

    private float maxPosition;

    private void Start() {
        maxPosition = -backgroundWidth * 1.5f;
    }

    private void Update() {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0));

        if (transform.position.x < maxPosition)
            RepositionBackground();
    }

    private void RepositionBackground() {

        Vector2 scrollOffset = new Vector2(backgroundWidth * totalBG, 0);
        transform.position = (Vector2)transform.position + scrollOffset;
    }
}
