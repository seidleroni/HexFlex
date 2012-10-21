using System;
using System.Windows.Forms;

namespace IntelHexViewer2
{
    public class HexGrid : DataGridView
    {
        /*
        private System.Collections.ArrayList ByteValueArray = new System.Collections.ArrayList();
                private System.Collections.ArrayList CompareArray = new System.Collections.ArrayList();*/
        
        private System.Collections.ArrayList ByteValueArray;
        private System.Collections.ArrayList CompareArray;

        private int start_address = 0;
        private int end_address = 0;

        private int num_columns = GRID_NUM_COLUMNS;
        private int next_byte_value_array_address = 0;
        private int next_compare_array_address = 0;

        private const int BYTE_VALUE_EMPTY_INT = -1;
        private const string BYTE_VALUE_EMPTY_STRING = "";

        private const int GRID_NUM_COLUMNS = 16;
        private const int COLUMN_WIDTH = 30;

        public HexGrid()
        {
            DoubleBuffered = true;

            ByteValueArray = new System.Collections.ArrayList();
            CompareArray = new System.Collections.ArrayList();

            this.VirtualMode = true;
            this.AutoSize = false;
            this.ColumnCount = num_columns;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.RowHeadersWidth = 80;

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;

            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.AllowUserToResizeRows = false;
            this.AllowUserToResizeColumns = false;
            this.ReadOnly = true;

            this.CellValueNeeded += new
                DataGridViewCellValueEventHandler(Event_CellValueNeeded);

            this.CellFormatting += new
                DataGridViewCellFormattingEventHandler(Event_CellFormatting);

            for (int i = 0; i < this.ColumnCount; i++)
            {
                this.Columns[i].Width = COLUMN_WIDTH;
                this.Columns[i].HeaderCell.Value = i.ToString("X2");
            }
        }

        void Event_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int address;

            address = this.CellToAddress(e.RowIndex, e.ColumnIndex);

            if (this.IsCompareEqual(address) == false)
                e.CellStyle.BackColor = System.Drawing.Color.Red;
        }

        private void Event_CellValueNeeded(object sender, System.Windows.Forms.DataGridViewCellValueEventArgs e)
        {
            string byte_string;
            int address;
            int row_start;
            string row_start_string;

            row_start = CellToRowStart(e.RowIndex);
            row_start_string = row_start.ToString("X6");

            if (
                (this.Rows[e.RowIndex].HeaderCell.Value == null) ||
                (this.Rows[e.RowIndex].HeaderCell.Value.ToString() != row_start_string)
                )
                this.Rows[e.RowIndex].HeaderCell.Value = row_start_string;

            address = this.CellToAddress(e.RowIndex, e.ColumnIndex);
            byte_string = this.GetByteString(address);

            e.Value = byte_string;
        }

        public void UpdateNumRows()
        {
            int num_rows;

            num_rows = (end_address - start_address + 1) / num_columns;
            this.RowCount = num_rows;
        }

        public void Clear()
        {
            ByteValueArray = new System.Collections.ArrayList();
            CompareArray = new System.Collections.ArrayList();
            next_byte_value_array_address = 0;
            next_compare_array_address = 0;
        }

        public void AddByte(int address, int byte_value)
        {
            if (next_byte_value_array_address == address)
            {
                this.ByteValueArray.Add(byte_value);
                AddCompareResult(address, true);
                next_byte_value_array_address++;
            }
            else if (address < next_byte_value_array_address)
            {
                /* replace old value with new value */
                this.ByteValueArray[address] = byte_value;
                AddCompareResult(address, true);
            }
            else
            {
                for (int i = next_byte_value_array_address; i <= address; i++)
                {
                    if (i == address)
                        this.ByteValueArray.Add(byte_value);
                    else
                        this.ByteValueArray.Add(BYTE_VALUE_EMPTY_INT);
                    AddCompareResult(address, true);
                }
                next_byte_value_array_address = ++address;
            }

            if (address > end_address)
                end_address = address;
        }

        public bool IsCompareEqual(int address)
        {
            return (bool)this.CompareArray[address];
        }

        public void AddCompareResult(int address, bool is_equal)
        {
            if (next_compare_array_address == address)
            {
                this.CompareArray.Add(is_equal);
                next_compare_array_address++;
            }
            else if (address < next_compare_array_address)
            {
                /* replace old compare value with new value */
                this.CompareArray[address] = is_equal;
            }
            else
            {
                for (int i = next_compare_array_address; i <= address; i++)
                {
                    /* mark all of them as equal if we have skipped them */
                    this.CompareArray.Add(true);
                }
                next_compare_array_address = address++;
            }
        }

        public int CellToAddress(int row, int column)
        {
            int address;

            address = (row * num_columns) + column + start_address;
            return address;
        }

        public void AddressToCell(int address, out int row, out int column)
        {
            row = (address - start_address) / num_columns;
            column = (address - start_address) % num_columns;
        }

        private int CellToRowStart(int row)
        {
            int row_address_start;

            row_address_start = CellToAddress(row, 0);
            return row_address_start;
        }

        public int GetByteInt(int address)
        {
            int byte_value;

            if (address > this.ByteValueArray.Count)
                byte_value = BYTE_VALUE_EMPTY_INT;
            else
                byte_value = (int)this.ByteValueArray[address];

            return byte_value;
        }

        public string GetByteString(int address)
        {
            string hex_string;

            if (address > this.ByteValueArray.Count)
                hex_string = BYTE_VALUE_EMPTY_STRING;
            else if ((int)this.ByteValueArray[address] == BYTE_VALUE_EMPTY_INT)
                hex_string = BYTE_VALUE_EMPTY_STRING;
            else
                hex_string = ((int)this.ByteValueArray[address]).ToString("X2");

            return hex_string;
        }

        public int StartAddress()
        {
            return start_address;
        }

        public void SetStartAddress(int new_start_address)
        {
            start_address = new_start_address;
        }

        public int EndAddress()
        {
            return end_address;
        }
    }
}