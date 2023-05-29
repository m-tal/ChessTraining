using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChessTraining
{
    internal class EightQueens: BaseClassForTasks
    {
        public override string FormName { get; set; }

        public override Bitmap FigureCursor { get; set; }

        private readonly int queenCount = 8;

        private readonly List<Button> fieldButtons;

        private List<Color> fieldColor;

        private enum FieldSearchCoefficients
        {
            One = 1,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Eleven = 11
        }

        private bool isFigureUnderAttack;

        public EightQueens()
        {
            FormName = ResourceStrings.EightQueens;
            FigureCursor = new Bitmap(Properties.Resources.queenAlfa2, 35, 35);

            controlButtons[0].Visible = true;

            gameFieldForm.Text = FormName;
            gameFieldForm.Icon = Properties.Resources.darkQueen_ico;

            fieldButtons = GameFieldForm.GetFieldButtons();

            SetCursorOnField(FigureCursor);
        }       

        private bool IsFiguresAttackIntersection()
        {
            foreach (Button button in fieldPressedButtons)
            {
                int tag = Convert.ToInt32(button.Tag);

                int tagCounter = 10 * (tag / 10) + (int)FieldSearchCoefficients.One;

                while (tagCounter % 10 <= (int)FieldSearchCoefficients.Eight)
                {
                    if (IsFieldUnderAttack(tag, tagCounter))
                    {
                        return true;
                    }

                    tagCounter += (int)FieldSearchCoefficients.One;
                }

                tagCounter = 10 + tag % 10;

                while (tagCounter / 10 <= (int)FieldSearchCoefficients.Eight)
                {
                    if (IsFieldUnderAttack(tag, tagCounter))
                    {
                        return true;
                    }

                    tagCounter += (int)FieldSearchCoefficients.Ten;
                }

                tagCounter = tag;

                while (tagCounter % 10 < (int)FieldSearchCoefficients.Eight && tagCounter / 10 < (int)FieldSearchCoefficients.Eight)
                {
                    tagCounter += (int)FieldSearchCoefficients.Eleven;
                }

                while (tagCounter % 10 >= (int)FieldSearchCoefficients.One && tagCounter / 10 >= (int)FieldSearchCoefficients.One)
                {
                    if (IsFieldUnderAttack(tag, tagCounter))
                    {
                        return true;
                    }

                    tagCounter -= (int)FieldSearchCoefficients.Eleven;
                }

                tagCounter = tag;

                while (tagCounter % 10 > (int)FieldSearchCoefficients.One && tagCounter / 10 < (int)FieldSearchCoefficients.Eight)
                {
                    tagCounter += (int)FieldSearchCoefficients.Nine;
                }

                while (tagCounter % 10 <= (int)FieldSearchCoefficients.Eight && tagCounter / 10 >= (int)FieldSearchCoefficients.One)
                {
                    if (IsFieldUnderAttack(tag, tagCounter))
                    {
                        return true;
                    }

                    tagCounter -= (int)FieldSearchCoefficients.Nine;
                }
            }

            return false;
        }

        private bool IsFieldUnderAttack(int tag, int tagCounter)
        {
            Button field = new Button();

            field = fieldButtons.Find(item => Convert.ToInt32(item.Tag) == tagCounter);

            return field.BackgroundImage != null && Convert.ToInt32(field.Tag) != tag;
        }

        private void ColorizeQueenFields()
        {
            fieldColor = new List<Color>();

            for (int i = 0; i < fieldPressedButtons.Count; i++)
            {
                fieldColor.Add(fieldPressedButtons[i].BackColor);

                fieldPressedButtons[i].BackColor = Color.LightGreen;
            }
        }

        public override void PressOnButton(object sender, EventArgs e)
        {
            if (!isTaskComplete)
            {
                Button pressedButton = sender as Button;

                if (moveCounter <= queenCount || isFigureUnderAttack)
                {
                    if (pressedButton.BackgroundImage == null)
                    {
                        if (!isFigureUnderAttack)
                        {
                            fieldPressedButtons.Add(pressedButton);

                            pressedButton.BackgroundImage = Properties.Resources.queenAlfa4;

                            moveCounter++;
                        }
                    }
                    else
                    {
                        pressedButton.BackgroundImage = null;

                        for (int i = 0; i < fieldPressedButtons.Count; i++)
                        {
                            if (fieldPressedButtons[i].Tag.Equals(pressedButton.Tag))
                            {
                                fieldPressedButtons.RemoveAt(i);

                                break;
                            }
                        }

                        moveCounter--;

                        if (fieldPressedButtons.Count < queenCount)
                        {
                            isFigureUnderAttack = false;
                        }
                    }
                }

                if (moveCounter > queenCount)
                {
                    if (IsFiguresAttackIntersection())
                    {
                        isFigureUnderAttack = true;

                        MessageBox.Show(ResourceStrings.WaysIntersect);
                    }
                    else
                    {
                        isTaskComplete = true;

                        ColorizeQueenFields();

                        MessageBox.Show(ResourceStrings.SuccessfullyCompleted);

                        SetCursorOnField(fingerUpCursor);
                    }
                }
            }
        }

        public override void StartOver()
        {
            for (int i = 0; i < fieldPressedButtons.Count; i++)
            {
                if (fieldColor != null && fieldColor.Count > 0)
                {
                    fieldPressedButtons[i].BackColor = fieldColor[i];
                }

                fieldPressedButtons[i].BackgroundImage = null;
            }

            if (fieldColor != null)
            {
                fieldColor.Clear();
            }

            fieldPressedButtons.Clear();

            SetCursorOnField(FigureCursor);

            isFigureUnderAttack = false;
            isTaskComplete = false;

            moveCounter = 1;
        }
    }
}
