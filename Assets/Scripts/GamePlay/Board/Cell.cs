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
        [SerializeField] private Image itemImg;

        public int Index => _index;
        public int RowIndex => _rowId;

        private int _index;
        private int _rowId;
        private string _selectedColorCode;

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

        public override void SetData(int id, int rowIndex, int index, Sprite image, string colorCode)
        {
            _rowId = rowIndex;
            _index = index;
            _selectedColorCode = "#" + colorCode;
            itemImg.sprite = image;
            Id = id;
            label.text = $"({_rowId},{_index})";
            itemImg.gameObject.SetActive(false);
            FindNeighbors(index);
        }

        public void UpdateCell(int selectedVal)
        {
            label.text = $"({_rowId},{_index},{selectedVal})";
            ColorUtility.TryParseHtmlString(_selectedColorCode, out Color parsedColor);
            imageObj.color = parsedColor;
            itemImg.gameObject.SetActive(true);
            IsSelected = true;
        }
    }
}

