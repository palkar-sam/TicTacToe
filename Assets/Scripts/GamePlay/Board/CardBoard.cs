using GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

namespace Board
{
    public class CardBoard : MonoBehaviour
    {
        public static string SelectedCode;

        [SerializeField] private TextMeshProUGUI playerStatus;
        [SerializeField] private int rowLength;
        [SerializeField] private int colLength;
        [SerializeField] private List<Row> rows;

        //private bool IsBoardComplete => _cells.FindAll(item => item > -1).Count > 0;

        private List<List<int>> _cells;
        private List<Cell> matchCells = new List<Cell>();
        private BoardPlayers _currentPlayer;

        private void Start()
        {
            _cells = new List<List<int>>();

            for (int i = 0; i < rows.Count; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < rows[i].TotalCells; j++)
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
            _cells[rowIndex][index] = _currentPlayer == BoardPlayers.PLAYER_X ? 1 : 0;
            matchCells.Clear();

            matchCells.Add(rows[rowIndex].Cells[index]);
            matchCells[0].UpdateLabel(_cells[rowIndex][index]);

            BaordValidator bv = new BaordValidator(_cells, rows);
            matchCells.AddRange(bv.ValidateBoard(rowIndex, index, out bool matchFound));

            LoggerUtil.Log($"Match Found : {matchFound} : " + string.Join(",", matchCells));

            if (matchFound)
            {
                LoggerUtil.Log("WIN : " + _currentPlayer);
                EventManager.TriggerEvent(Props.GameEvents.ON_ROUND_COMPLETE);
            }
            else
            {
                LoggerUtil.Log("Changing Player ---- ");
                matchCells.Clear();
                _currentPlayer = _currentPlayer == BoardPlayers.PLAYER_X ? BoardPlayers.PLAYER_O : BoardPlayers.PLAYER_X;
                LoggerUtil.Log("Next Player : " + _currentPlayer);
                UpdateRoundText();
            }
        }
    }
}

