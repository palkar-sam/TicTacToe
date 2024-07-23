using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] private List<Cell> cells;

    public int TotalCells => cells.Count;
    public List<Cell> Cells => cells;
}
