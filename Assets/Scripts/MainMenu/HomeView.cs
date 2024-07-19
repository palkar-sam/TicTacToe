using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class HomeView : BaseView
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;

    public event Action OnCreate;
    public event Action OnOpenJoinDialog;
    

    private void Start()
    {
        //createButton.onClick.AddListener(ShowCreateDialog);
        //joinButton.onClick.AddListener(ShowJoinDialog);
    }

    public void ShowCreateDialog()
    {
        OnCreate?.Invoke();
    }

    public void ShowJoinDialog()
    {
        OnOpenJoinDialog?.Invoke();
    }
}
