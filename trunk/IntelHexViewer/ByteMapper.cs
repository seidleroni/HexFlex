using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IntelHexViewer2
{
    public class ByteMapper
    {
        private uint byte_offset;
        private uint byte_address;
        private uint lowest_byte_address;
        private uint highest_byte_address;
        HexGrid hex_grid;

        public ByteMapper(HexGrid destination_hex_grid)
        {
            byte_offset = 0;
            byte_address = 0;
            lowest_byte_address = 99999;
            highest_byte_address = 0;
            hex_grid = destination_hex_grid;
        }

        public void NewExtendedAddr(uint new_upper_16)
        {
            byte_address = new_upper_16 << 16;
        }

        public void SetExtendedSegmentAddress(uint new_address_offset)
        {
            byte_address = 0;
            byte_offset = new_address_offset;
        }

        public void NewLowerAddr(uint new_lower_16)
        {
            /* get rid of lower 16 bits */
            byte_address &= 0xFFFF0000;

            /* stuff lower 16 bits */
            byte_address |= new_lower_16;
        }

        public void StuffByte(byte new_byte)
        {
            uint actual_byte_address;

            actual_byte_address = byte_address + byte_offset;

            if (actual_byte_address > highest_byte_address)
                highest_byte_address = actual_byte_address;

            if (actual_byte_address < lowest_byte_address)
            {
                lowest_byte_address = actual_byte_address;
                hex_grid.SetStartAddress((int)lowest_byte_address);
            }

            hex_grid.AddByte((int)actual_byte_address, (int)new_byte);
            byte_address++;
        }

        public void EndOfMap(HexGrid hex_grid)
        {
/*
                    int address_range, num_rows;
                    int num_columns = 16;
                    int i;
                    ByteEntry be;
        
                    address_range = (int)highest_byte_address - (int)lowest_byte_address + 1;
        
                    num_rows = (address_range / num_columns);
                    if ((address_range % num_columns) > 0)
                        num_rows++;
        
        
                    hex_grid.DataGridInit(num_rows, num_columns, (int)lowest_byte_address);
        
                    for (i = 0; i < ItemList.Count; i++)
                    {
                        be = (ByteEntry)ItemList[i];
                        hex_grid.DataGridSetAddressValue((int)lowest_byte_address, (int)be.address, be.byte_val, num_columns);
                    }
*/      }
        
    }
}
