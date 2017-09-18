using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MoneyEntry.DataAccess
{
  public sealed class SQLTalker2 : IDisposable
  {
    private string Server { get; set; }
    private string Database { get; set; }
    private string Restoreloc;

    public string Backup { get; set; }
    public string Initialbackup { get; set; }

    // Only Cnx string to database will be available to see to the caller of the class.
    public string Cnx { get; set; }

    // default values of the library is set to local database that has a test database.
    public SQLTalker2(string server = "(local)", string database = "Expenses")
    {
      Server = server;
      Database = database;
      Cnx = "Integrated Security=SSPI;Persist Security Info=False;Data Source =" + server + ";Initial Catalog=" + database;

      DateTime dt = DateTime.Now;

      Initialbackup = Database + "Start_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";
      Backup = Database + "_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";
    }

    public KeyValuePair<bool, string> BackupDB(string aLoc, bool aStartup)
    {
      try
      {
        return new KeyValuePair<bool, string>(true, aStartup
            ? Procer("backup Database " + Database + "\nto disk = '" + aLoc + "\\" + Initialbackup + "'\nwith format", false)
            : Procer("backup Database " + Database + "\nto disk = '" + aLoc + "\\" + Backup + "'\nwith format", false));
      }
      catch (Exception ex) { return new KeyValuePair<bool, string>(false, "Could not backup"); }
    }

    public KeyValuePair<bool, string> KillConnectionsToDatabase()
    {
      try
      {
        SQLTalker2 master = new SQLTalker2("(local)", "master");

        DataTable temp = master.GetData("declare @Temp table\n(spid\tint\n,\tecid\tint\n,\tstatus\tvarchar(128)\n,\tloginame\tvarchar(128)\n,\thostname\tvarchar(128)" +
           "\n,\tblk\tint\n,\tdbname\tvarchar(128)\n,\tcmd\tvarchar(128)\n,\trequest_id\tint)\n\ninsert into @Temp\nexec sp_who\n\n" +
           "select spid\nfrom @Temp\nwhere dbname like '%Expenses%'");

        foreach (DataRow data in temp.Rows)
        {
          master.Procer("kill " + data[0].ToString(), false);
        }

        return new KeyValuePair<bool, string>(true, "Killed existing connection to Expenses");
      }
      catch (Exception ex) { return new KeyValuePair<bool, string>(false, "Cancelled or problem"); }
    }

    public KeyValuePair<bool, string> RestoreDB(string aLoc)
    {
      try
      {
        using (var master = new SQLTalker2("(local)", "master"))
        {
          return new KeyValuePair<bool, string>(true, master.Procer("restore database " + Database + "\nfrom disk = '" + aLoc + "'\nwith replace", false));
        }
      }
      catch (Exception ex) { return new KeyValuePair<bool, string>(false, "Could not restore database");  }
    }

    #region Related to populating data sets

    public string Procer(string sql, bool counts)
    {
      using (SqlConnection cn = new SqlConnection(Cnx))
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

    public void Dispose()
    {
      throw new NotImplementedException();
    }
    #endregion

  }
}
