using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        ReadOnlyRichTextBox SelectedCell;
        ReadOnlyRichTextBox SelectedNoteCell;
        List<ReadOnlyRichTextBox> AllCells = new List<ReadOnlyRichTextBox>();
        List<ReadOnlyRichTextBox> AllNoteCells = new List<ReadOnlyRichTextBox>();
        List<ReadOnlyRichTextBox> MistakeCells = new List<ReadOnlyRichTextBox>();
        List<ReadOnlyRichTextBox> RemovedCells = new List<ReadOnlyRichTextBox>();
        bool Notes = false;
        bool BreakUsed = false;

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
            foreach(Control Cell in form.Controls)
                if(Cell is ReadOnlyRichTextBox && string.Equals(SelectedCell.Name[1].ToString(), Cell.Name[1].ToString()))
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

        public static ReadOnlyRichTextBox GetBox(Form1 form, int BoxNum)
        {
            switch (BoxNum)
            {
                case 1:
                    return form.r1c1;
                case 2:
                    return form.r1c4;
                case 3:
                    return form.r1c7;
                case 4:
                    return form.r4c1;
                case 5:
                    return form.r4c4;
                case 6:
                    return form.r4c7;
                case 7:
                    return form.r7c1;
                case 8:
                    return form.r7c4;
                case 9:
                    return form.r7c7;
                default:
                    break;
            }
            return form.r1c1;
        }

        public static ReadOnlyRichTextBox GetLine(Form1 form, int LineNum)
        {
            switch (LineNum)
            {
                case 1:
                    return form.r1c1;
                case 2:
                    return form.r2c1;
                case 3:
                    return form.r3c1;
                case 4:
                    return form.r4c1;
                case 5:
                    return form.r5c1;
                case 6:
                    return form.r6c1;
                case 7:
                    return form.r7c1;
                case 8:
                    return form.r8c1;
                case 9:
                    return form.r9c1;
                default:
                    break;
            }
            return form.r1c1;
        }

        public static ReadOnlyRichTextBox GetColumn(Form1 form, int ColNum)
        {
            switch (ColNum)
            {
                case 1:
                    return form.r1c1;
                case 2:
                    return form.r1c2;
                case 3:
                    return form.r1c3;
                case 4:
                    return form.r1c4;
                case 5:
                    return form.r1c5;
                case 6:
                    return form.r1c6;
                case 7:
                    return form.r1c7;
                case 8:
                    return form.r1c8;
                case 9:
                    return form.r1c9;
                default:
                    break;
            }
            return form.r1c1;
        }

        public Form1()
        {
            InitializeComponent();
            Form1 f = this;
            this.AllCells = GetCells(f);
            this.AllNoteCells = GetNotesCells(f);

            foreach (ReadOnlyRichTextBox Cell in AllCells)
                Cell.TabStop = false;
            foreach (ReadOnlyRichTextBox NoteCell in AllNoteCells)
                NoteCell.TabStop = false;

            //initial selected cell is set to the leftmost and topmost cell
            this.SelectedCell = this.r1c1;
            this.SelectedNoteCell = this.r1c11;

            //initial highlighting
            foreach (ReadOnlyRichTextBox Cell in this.AllCells)
                Cell.BackColor = Color.White;
            foreach (ReadOnlyRichTextBox Cell in this.AllNoteCells)
                Cell.BackColor = Color.White;
            foreach (ReadOnlyRichTextBox Cell in GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (ReadOnlyRichTextBox Cell in GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                Cell.BackColor = Color.LightSkyBlue;
            this.SelectedCell.BackColor = Color.LightSkyBlue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                        foreach (var Cell1 in GetCellsInBox(this, GetBox(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetCellsInBox(this, GetBox(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                            {
                                if (Cell1 != Cell2 && Cell1.Text == Cell2.Text)
                                {
                                    Cell1.ForeColor = Color.Red;
                                    Cell1.BackColor = Color.MistyRose;
                                    Cell2.ForeColor = Color.Red;
                                    Cell2.BackColor = Color.MistyRose;

                                    if (!this.MistakeCells.Contains(Cell1))
                                        this.MistakeCells.Add(Cell1);
                                    if (!this.MistakeCells.Contains(Cell2))
                                        this.MistakeCells.Add(Cell2);
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetCellsInLine(this, GetLine(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetCellsInLine(this, GetLine(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                            {
                                if (Cell1 != Cell2 && Cell1.Text == Cell2.Text)
                                {
                                    Cell1.ForeColor = Color.Red;
                                    Cell1.BackColor = Color.MistyRose;
                                    Cell2.ForeColor = Color.Red;
                                    Cell2.BackColor = Color.MistyRose;

                                    if (!this.MistakeCells.Contains(Cell1))
                                        this.MistakeCells.Add(Cell1);
                                    if (!this.MistakeCells.Contains(Cell2))
                                        this.MistakeCells.Add(Cell2);
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetCellsInColumn(this, GetColumn(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetCellsInColumn(this, GetColumn(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                            {
                                if (Cell1 != Cell2 && Cell1.Text == Cell2.Text)
                                {
                                    Cell1.ForeColor = Color.Red;
                                    Cell1.BackColor = Color.MistyRose;
                                    Cell2.ForeColor = Color.Red;
                                    Cell2.BackColor = Color.MistyRose;

                                    if (!this.MistakeCells.Contains(Cell1))
                                        this.MistakeCells.Add(Cell1);
                                    if (!this.MistakeCells.Contains(Cell2))
                                        this.MistakeCells.Add(Cell2);
                                }
                            }
                        }
                    }

                    foreach (var MistakeCell in this.MistakeCells)
                    {
                        foreach (var Cell in GetCellsInBox(this, MistakeCell).Union(GetCellsInLine(this, MistakeCell).Union(GetCellsInColumn(this, MistakeCell))).Where(C => C.Text != "" && C.Name.Length == 4))
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
                        else if (GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))).Where(C => C.Text != "" && C.Name.Length == 4).Contains(RemovedCell))
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
                    foreach(var NoteCell in GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
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
                        foreach (var Cell1 in GetCellsInBox(this, GetBox(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetCellsInBox(this, GetBox(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                            {
                                if (Cell1 != Cell2 && Cell1.Text == Cell2.Text)
                                {
                                    Cell1.ForeColor = Color.Red;
                                    Cell1.BackColor = Color.MistyRose;
                                    Cell2.ForeColor = Color.Red;
                                    Cell2.BackColor = Color.MistyRose;

                                    if (!this.MistakeCells.Contains(Cell1))
                                        this.MistakeCells.Add(Cell1);
                                    if (!this.MistakeCells.Contains(Cell2))
                                        this.MistakeCells.Add(Cell2);

                                    if (Cell1 == this.SelectedCell || Cell2 == this.SelectedCell)
                                        this.SelectedCell.BackColor = Color.LightSalmon;
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetCellsInLine(this, GetLine(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetCellsInLine(this, GetLine(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                            {
                                if (Cell1 != Cell2 && Cell1.Text == Cell2.Text)
                                {
                                    Cell1.ForeColor = Color.Red;
                                    Cell1.BackColor = Color.MistyRose;
                                    Cell2.ForeColor = Color.Red;
                                    Cell2.BackColor = Color.MistyRose;

                                    if (!this.MistakeCells.Contains(Cell1))
                                        this.MistakeCells.Add(Cell1);
                                    if (!this.MistakeCells.Contains(Cell2))
                                        this.MistakeCells.Add(Cell2);

                                    if (Cell1 == this.SelectedCell || Cell2 == this.SelectedCell)
                                        this.SelectedCell.BackColor = Color.LightSalmon;
                                }
                            }
                        }
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        foreach (var Cell1 in GetCellsInColumn(this, GetColumn(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                        {
                            foreach (var Cell2 in GetCellsInColumn(this, GetColumn(this, i)).Where(C => C.Text != "" && C.Name.Length == 4))
                            {
                                if (Cell1 != Cell2 && Cell1.Text == Cell2.Text)
                                {
                                    Cell1.ForeColor = Color.Red;
                                    Cell1.BackColor = Color.MistyRose;
                                    Cell2.ForeColor = Color.Red;
                                    Cell2.BackColor = Color.MistyRose;

                                    if (!this.MistakeCells.Contains(Cell1))
                                        this.MistakeCells.Add(Cell1);
                                    if (!this.MistakeCells.Contains(Cell2))
                                        this.MistakeCells.Add(Cell2);

                                    if (Cell1 == this.SelectedCell ||  Cell2 == this.SelectedCell)
                                        this.SelectedCell.BackColor = Color.LightSalmon;
                                }
                            }
                        }
                    }

                    foreach (var MistakeCell in this.MistakeCells)
                    {
                        foreach (var Cell in GetCellsInBox(this, MistakeCell).Union(GetCellsInLine(this, MistakeCell).Union(GetCellsInColumn(this, MistakeCell))).Where(C => C.Text != "" && C.Name.Length == 4))
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
                        else if (GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))).Where(C => C.Text != "" && C.Name.Length == 4).Contains(RemovedCell))
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
                    foreach (var Cell in GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))).Where(C => C.Text != "" && C.Name.Length == 5))
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
                foreach (var NoteCell in GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
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
            this.SelectedNoteCell = GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell)[0];

            //colouring the cells that the selected cell sees for ease of use
            foreach (var Cell in this.AllCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in this.AllNoteCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
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
            this.SelectedCell = GetSpecificCell(this.AllCells, SelectedNoteCell);

            //colouring the cells that the selected cell sees for ease of use
            foreach (var Cell in this.AllCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in this.AllNoteCells)
                Cell.BackColor = Color.White;
            foreach (var Cell in GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
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
            foreach(var Cell in GetNotesCellsSubset(this.AllNoteCells, this.SelectedCell))
                Cell.Text = string.Empty;
        }
    }
}
