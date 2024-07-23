using GamePlay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static string SelectedCode;

    [SerializeField] private TextMeshProUGUI playerStatus;
    [SerializeField] private int rowLength;
    [SerializeField] private int colLength;
    [SerializeField] private List<Row> rows;
    [SerializeField] private BoardPlayers player;

    //private bool IsBoardComplete => _cells.FindAll(item => item > -1).Count > 0;

    private List<List<int>> _cells;
    private List<Cell> matchCells = new List<Cell>();
    private BoardPlayers _currentPlayer;

    private void Start()
    {
        _cells = new List<List<int>>();

        for(int i = 0;i<rows.Count; i++)
        {
            List<int> tempList = new List<int>();
            for(int j = 0; j<rows[i].TotalCells; j++)
            {
                rows[i].Cells[j].OnCellSelected += OnCellSelected;
                rows[i].Cells[j].SetData(i, j);
                tempList.Add(-1);
            }
            _cells.Add(tempList);
        }

        _currentPlayer = BoardPlayers.PLAYER_X;
        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        yield return new WaitForSeconds(1.0f);
        UpdateRoundText();
    }

    private void UpdateRoundText()
    {
        playerStatus.text = $"Player {_currentPlayer} turn";
    }

    private void OnCellSelected(int rowIndex, int index)
    {
        _cells[rowIndex][index] = player == BoardPlayers.PLAYER_X ? 1 : 0;
        matchCells.Clear();

        matchCells.Add(rows[rowIndex].Cells[index]);
        bool matchFound = false;
        //check for Horizontal right - 
        for(int i = index + 1; i < _cells[rowIndex].Count; i++)
        {
            if (!matchFound && _cells[rowIndex][index] == _cells[rowIndex][i])
            {
                
                matchCells.Add(rows[rowIndex].Cells[i]);
                if (matchCells.Count >= 3)
                    matchFound = true;
            }
            else
                break;
        }

        //check for Horizontal left - 
        for (int i = index - 1; i > 0; i--)
        {
            if (!matchFound && _cells[rowIndex][index] == _cells[rowIndex][i])
            {

                matchCells.Add(rows[rowIndex].Cells[i]);
                if (matchCells.Count >= 3)
                    matchFound = true;
            }
            else
                break;
        }

        //check for Vetical Up - 
        for (int i = rowIndex - 1; i > 0; i--)
        {
            if (!matchFound && _cells[rowIndex][index] == _cells[i][index])
            {

                matchCells.Add(rows[i].Cells[index]);
                if (matchCells.Count >= 3)
                    matchFound = true;
            }
            else
                break;
        }

        //check for Vetical Down - 
        for (int i = index + 1; i < _cells.Count; i++)
        {
            if (!matchFound && _cells[rowIndex][index] == _cells[i][index])
            {

                matchCells.Add(rows[i].Cells[index]);
                if (matchCells.Count >= 3)
                    matchFound = true;
            }
            else
                break;
        }

        Debug.Log("Match Found : " + string.Join(",",matchCells));

        if (matchFound)
        {
            Debug.Log("WIN : " + _currentPlayer);
        }
        else
        {
            Debug.Log("Changing Player ---- ");
            matchCells.Clear();
            _currentPlayer = _currentPlayer == BoardPlayers.PLAYER_X ? BoardPlayers.PLAYER_O : BoardPlayers.PLAYER_X;
            UpdateRoundText(); 
        }
    }



}
