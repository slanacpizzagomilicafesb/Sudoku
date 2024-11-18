using System.Windows.Forms.VisualStyles;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        ReadOnlyRichTextBox SelectedCell;
        ReadOnlyRichTextBox SelectedNoteCell;
        Boolean Notes = false;

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

        public Form1()
        {
            InitializeComponent();
            Form1 f = this;
            var Cells = GetCells(f);
            foreach (var Cell in Cells)
                Cell.TabStop = false;

            var NoteCells = GetNotesCells(f);
            foreach (var NoteCell in NoteCells)
                NoteCell.TabStop = false;

            //initial selected cell is set to the leftmost and topmost cell
            this.SelectedCell = this.r1c1;
            this.SelectedNoteCell = this.r1c11;

            //initial highlighting
            foreach (var Cell in GetCells(this))
                Cell.BackColor = Color.White;
            foreach (var Cell in GetNotesCells(this))
                Cell.BackColor = Color.White;
            foreach (var Cell in GetCellsInLine(this, this.SelectedCell))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetCellsInColumn(this, this.SelectedCell))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetCellsInBox(this, this.SelectedCell))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetNotesCellsSubset(GetNotesCells(this), this.SelectedCell))
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
                }
                else
                {
                    //write the value into the cell
                    this.SelectedCell.BringToFront();
                    this.SelectedCell.Text = string.Empty;
                    this.SelectedCell.Text = Button.Text;

                    //erase all the notes from the cells associated with this cell
                    foreach(var NoteCell in GetNotesCellsSubset(GetNotesCells(this), this.SelectedCell))
                        NoteCell.Text = string.Empty;

                    //align text in centre of the textbox
                    this.SelectedCell.SelectAll();
                    this.SelectedCell.SelectionAlignment = HorizontalAlignment.Center;
                    this.SelectedCell.DeselectAll();

                    //************************************
                    //          ERROR CHECK
                    //************************************

                    //iterates through regular non-empty cells that the selected cell sees
                    foreach (var Cell in GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))).Where(C => C.Text != "" && C.Name.Length == 4))
                    {
                        if (this.SelectedCell != Cell && this.SelectedCell.Text != "" && this.SelectedCell.Text == Cell.Text)
                        {
                            //if true, colour the conflicting cells and the number in the selected cell a shade of red to indicate conflict
                            this.SelectedCell.ForeColor = Color.Red;
                            this.SelectedCell.BackColor = Color.LightSalmon;
                            Cell.BackColor = Color.MistyRose;
                        }
                        else
                        {
                            //if not, return to neutral state (this part is still unfinished, I'm counting on the player to correct their mistake immediately)
                            this.SelectedCell.ForeColor = Color.Black;
                            this.SelectedCell.BackColor = Color.LightSkyBlue;
                            Cell.BackColor = Color.LightCyan;
                            Cell.ForeColor = Color.Black;
                            foreach (var MatchingCell in GetCells(this).Where(C => C.Text != ""))
                                if (MatchingCell.Text == this.SelectedCell.Text && this.SelectedCell.Text != "")
                                    MatchingCell.BackColor = Color.LightSkyBlue;
                        }
                    }

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
                foreach (var NoteCell in GetNotesCellsSubset(GetNotesCells(this), this.SelectedCell))
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
            this.SelectedNoteCell = GetNotesCellsSubset(GetNotesCells(this), this.SelectedCell)[0];

            //colouring the cells that the selected cell sees for ease of use
            foreach (var Cell in GetCells(this))
                Cell.BackColor = Color.White;
            foreach (var Cell in GetNotesCells(this))
                Cell.BackColor = Color.White;
            foreach (var Cell in GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetNotesCellsSubset(GetNotesCells(this), this.SelectedCell))
                Cell.BackColor = Color.LightSkyBlue;

            //colouring all the cells with the same number as the selected cell for ease of use
            foreach (var Cell in GetCells(this))
                if (Cell.Text == this.SelectedCell.Text && Cell.Text != "" && this.SelectedCell.Text != "")
                    Cell.BackColor = Color.LightSkyBlue;
            this.SelectedCell.BackColor = Color.LightSkyBlue;
        }

        private void Notes_Cell_Click(object sender, EventArgs e)
        {
            //setting the SelectedCell and SelectedNoteCell to the clicked cell
            this.SelectedNoteCell = (ReadOnlyRichTextBox)sender;
            this.SelectedCell = GetSpecificCell(GetCells(this), SelectedNoteCell);

            //colouring the cells that the selected cell sees for ease of use
            foreach (var Cell in GetCells(this))
                Cell.BackColor = Color.White;
            foreach (var Cell in GetNotesCells(this))
                Cell.BackColor = Color.White;
            foreach (var Cell in GetCellsInBox(this, this.SelectedCell).Union(GetCellsInLine(this, this.SelectedCell).Union(GetCellsInColumn(this, this.SelectedCell))))
                Cell.BackColor = Color.LightCyan;
            foreach (var Cell in GetNotesCellsSubset(GetNotesCells(this), this.SelectedCell))
                Cell.BackColor = Color.LightSkyBlue;
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
            foreach(var Cell in GetNotesCellsSubset(GetNotesCells(this), this.SelectedCell))
                Cell.Text = string.Empty;
        }
    }
}
