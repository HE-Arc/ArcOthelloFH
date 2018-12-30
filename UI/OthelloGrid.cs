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
        private OthelloLogic logic;

        public OthelloGrid(OthelloLogic logic) : base()
        {
            this.logic = logic;

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

            for(int row = 0;row < logic.Rows; row++)
            {
                for (int column = 0; column < logic.Columns; column++)
                {
                    SlotGrid slot = new SlotGrid(row, column);
                    slot.Click += OnClickEvent;
                    slot.SetContent((SlotContent)logic.GameBoard[row, column]);

                    this.Children.Add(slot);
                    Grid.SetRow(slot, row);
                    Grid.SetColumn(slot, column);
                }
            }
        }

        public void OnClickEvent(Object sender, RoutedEventArgs args)
        {
            SlotGrid slot = sender as SlotGrid;
            GridPos position = slot.GetPosition();

            SlotContent enumSlot = SlotContent.White;
            slot.SetContent(enumSlot);
            logic.GameBoard[position.Row, position.Column] = (int)enumSlot;
            Console.WriteLine("Position: " + position.Row + ":" + position.Column);
        }
    }
}
