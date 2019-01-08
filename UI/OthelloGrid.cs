using Orthello.UI;
using Othello.Data;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Othello.UI
{
    class OthelloGrid: Grid
    {
        private Slot[,] slotsArray;
        private OthelloLogic logic;

        public OthelloGrid(OthelloLogic logic) : base()
        {
            this.logic = logic;
            this.slotsArray = new Slot[logic.Rows, logic.Columns];

            this.PrepareGeometry();
        }

        public void PrepareGeometry()
        {
            StringBuilder builder = new StringBuilder();

            for (int row = 0; row < logic.Rows; row++)
            {
                RowDefinition rowDef = new RowDefinition();
                this.RowDefinitions.Add(rowDef);
            }

            for (int column = 0; column < logic.Columns; column++)
            {
                ColumnDefinition columnDef = new ColumnDefinition();
                this.ColumnDefinitions.Add(columnDef);
            }

            for (int row = 0;row < logic.Rows; row++)
            {
                for (int column = 0; column < logic.Columns; column++)
                {
                    Slot slot = new Slot(row, column);
                    slot.Click += OnClickEvent;
                    slot.SetContent((SlotContent)logic.GameBoard[row, column]);

                    this.Children.Add(slot);
                    slotsArray[row, column] = slot;

                    Grid.SetRow(slot, row);
                    Grid.SetColumn(slot, column);
                }
            }
        }

        public void OnClickEvent(Object sender, RoutedEventArgs args)
        {
            Slot slot = sender as Slot;
            IntPosition position = slot.GetPosition();

            SlotContent slotContent = SlotContent.White;
            slot.SetContent(slotContent);
            logic.GameBoard[position.Row, position.Column] = (int)slotContent;
            Console.WriteLine("Position: " + position.Row + ":" + position.Column);
        }

        public void MarkSlot(IntPosition slotPosition)
        {
            this.slotsArray[slotPosition.Row, slotPosition.Column].Mark();
        }

        public void UnMarkSlot(IntPosition slotPosition)
        {
            this.slotsArray[slotPosition.Row, slotPosition.Column].Unmark();
        }
    }
}
