using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello.UI
{
    /// <summary>
    /// Logique d'interaction pour GameControlsUI.xaml
    /// </summary>
    public partial class GameControlsUI : UserControl
    {
        private event EventHandler eventEndTurnClicked;
        private event EventHandler eventUndoClicked;

        public GameControlsUI()
        {
            InitializeComponent();
        }

        public EventHandler EventEndTurnClicked
        {
            get { return this.eventEndTurnClicked; }
            set { this.eventEndTurnClicked = value; }
        }

        public EventHandler EventUndoClicked
        {
            get { return this.eventUndoClicked; }
            set { this.eventUndoClicked = value; }
        }

        private void OnEndOfTurnClicked(object sender, RoutedEventArgs e)
        {
            EventHandler handler = eventEndTurnClicked;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnUndoClicked(object sender, RoutedEventArgs e)
        {
            EventHandler handler = eventUndoClicked;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
