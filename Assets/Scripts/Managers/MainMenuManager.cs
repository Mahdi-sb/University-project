    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backToMainMenuButton;

    private void Awake()
    {
        playButton?.onClick.AddListener(() => SceneManager.LoadScene(1));
        quitButton?.onClick.AddListener(Application.Quit);
        backToMainMenuButton?.onClick.AddListener(() => SceneManager.LoadScene(0));
    }
}
