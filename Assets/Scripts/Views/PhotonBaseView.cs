using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class PhotonBaseView : MonoBehaviourPunCallbacks, IView
    {
        [SerializeField] private Button backButton;

        public event Action OnHide;

        public override void OnEnable()
        {
            base.OnEnable();
            OnShow();
        }

        protected virtual void OnBackClick()
        {
            SetVisibility(false);
        }

        public virtual void SetVisibility(bool isVisible)
        {
            if (gameObject.activeInHierarchy == isVisible)
                return;

            gameObject.SetActive(isVisible);

            if (!isVisible)
                OnHide?.Invoke();
        }

        public virtual void OnInitialize()
        {
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClick);
        }

        public virtual void OnShow()
        {
        }

        private void Start()
        {
            OnInitialize();
        }
    }
}