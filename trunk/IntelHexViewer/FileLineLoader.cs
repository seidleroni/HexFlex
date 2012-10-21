using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IntelHexViewer2
{
    class FileLineLoader
    {
        public static void LoadFile(string file_to_open, HexGrid dest_hex_grid)
        {
            string line_string;
            int line_count = 0;

            ByteMapper hex_byte_map = new ByteMapper(dest_hex_grid);
            HexLine hxp = new HexLine(hex_byte_map);
            StreamReader sr = new StreamReader(file_to_open);

            dest_hex_grid.Clear();

            /* read a line into the hex line parser */
            while (!sr.EndOfStream)
            {
                line_string = sr.ReadLine();
                hxp.ProcessLine(line_string);
                line_count++;
            }
            sr.Close();
            dest_hex_grid.UpdateNumRows();
        }
    }
}
