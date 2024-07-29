using System.Data;

namespace CPUWindowsFormFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable? targetdt, string tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            if(targetdt != null)
            {
                lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void SetControlBinding(Control ctrl, BindingSource bindingSource)
        {
            if (ctrl == null) throw new ArgumentNullException(nameof(ctrl), "Control cannot be null.");
            if (bindingSource == null) throw new ArgumentNullException(nameof(bindingSource), "DataTable cannot be null.");
            if (ctrl.Name.Length < 3) throw new ArgumentException("Control name format is invalid. It should be at least 4 characters long.", nameof(ctrl.Name));

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

            if (propertyName != "" && columnName != "")
            {
                ctrl.DataBindings.Add(propertyName, bindingSource, columnName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void FormatGridForSearchResults(DataGridView grid, string tablename)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid), "DataGridView cannot be null.");

            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoFormatGrid(grid, tablename);

            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9.75F, FontStyle.Bold);
            grid.RowHeadersVisible = false;

            grid.Dock = DockStyle.Fill;
        }

        public static void FormatGridForEdit(DataGridView grid, string tablename)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            DoFormatGrid(grid, tablename);
        }

        private static void DoFormatGrid(DataGridView grid, string tablename)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.RowHeadersWidth = 25;
            foreach(DataGridViewColumn col in grid.Columns)
            {
                if(col.Name.EndsWith("Id"))
                {
                    col.Visible = false;
                }
            }
            string pkname = tablename + "Id";
            if(grid.Columns.Contains(pkname))
            {
                grid.Columns[pkname].Visible = false;
            }
        }

        public static int GetIdFromGrid(DataGridView grid, int rowindex, string columnname)
        {
            int id = 0;
            if (rowindex < grid.Rows.Count && grid.Columns.Contains(columnname) && grid.Rows[rowindex].Cells[columnname].Value != DBNull.Value)
            {
                if (grid.Rows[rowindex].Cells[columnname].Value is int)
                {
                    id = (int)grid.Rows[rowindex].Cells[columnname].Value;
                }
            }
            return id;
        }

        public static int GetIdFromComboBox(ComboBox lst)
        {
            int value = 0;
            if (lst.SelectedValue != null && lst.SelectedValue is int)
            {
                value = (int)lst.SelectedValue;
            }
            return value;
        }

        public static void AddComboBoxToGrid(DataGridView grid, DataTable datasource, string tablename, string displaymember)
        {
            DataGridViewComboBoxColumn c = new();
            c.DataSource = datasource;
            c.DisplayMember = displaymember;
            c.ValueMember = tablename + "Id";
            c.DataPropertyName = c.ValueMember;
            c.HeaderText = tablename;
            grid.Columns.Insert(0, c);
        }

        public static void AddDeleteButtonToGrid(DataGridView grid, string deleteColName)
        {
            grid.Columns.Add(new DataGridViewButtonColumn() { Text = "X", HeaderText = "Delete", Name = deleteColName, UseColumnTextForButtonValue = true });
        }

        public static bool IsFormOpen(Type formType, int pkValue = 0)
        {
            bool exists = false;
            foreach (Form frm in Application.OpenForms)
            {
                int frmpkValue = 0;
                if (frm.Tag != null && frm.Tag is int)
                {
                    frmpkValue = (int)frm.Tag;
                }

                if (frm.GetType() == formType && frmpkValue == pkValue)
                {
                    frm.Activate();
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        public static void SetupNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (f.IsMdiContainer == false)
                {
                    ToolStripButton btn = new();
                    btn.Text = f.Text;
                    btn.Tag = f;
                    btn.Click += Btn_Click;
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }

        private static void Btn_Click(object? sender, EventArgs e)
        {
            if (sender != null && sender is ToolStripButton)
            {
                ToolStripButton btn = (ToolStripButton)sender;
                if (btn.Tag != null && btn.Tag is Form)
                {
                    ((Form)btn.Tag).Activate();
                }
            }
        }
    }
}
