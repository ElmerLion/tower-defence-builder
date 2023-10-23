using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour {

    public static GameOverUI Instance { get; private set; }

    [SerializeField] Transform retryButton;
    [SerializeField] Transform mainMenuButton;
    [SerializeField] Transform wavesSurvivedText;

    private void Awake() {
        Instance = this;

        retryButton.GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });

        mainMenuButton.GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);

        wavesSurvivedText.GetComponent<TextMeshProUGUI>().SetText("Your Survived " + EnemyWaveManager.Instance.GetWaveNumber() + " Waves!");
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}
