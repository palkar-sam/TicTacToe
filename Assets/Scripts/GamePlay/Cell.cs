using Board;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image imageObj;
    [SerializeField] private Text label;

    public event Action<int, int> OnCellSelected;

    public bool IsSelected { get; private set; }

    public int ID => _id;
    
    
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
            OnCellSelected?.Invoke(_rowId, _id);
        }
        else
        {
            LoggerUtil.Log("Item Already Selected.....");
        }
    }

    public void SetData(int rowIndex, int index)
    {
        _rowId = rowIndex;
        _id = index;
        label.text = $"({_rowId},{_id})";
    }

    public void UpdateLabel(int selectedVal)
    {
        label.text = $"({_rowId},{_id},{selectedVal})"; 
    }
}
