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
    public class CardBoard : PhotonBaseView, IPunObservable, IOnEventCallback
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
        private BaordValidator baordValidator;
        private Vector2 _selectedCells = new Vector2(-1,-1);

        private MarkType _myTurn;
        private MarkType _turn;

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
                    rows[i].Cells[j].SetData(index, i, j);
                    tempList.Add(-1);
                    _cellIndexes.Add(index++);

                }
                _cells.Add(tempList);
            }

            baordValidator = new BaordValidator(_cells, rows);
            baordValidator.OnBoardValidate += OnBoardValidate;
            _myTurn = MarkType.X;
            _turn = MarkType.X;
            
            StartCoroutine(StartRound());

            if(GameManager.Instance.IsMultiplayer)
            {
                if(photonView != null && photonView.IsMine)
                {
                    _myTurn = MarkType.X;
                    _turn = Random.Range(0, 2) == 0 ? MarkType.O : MarkType.X;
                }
                else
                {
                    _myTurn = MarkType.O;
                }
            }

        }

        private IEnumerator StartRound()
        {
            yield return new WaitForSeconds(1.0f);
            UpdateRoundText();
        }

        private void UpdateRoundText()
        {
            playerStatus.text = $"Player {_turn} turn";
        }

        private void OnCellSelected(int rowIndex, int index)
        {
            LoggerUtil.Log("CardBoard : OnCellSelected : MP Mode : " + GameManager.Instance.IsMultiplayer);
            if (GameManager.Instance.IsMultiplayer)
            {
                LoggerUtil.Log("CardBoard : OnCellSelected : Turn : " + _turn);
                if(_myTurn != _turn)
                {
                    if (photonView != null)
                    {
                        LoggerUtil.Log("CardBoard : OnCellSelected : PhotonView.isMine : " + photonView.IsMine);
                        if (photonView.IsMine)
                        {
                            CellPlayed(rowIndex, index);
                        }
                        else
                        {
                            NetworkManager.Instance.RaiseEvent(NetworkEvents.MOVE_EVENT, new Vector2(rowIndex, index),
                                Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);
                        }
                    }
                    else
                    {
                        LoggerUtil.Log("CardBoard : OnCellSelected : PhotonView : is Null.");
                    }
                }
                else
                {
                    LoggerUtil.Log("CardBoard : OnCellSelected : Turn : This is not you turn oppent is playing.");
                }
                
            }
            else
            {
                CellPlayed(rowIndex, index);
            }
        }

        private void CellPlayed(int rowIndex, int index)
        {
            _cells[rowIndex][index] = _turn == _myTurn ? (int)MarkType.X : (int)MarkType.O;
            Cell selectedCell = rows[rowIndex].Cells[index];
            selectedCell.UpdateCell(_cells[rowIndex][index], SelectedCode);
            _cellIndexes.Remove(selectedCell.Id);
            _selectedCells = new Vector2(rowIndex, index);

            if (_myTurn == MarkType.X)
            {
                _userDaubedCells.Add(rows[rowIndex].Cells[index]);
            }

            baordValidator.ValidateBoard(rowIndex, index);
        }

        private void OnBoardValidate(BoardValidType type)
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

                if ( _turn == _myTurn)
                {
                    _turn = MarkType.O;
                    SelectedCode = GameManager.Instance.AiColorCode;
                }
                else
                {
                    _turn = MarkType.X;
                    SelectedCode = GameManager.Instance.UserColorCode;
                }

                LoggerUtil.Log("Next Player : " + _turn);
                UpdateRoundText();
                if (_turn == MarkType.O && GameManager.Instance.IsSinglePlayer)
                    StartCoroutine(PlayAiMove());

            }
        }

        private IEnumerator PlayAiMove()
        {
            LoggerUtil.Log("Playing AI : " + _turn);
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
                    rowIndex = aiCell.RowIndex;
                    index = aiCell.Index;
                    break;
                }
            }
            
            LoggerUtil.Log($"Ai : Location : ({rowIndex} , {index}) : AI Turn : {_turn}");
            LoggerUtil.Log($"Remaining cell Positions : ({string.Join(",", _cellIndexes)})");
            OnCellSelected(rowIndex, index);
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

        #region IPunObservable interface Functions, Photon event handling and synchronisaton.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(GameManager.Instance.IsMultiplayer)
            {
                if (stream.IsWriting)
                {
                    LoggerUtil.Log("CardBoard : OnPhotonSerializeView : sending : " + _selectedCells);
                    if (_selectedCells.x > -1 || _selectedCells.y > -1)
                    {
                        stream.SendNext(_selectedCells);
                        stream.SendNext(_turn);
                        stream.SendNext(0);
                    }
                }
                else
                {
                    Vector2 cells = (Vector2)stream.ReceiveNext();
                    _turn = (MarkType)stream.ReceiveNext();
                    int colorIndex = (int)stream.ReceiveNext();
                    SelectedCode = PaletteView.GetColorCodeAtIndex(colorIndex);
                    LoggerUtil.Log("CardBoard : OnPhotonSerializeView : recieving : " + cells + " : Color ind : " + colorIndex);
                    ProcessCell((int)cells.x, (int)cells.y);
                }
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            if(photonView.IsMine)
            {
                NetworkEvents code = (NetworkEvents)photonEvent.Code;
                switch (code)
                {
                    case NetworkEvents.MOVE_EVENT:
                        Vector2 cells = (Vector2)photonEvent.CustomData;
                        LoggerUtil.Log("CardBoard : OnEvent : " + cells);
                        ProcessCell((int)cells.x, (int)cells.y);
                        break;

                    default:
                        LoggerUtil.Log("CardBoard : OnEvent : No Case Found");
                        break;
                }
            }
        }

        private void ProcessCell(int rowIndex, int cellIndex)
        {
            CellPlayed(rowIndex, cellIndex);
            rows[rowIndex].Cells[cellIndex].UpdateCell(_cells[rowIndex][cellIndex], SelectedCode);
        }
        #endregion
    }

    public enum MarkType
    {
        NONE,
        O,
        X
    }
}

