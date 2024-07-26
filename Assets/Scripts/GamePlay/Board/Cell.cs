using Board;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Board
{
    public class Cell : BaseCell, IPointerClickHandler
    {
        [SerializeField] private Image imageObj;

        private int _id;
        private int _rowId;

        public void OnPointerClick(PointerEventData eventData)
        {
            LoggerUtil.Log(name + " Game Object Clicked!");
            if (!IsSelected)
            {
                ColorUtility.TryParseHtmlString("#" + CardBoard.SelectedCode, out Color parsedColor);
                imageObj.color = parsedColor;
                IsSelected = true;
                OnCellClicked(_rowId, _id);
            }
            else
            {
                LoggerUtil.Log("Item Already Selected.....");
            }
        }

        public override void SetData(int rowIndex, int index)
        {
            _rowId = rowIndex;
            _id = index;
            Id = _id;
            label.text = $"({_rowId},{_id})";
            FindNeighbors(index);
        }

        public void UpdateLabel(int selectedVal)
        {
            label.text = $"({_rowId},{_id},{selectedVal})";
        }
    }
}

