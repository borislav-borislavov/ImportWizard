using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.WinForms.Extensions
{
    public static class GridExtensions
    {
        public static DataGridViewTextBoxColumn AddTextBoxCol(this DataGridView grid, string property, string headerText = "", int width = -1)
        {
            if (string.IsNullOrEmpty(headerText))
                headerText = property;

            var col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = property;
            col.Name = property;
            col.HeaderText = headerText;

            if (width > -1) col.Width = width;

            grid.Columns.Add(col);

            return col;

        }
        public static DataGridViewCheckBoxColumn AddCheckBoxCol(this DataGridView grid, string property, string headerText = "", int width = -1)
        {
            if (string.IsNullOrEmpty(headerText))
                headerText = property;

            var col = new DataGridViewCheckBoxColumn();
            col.DataPropertyName = property;
            col.Name = property;
            col.HeaderText = headerText;

            if (width > -1) col.Width = width;

            grid.Columns.Add(col);

            return col;
        }

        public static DataGridViewComboBoxColumn AddComboCol(this DataGridView grid, string property, object dataSource, string displayMember, string valueMember, string headerText = "")
        {
            if (string.IsNullOrEmpty(headerText))
                headerText = property;

            var col = new DataGridViewComboBoxColumn();
            col.Name = property;
            col.HeaderText = headerText;
            col.DataPropertyName = property;
            col.DataSource = dataSource;
            col.DisplayMember = displayMember;
            col.ValueMember = valueMember;
            grid.Columns.Add(col);

            return col;
        }

        public static void EnableCopyPasteAndDelete(this DataGridView grid)
        {
            grid.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (grid.SelectedCells.Count == 1)
                    {
                        var cell = grid.SelectedCells[0];

                        if (cell.ValueType == typeof(string))
                        {
                            cell.Value = null;
                        }
                        else if (cell.ValueType == typeof(int?))
                        {
                            cell.Value = null;
                        }
                        else if (cell.ValueType == typeof(int))
                        {
                            cell.Value = 0;
                        }

                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                }
                else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                {
                    if (grid.SelectedCells.Count == 1)
                    {
                        var cell = grid.SelectedCells[0];

                        if (cell.ValueType == typeof(string))
                        {
                            cell.Value = Clipboard.GetText();
                        }
                        else if (cell.ValueType == typeof(int))
                        {
                            if (int.TryParse(Clipboard.GetText(), out var number))
                            {
                                cell.Value = number;
                            }
                        }

                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                }
            };
        }
    }
}
