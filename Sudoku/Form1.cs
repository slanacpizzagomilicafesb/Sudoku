using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public ReadOnlyRichTextBox SelectedCell;
        public ReadOnlyRichTextBox SelectedNoteCell;
        public List<ReadOnlyRichTextBox> AllCells = new List<ReadOnlyRichTextBox>();
        public List<ReadOnlyRichTextBox> AllNoteCells = new List<ReadOnlyRichTextBox>();
        public List<ReadOnlyRichTextBox> MistakeCells = new List<ReadOnlyRichTextBox>();
        public List<ReadOnlyRichTextBox> RemovedCells = new List<ReadOnlyRichTextBox>();
        bool Notes = false;
        bool BreakUsed = false;

        public Form1()
        {
            InitializeComponent();
            Form1 f = this;
            this.AllCells = GetFunctions.GetCells(f);
            this.AllNoteCells = GetFunctions.GetNotesCells(f);

            foreach (ReadOnlyRichTextBox Cell in this.AllCells)
                Cell.TabStop = false;
            foreach (ReadOnlyRichTextBox NoteCell in this.AllNoteCells)
                NoteCell.TabStop = false;

            //initial selected cell is set to the leftmost and topmost cell
            this.SelectedCell = this.r1c1;
            this.SelectedNoteCell = this.r1c11;

            //initial highlighting
            foreach (ReadOnlyRichTextBox Cell in this.AllCells)
                Cell.BackColor = Color.White;
            foreach (ReadOnlyRichTextBox Cell in this.AllNoteCells)
                Cell.BackColor = Color.White;
            foreach (ReadOnlyRichTextBox Cell in GetFunctions.GetCellsInBox(this, this.SelectedCell).Union(GetFunctions.GetCellsInLine(this, this.SelectedCell).Union(GetFunctions.GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (ReadOnlyRichTextBox Cell in GetFunctions.GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                Cell.BackColor = Color.LightSkyBlue;
            this.SelectedCell.BackColor = Color.LightSkyBlue;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button Button = (Button)sender;

            //checks if the notes button is active
            if (this.Notes == false)
            {
                if (Button.Text == this.SelectedCell.Text)
                {
                    //if you enter the same value as the value that's written in, it gets erased
                    this.SelectedCell.BringToFront();
                    this.SelectedCell.Text = string.Empty;

                    foreach (var Cell in this.AllCells)
                        if (Cell.Text == Button.Text && Cell.Text != "" && Button.Text != "")
                            Cell.BackColor = Color.White;

                    //************************************
                    //          ERROR CHECK
                    //************************************

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetFunctions.GetCellsInBox(this, GetFunctions.GetBox(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetFunctions.GetCellsInBox(this, GetFunctions.GetBox(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4 && C != Cell1))
                            {
                                if (Cell1.Text == Cell2.Text)
                                {
                                    ErrorFunctions.ErrorColour(Cell1, Cell2);
                                    ErrorFunctions.MistakeCellsAdd(this, this.MistakeCells, Cell1, Cell2);
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetFunctions.GetCellsInLine(this, GetFunctions.GetLine(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetFunctions.GetCellsInLine(this, GetFunctions.GetLine(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4 && C != Cell1))
                            {
                                if (Cell1.Text == Cell2.Text)
                                {
                                    ErrorFunctions.ErrorColour(Cell1, Cell2);
                                    ErrorFunctions.MistakeCellsAdd(this, this.MistakeCells, Cell1, Cell2);
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetFunctions.GetCellsInColumn(this, GetFunctions.GetColumn(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetFunctions.GetCellsInColumn(this, GetFunctions.GetColumn(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4 && C != Cell1))
                            {
                                if (Cell1.Text == Cell2.Text)
                                {
                                    ErrorFunctions.ErrorColour(Cell1, Cell2);
                                    ErrorFunctions.MistakeCellsAdd(this, this.MistakeCells, Cell1, Cell2);
                                }
                            }
                        }
                    }

                    foreach (var MistakeCell in this.MistakeCells)
                    {
                        foreach (var Cell in GetFunctions.GetCellsInBox(this, MistakeCell).Union(GetFunctions.GetCellsInLine(this, MistakeCell).Union(GetFunctions.GetCellsInColumn(this, MistakeCell))).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            if (MistakeCell != Cell && MistakeCell.Text != "" && MistakeCell.Text == Cell.Text)
                            {
                                BreakUsed = true;
                                break;
                            }
                        }
                        if (!BreakUsed)
                        {
                            this.RemovedCells.Add(MistakeCell);
                        }
                        BreakUsed = false;
                    }
                    foreach (var RemovedCell in this.RemovedCells)
                    {
                        RemovedCell.ForeColor = Color.Black;
                        if (RemovedCell == this.SelectedCell)
                            RemovedCell.BackColor = Color.LightSkyBlue;
                        else if (GetFunctions.GetCellsInBox(this, this.SelectedCell).Union(GetFunctions.GetCellsInLine(this, this.SelectedCell).Union(GetFunctions.GetCellsInColumn(this, this.SelectedCell))).Where(C => C.Text != "" && C.Name.Length == 4).Contains(RemovedCell))
                            RemovedCell.BackColor = Color.LightCyan;
                        else
                            RemovedCell.BackColor = Color.White;
                        this.MistakeCells.Remove(RemovedCell);
                    }
                    this.RemovedCells.Clear();

                    //************************************
                    //          ERROR CHECK END
                    //************************************
                }
                else
                {
                    //write the value into the cell
                    this.SelectedCell.BringToFront();
                    this.SelectedCell.Text = string.Empty;
                    this.SelectedCell.Text = Button.Text;

                    //erase all the notes from the cells associated with this cell
                    foreach(var NoteCell in GetFunctions.GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                        NoteCell.Text = string.Empty;

                    //align text in centre of the textbox
                    this.SelectedCell.SelectAll();
                    this.SelectedCell.SelectionAlignment = HorizontalAlignment.Center;
                    this.SelectedCell.DeselectAll();

                    foreach (var Cell in this.AllCells)
                        if (Cell.Text == this.SelectedCell.Text && Cell.Text != "" && this.SelectedCell.Text != "")
                            Cell.BackColor = Color.LightSkyBlue;

                    //************************************
                    //          ERROR CHECK
                    //************************************

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetFunctions.GetCellsInBox(this, GetFunctions.GetBox(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetFunctions.GetCellsInBox(this, GetFunctions.GetBox(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4 && C != Cell1))
                            {
                                if (Cell1.Text == Cell2.Text)
                                {
                                    ErrorFunctions.ErrorColour(Cell1, Cell2);
                                    ErrorFunctions.MistakeCellsAdd(this, this.MistakeCells, Cell1, Cell2);

                                    if (Cell1 == this.SelectedCell || Cell2 == this.SelectedCell)
                                        this.SelectedCell.BackColor = Color.LightSalmon;
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetFunctions.GetCellsInLine(this, GetFunctions.GetLine(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetFunctions.GetCellsInLine(this, GetFunctions.GetLine(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4 && C != Cell1))
                            {
                                if (Cell1.Text == Cell2.Text)
                                {
                                    ErrorFunctions.ErrorColour(Cell1, Cell2);
                                    ErrorFunctions.MistakeCellsAdd(this, this.MistakeCells, Cell1, Cell2);

                                    if (Cell1 == this.SelectedCell || Cell2 == this.SelectedCell)
                                        this.SelectedCell.BackColor = Color.LightSalmon;
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetFunctions.GetCellsInColumn(this, GetFunctions.GetColumn(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetFunctions.GetCellsInColumn(this, GetFunctions.GetColumn(this.AllCells, i)).Where(C => C.Text != "" && C.Name.Length == 4 && C != Cell1))
                            {
                                if (Cell1.Text == Cell2.Text)
                                {
                                    ErrorFunctions.ErrorColour(Cell1, Cell2);
                                    ErrorFunctions.MistakeCellsAdd(this, this.MistakeCells, Cell1, Cell2);

                                    if (Cell1 == this.SelectedCell ||  Cell2 == this.SelectedCell)
                                        this.SelectedCell.BackColor = Color.LightSalmon;
                                }
                            }
                        }
                    }

                    foreach (var MistakeCell in this.MistakeCells)
                    {
                        foreach (var Cell in GetFunctions.GetCellsInBox(this, MistakeCell).Union(GetFunctions.GetCellsInLine(this, MistakeCell).Union(GetFunctions.GetCellsInColumn(this, MistakeCell))).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            if (MistakeCell != Cell && MistakeCell.Text != "" && MistakeCell.Text == Cell.Text)
                            {
                                BreakUsed = true;
                                break;
                            }   
                        }
                        if (!BreakUsed)
                        {
                            this.RemovedCells.Add(MistakeCell);
                        }
                        BreakUsed = false;
                    }
                    foreach (var RemovedCell in this.RemovedCells)
                    {
                        RemovedCell.ForeColor = Color.Black;
                        if (RemovedCell == this.SelectedCell)
                            RemovedCell.BackColor = Color.LightSkyBlue;
                        else if (GetFunctions.GetCellsInBox(this, this.SelectedCell).Union(GetFunctions.GetCellsInLine(this, this.SelectedCell).Union(GetFunctions.GetCellsInColumn(this, this.SelectedCell))).Where(C => C.Text != "" && C.Name.Length == 4).Contains(RemovedCell))
                            RemovedCell.BackColor = Color.LightCyan;
                        else
                            RemovedCell.BackColor= Color.White;
                        this.MistakeCells.Remove(RemovedCell);
                    }
                    this.RemovedCells.Clear();

                    //************************************
                    //          ERROR CHECK END
                    //************************************

                    //iterates through note non-empty cells that the selected cell sees
                    foreach (var Cell in GetFunctions.GetCellsInBox(this, this.SelectedCell).Union(GetFunctions.GetCellsInLine(this, this.SelectedCell).Union(GetFunctions.GetCellsInColumn(this, this.SelectedCell))).Where(C => C.Text != "" && C.Name.Length == 5))
                    {
                        //checks if the entered number is the same as the numbers in the note cells
                        //if it is, it deletes the associated note as that cell can no longer contain that number (in theory)
                        if (string.Equals(Button.Text, Cell.Name[4].ToString()) && string.Equals(Button.Text, Cell.Text))
                            Cell.Text = string.Empty;
                        else
                            continue;
                    }


                }
            }
            else
            {
                //iterates through the note cells associated with the selected cell
                foreach (var NoteCell in GetFunctions.GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                {
                    NoteCell.BringToFront();

                    //checks if the entered number is the same as the number that note cell contains
                    if (string.Equals(Button.Text, NoteCell.Name[4].ToString()))
                    {
                        if(Button.Text.Equals(NoteCell.Text))
                            NoteCell.Text = string.Empty; //erases note cell contents if we enter the number that's already written in
                        else
                        {
                            //writes the entered number in the note cell and aligns it
                            this.SelectedCell.Text = string.Empty;
                            NoteCell.Text = Button.Text;
                            NoteCell.SelectAll();
                            NoteCell.SelectionAlignment = HorizontalAlignment.Center;
                            NoteCell.DeselectAll();
                        }
                    }
                    else
                        continue; //skips if not
                }

            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            //setting the SelectedCell and SelectedNoteCell to the clicked cell
            this.SelectedCell = (ReadOnlyRichTextBox)sender;
            this.SelectedNoteCell = GetFunctions.GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell)[0];

            //colouring the cells that the selected cell sees for ease of use
            foreach (var Cell in this.AllCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in this.AllNoteCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in GetFunctions.GetCellsInBox(this, this.SelectedCell).Union(GetFunctions.GetCellsInLine(this, this.SelectedCell).Union(GetFunctions.GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetFunctions.GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                Cell.BackColor = Color.LightSkyBlue;

            //colouring all the cells with the same number as the selected cell for ease of use
            foreach (var Cell in this.AllCells)
                if (Cell.Text == this.SelectedCell.Text && Cell.Text != "" && this.SelectedCell.Text != "")
                    Cell.BackColor = Color.LightSkyBlue;

            //colouring mistake cells
            foreach (var Cell in this.MistakeCells)
                Cell.BackColor = Color.MistyRose;

            if(this.MistakeCells.Contains(this.SelectedCell))
                this.SelectedCell.BackColor = Color.LightSalmon;
            else
                this.SelectedCell.BackColor = Color.LightSkyBlue;
        }

        private void Notes_Cell_Click(object sender, EventArgs e)
        {
            //setting the SelectedCell and SelectedNoteCell to the clicked cell
            this.SelectedNoteCell = (ReadOnlyRichTextBox)sender;
            this.SelectedCell = GetFunctions.GetSpecificCell(this.AllCells, SelectedNoteCell);

            //colouring the cells that the selected cell sees for ease of use
            foreach (var Cell in this.AllCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in this.AllNoteCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in GetFunctions.GetCellsInBox(this, this.SelectedCell).Union(GetFunctions.GetCellsInLine(this, this.SelectedCell).Union(GetFunctions.GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetFunctions.GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                Cell.BackColor = Color.LightSkyBlue;

            //colouring mistake cells
            foreach (var Cell in this.MistakeCells)
                Cell.BackColor = Color.MistyRose;

            this.SelectedCell.BackColor = Color.LightSkyBlue;
        }

        private void NotesButton_Click(object sender, EventArgs e)
        {
            //toggling thte notes button
            this.Notes = !this.Notes;

            //toggling the flatStyle of the button upon click
            Button NoteButton = (Button)sender;
            if (NoteButton.FlatStyle == FlatStyle.Flat)
                NoteButton.FlatStyle = FlatStyle.Standard;
            else if (NoteButton.FlatStyle == FlatStyle.Standard)
                NoteButton.FlatStyle = FlatStyle.Flat;
        }

        private void ClearButton_Click(Object sender, EventArgs e)
        {
            this.SelectedCell.Text = string.Empty;
            foreach(var Cell in GetFunctions.GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                Cell.Text = string.Empty;
        }
    }
}
