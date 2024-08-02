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
        private BoardPlayers _currentPlayer;
        private BaordValidator baordValidator;
        private Vector2 _selectedCells;

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
            _currentPlayer = BoardPlayers.PLAYER_X;
            NetworkManager.Instance.UserCellColorIndex = PaletteView.UserCellColorIndex;
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
            if(GameManager.Instance.IsMultiplayer)
            {
                if (photonView != null && photonView.IsMine)
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
                CellPlayed(rowIndex, index);
            }
        }

        private void CellPlayed(int rowIndex, int index)
        {
            _cells[rowIndex][index] = _currentPlayer == BoardPlayers.PLAYER_X ? 1 : 0;
            NetworkManager.Instance.CellIndexs = new Vector2(rowIndex, index);
            Cell selectedCell = rows[rowIndex].Cells[index];
            selectedCell.UpdateLabel(_cells[rowIndex][index]);
            _cellIndexes.Remove(selectedCell.Id);
            _selectedCells = new Vector2(rowIndex, index);

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

        #region IPunObservable interface Functions, Photon event handling and synchronisaton.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                LoggerUtil.Log("CardBoard : OnPhotonSerializeView : sending : " + _selectedCells);
                stream.SendNext(_selectedCells);
                stream.SendNext(0);
            }
            else
            {
                Vector2 cells = (Vector2)stream.ReceiveNext();
                int colorIndex = (int)stream.ReceiveNext();
                SelectedCode = PaletteView.GetColorCodeAtIndex(colorIndex);
                LoggerUtil.Log("CardBoard : OnPhotonSerializeView : recieving : " + cells + " : Color ind : " + colorIndex);
                ProcessCell((int)cells.x, (int)cells.y);
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
            OnCellSelected(rowIndex, cellIndex);
            rows[rowIndex].Cells[cellIndex].UpdateCell();
        }
        #endregion
    }
}

