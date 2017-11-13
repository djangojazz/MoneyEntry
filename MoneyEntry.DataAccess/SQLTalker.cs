using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MoneyEntry.DataAccess
{
  public sealed class SQLTalker : IDisposable
  {
    private string _server { get; set; }
    private string _database { get; set; }
    private string _backup { get; set; }
    private string _initialbackup { get; set; }
    private string _connection { get; set; }

    // default values of the library is set to local database that has a test database.
    public SQLTalker(string server = "(local)", string database = "Expenses")
    {
      DateTime dt = DateTime.Now;
      _server = server;
      _database = database;
      _connection = "Integrated Security=SSPI;Persist Security Info=False;Data Source =" + server + ";Initial Catalog=" + database;
      _initialbackup = _database + "Start_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";
      _backup = _database + "_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";
    }

    public KeyValuePair<bool, string> BackupDB(string aLoc, bool aStartup)
    {
      try
      {
        return new KeyValuePair<bool, string>(true, aStartup
            ? Procer("backup Database " + _database + "\nto disk = '" + aLoc + "\\" + _initialbackup + "'\nwith format", false)
            : Procer("backup Database " + _database + "\nto disk = '" + aLoc + "\\" + _backup + "'\nwith format", false));
      }
      catch (Exception) { return new KeyValuePair<bool, string>(false, "Could not backup"); }
    }

    public KeyValuePair<bool, string> KillConnectionsToDatabase()
    {
      try
      {
        using (var master = new SQLTalker("(local)", "master"))
        {
          DataTable temp = master.GetData("declare @Temp table\n(spid\tint\n,\tecid\tint\n,\tstatus\tvarchar(128)\n,\tloginame\tvarchar(128)\n,\thostname\tvarchar(128)" +
             "\n,\tblk\tint\n,\tdbname\tvarchar(128)\n,\tcmd\tvarchar(128)\n,\trequest_id\tint)\n\ninsert into @Temp\nexec sp_who\n\n" +
             "select spid\nfrom @Temp\nwhere dbname like '%Expenses%'");

          foreach (DataRow data in temp.Rows)
          {
            master.Procer("kill " + data[0].ToString(), false);
          }
        }

        return new KeyValuePair<bool, string>(true, "Killed existing connection to Expenses");
      }
      catch (Exception) { return new KeyValuePair<bool, string>(false, "Cancelled or problem"); }
    }

    public KeyValuePair<bool, string> RestoreDB(string aLoc)
    {
      try
      {
        using (var master = new SQLTalker("(local)", "master"))
        {
          return new KeyValuePair<bool, string>(true, master.Procer("restore database " + _database + "\nfrom disk = '" + aLoc + "'\nwith replace", false));
        }
      }
      catch (Exception) { return new KeyValuePair<bool, string>(false, "Could not restore database"); }
    }

    #region Related to populating data sets

    private string Procer(string sql, bool counts)
    {
      using (SqlConnection cn = new SqlConnection(_connection))
      using (SqlCommand cmd = new SqlCommand(sql, cn))
      {
        cmd.CommandTimeout = 5000;
        StringBuilder sb = new StringBuilder();
        cn.Open();

        SqlParameter rtn = cmd.Parameters.Add("return", SqlDbType.Int);
        rtn.Direction = ParameterDirection.ReturnValue;

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

    private DataTable GetData(string sqlquery)
    {
      using (SqlConnection cn = new SqlConnection(_connection))
      using (SqlCommand cmd = new SqlCommand(sqlquery, cn))  
      using (SqlDataAdapter adapter = new SqlDataAdapter())
      using (DataTable table = new DataTable())
      {
        cmd.CommandTimeout = 5000;
        cn.Open();
        adapter.SelectCommand = cmd;
        table.Locale = System.Globalization.CultureInfo.InvariantCulture;
        adapter.Fill(table);
        cn.Close();
        return table;
      }
    }

    public void Dispose() {}
    #endregion

  }
}
