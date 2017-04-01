using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace MoneyEntry.DataAccess
{
    class SQLTalker
    {
        private string Server { get; set; }
        private string Database { get; set; }
        private string Restoreloc;
        
        public string Backup { get; set; }
        public string Initialbackup { get; set; }

        // Only Cnx string to database will be available to see to the caller of the class.
        public string Cnx { get; set; }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        
        // default values of the library is set to local database that has a test database.
        public SQLTalker(string server = "(local)", string database = "Expenses")
        {
            Server = server;
            Database = database;
            Cnx = "Integrated Security=SSPI;Persist Security Info=False;Data Source =" + server + ";Initial Catalog=" + database;

            DateTime dt = DateTime.Now;

            Initialbackup = Database + "Start_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";
            Backup = Database + "_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";
        }

        public void OpenLocation(string aLoc)
        {
            if (Directory.Exists(aLoc))
            {
                System.Diagnostics.Process.Start(aLoc);
            }
            else
            {
                MessageBox.Show("Directory does not exist");
            }
        }

        public void BackupDB(string aLoc, bool aStartup)
        {
            if (aStartup == true)
            {
                this.Procer(
                    "backup Database " + Database + "\nto disk = '" + aLoc + "\\" + Initialbackup + "'\nwith format",
                    false);
            }
            else
            {
                MessageBox.Show(
                this.Procer("backup Database " + Database + "\nto disk = '" + aLoc + "\\" + Backup + "'\nwith format", false)
                , "Back Response");
            }
        }

        public void RestoreDB(string aLoc)
        {
            SQLTalker master = new SQLTalker("(local)", "master");

            DataTable temp = master.GetData("declare @Temp table\n(spid\tint\n,\tecid\tint\n,\tstatus\tvarchar(128)\n,\tloginame\tvarchar(128)\n,\thostname\tvarchar(128)" +
               "\n,\tblk\tint\n,\tdbname\tvarchar(128)\n,\tcmd\tvarchar(128)\n,\trequest_id\tint)\n\ninsert into @Temp\nexec sp_who\n\n" +
               "select spid\nfrom @Temp\nwhere dbname like '%Expenses%'");
            try
            {
                switch (
                MessageBox.Show(
                    "Kill existing connection to Expenses and restore?\n\nWARNING IF YOU REVERT, THE APPLICATION\nNEEDS TO CLOSE BEFORE YOU MAY RESTART.",
                    "Restore", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                {
                    case MessageBoxResult.Yes:
                        {
                            this.openFileDialog.InitialDirectory = "C:\\SQLServer\\Backups\\";
                            this.openFileDialog.Filter = "Backup Files (.bak)|*.bak|All Files (*.*)|*.*";

                            // per online this should be WPF method : bool? result = dlg.ShowDialog();

                            bool? result = openFileDialog.ShowDialog();

                            if (result == true)
                            {
                                Restoreloc = openFileDialog.FileName;

                                 switch (MessageBox.Show("You wish to restore backup: " + Restoreloc + "? ", "Restore",  MessageBoxButton.OKCancel, MessageBoxImage.Question))
                                {
                                    case MessageBoxResult.OK:
                                        {
                                            foreach (DataRow data in temp.Rows)
                                            {
                                                master.Procer("kill " + data[0].ToString(), false);
                                            }

                                            MessageBox.Show(master.Procer("restore database " + Database + "\nfrom disk = '" + Restoreloc + "'\nwith replace", false) + "\n\nClosing Application."
                                                , "Restore");

                                            Application.Current.Shutdown();
                                        }
                                        break;
                                    case MessageBoxResult.Cancel: MessageBox.Show("Operation cancelled.", "Restore");
                                        break;
                                }
                            }

                        }
                        break;
                    case MessageBoxResult.No: MessageBox.Show("Operation cancelled.", "Restore");
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Process failed...", "Restore");
            }
        }             

        #region Related to populating data sets
        
        public string Procer(string sql, bool counts)
        {
            using (SqlConnection cn = new SqlConnection(Cnx))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.CommandTimeout = 60000;

                    StringBuilder sb = new StringBuilder();

                    cn.Open();

                    SqlParameter rtn = cmd.Parameters.Add("return", SqlDbType.Int);
                    rtn.Direction = ParameterDirection.ReturnValue;

                    // timeout after an hour.
                    cmd.CommandTimeout = 60000;

                    if (counts == true)
                    {
                        sb.Append("RowsAffected:\t" + cmd.ExecuteNonQuery().ToString());
                        sb.Append("\n");

                        if ((int)rtn.Value == 0)
                            sb.Append("Results:\tSuccess");
                        else
                            sb.Append("Result:\tFailure!");
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();

                        if ((int)rtn.Value == 0)
                            sb.Append("Results:\tSuccess");
                        else
                            sb.Append("Result:\tFailure!");
                    }

                    cn.Close();

                    return sb.ToString();
                }
            }
        }

        public DataTable GetData(string sqlquery)
        {
            using (SqlConnection cn = new SqlConnection(Cnx))
            {
                using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                {
                    cmd.CommandTimeout = 5000;  // Timeout after 5 minutes.

                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        using (DataTable table = new DataTable())
                        {
                            cn.Open();

                            adapter.SelectCommand = cmd;

                            table.Locale = System.Globalization.CultureInfo.InvariantCulture;

                            adapter.Fill(table);

                            cn.Close();

                            return table;

                        }

                    }
                }
            }
        }
        #endregion

    }
}
