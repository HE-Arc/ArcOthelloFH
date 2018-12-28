using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Orthello.UI
{
    struct GridPos
    {
        private int row;
        private int column;

        public GridPos(int row = -1, int column = -1)
        {
            this.row = row;
            this.column = column;
        }

        public int Row
        {
            get { return this.row; }
        }

        public int Column
        {
            get { return this.column; }
        }
    }

    class SlotGrid : Button
    {
        private static readonly BitmapImage BLACK_PAWN = null;
        private static readonly BitmapImage WHITE_PAWN = null;

        static SlotGrid()
        {
            Uri blackPawnUri = new Uri("pack://application:,,,/Othello;component/Resources/Black_Pawn.png", UriKind.Absolute);
            Uri whitePawnUri = new Uri("pack://application:,,,/Othello;component/Resources/White_Pawn.png", UriKind.Absolute);

            BLACK_PAWN = new BitmapImage(blackPawnUri);
            WHITE_PAWN = new BitmapImage(whitePawnUri);
        }

        /// <summary>
        /// Constructor: row and column of the slot is necessary to define the position
        /// </summary>
        /// <param name="row">Row of the slot in the grid</param>
        /// <param name="column">Column of the slot in the grid</param>
        public SlotGrid(int row, int column): base()
        {
            this.Style = this.FindResource("slotStyle") as Style;
            this.AttributeName(row, column);
        }

        /// <summary>
        /// Give a specific name to the component
        /// </summary>
        /// <param name="row">Row of the slot</param>
        /// <param name="column">Column of the slot</param>
        private void AttributeName(int row, int column)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("btn_");
            builder.Append(row);
            builder.Append("_");
            builder.Append(column);

            this.Name = builder.ToString();
        }

        /// <summary>
        /// Get the position of the slot (parsing the name which contains the info)
        /// </summary>
        /// <returns>Position in the grid of the current component</returns>
        public GridPos GetPosition()
        {
            String[] splitted = this.Name.Split('_');
            int row = int.Parse(splitted[1]);
            int column = int.Parse(splitted[2]);
            return new GridPos(row, column);
        }

        // Todo: Modify the parameter with enum
        public void SetContent(int contentType)
        {
            if (contentType == 0)
            {
                Image img = new Image();
                img.Source = WHITE_PAWN;
                this.Content = img;
            }
            if (contentType == 1)
            {
                Image img = new Image();
                img.Source = BLACK_PAWN;
                this.Content = img;
            }
        }
    }
}
