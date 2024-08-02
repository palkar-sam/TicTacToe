using GamePlay;
using Model;
using Palettes;
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

        private List<List<int>> _cells; // This stores status of board. 0 - Ai, 1 - user, -1 - Not selected
        private List<int> _cellIndexes; // This stores all cells indexs - 0,1,2,3,4,5,6,7,8,9
        private List<Cell> _userDaubedCells = new List<Cell>();
        private BoardPlayers _currentPlayer;
        private BaordValidator baordValidator;

        private void Start()
        {
            _cells = new List<List<int>>();
            _cellIndexes = new List<int>();
            int index = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < rows[i].TotalCells; j++)
                {
                    rows[i].Cells[j].OnCellSelected += OnCellSelected;
                    rows[i].Cells[j].SetData(index, i, j);
                    tempList.Add(-1);
                    _cellIndexes.Add(index++);

                }
                _cells.Add(tempList);
            }

            baordValidator = new BaordValidator(_cells, rows);
            baordValidator.OnBoardValidate += OnBoardValidate;
            _currentPlayer = BoardPlayers.PLAYER_X;
            NetworkManager.Instance.UserCellColorIndex = PaletteView.UserCellColorIndex;
            StartCoroutine(StartRound());
        }

        public void UpdateNetworkData(Vector2 cellIndexs, int colorIndex)
        {
            SelectedCode = PaletteView.GetColorCodeAtIndex(colorIndex);
            OnCellSelected((int)cellIndexs.x, (int)cellIndexs.y);
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
            NetworkManager.Instance.CellIndexs = new Vector2(rowIndex, index);
            Cell selectedCell = rows[rowIndex].Cells[index];
            selectedCell.UpdateLabel(_cells[rowIndex][index]);
            _cellIndexes.Remove(selectedCell.Id);

            if (_currentPlayer == BoardPlayers.PLAYER_X)
            {
                _userDaubedCells.Add(rows[rowIndex].Cells[index]);
            }

            baordValidator.ValidateBoard(rowIndex, index);
        }

        private void OnBoardValidate(BoardValidType type)
        {
            if (type == BoardValidType.WIN)
            {
                LoggerUtil.Log("WIN : " + _currentPlayer);
                EventManager<BoardModel>.TriggerEvent(Props.GameEvents.ON_ROUND_COMPLETE, new BoardModel { Type = BoardValidType.WIN,
                    Winner = _currentPlayer == BoardPlayers.PLAYER_X ? Winner.USER : Winner.AI });
            }
            else
            {
                LoggerUtil.Log("Changing Player ---- ");

                if (_currentPlayer == BoardPlayers.PLAYER_X)
                {
                    _currentPlayer = BoardPlayers.PLAYER_O;
                    SelectedCode = GameManager.Instance.AiColorCode;
                }
                else
                {
                    _currentPlayer = BoardPlayers.PLAYER_X;
                    SelectedCode = GameManager.Instance.UserColorCode;
                }
                //_currentPlayer = _currentPlayer == BoardPlayers.PLAYER_X ? BoardPlayers.PLAYER_O : BoardPlayers.PLAYER_X;
                LoggerUtil.Log("Next Player : " + _currentPlayer);
                UpdateRoundText();
                if (_currentPlayer == BoardPlayers.PLAYER_O && GameManager.Instance.IsSinglePlayer)
                    StartCoroutine(PlayAiMove());

            }
        }

        private IEnumerator PlayAiMove()
        {
            yield return new WaitForSeconds(0.5f);
            int cellId = GetAiMove();
            LoggerUtil.Log($"Ai : New cellId : ({cellId}");
            Cell aiCell = null;
            int index = -1;
            int rowIndex = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                aiCell = rows[i].Cells.Find(item => item.Id == cellId);
                if (aiCell != null)
                {
                    index = i;
                    rowIndex = aiCell.Index;
                    break;
                }
            }

            LoggerUtil.Log($"Ai : Location : ({index} , {rowIndex})");
            LoggerUtil.Log($"Remaining cell Positions : ({string.Join(",", _cellIndexes)})");
            aiCell.UpdateCell();
        }

        private int GetAiMove()
        {
            //if(_userDaubedCells.Count == 1)
            {
                LoggerUtil.Log($"Ai : _cellIndexes.Count : ({_cellIndexes.Count})");
                return _cellIndexes[Random.Range(0, _cellIndexes.Count - 1)];
            }
            //return 0;
        }
    }
}

