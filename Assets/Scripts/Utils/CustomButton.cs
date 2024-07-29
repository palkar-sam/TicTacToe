using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(Button))]
    public class CustomButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private int _id;

        public void AddListener(Action<int> callback, int id = 0)
        {
            if (_button == null) GetButton();

            _id = id;
            _button.onClick.AddListener(() => { callback?.Invoke(_id); });
        }

        public void RemoveListener()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void GetButton()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void Start()
        {
            GetButton();
        }
    }
}