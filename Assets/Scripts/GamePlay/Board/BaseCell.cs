using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    public abstract class BaseCell : MonoBehaviour
    {
        [SerializeField] protected Text label;

        public int Id { get; protected set; }
        public bool IsSelected { get; protected set; }

        public event Action<int, int> OnCellSelected;

        protected List<List<int>> neighbors;

        public abstract void SetData(int id, int rowIndex, int index, Sprite image);

        protected void OnCellClicked(int rowIndex, int index)
        {
            OnCellSelected?.Invoke(rowIndex, index);
        }

        protected void FindNeighbors(int index)
        {
            neighbors = new List<List<int>>();
            List<int> tempArr;

            tempArr = null;
            tempArr = new List<int>();
            tempArr.Add(index - 3);
            tempArr.Add(index);
            tempArr.Add(index + 3);

            neighbors.Add(tempArr);

            tempArr = null;
            tempArr = new List<int>();
            tempArr.Add(index - 1);
            tempArr.Add(index);
            tempArr.Add(index + 1);

            neighbors.Add(tempArr);

            tempArr = null;
            tempArr = new List<int>();
            tempArr.Add(index - (3 - 1));
            tempArr.Add(index);
            tempArr.Add(index + (3 - 1));

            neighbors.Add(tempArr);

            tempArr = null;
            tempArr = new List<int>();
            tempArr.Add(index - (3 + 1));
            tempArr.Add(index);
            tempArr.Add(index + (3 + 1));

            neighbors.Add(tempArr);
        }
    }
}