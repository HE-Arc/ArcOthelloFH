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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.GetType().Name);
            builder.Append(":");
            builder.Append(row);
            builder.Append(":");
            builder.Append(column);
            return builder.ToString();
        }

        public static IntPosition operator+(IntPosition intA, IntPosition intB)
        {
            return new IntPosition(intA.Row + intB.Row, intA.Column + intB.Column);
        }

        public static IntPosition operator-(IntPosition intA, IntPosition intB)
        {
            return new IntPosition(intA.Row - intB.Row, intA.Column - intB.Column);
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
