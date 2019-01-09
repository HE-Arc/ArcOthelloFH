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

        private EventHandler eventSlotClicked;

        public OthelloGrid(IntPosition size) : base()
        {
            this.slotsArray = new Slot[size.Row, size.Column];

            this.PrepareGeometry(size);
        }

        public void PrepareGeometry(IntPosition size)
        {
            StringBuilder builder = new StringBuilder();

            for (int row = 0; row < size.Row; row++)
            {
                RowDefinition rowDef = new RowDefinition();
                this.RowDefinitions.Add(rowDef);
            }

            for (int column = 0; column < size.Column; column++)
            {
                ColumnDefinition columnDef = new ColumnDefinition();
                this.ColumnDefinitions.Add(columnDef);
            }

            for (int row = 0;row < size.Row; row++)
            {
                for (int column = 0; column < size.Column; column++)
                {
                    Slot slot = new Slot(row, column);
                    slot.Click += OnClickEvent;

                    this.Children.Add(slot);
                    slotsArray[row, column] = slot;

                    Grid.SetRow(slot, row);
                    Grid.SetColumn(slot, column);
                }
            }
        }

        public void OnClickEvent(Object sender, RoutedEventArgs args)
        {
            EventHandler handler = eventSlotClicked;

            if(handler != null)
            {
                handler(this, args);
            }
        }

        public EventHandler EventSlotClicked
        {
            get { return this.eventSlotClicked; }
            set { this.eventSlotClicked = value; }
        }

        public Slot[,] SlotsArray
        {
            get { return SlotsArray; }
        }
    }
}
