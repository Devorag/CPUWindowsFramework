using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUWindowsFormFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable targetdt, string tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public static void SetControlBinding(Control ctrl, DataTable dt)
        {
            string propertyName = "";
            string controlName = ctrl.Name.ToLower();
            string controltype = controlName.Substring(0, 3);
            string columnName = controlName.Substring(3);
            switch (controltype)
            {
                case "txt":
                case "lbl":
                    propertyName = "Text";
                    break;
                case "dtp":
                    propertyName = "Value";
                    break;
            }

            if (propertyName != "" && columnName != "")
            {
                ctrl.DataBindings.Add(propertyName, dt, columnName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void FormatGridForSearchResultst(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

    }
}
