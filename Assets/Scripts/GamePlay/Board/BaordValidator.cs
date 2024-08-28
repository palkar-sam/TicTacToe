using System;
using System.Collections.Generic;
using Aik.Utils;

namespace Board
{
    public class BaordValidator
    {
        private const int MATCH_COUNT = 3;

        public event Action<BoardValidType> OnBoardValidate;

        private List<List<int>> _cells;
        private List<Cell> _matchCells;
        private List<Row> _rows;

        public BaordValidator(List<List<int>> cells, List<Row> rows)
        {
            _cells = cells;
            _rows = rows;
        }

        public void ValidateBoard(int rowIndex, int index)
        {
            _matchCells = new List<Cell>();
            ResetMatchCells(rowIndex, index);
            CheckForHorizontal(rowIndex, index, out bool matchFound);
            if (!matchFound)
                CheckForVertical(rowIndex, index, out matchFound);
            if (!matchFound)
                CheckForDigonallyDownRightLeft(rowIndex, index, out matchFound);
            if (!matchFound)
                CheckForDigonallyUpRightLeft(rowIndex, index, out matchFound);

            Log("ValidateBoard status : " + _matchCells.Count + " : Match Found : " + matchFound);

            if (matchFound)
            {
                OnBoardValidate?.Invoke(BoardValidType.WIN);
            }
            else if (_matchCells.Count == (_cells[0].Count * _cells.Count))
            {
                OnBoardValidate?.Invoke(BoardValidType.DRAW);
            }
            else
            {
                OnBoardValidate?.Invoke(BoardValidType.NEXT_ROUND);
            }

            _matchCells.Clear();
        }

        private void ResetMatchCells(int rowIndex, int index)
        {
            _matchCells.Clear();
            _matchCells.Add(_rows[rowIndex].Cells[index]);
        }

        private void CheckForDigonallyUpRightLeft(int rowIndex, int index, out bool matchFound)
        {
            matchFound = false;
            ResetMatchCells(rowIndex, index);
            //Check For Digonally Up - Right
            Log("Checking Daigonally Up : Right : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
            for (int i = index + 1, j = rowIndex - 1; i < _cells[rowIndex].Count; i++, j--)
            {
                if (j >= 0)
                {
                    if (!matchFound && _cells[rowIndex][index] == _cells[j][i])
                    {

                        _matchCells.Add(_rows[j].Cells[i]);
                        if (_matchCells.Count >= MATCH_COUNT)
                            matchFound = true;
                    }
                    else
                        break;
                }
            }

            //Check For Digonally Up - Left
            Log("Checking Daigonally Up : Left : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
            for (int i = index - 1, j = rowIndex - 1; i >= 0; i--, j--)
            {
                if (j >= 0)
                {
                    if (!matchFound && _cells[rowIndex][index] == _cells[j][i])
                    {

                        _matchCells.Add(_rows[j].Cells[i]);
                        if (_matchCells.Count >= MATCH_COUNT)
                            matchFound = true;
                    }
                    else
                        break;
                }
            }
        }

        private void CheckForDigonallyDownRightLeft(int rowIndex, int index, out bool matchFound)
        {
            matchFound = false;
            ResetMatchCells(rowIndex, index);
            //Check For Digonally Down - Right
            Log("Checking Daigonally Down : Right : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
            for (int i = index + 1, j = rowIndex + 1; i < _cells[rowIndex].Count; i++, j++)
            {
                if (j < _cells.Count)
                {
                    if (!matchFound && _cells[rowIndex][index] == _cells[j][i])
                    {

                        _matchCells.Add(_rows[j].Cells[i]);
                        if (_matchCells.Count >= MATCH_COUNT)
                            matchFound = true;
                    }
                    else
                        break;
                }
            }

            //Check For Digonally Down - Left
            Log("Checking Daigonally Down : Left : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
            for (int i = index - 1, j = rowIndex + 1; i >= 0; i--, j++)
            {
                if (j < _cells.Count)
                {
                    if (!matchFound && _cells[rowIndex][index] == _cells[j][i])
                    {

                        _matchCells.Add(_rows[j].Cells[i]);
                        if (_matchCells.Count >= MATCH_COUNT)
                            matchFound = true;
                    }
                    else
                        break;
                }
            }
        }

        private void CheckForVertical(int rowIndex, int index, out bool matchFound)
        {
            matchFound = false;
            ResetMatchCells(rowIndex, index);
            //check for Vetical Up - 
            Log("Checking Vertical : Up : Count : " + _matchCells.Count);
            for (int i = rowIndex - 1; i >= 0; i--)
            {
                if (!matchFound && _cells[rowIndex][index] == _cells[i][index])
                {

                    _matchCells.Add(_rows[i].Cells[index]);
                    if (_matchCells.Count >= MATCH_COUNT)
                        matchFound = true;
                }
                else
                    break;
            }

            //check for Vetical Down - 
            Log("Checking Vertical : Down : Count : " + _matchCells.Count);
            for (int i = rowIndex + 1; i < _cells.Count; i++)
            {
                if (!matchFound && _cells[rowIndex][index] == _cells[i][index])
                {

                    _matchCells.Add(_rows[i].Cells[index]);
                    if (_matchCells.Count >= MATCH_COUNT)
                        matchFound = true;
                }
                else
                    break;
            }
        }

        private void CheckForHorizontal(int rowIndex, int index, out bool matchFound)
        {
            matchFound = false;
            ResetMatchCells(rowIndex, index);
            //check for Horizontal right - 
            Log("Checking Horizontal : Right : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
            for (int i = index + 1; i < _cells[rowIndex].Count; i++)
            {
                if (!matchFound && _cells[rowIndex][index] == _cells[rowIndex][i])
                {

                    _matchCells.Add(_rows[rowIndex].Cells[i]);
                    if (_matchCells.Count >= MATCH_COUNT)
                        matchFound = true;
                }
                else
                    break;
            }

            //check for Horizontal left - 
            Log("Checking Horizontal : Left : Count : " + _matchCells.Count);
            for (int i = index - 1; i >= 0; i--)
            {
                if (!matchFound && _cells[rowIndex][index] == _cells[rowIndex][i])
                {

                    _matchCells.Add(_rows[rowIndex].Cells[i]);
                    if (_matchCells.Count >= MATCH_COUNT)
                        matchFound = true;
                }
                else
                    break;
            }
        }

        private void Log(string str)
        {
            LoggerUtil.Log("Board Validation : " + str);
        }
    }

    public enum BoardValidType
    {
        WIN,
        DRAW,
        NEXT_ROUND
    }

    public enum Winner
    {
        USER,
        AI
    }
}