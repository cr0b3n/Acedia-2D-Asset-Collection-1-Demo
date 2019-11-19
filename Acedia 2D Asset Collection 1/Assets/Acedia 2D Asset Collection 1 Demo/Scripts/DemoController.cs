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

    public bool enableCharacterSelection;
    public Collider2D[] ignoringColliders;


    public event Action OnAppearanceButtonClick;

    private RaycastHit2D[] hits = new RaycastHit2D[7];
    private Camera cam;
    private IActivatable curActivatable;

    private void Start() {
        cam = Camera.main;
        IgnoreColliderCollision();
    }

    private void Update() {

        CheckActivePlayer();
    }

    private void IgnoreColliderCollision() {

        //List<int> intergers = new List<int>();

        //for (int i = 0; i < 3; i++) {
        //    intergers.Add(i);
        //}

        //for (int i = 0; i < intergers.Count; i++) {

        //    for (int j = i + 1; j < intergers.Count; j++) {

        //        Debug.Log(string.Format("{0} - {1}", intergers[i], intergers[j]));
        //    }
        //}

        if (ignoringColliders.Length <= 1) return;

        for (int i = 0; i < ignoringColliders.Length; i++) {

            for (int j = i + 1; j < ignoringColliders.Length; j++) {

                if (ignoringColliders[i] != null && ignoringColliders[j] != null) {
                    Physics2D.IgnoreCollision(ignoringColliders[i], ignoringColliders[j]);
                }

            }

        }
    }

    private void CheckActivePlayer() {

        if (!enableCharacterSelection || !Input.GetMouseButtonDown(1))
            return;

        int hitCount = Physics2D.RaycastNonAlloc(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, hits, 100.0f);

        if (hitCount == 0) return;

        for (int i = 0; i < hitCount; i++) {

            IActivatable activatable = hits[i].collider.GetComponent<IActivatable>();

            if(activatable != null) {

                curActivatable?.Active(false, 1);
                activatable.Active(true, 10);
                curActivatable = activatable;
                return;
            }
        }
    }

    public void ChangeAppearance() {
        OnAppearanceButtonClick?.Invoke();
    }
}
