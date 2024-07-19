using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace Views
{
    public class PhotonBaseView : MonoBehaviourPunCallbacks, IView
    {
        public override void OnEnable()
        {
            base.OnEnable();
            OnShow();
        }

        public virtual void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        public virtual void OnInitialize()
        {
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