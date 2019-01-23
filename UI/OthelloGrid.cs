/// ----------------------------------------------------------------------------------
/// Othello
/// 
/// Course : C#
/// Authors: Malik Fleury, Thibault Haldenwang
/// ----------------------------------------------------------------------------------

using Othello.UI;
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

        /// <summary>
        /// Overload constructor : can define the size of the grid
        /// </summary>
        /// <param name="size">Size of the grid</param>
        public OthelloGrid(IntPosition size) : base()
        {
            this.slotsArray = new Slot[size.Column, size.Row];

            this.PrepareGeometry(size);
        }

        /// <summary>
        /// Prepare the grid
        /// </summary>
        /// <param name="size">Size of the grid</param>
        public void PrepareGeometry(IntPosition size)
        {
            StringBuilder builder = new StringBuilder();

            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;

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
                    Slot slot = new Slot(column, row);
                    slot.MinHeight = 10;
                    slot.MinWidth = 10;
                    slot.Height = 40;
                    slot.Width = 40;
                    slot.Click += OnClickEvent;

                    this.Children.Add(slot);
                    slotsArray[column, row] = slot;

                    Grid.SetRow(slot, row);
                    Grid.SetColumn(slot, column);
                }
            }
        }

        /// <summary>
        /// Manage the click and throw the event to EventSlotClicked
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="args">Event</param>
        public void OnClickEvent(Object sender, RoutedEventArgs args)
        {
            EventHandler handler = eventSlotClicked;

            if(handler != null)
            {
                handler(sender, args);
            }
        }
        
        /// <summary>
        /// Getter and setter of the event "SlotClicked"
        /// </summary>
        public EventHandler EventSlotClicked
        {
            get { return this.eventSlotClicked; }
            set { this.eventSlotClicked = value; }
        }

        /// <summary>
        /// Getter of the array of slots
        /// </summary>
        public Slot[,] SlotsArray
        {
            get { return slotsArray; }
        }
    }
}
