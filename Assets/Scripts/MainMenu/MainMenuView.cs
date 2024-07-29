using UnityEngine;
using Views;

namespace MainMenu
{
    public class MainMenuView : BaseView
    {
        [SerializeField] private CreateView createView;
        [SerializeField] private JoinView joinView;
        [SerializeField] private HomeView homeView;

        private void Start()
        {
            createView.SetVisibility(false);
            joinView.SetVisibility(false);
            //homeView.SetVisibility(true);

            homeView.OnCreate += OnShowCreateDialog;
            homeView.OnOpenJoinDialog += OnOpenJoinDialog;
        }

        private void OnShowCreateDialog()
        {
            createView.SetVisibility(true);
            homeView.SetVisibility(false);
            joinView.SetVisibility(false);
        }

        private void OnOpenJoinDialog()
        {
            joinView.SetVisibility(true);
            createView.SetVisibility(false);
            homeView.SetVisibility(false);
        }
    }
}

