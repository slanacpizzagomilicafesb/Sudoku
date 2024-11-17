using System.Windows.Forms.VisualStyles;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        ReadOnlyRichTextBox SelectedCell;
        ReadOnlyRichTextBox SelectedNoteCell;
        Boolean Notes = false;

        public static List<ReadOnlyRichTextBox> GetCells(Form form)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in form.Controls)
            {
                if (Cell is ReadOnlyRichTextBox && Cell.Name.Length == 4)
                {
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);
                }
            }
            return richTextBoxList;
        }

        public static List<ReadOnlyRichTextBox> GetNotesCells(Form form)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in form.Controls)
            {
                if (Cell is ReadOnlyRichTextBox && Cell.Name.Length == 5)
                {
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);
                }
            }
            return richTextBoxList;
        }

        public static List<ReadOnlyRichTextBox> GetNotesCellsSubset(List<ReadOnlyRichTextBox> NoteCells, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in NoteCells)
            {
                if (Cell is ReadOnlyRichTextBox && Cell.Name.Length == 5 && Cell.Name.Substring(0, 4).Contains(SelectedCell.Name))
                {
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);
                }
            }
            return richTextBoxList;
        }

        public static ReadOnlyRichTextBox GetSpecificCell(List<ReadOnlyRichTextBox> Cells, ReadOnlyRichTextBox SelectedNoteCell)
        {
            ReadOnlyRichTextBox richTextBox = SelectedNoteCell;
            foreach (Control Cell in Cells)
            {
                if (Cell is ReadOnlyRichTextBox && SelectedNoteCell.Name.Substring(0, 4).Contains(Cell.Name))
                {
                    richTextBox = (ReadOnlyRichTextBox)Cell;
                    break;
                }
            }
            return richTextBox;
        }

        public static List<ReadOnlyRichTextBox> GetCellsInLine(Form1 form, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach(Control Cell in form.Controls)
            {
                if(Cell is ReadOnlyRichTextBox && string.Equals(SelectedCell.Name[1].ToString(), Cell.Name[1].ToString()))
                {
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);
                }
            }
            return richTextBoxList;
        }

        public static List<ReadOnlyRichTextBox> GetCellsInColumn(Form1 form, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();
            foreach (Control Cell in form.Controls)
            {
                if (Cell is ReadOnlyRichTextBox && string.Equals(SelectedCell.Name[3].ToString(), Cell.Name[3].ToString()))
                {
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);
                }
            }
            return richTextBoxList;
        }

        public static List<ReadOnlyRichTextBox> GetCellsInBox(Form1 form, ReadOnlyRichTextBox SelectedCell)
        {
            List<ReadOnlyRichTextBox> richTextBoxList = new List<ReadOnlyRichTextBox>();

            int RowRatio = (int.Parse(SelectedCell.Name[1].ToString()) - 1) / 3;
            int ColumnRatio = (int.Parse(SelectedCell.Name[3].ToString()) - 1) / 3;

            foreach (Control Cell in form.Controls)
            {
                if (Cell is ReadOnlyRichTextBox && (int.Parse(Cell.Name[1].ToString()) - 1) / 3 == RowRatio && (int.Parse(Cell.Name[3].ToString()) - 1) / 3 == ColumnRatio)
                {
                    richTextBoxList.Add((ReadOnlyRichTextBox)Cell);
                }
            }
            return richTextBoxList;
        }

        public Form1()
        {
            InitializeComponent();
            Form1 f = this;
            var Cells = GetCells(f);
            foreach (var Cell in Cells)
            {
                Cell.TabStop = false;
            }

            var NoteCells = GetCells(f);
            foreach (var NoteCell in NoteCells)
            {
                NoteCell.TabStop = false;
            }

            this.SelectedCell = this.r1c1;
            this.SelectedNoteCell = this.r1c11;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button Button = (Button)sender;

            if (this.Notes == false)
            {
                if (Button.Text == this.SelectedCell.Text)
                {
                    this.SelectedCell.BringToFront();
                    this.SelectedCell.Text = string.Empty;
                }
                else
                {
                    this.SelectedCell.BringToFront();
                    this.SelectedCell.Text = string.Empty;
                    this.SelectedCell.Text = Button.Text;

                    var NoteCells = GetNotesCells(this);
                    var NoteCellsSubset = GetNotesCellsSubset(NoteCells, SelectedCell);

                    foreach(var NoteCell in NoteCellsSubset)
                    {
                        NoteCell.Text = string.Empty;
                    }

                    this.SelectedCell.SelectAll();
                    this.SelectedCell.SelectionAlignment = HorizontalAlignment.Center;
                    this.SelectedCell.DeselectAll();
                }
            }
            else
            {
                var NoteCells = GetNotesCells(this);
                var NoteCellsSubset = GetNotesCellsSubset(NoteCells, this.SelectedCell);
                foreach (var NoteCell in NoteCellsSubset)
                {
                    NoteCell.BringToFront();

                    if (string.Equals(Button.Text, NoteCell.Name[4].ToString()))
                    {
                        if(Button.Text.Equals(NoteCell.Text))
                        {
                            NoteCell.Text = string.Empty;
                        }
                        else
                        {
                            this.SelectedCell.Text = string.Empty;
                            NoteCell.Text = Button.Text;
                            NoteCell.SelectAll();
                            NoteCell.SelectionAlignment = HorizontalAlignment.Center;
                            NoteCell.DeselectAll();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            this.SelectedCell = (ReadOnlyRichTextBox)sender;
            var Cells = GetCells(this);
            var NoteCells = GetNotesCells(this);
            var NoteCellsSubset = GetNotesCellsSubset(NoteCells, this.SelectedCell);
            this.SelectedNoteCell = NoteCellsSubset[0];
            var LineCells = GetCellsInLine(this, SelectedCell);
            var ColumnCells = GetCellsInColumn(this, SelectedCell);
            var BoxCells = GetCellsInBox(this, SelectedCell);

            foreach (var Cell in Cells)
            {
                Cell.BackColor = Color.White;
            }
            foreach (var Cell in NoteCells)
            {
                Cell.BackColor = Color.White;
            }
            foreach (var Cell in LineCells)
            {
                Cell.BackColor = Color.LightCyan;
            }
            foreach (var Cell in ColumnCells)
            {
                Cell.BackColor = Color.LightCyan;
            }
            foreach (var Cell in BoxCells)
            {
                Cell.BackColor = Color.LightCyan;
            }
            foreach (var Cell in NoteCellsSubset)
            {
                Cell.BackColor = Color.LightSkyBlue;
            }
            this.SelectedCell.BackColor = Color.LightSkyBlue;
        }

        private void Notes_Cell_Click(object sender, EventArgs e)
        {
            this.SelectedNoteCell = (ReadOnlyRichTextBox)sender;
            var Cells = GetCells(this);
            var NoteCells = GetNotesCells(this);
            this.SelectedCell = GetSpecificCell(Cells, SelectedNoteCell);
            var NoteCellsSubset = GetNotesCellsSubset(NoteCells, SelectedCell);
            var LineCells = GetCellsInLine(this, SelectedCell);
            var ColumnCells = GetCellsInColumn(this, SelectedCell);
            var BoxCells = GetCellsInBox(this, SelectedCell);

            foreach (var Cell in Cells)
            {
                Cell.BackColor = Color.White;
            }
            foreach (var Cell in NoteCells)
            {
                Cell.BackColor = Color.White;
            }
            foreach (var Cell in LineCells)
            {
                Cell.BackColor = Color.LightCyan;
            }
            foreach (var Cell in ColumnCells)
            {
                Cell.BackColor = Color.LightCyan;
            }
            foreach (var Cell in BoxCells)
            {
                Cell.BackColor = Color.LightCyan;
            }
            foreach (var Cell in NoteCellsSubset)
            {
                Cell.BackColor = Color.LightSkyBlue;
            }
            this.SelectedCell.BackColor = Color.LightSkyBlue;
        }

        private void NotesButton_Click(object sender, EventArgs e)
        {
            this.Notes = !this.Notes;
            Button NoteButton = (Button)sender;
            if (NoteButton.FlatStyle == FlatStyle.Flat)
            {
                NoteButton.FlatStyle = FlatStyle.Standard;
            }
            else if (NoteButton.FlatStyle == FlatStyle.Standard)
            {
                NoteButton.FlatStyle = FlatStyle.Flat;
            }
        }

    }
}
