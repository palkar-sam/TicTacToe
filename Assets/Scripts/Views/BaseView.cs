using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{ 
    public class BaseView : MonoBehaviour, IView
    {
        [SerializeField] private Button backButton;

        public event Action OnHide;

        protected virtual void OnBackClick()
        {
            SetVisibility(false);
        }

        public virtual void OnInitialize()
        {
            if(backButton != null)
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