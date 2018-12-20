using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

            this.prepareGeometry();
        }

        public void prepareGeometry()
        {
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
                    Button button = new Button();

                    this.Children.Add(button);
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, column);
                }
            }
        }
    }
}
