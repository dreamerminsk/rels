using System;
using System.Collections;
using System.Windows.Forms;

namespace rels.UI
{
    public class ListViewItemComparer : IComparer
    {

        private int column;
        private bool numeric = false;

        public int Column
        {

            get { return column; }
            set { column = value; }
        }

        public bool Numeric
        {

            get { return numeric; }
            set { numeric = value; }
        }

        public ListViewItemComparer(int columnIndex)
        {

            Column = columnIndex;
        }

        public int Compare(object x, object y)
        {

            ListViewItem listX = (ListViewItem)x;
            ListViewItem listY = (ListViewItem)y;

            if (Numeric)
            {

                // Convert column text to numbers before comparing.
                // If the conversion fails, just use the value 0.
                decimal listXVal, listYVal;
                try
                {
                    listXVal = Decimal.Parse(listX.SubItems[Column].Text);
                }
                catch
                {
                    listXVal = 0;
                }

                try
                {
                    listYVal = Decimal.Parse(listY.SubItems[Column].Text);
                }
                catch
                {
                    listYVal = 0;
                }

                return -Decimal.Compare(listXVal, listYVal);
            }
            else
            {
                string listXText, listYText;
                // Keep the column text in its native string format
                // and perform an alphabetic comparison.
                try
                {
                    listXText = listX.SubItems[Column].Text;
                }
                catch (Exception e)
                {
                    listXText = "";
                }
                try
                {
                    listYText = listY.SubItems[Column].Text;
                }
                catch (Exception e)
                {
                    listYText = "";
                }

                return String.Compare(listXText, listYText);
            }
        }
    }
}
