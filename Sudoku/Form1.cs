using System.Windows.Forms.VisualStyles;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        RichTextBox selected_cell;
        Boolean Notes = false;

        public List<RichTextBox> GetRichTextBoxes(Form form)
        {
            List<RichTextBox> richTextBoxList = new List<RichTextBox>();
            foreach (Control a in form.Controls)
            {
                if (a is RichTextBox)
                {
                    richTextBoxList.Add(a as RichTextBox);
                }
            }
            return richTextBoxList;
        }

        public Form1()
        {
            InitializeComponent();
            Form1 f = this;
            var richTextBoxes = GetRichTextBoxes(f);
            foreach (var richTextBox in richTextBoxes)
            {
                richTextBox.TabStop = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button Button = (Button)sender;

            if (this.Notes == false)
            {
                if (Button.Text == this.selected_cell.Text)
                {
                    this.selected_cell.BringToFront();
                    this.selected_cell.Text = string.Empty;
                }
                else
                {
                    this.selected_cell.BringToFront();
                    this.selected_cell.Text = string.Empty;
                    this.selected_cell.Text = Button.Text;
                    this.selected_cell.SelectAll();
                    this.selected_cell.SelectionAlignment = HorizontalAlignment.Center;
                    this.selected_cell.DeselectAll();
                }
            }
            else
            {
                if (Button.Text == r1c12.Text)
                {

                }
                else
                {

                }
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            this.selected_cell = (RichTextBox)sender;
            var richTextBoxes = GetRichTextBoxes(this);
            foreach (var richTextBox in richTextBoxes)
            {
                richTextBox.BorderStyle = BorderStyle.None;
            }
            this.selected_cell.BorderStyle = BorderStyle.FixedSingle;

        }

        private void NotesButton_Click(object sender, EventArgs e)
        {
            this.Notes = !this.Notes;
        }

    }
}
