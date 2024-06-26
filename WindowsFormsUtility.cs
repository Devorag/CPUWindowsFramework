using System.Data;

namespace CPUWindowsFormFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable targetdt, string tablename)
        {
            if (lst == null) throw new ArgumentNullException(nameof(lst), "ComboBox cannot be null.");
            if (sourcedt == null) throw new ArgumentNullException(nameof(sourcedt), "Source DataTable cannot be null.");
            if (targetdt == null) throw new ArgumentNullException(nameof(targetdt), "Target DataTable cannot be null.");
            if (string.IsNullOrWhiteSpace(tablename)) throw new ArgumentException("Table name cannot be null or whitespace.", nameof(tablename));

            string valueMember = tablename + "Id";
            string displayMember = lst.Name.Length > 3 ? lst.Name.Substring(3) : string.Empty;

            if (string.IsNullOrWhiteSpace(displayMember))
                throw new ArgumentException("Control name format is invalid. It should be at least 4 characters long.", nameof(lst.Name));

            // Print available columns for debugging
            string availableColumns = string.Join(", ", sourcedt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Console.WriteLine($"Available columns in source DataTable: {availableColumns}");

            if (!sourcedt.Columns.Contains(valueMember))
                throw new ArgumentException($"Source DataTable does not contain a column named '{valueMember}'. Available columns: {availableColumns}");

            lst.DataSource = sourcedt;
            lst.ValueMember = valueMember;
            lst.DisplayMember = displayMember;

            lst.DataBindings.Add("SelectedValue", targetdt, valueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public static void SetControlBinding(Control ctrl, DataTable dt)
        {
            if (ctrl == null) throw new ArgumentNullException(nameof(ctrl), "Control cannot be null.");
            if (dt == null) throw new ArgumentNullException(nameof(dt), "DataTable cannot be null.");
            if (ctrl.Name.Length <= 3) throw new ArgumentException("Control name format is invalid. It should be at least 4 characters long.", nameof(ctrl.Name));

            string controlName = ctrl.Name.ToLower();
            string controlType = controlName.Substring(0, 3);
            string columnName = controlName.Substring(3);

            string propertyName = controlType switch
            {
                "txt" => "Text",
                "lbl" => "Text",
                "dtp" => "Value",
                _ => throw new NotSupportedException($"Control type '{controlType}' is not supported.")
            };

            // Print available columns for debugging
            string availableColumns = string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Console.WriteLine($"Available columns in DataTable: {availableColumns}");

            if (!dt.Columns.Contains(columnName))
                throw new ArgumentException($"DataTable does not contain a column named '{columnName}'. Available columns: {availableColumns}");

            ctrl.DataBindings.Add(propertyName, dt, columnName, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public static void FormatGridForSearchResults(DataGridView grid)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid), "DataGridView cannot be null.");

            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
