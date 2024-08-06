using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Board
{
    public class BoardPanelView : BaseView
    {
        [SerializeField] private List<GameObject> boards;
        [SerializeField] private Transform boardContainer;

        public override void OnInitialize()
        {
            base.OnInitialize();
            if(GameManager.Instance.IsMultiplayer)
                Instantiate(boards[0], boardContainer);
            else if(GameManager.Instance.IsSinglePlayer || GameManager.Instance.Is1vs1Enabled)
                Instantiate(boards[1], boardContainer);
        }
    }
}

