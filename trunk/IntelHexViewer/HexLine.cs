using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelHexViewer2
{
    public class HexLine
    {
        int byte_count;
        uint address;
        int start_address;
        int record_type;

        ByteMapper destination_byte_map;

        public const int RECORD_TYPE_DATA_RECORD = 0;
        public const int RECORD_TYPE_END_OF_FILE = 1;
        public const int RECORD_TYPE_EXTENDED_SEGMENT_ADDR = 2;
        public const int RECORD_TYPE_START_SEGMENT_ADDR = 3;
        public const int RECORD_TYPE_EXTENDED_LINEAR_ADDR = 4;
        public const int RECORD_TYPE_START_LINEAR_ADDR = 5;

        public HexLine(ByteMapper hex_byte_mapper)
        {
            destination_byte_map = hex_byte_mapper;
        }

        public void ProcessLine(string hex_line_string)
        {
            string data;

            byte_count = int.Parse(hex_line_string.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            address = (uint)int.Parse(hex_line_string.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
            record_type = int.Parse(hex_line_string.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);

            data = hex_line_string.Substring(9, byte_count * 2);

            switch (record_type)
            {
                case RECORD_TYPE_DATA_RECORD:
                    HexLine_ParseDataRecord(address, data);
                    break;

                case RECORD_TYPE_END_OF_FILE:
                    /* do nothing */
                    break;

                case RECORD_TYPE_START_SEGMENT_ADDR:
                    /* do nothing */
                    break;

                case RECORD_TYPE_EXTENDED_SEGMENT_ADDR:
                    HexLine_ParseExtendedSegmentAddress(data);
                    break;

                case RECORD_TYPE_EXTENDED_LINEAR_ADDR:
                    HexLine_ParseExtendedLinearAddress(data);
                    break;

                case RECORD_TYPE_START_LINEAR_ADDR:
                    /* do nothing */
                    break;

                default:
                    break;

            }
        }

        private void HexLine_ParseDataRecord(uint new_lower_addr, string byte_data_string)
        {
            int data_length = byte_data_string.Length / 2;
            int i;
            string byte_str;
            byte data_byte;

            destination_byte_map.NewLowerAddr(new_lower_addr);

            for (i = 0; i < data_length; i++)
            {
                byte_str = byte_data_string.Substring(i * 2, 2);
                data_byte = (byte)int.Parse(byte_str, System.Globalization.NumberStyles.HexNumber);

                destination_byte_map.StuffByte(data_byte);
            }

        }

        private void HexLine_ParseExtendedSegmentAddress(string byte_segment_addr)
        {
            uint extended_address;

            extended_address = (uint)int.Parse(byte_segment_addr, System.Globalization.NumberStyles.HexNumber);
            extended_address *= 16;
            destination_byte_map.SetExtendedSegmentAddress(extended_address);
        }

        private void HexLine_ParseExtendedLinearAddress(string byte_ext_addr)
        {
            uint extended_address;
            extended_address = (uint)int.Parse(byte_ext_addr, System.Globalization.NumberStyles.HexNumber);

            destination_byte_map.NewExtendedAddr(extended_address);
        }
    }
}
