using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DemoBuildSceneController : MonoBehaviour {

    private static DemoBuildSceneController instance;

    private bool buttonsIsVisible;
    private List<Button> buttons = new List<Button>();

    private void Awake() {

        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.F1)) {
            //Load Demo 1
            SceneManager.LoadScene(1);
        } else if (Input.GetKeyDown(KeyCode.F2)) {
            //Load Demo 2
            SceneManager.LoadScene(2);
        } else if (Input.GetKeyDown(KeyCode.F4)) {
            //Reset Scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (Input.GetKeyDown(KeyCode.F8)) {
            //Load Demo 0
            SceneManager.LoadScene(0);
        } else if (Input.GetKeyDown(KeyCode.F9)) {
            //Hide All Buttons

            if(!buttonsIsVisible) {

                buttons = FindObjectsOfType<Button>().ToList();

                for (int i = 0; i < buttons.Count; i++) 
                    buttons[i].gameObject.SetActive(buttonsIsVisible);

            } else {

                for (int i = 0; i < buttons.Count; i++)
                    buttons[i].gameObject.SetActive(buttonsIsVisible);

            }

            buttonsIsVisible = !buttonsIsVisible;
        }
    }
}
