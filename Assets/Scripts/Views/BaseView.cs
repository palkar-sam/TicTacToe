using UnityEngine;

namespace Views
{ 
    public class BaseView : MonoBehaviour, IView
    {
        public virtual void OnInitialize()
        {
        }

        public virtual void OnShow()
        {
        }

        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
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