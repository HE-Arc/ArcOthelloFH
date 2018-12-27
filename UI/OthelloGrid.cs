using Orthello.UI;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Othello.UI
{
    class OthelloGrid: Grid
    {
        private int rows;
        private int columns;

        public OthelloGrid(int rows, int columns): base()
        {
            this.rows = rows;
            this.columns = columns;

            this.PrepareGeometry();
        }

        public void PrepareGeometry()
        {
            StringBuilder builder = new StringBuilder();

            for (int row = 0; row < rows; row++)
            {
                RowDefinition rowDef = new RowDefinition();
                this.RowDefinitions.Add(rowDef);
            }

            for (int column = 0; column < columns; column++)
            {
                ColumnDefinition columnDef = new ColumnDefinition();
                this.ColumnDefinitions.Add(columnDef);
            }

            for(int row = 0;row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    SlotGrid slot = new SlotGrid(row, column);
                    slot.Click += OnClickEvent;

                    this.Children.Add(slot);
                    Grid.SetRow(slot, row);
                    Grid.SetColumn(slot, column);
                }
            }
        }

        public void OnClickEvent(Object sender, RoutedEventArgs args)
        {
            SlotGrid slot = sender as SlotGrid;

            // Todo: use an enum instead integer
            slot.SetContent(0);
            GridPos position = slot.GetPosition();
            Console.WriteLine("Position: " + position.Row + ":" + position.Column);
        }
    }
}
