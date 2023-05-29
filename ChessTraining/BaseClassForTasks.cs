using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChessTraining
{
    internal abstract class BaseClassForTasks
    {
        public abstract string FormName { get; set; }

        public abstract Bitmap FigureCursor { get; set; }

        public readonly Bitmap fingerUpCursor = new Bitmap(Properties.Resources.fingerUp, 70, 70);

        public readonly Form gameFieldForm;        

        public readonly List<Button> controlButtons;
        public readonly List<Button> fieldPressedButtons = new List<Button>();

        public readonly Panel playingFieldPanel;

        public int moveCounter = 1;

        public bool isTaskComplete;

        public BaseClassForTasks()
        {
            controlButtons = GameFieldForm.GetControlButtons();

            playingFieldPanel = GameFieldForm.GetPanel();

            gameFieldForm = GameFieldForm.GetForm();
        }

        public void SetCursorOnField(Bitmap cursorIcon)
        {
            playingFieldPanel.Cursor = new Cursor(cursorIcon.GetHicon());
        }

        public abstract void PressOnButton(object sender, EventArgs e);

        public abstract void StartOver();
    }
}
