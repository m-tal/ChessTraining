using System;
using System.Windows.Forms;

namespace ChessTraining
{
    public partial class TaskSelectionForm : Form
    {
        public TaskSelectionForm()
        {
            InitializeComponent();            
        }

        private void TaskSelectionForm_Load(object sender, EventArgs e)
        {
            knightWalk_button.Text = ResourceStrings.KnightWalk;
            eightQueens_button.Text = ResourceStrings.EightQueens;
        }

        private void KnightWalk_button_Click(object sender, EventArgs e)
        {
            GameFieldForm newForm = new GameFieldForm(knightWalk_button.TabIndex);
            newForm.ShowDialog();
        }

        private void EightQueens_button_Click(object sender, EventArgs e)
        {
            GameFieldForm newForm = new GameFieldForm(eightQueens_button.TabIndex);
            newForm.ShowDialog();
        }
    }
}
