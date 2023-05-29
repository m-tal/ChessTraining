using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChessTraining
{
    internal class KnightWalk : BaseClassForTasks
    {
        public override string FormName { get; set; }

        public override Bitmap FigureCursor { get; set; }

        private readonly ComboBox moveCountComboBox;

        private readonly List<Button> fieldPressedButtonsBuffer = new List<Button>();

        private enum CorrectMoveCoefficients
        {
            Eight = 8,
            Twelve = 12,
            Nineteen = 19,
            TwentyOne = 21
        }

        private Color lastButtonForeColor = new Color();
        private Color lastButtonBackColor = new Color();

        private readonly int fieldsCount = 64;

        private struct WrongMove
        {
            public bool isWrongMove;

            public int wrongMoveIndex;
        }

        WrongMove wrongMove;

        public KnightWalk()
        {
            FormName = ResourceStrings.KnightWalk;
            FigureCursor = new Bitmap(Properties.Resources.knightAlfa, 40, 40);

            for (int i = 0; i < controlButtons.Count; i++)
            {
                controlButtons[i].Visible = true;
            }

            moveCountComboBox = GameFieldForm.GetComboBox();
            moveCountComboBox.Visible = true;

            gameFieldForm.Text = FormName;
            gameFieldForm.Icon = Properties.Resources.darkKnight_ico;

            SetCursorOnField(FigureCursor);
        }

        private void PressLastButton(Button lastButton)
        {
            if (wrongMove.isWrongMove)
            {
                MessageBox.Show(ResourceStrings.WrongMove);

                DoBackMove();
            }
            else
            {
                lastButtonBackColor = lastButton.BackColor;

                lastButton.Text = ResourceStrings.Empty;
                lastButton.BackColor = Color.LightGreen;
                lastButton.BackgroundImage = Properties.Resources.knightAlfa;

                lastButton.Enabled = false;

                for (int i = 1; i < controlButtons.Count; i++)
                {
                    controlButtons[i].Enabled = false;
                }

                moveCountComboBox.Enabled = false;

                isTaskComplete = true;

                MessageBox.Show(ResourceStrings.SuccessfullyCompleted);

                SetCursorOnField(fingerUpCursor);
            }
        }

        private void SetCurrentButtonForeColor(List<Button> buttons, Button currentButton)
        {
            if (buttons.Count > 0)
            {
                buttons[buttons.Count - 1].ForeColor = lastButtonForeColor;
            }

            lastButtonForeColor = currentButton.ForeColor;
            currentButton.ForeColor = Color.DeepSkyBlue;
        }

        public void CheckMoveCorrectness(Button currentButton)
        {
            if (wrongMove.isWrongMove && fieldPressedButtons.Count < wrongMove.wrongMoveIndex)
            {
                wrongMove.isWrongMove = false;
            }

            if (fieldPressedButtons.Count > 0 &&
                !Enum.IsDefined(typeof(CorrectMoveCoefficients),
                Math.Abs(Convert.ToInt32(fieldPressedButtons[fieldPressedButtons.Count - 1].Tag) - Convert.ToInt32(currentButton.Tag))))
            {
                if (!wrongMove.isWrongMove)
                {
                    wrongMove.isWrongMove = true;
                    wrongMove.wrongMoveIndex = moveCounter;
                }
            }
        }

        public void DoBackMove()
        {
            if (moveCounter > 1)
            {
                if (fieldPressedButtons.Count > 1)
                {
                    SetCurrentButtonForeColor(fieldPressedButtons, fieldPressedButtons[fieldPressedButtons.Count - 2]);

                    fieldPressedButtons[fieldPressedButtons.Count - 1].Text = ResourceStrings.Empty;
                    fieldPressedButtons.RemoveAt(fieldPressedButtons.Count - 1);

                    moveCountComboBox.Items.RemoveAt(moveCountComboBox.Items.Count - 1);

                    moveCounter--;
                }
            }
        }

        public void DoForwardMove()
        {
            if (moveCounter <= fieldPressedButtonsBuffer.Count && moveCounter < fieldsCount)
            {
                fieldPressedButtonsBuffer[moveCounter - 1].Text = moveCounter.ToString();

                SetCurrentButtonForeColor(fieldPressedButtons, fieldPressedButtonsBuffer[moveCounter - 1]);

                fieldPressedButtons.Add(fieldPressedButtonsBuffer[moveCounter - 1]);

                moveCountComboBox.Items.Add(fieldPressedButtons[fieldPressedButtons.Count - 1].Text);

                moveCounter++;
            }
        }

        public void SetSelectedMove()
        {
            int index = Convert.ToInt32(moveCountComboBox.Text);

            moveCountComboBox.Items.Clear();

            for (int i = 1; i <= index; i++)
            {
                moveCountComboBox.Items.Add(i.ToString());
            }

            for (int i = index; i < fieldPressedButtonsBuffer.Count; i++)
            {
                fieldPressedButtonsBuffer[i].Text = ResourceStrings.Empty;
            }

            SetCurrentButtonForeColor(fieldPressedButtons, fieldPressedButtonsBuffer[index - 1]);

            fieldPressedButtons.RemoveRange(index, fieldPressedButtons.Count - index);

            fieldPressedButtonsBuffer.Clear();
            fieldPressedButtonsBuffer.AddRange(fieldPressedButtons);

            moveCounter = index + 1;
        }

        public override void PressOnButton(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;

            if (string.IsNullOrEmpty(pressedButton.Text))
            {
                pressedButton.Text = moveCounter.ToString();

                CheckMoveCorrectness(pressedButton);

                SetCurrentButtonForeColor(fieldPressedButtons, pressedButton);

                fieldPressedButtons.Add(pressedButton);
                fieldPressedButtonsBuffer.Clear();
                fieldPressedButtonsBuffer.AddRange(fieldPressedButtons);

                moveCounter++;

                moveCountComboBox.Items.Add(fieldPressedButtonsBuffer[fieldPressedButtonsBuffer.Count - 1].Text);

                if (moveCounter > fieldsCount)
                {
                    PressLastButton(pressedButton);
                }
            }
        }

        public override void StartOver()
        {
            if (fieldPressedButtons.Count != 0)
            {
                fieldPressedButtons[fieldPressedButtons.Count - 1].ForeColor = lastButtonForeColor;
            }

            if (isTaskComplete)
            {
                fieldPressedButtons[fieldPressedButtons.Count - 1].BackgroundImage = null;
                fieldPressedButtons[fieldPressedButtons.Count - 1].BackColor = lastButtonBackColor;

                fieldPressedButtons[fieldPressedButtons.Count - 1].Enabled = true;

                for (int i = 1; i < controlButtons.Count; i++)
                {
                    controlButtons[i].Enabled = true;
                }

                moveCountComboBox.Enabled = true;

                SetCursorOnField(FigureCursor);

                isTaskComplete = false;
            }

            moveCountComboBox.Items.Clear();

            fieldPressedButtons.ForEach(button => button.Text = ResourceStrings.Empty);
            fieldPressedButtons.Clear();
            fieldPressedButtonsBuffer.Clear();

            moveCounter = 1;
        }
    }
}
