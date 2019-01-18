using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Data
{
    [Serializable]
    public struct IntPosition
    {
        private int row;
        private int column;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Value for row</param>
        /// <param name="column">Value for column</param>
        public IntPosition(int row = -1, int column = -1)
        {
            this.row = row;
            this.column = column;
        }

        /// <summary>
        /// Give a string representation of this object
        /// </summary>
        /// <returns>String representation of this object</returns>
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

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="intA"></param>
        /// <param name="intB"></param>
        /// <returns></returns>
        public static IntPosition operator+(IntPosition intA, IntPosition intB)
        {
            return new IntPosition(intA.Row + intB.Row, intA.Column + intB.Column);
        }

        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="intA"></param>
        /// <param name="intB"></param>
        /// <returns></returns>
        public static IntPosition operator-(IntPosition intA, IntPosition intB)
        {
            return new IntPosition(intA.Row - intB.Row, intA.Column - intB.Column);
        }

        /// <summary>
        /// Get number of rows
        /// </summary>
        public int Row
        {
            get { return this.row; }
        }

        /// <summary>
        /// Get number of columns
        /// </summary>
        public int Column
        {
            get { return this.column; }
        }
    }
}
