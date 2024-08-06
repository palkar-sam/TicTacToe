using ExitGames.Client.Photon;
using GamePlay;
using Model;
using Palettes;
using Photon.Pun;
using Photon.Realtime;
using Props;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;
using Views;

namespace Board
{
    public class CardBoard : BaseView
    {
        public static string SelectedCode; 

        [SerializeField] protected TextMeshProUGUI playerStatus;
        [SerializeField] private int rowLength;
        [SerializeField] private int colLength;
        [SerializeField] protected List<Row> rows;
        [SerializeField] protected List<Sprite> playersSymbols;

        //private bool IsBoardComplete => _cells.FindAll(item => item > -1).Count > 0;

        protected List<List<int>> _cells; // This stores status of board. 0 - Ai, 1 - user, -1 - Not selected
        protected List<int> _cellIndexes; // This stores all cells indexs - 0,1,2,3,4,5,6,7,8,9
        protected BaordValidator baordValidator;
        protected Vector2 _selectedCells = new Vector2(-1,-1);

        protected MarkType _myTurn;
        protected MarkType _turn;
        protected Sprite _defaultSymbol;

        private int _defaultSymbolInd;

        public override void OnInitialize()
        {
            base.OnInitialize();

            _cells = new List<List<int>>();
            _cellIndexes = new List<int>();
            int index = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < rows[i].TotalCells; j++)
                {
                    rows[i].Cells[j].OnCellSelected += OnCellSelected;
                    rows[i].Cells[j].SetData(index, i, j, _defaultSymbol);
                    tempList.Add(-1);
                    _cellIndexes.Add(index++);

                }
                _cells.Add(tempList);
            }

            baordValidator = new BaordValidator(_cells, rows);
            baordValidator.OnBoardValidate += OnBoardValidate;

            StartCoroutine(StartRound());
        }

        protected virtual IEnumerator StartRound()
        {
            yield return new WaitForSeconds(1.0f);

            UpdateRoundText();
        }

        protected virtual void OnCellSelected(int rowIndex, int index)
        {
            LoggerUtil.Log("CardBoard : OnCellSelected : Turn : " + _turn);
            SelectedCode = _turn == MarkType.X ? GameManager.Instance.UserColorCode : GameManager.Instance.AiColorCode;
            CellPlayed(rowIndex, index, _defaultSymbolInd);
        }

        protected virtual void OnBoardValidate(BoardValidType type)
        {
            if (type == BoardValidType.WIN)
            {
                LoggerUtil.Log("WIN : " + _turn);
                EventManager<BoardModel>.TriggerEvent(Props.GameEvents.ON_ROUND_COMPLETE, new BoardModel { Type = BoardValidType.WIN,
                    Winner = _turn == MarkType.X ? Winner.USER : Winner.AI });
            }
            else
            {
                LoggerUtil.Log("Changing Player ---- ");

                if (_turn == MarkType.X)
                {
                    _turn = MarkType.O;
                    _defaultSymbolInd = 1;
                }
                else
                {
                    _turn = MarkType.X;
                    _defaultSymbolInd = 0;
                }
                        

                LoggerUtil.Log("Next Player : " + _turn);
                UpdateRoundText();
            }
        }

        private void CellPlayed(int rowIndex, int index, int itemImageIndex = -1)
        {
            LoggerUtil.Log("CardBoard : CellPlayed : Turn : " + _turn);
            _cells[rowIndex][index] = (int)_turn;
            Cell selectedCell = rows[rowIndex].Cells[index];
            if (itemImageIndex > -1)
                selectedCell.UpdateImage(playersSymbols[itemImageIndex]);
            selectedCell.UpdateCell(_cells[rowIndex][index], SelectedCode);
            _cellIndexes.Remove(selectedCell.Id);
            _selectedCells = new Vector2(rowIndex, index);

            baordValidator.ValidateBoard(rowIndex, index);
        }

        private void UpdateRoundText()
        {
            playerStatus.text = $"Player {_turn} turn";
        }
    }

    public enum MarkType
    {
        NONE,
        O,
        X
    }
}

