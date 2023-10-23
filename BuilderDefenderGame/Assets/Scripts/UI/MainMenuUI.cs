using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Transform playButton;
    [SerializeField] private Transform quitButton;

    private void Awake() {
        playButton.GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });
        quitButton.GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });
    }

}
