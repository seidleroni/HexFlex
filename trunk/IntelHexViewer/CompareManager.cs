using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntelHexViewer2
{
    public class CompareManager
    {
        public void test()
        {

        }

        public void Compare(HexGrid hex_grid_1, HexGrid hex_grid_2)
        {
            int start_address;
            int end_address;
            int i;
            int address_1;
            int address_2;
            bool all_equal = true;

            if (hex_grid_1.StartAddress() < hex_grid_2.StartAddress())
                start_address = hex_grid_1.StartAddress();
            else
                start_address = hex_grid_2.StartAddress();

            if (hex_grid_1.EndAddress() > hex_grid_2.EndAddress())
                end_address = hex_grid_1.EndAddress();
            else
                end_address = hex_grid_2.EndAddress();

            for (i = start_address; i <= end_address; i++)
            {
                address_1 = hex_grid_1.GetByteInt(i);
                address_2 = hex_grid_2.GetByteInt(i);

                if (address_1 != address_2)
                {
                    hex_grid_1.AddCompareResult(i, false);
                    hex_grid_2.AddCompareResult(i, false);
                    all_equal = false;
                }
            }

            if (all_equal == true)
            {
                MessageBox.Show("Files are identical.", "Compare Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
