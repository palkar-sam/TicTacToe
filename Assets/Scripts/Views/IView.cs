using System;
using System.Collections;
using UnityEngine;

namespace Views
{
    public interface IView
    {
        public void SetVisibility(bool isVisible);
        public void OnInitialize();
        public void OnShow();
    }
}