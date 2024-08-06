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
    public class SinglePlayerCardBoard : CardBoard
    {
        public override void OnInitialize()
        {
           
            _defaultSymbol = playersSymbols[0];
            _myTurn = MarkType.X;
            _turn = MarkType.X;

            base.OnInitialize();
        }

        protected override void OnBoardValidate(BoardValidType type)
        {
            base.OnBoardValidate(type);

            LoggerUtil.Log("OnBoardValidate : SinglePlayerCardBoard : Type : " + type+" : Turn : "+_turn);

            if (type != BoardValidType.WIN && GameManager.Instance.IsSinglePlayer)
            {
                if (_turn == MarkType.O )
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
    }
}

