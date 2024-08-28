using Model;
using Props;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class BaseView : MonoBehaviour, IView
    {
        [SerializeField] private ScreenType screenType;
        [SerializeField] private Button backButton;

        public event Action OnHide;
        public bool IsVisible => gameObject.activeInHierarchy;

        protected virtual void OnBackClick()
        {
            EventManager<ScreenModel>.TriggerEvent(GameEvents.ON_CLOSE_VIEW, new ScreenModel { Type = screenType });
            SetVisibility(false);
        }

        public virtual void OnInitialize()
        {
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClick);
        }

        public virtual void OnShow()
        {
        }

        public virtual void OnHidePanel()
        {
        }

        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);

            if (!isVisible)
            {
                OnHidePanel();
                OnHide?.Invoke();
            }
        }

        private void OnEnable()
        {
            OnShow();
        }

        private void Start()
        {
            OnInitialize();
        }
    }
}