

using System.Collections.Generic;
using Utils;

namespace Board
{
    public class BaordValidator
    {
        private const int MATCH_COUNT = 2;
        private List<List<int>> _cells;
        private List<Cell> _matchCells;
        private List<Row> _rows;

        public BaordValidator(List<List<int>> cells, List<Row> rows)
        {
            _cells = cells;
            _rows = rows;
        }

        public List<Cell> ValidateBoard(int rowIndex, int index, out bool matchFound)
        {
            _matchCells = new List<Cell>();

            CheckForHorizontal(rowIndex, index, out matchFound);
            if (!matchFound)
                CheckForVertical(rowIndex, index, out matchFound);
            if (!matchFound)
                CheckForDigonallyDownRightLeft(rowIndex, index, out matchFound);
            if (!matchFound)
                CheckForDigonallyUpRightLeft(rowIndex, index, out matchFound);

            return _matchCells;
        }

        private void CheckForDigonallyUpRightLeft(int rowIndex, int index, out bool matchFound)
        {
            matchFound = false;
            //Check For Digonally Up - Right
            LoggerUtil.Log("Checking Daigonally Up : Right : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
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
            LoggerUtil.Log("Checking Daigonally Up : Left : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
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
            //Check For Digonally Down - Right
            LoggerUtil.Log("Checking Daigonally Down : Right : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
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
            LoggerUtil.Log("Checking Daigonally Down : Left : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
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
            //check for Vetical Up - 
            LoggerUtil.Log("Checking Vertical : Up : Count : " + _matchCells.Count);
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
            LoggerUtil.Log("Checking Vertical : Down : Count : " + _matchCells.Count);
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
            //check for Horizontal right - 
            LoggerUtil.Log("Checking Horizontal : Right : Count : " + _matchCells.Count + " : RowIndex : " + rowIndex + " : Index : " + index);
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
            LoggerUtil.Log("Checking Horizontal : Left : Count : " + _matchCells.Count);
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
    }
}