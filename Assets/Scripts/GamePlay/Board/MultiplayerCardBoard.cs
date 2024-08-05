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
    public class MultiplayerCardBoard : PhotonBaseView, IPunObservable, IOnEventCallback
    {
        public static string SelectedCode;

        [SerializeField] private TextMeshProUGUI playerStatus;
        [SerializeField] private int rowLength;
        [SerializeField] private int colLength;
        [SerializeField] private List<Row> rows;
        [SerializeField] private List<Sprite> playersSymbols;

        //private bool IsBoardComplete => _cells.FindAll(item => item > -1).Count > 0;

        private List<List<int>> _cells; // This stores status of board. 0 - Ai, 1 - user, -1 - Not selected
        private List<int> _cellIndexes; // This stores all cells indexs - 0,1,2,3,4,5,6,7,8,9
        private BaordValidator baordValidator;
        private Vector2 _selectedCells = new Vector2(-1,-1);

        private MarkType _myTurn;
        private MarkType _turn;

        public override void OnInitialize()
        {
            base.OnInitialize();

            Sprite selectedSymbol = null;
            _myTurn = MarkType.X;
            _turn = MarkType.X;

            if (GameManager.Instance.IsMultiplayer)
            {
                if (photonView != null && photonView.IsMine)
                {
                    _myTurn = MarkType.X;
                    _turn = Random.Range(0, 2) == 0 ? MarkType.O : MarkType.X;
                    selectedSymbol = playersSymbols[0];
                }
                else
                {
                    _myTurn = MarkType.O;
                    selectedSymbol = playersSymbols[1];
                }
            }

            _cells = new List<List<int>>();
            _cellIndexes = new List<int>();
            int index = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < rows[i].TotalCells; j++)
                {
                    rows[i].Cells[j].OnCellSelected += OnCellSelected;
                    rows[i].Cells[j].SetData(index, i, j, selectedSymbol);
                    tempList.Add(-1);
                    _cellIndexes.Add(index++);

                }
                _cells.Add(tempList);
            }

            baordValidator = new BaordValidator(_cells, rows);
            baordValidator.OnBoardValidate += OnBoardValidate;

            StartCoroutine(StartRound());
        }

        private IEnumerator StartRound()
        {
            yield return new WaitForSeconds(1.0f);

            if (GameManager.Instance.IsMultiplayer && photonView.IsMine)
                NetworkManager.Instance.RaiseEvent(NetworkEvents.START_ROUND_EVENT, new int[] { (int)_turn });

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
                if(_myTurn == _turn)
                {
                    if (photonView != null)
                    {
                        LoggerUtil.Log("CardBoard : OnCellSelected : PhotonView.isMine : " + photonView.IsMine);
                        if (photonView.IsMine)
                        {
                            SelectedCode = GameManager.Instance.UserColorCode;
                            CellPlayed(rowIndex, index);
                        }
                        else
                        {
                            NetworkManager.Instance.RaiseEvent(NetworkEvents.MOVE_EVENT, new int[]{ rowIndex, index, (int)_myTurn, GameManager.Instance.MultiPlayerUserColorIndex, 1 });
                            CellPlayed(rowIndex, index);
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

        private void CellPlayed(int rowIndex, int index, int itemImageIndex = -1)
        {
            LoggerUtil.Log("CardBoard : CellPlayed : Turn : "+_turn);
            _cells[rowIndex][index] = (int)_turn;
            Cell selectedCell = rows[rowIndex].Cells[index];
            if (itemImageIndex > -1)
                selectedCell.UpdateImage(playersSymbols[itemImageIndex]);
            selectedCell.UpdateCell(_cells[rowIndex][index], SelectedCode);
            _cellIndexes.Remove(selectedCell.Id);
            _selectedCells = new Vector2(rowIndex, index);

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

                if ( _turn == MarkType.X)
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
            LoggerUtil.Log($"Ai : _cellIndexes.Count : ({_cellIndexes.Count})");
            return _cellIndexes[Random.Range(0, _cellIndexes.Count - 1)];
        }

        #region IPunObservable interface Functions, Photon event handling and synchronisaton.

        private Vector2 previousVal = Vector2.zero;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(GameManager.Instance.IsMultiplayer)
            {
                if (stream.IsWriting)
                {
                    if(previousVal.x != _selectedCells.x || previousVal.y != _selectedCells.y)
                    {
                        previousVal = _selectedCells;
                        LoggerUtil.Log("CardBoard : OnPhotonSerializeView : sending : Cells : " + _selectedCells+" : Turn : "+_turn+" : ColorCodeIndex : "+ GameManager.Instance.MultiPlayerUserColorIndex+" : Image Index : 0");
                        if (_selectedCells.x > -1 || _selectedCells.y > -1)
                        {
                            stream.SendNext(_selectedCells);
                            stream.SendNext(_myTurn);
                            stream.SendNext(GameManager.Instance.MultiPlayerUserColorIndex);
                            stream.SendNext(0);
                        }
                    }
                }
                else
                {
                    Vector2 cells = (Vector2)stream.ReceiveNext();
                    _turn = (MarkType)stream.ReceiveNext();
                    int colorIndex = (int)stream.ReceiveNext();
                    SelectedCode = PaletteView.GetColorCodeAtIndex(colorIndex);
                    int imageIndex = (int)stream.ReceiveNext();
                    LoggerUtil.Log("CardBoard : OnPhotonSerializeView : recieving : " + cells + " : Color ind : " + colorIndex);
                    LoggerUtil.Log("CardBoard : OnPhotonSerializeView : sending : Cells : " + cells + " : Turn : " + _turn + " : ColorCodeIndex : " + colorIndex + " : Image Index : "+imageIndex);
                    ProcessCell((int)cells.x, (int)cells.y, imageIndex);
                }
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            NetworkEvents code = (NetworkEvents)photonEvent.Code;
            LoggerUtil.Log("CardBoard : OnEvent : Photon Mine : " + photonView.IsMine + " : Event Code : " + code);
            int[] data = null;
            if (photonView.IsMine)
            {
                switch (code)
                {
                    case NetworkEvents.MOVE_EVENT:
                        data = (int[])photonEvent.CustomData;
                        LoggerUtil.Log("CardBoard : OnEvent : " + string.Join(",", data));
                        _turn = (MarkType)data[2];
                        SelectedCode = PaletteView.GetColorCodeAtIndex((int)data[3]);

                        ProcessCell((int)data[0], (int)data[1], (int)data[4]);
                        break;

                    default:
                        LoggerUtil.Log("CardBoard : OnEvent : No Case Found");
                        break;
                }
            }
            else
            {
                switch(code)
                {
                    case NetworkEvents.START_ROUND_EVENT:
                        data = (int[])photonEvent.CustomData;
                        LoggerUtil.Log("CardBoard : OnEvent : Data : " + string.Join(",", data));
                        _turn = (MarkType)data[0];
                        UpdateRoundText();
                        break;

                    default:
                        LoggerUtil.Log("CardBoard : OnEvent : No Case Found");
                        break;
                }
            }
        }

        private void ProcessCell(int rowIndex, int cellIndex, int itemImageIndex)
        {
            if (!rows[rowIndex].Cells[cellIndex].IsSelected)
            {
                CellPlayed(rowIndex, cellIndex, itemImageIndex);
            }
            else
            {
                LoggerUtil.Log("CardBoard : ProcessCell : Cell is already selected.");
            }
        }
        #endregion
    }
}

