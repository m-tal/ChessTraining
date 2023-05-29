using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChessTraining
{
    public partial class GameFieldForm : Form
    {
        private readonly int taskIndex;

        private readonly KnightWalk task1;
        private readonly EightQueens task2;

        private static Form mainForm;
        private static Panel mainPanel1;
        private static Panel mainPanel2;
        private static ComboBox mainComboBox;

        public GameFieldForm(int index)
        {
            InitializeComponent();

            mainForm = this;
            mainPanel1 = panel1;
            mainPanel2 = panel2;
            mainComboBox = moveCountComboBox;

            taskIndex = index;

            switch (taskIndex)
            {
                case 1:
                    task1 = new KnightWalk();
                    break;
                default:
                    task2 = new EightQueens();
                    break;
            }
        }

        public static Form GetForm()
        {
            return mainForm;
        }

        public static List<Button> GetFieldButtons()
        {
            List<Button> mainFieldButtons = new List<Button>();

            foreach (Button button in mainPanel2.Controls)
            {
                mainFieldButtons.Add(button);
            }

            return mainFieldButtons;
        }

        public static List<Button> GetControlButtons()
        {
            List<Button> mainControlButtons = new List<Button>();

            foreach (Button button in mainPanel1.Controls)
            {
                mainControlButtons.Add(button);
            }

            return mainControlButtons;
        }

        public static Panel GetPanel()
        {
            return mainPanel2;
        }

        public static ComboBox GetComboBox()
        {
            return mainComboBox;
        }

        private void PressOnButton(object sender, EventArgs e)
        {
            switch (taskIndex)
            {
                case 1:
                    task1.PressOnButton(sender, e);
                    break;
                default:
                    task2.PressOnButton(sender, e);
                    break;
            }
        }

        private void RetryButton_Click(object sender, EventArgs e)
        {
            switch (taskIndex)
            {
                case 1:
                    task1.StartOver();
                    break;
                default:
                    task2.StartOver();
                    break;
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            switch (taskIndex)
            {
                case 1:
                    task1.DoBackMove();
                    break;
            }
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            switch (taskIndex)
            {
                case 1:
                    task1.DoForwardMove();
                    break;
            }
        }

        private void MoveCountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (taskIndex)
            {
                case 1:
                    task1.SetSelectedMove();
                    break;
            }
        }

        private void TaskInfoButton_Click(object sender, EventArgs e)
        {
            string message;

            switch (taskIndex)
            {
                case 1:
                    message = ResourceStrings.KnightWalkTaskInfo;
                    break;
                default:
                    message = ResourceStrings.EightQueensTaskInfo;
                    break;
            }

            MessageBox.Show(message);
        }
    }
}
