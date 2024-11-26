using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class GetFunctions
    {
        //Function for getting all regular non-note cells
        public static List<ReadOnlyRichTextBox> GetCells(Form form)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in form.Controls)
                if (Cell is ReadOnlyRichTextBox && Cell.Name.Length == 4)
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);

            return richTextBoxList;
        }

        //Function for getting all note cells
        public static List<ReadOnlyRichTextBox> GetNotesCells(Form form)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in form.Controls)
                if (Cell is ReadOnlyRichTextBox && Cell.Name.Length == 5)
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);

            return richTextBoxList;
        }

        //Function for getting all notes cells associated with a single regular cell
        public static List<ReadOnlyRichTextBox> GetNotesCellsSubset(List<ReadOnlyRichTextBox> NoteCells, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in NoteCells)
                if (Cell is ReadOnlyRichTextBox && Cell.Name.Substring(0, 4).Contains(SelectedCell.Name))
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);

            return richTextBoxList;
        }

        //Function for getting a regular cell from any note cell associated with it
        public static ReadOnlyRichTextBox GetSpecificCell(List<ReadOnlyRichTextBox> Cells, ReadOnlyRichTextBox SelectedNoteCell)
        {
            ReadOnlyRichTextBox richTextBox = SelectedNoteCell;
            foreach (Control Cell in Cells)
                if (Cell is ReadOnlyRichTextBox && SelectedNoteCell.Name.Substring(0, 4).Contains(Cell.Name))
                {
                    richTextBox = (ReadOnlyRichTextBox)Cell;
                    break;
                }

            return richTextBox;
        }

        //Function for getting all cells in the line of the selected cell
        public static List<ReadOnlyRichTextBox> GetCellsInLine(Form1 form, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in form.Controls)
                if (Cell is ReadOnlyRichTextBox && string.Equals(SelectedCell.Name[1].ToString(), Cell.Name[1].ToString()))
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);

            return richTextBoxList;
        }

        //Function for getting all cells in the column of the selected cell
        public static List<ReadOnlyRichTextBox> GetCellsInColumn(Form1 form, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in form.Controls)
                if (Cell is ReadOnlyRichTextBox && string.Equals(SelectedCell.Name[3].ToString(), Cell.Name[3].ToString()))
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);

            return richTextBoxList;
        }

        //Function for getting all cells in the box of the selected cell
        public static List<ReadOnlyRichTextBox> GetCellsInBox(Form1 form, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();

            int RowRatio = (int.Parse(SelectedCell.Name[1].ToString()) - 1) / 3;
            int ColumnRatio = (int.Parse(SelectedCell.Name[3].ToString()) - 1) / 3;

            foreach (Control Cell in form.Controls)
                if (Cell is ReadOnlyRichTextBox && (int.Parse(Cell.Name[1].ToString()) - 1) / 3 == RowRatio && (int.Parse(Cell.Name[3].ToString()) - 1) / 3 == ColumnRatio)
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);

            return richTextBoxList;
        }

        //Function for getting the beginning cell of a box based on the box number
        public static ReadOnlyRichTextBox GetBox(List<ReadOnlyRichTextBox> Cells, int BoxNum)
        {
            ReadOnlyRichTextBox returnCell = null;
            foreach (ReadOnlyRichTextBox Cell in Cells)
                if (string.Equals(Cell.Name[1].ToString(), BoxNum.ToString()))
                {
                    if (BoxNum % 3 != 0)
                    {
                        if (string.Equals(Cell.Name[3].ToString(), ((BoxNum % 3) * 3).ToString()))
                            returnCell = Cell;
                    }
                    else
                    {
                        if (string.Equals(Cell.Name[3].ToString(), "9"))
                            returnCell = Cell;
                    }
                }
            return returnCell!;
        }

        //Function for getting the beginning cell of a line based on the line number
        public static ReadOnlyRichTextBox GetLine(List<ReadOnlyRichTextBox> Cells, int LineNum)
        {
            ReadOnlyRichTextBox returnCell = null;
            foreach (ReadOnlyRichTextBox Cell in Cells)
                if (string.Equals(Cell.Name[1].ToString(), LineNum.ToString()))
                    returnCell = Cell;
            return returnCell!;
        }

        //Function for getting the beginning cell of a column based on the column number
        public static ReadOnlyRichTextBox GetColumn(List<ReadOnlyRichTextBox> Cells, int ColNum)
        {
            ReadOnlyRichTextBox returnCell = null;
            foreach (ReadOnlyRichTextBox Cell in Cells)
                if (string.Equals(Cell.Name[3].ToString(), ColNum.ToString()))
                    returnCell = Cell;
            return returnCell!;
        }
    }
}
