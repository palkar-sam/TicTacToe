using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Views;

public class HomeView : BaseView
{
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button mulitiPlayerButton;
    [SerializeField] private GameObject singlePlayerPanel;
    [SerializeField] private GameObject mulitiPlayerPanel;

    public event Action OnCreate;
    public event Action OnOpenJoinDialog;

    public override void OnInitialize()
    {
        base.OnInitialize();
        singlePlayerButton.onClick.AddListener(() => { ShowModePanel(0); });
        mulitiPlayerButton.onClick.AddListener(() => { ShowModePanel(1); });
    }

    public void ShowCreateDialog()
    {
        OnCreate?.Invoke();
    }

    public void ShowJoinDialog()
    {
        OnOpenJoinDialog?.Invoke();
    }

    public void PlaySinglePlayerRound()
    {
        SceneManager.LoadSceneAsync(2);
    }

    private void ShowModePanel(int mode)
    {
        buttonsContainer.SetActive(false);
        singlePlayerPanel.SetActive(mode == 0);
        mulitiPlayerPanel.SetActive(mode == 1);

        if(mode == 1)
        {
            GameManager.Instance.IsMultiplayer = true;
            mulitiPlayerPanel.SetActive(true);
            NetworkManager.Instance.ConnectToServer();
        }
    }
}
