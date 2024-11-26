using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class ErrorFunctions
    {
        public static void ErrorColour(ReadOnlyRichTextBox Cell1, ReadOnlyRichTextBox Cell2)
        {
            Cell1.ForeColor = Color.Red;
            Cell1.BackColor = Color.MistyRose;
            Cell2.ForeColor = Color.Red;
            Cell2.BackColor = Color.MistyRose;
        }

        public static void MistakeCellsAdd(Form1 form, List<ReadOnlyRichTextBox> MistakeCells, ReadOnlyRichTextBox Cell1, ReadOnlyRichTextBox Cell2)
        {
            if (!form.MistakeCells.Contains(Cell1))
                form.MistakeCells.Add(Cell1);
            if (!form.MistakeCells.Contains(Cell2))
                form.MistakeCells.Add(Cell2);
        }
    }
}
