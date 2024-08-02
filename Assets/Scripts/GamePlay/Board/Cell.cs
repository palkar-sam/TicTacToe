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

        public int Index => _index;
        public int RowIndex => _rowId;

        private int _index;
        private int _rowId;

        public void OnPointerClick(PointerEventData eventData)
        {
            LoggerUtil.Log(name + " Game Object Clicked!");
            if (!IsSelected)
            {
                OnCellClicked(_rowId, _index);
            }
            else
            {
                LoggerUtil.Log("Item Already Selected.....");
            }
        }

        public override void SetData(int id, int rowIndex, int index)
        {
            _rowId = rowIndex;
            _index = index;
            Id = id;
            label.text = $"({_rowId},{_index})";
            FindNeighbors(index);
        }

        public void UpdateCell(int selectedVal, string colorCode)
        {
            label.text = $"({_rowId},{_index},{selectedVal})";
            ColorUtility.TryParseHtmlString("#" + colorCode, out Color parsedColor);
            imageObj.color = parsedColor;
            IsSelected = true;
        }
    }
}

