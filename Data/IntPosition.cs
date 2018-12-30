using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Data
{
    struct IntPosition
    {
        private int row;
        private int column;

        public IntPosition(int row = -1, int column = -1)
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
}
