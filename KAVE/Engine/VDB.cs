using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using KAVE.BaseEngine;

namespace KAVE
{
   public static class VDB
   {
       #region variables
       
        internal static SQLiteConnection HADB;
        internal static SQLiteConnection PEDB;
        public static SQLiteConnection SDB;
        internal static SQLiteConnection PGDB;
        internal static SQLiteConnection WDB;
        public static int version = 0;
        public static int vdbcount = 0;

        public static bool Initialized = false;

       #endregion

        /// <summary>
       /// Initialize VDB
       /// </summary>
       public static void Initialize()
       {
           try
           {
               WDB = new SQLiteConnection(BuildConString(Application.StartupPath + @"\VDB\WDB.avdb", SettingsManager.CacheSize, 3, SettingsManager.MaxPages, SettingsManager.PageSize, false, false));
               PEDB = new SQLiteConnection(BuildConString(Application.StartupPath + @"\VDB\PEDB.avdb", SettingsManager.CacheSize, 3, SettingsManager.MaxPages, SettingsManager.PageSize, false, false));
               HADB = new SQLiteConnection(BuildConString(Application.StartupPath + @"\VDB\HDB.avdb", SettingsManager.CacheSize, 3, SettingsManager.MaxPages, SettingsManager.PageSize, false, false));
               SDB = new SQLiteConnection(BuildConString(Application.StartupPath + @"\VDB\SDB.avdb", SettingsManager.CacheSize, 3, SettingsManager.MaxPages, SettingsManager.PageSize, false, false));
               PGDB = new SQLiteConnection(BuildConString(Application.StartupPath + @"\VDB\PROGRAM.avdb", SettingsManager.CacheSize, 3, SettingsManager.MaxPages, SettingsManager.PageSize, false, false));
               Open();

               SQLiteCommand cmd = new SQLiteCommand();

               cmd.Connection = SDB;
               cmd.CommandText = string.Format("SELECT version FROM VERSION WHERE [vd]='{0}'", 1);
               object result = cmd.ExecuteScalar();


               version = Convert.ToInt32(result);
               VDB.VDBDefinitions();

           }
           catch (Exception ex)
           {
               Initialized = false;
               AntiCrash.LogException(ex);
           }
           finally
           {

           }

       }
       /// <summary>
       /// Open VDB Connection
       /// </summary>
       public static void Open()
       {
           try
           {
               HADB.Open();
               PEDB.Open();
               PGDB.Open();
               SDB.Open();
               WDB.Open();
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }

       /// <summary>
       /// Close VDB
       /// </summary>
       public static void Close()
       {
           try
           {
               HADB.Close();
               PEDB.Close();
               PGDB.Close();
               SDB.Close();
               WDB.Close();
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
      
       /// <summary>
       /// Restart VDB
       /// </summary>
       public static void Restart()
       {

           try
           {
               HADB.Open();
               PEDB.Open();
               PGDB.Open();
               SDB.Open();
               WDB.Open();

               HADB.Close();
               PEDB.Close();
               PGDB.Close();
               SDB.Close();
               WDB.Close();
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
      
       internal static string BuildConString(string dbfile, int cachesize, int version, int maxpagecount, int pagesize, bool pooling, bool Sync)
       {
           SQLiteConnectionStringBuilder wdbcons = new SQLiteConnectionStringBuilder();
           wdbcons.CacheSize = cachesize;
           wdbcons.DataSource = dbfile;
           wdbcons.Version = version;
           wdbcons.MaxPageCount = maxpagecount;
           wdbcons.PageSize = pagesize;
           wdbcons.Pooling = pooling;
           wdbcons.JournalMode = SQLiteJournalModeEnum.Off;
           if (Sync)
           {
               wdbcons.SyncMode = SynchronizationModes.Full;
           }
           else
           {
               wdbcons.SyncMode = SynchronizationModes.Off;
           }
           wdbcons.FailIfMissing = true;
           return wdbcons.ConnectionString;
       }
       internal static void ExecutePragma()
       {
           using (SQLiteTransaction tr = SDB.BeginTransaction())
           {
               using (SQLiteCommand cmd = SDB.CreateCommand())
               {

                   cmd.CommandText = "PRAGMA temp_store = 1";
                   cmd.ExecuteNonQuery();

                   cmd.CommandText = "PRAGMA count_changes = OFF";
                   cmd.ExecuteNonQuery();

                   cmd.CommandText = "PRAGMA page_size=4096";
                   cmd.ExecuteNonQuery();


               }
           }
           using (SQLiteTransaction tr = PEDB.BeginTransaction())
           {
               using (SQLiteCommand cmd = PEDB.CreateCommand())
               {


                   cmd.CommandText = "PRAGMA temp_store = 1";
                   cmd.ExecuteNonQuery();

                   cmd.CommandText = "PRAGMA count_changes = OFF";
                   cmd.ExecuteNonQuery();




                   cmd.CommandText = "PRAGMA page_size=4096";
                   cmd.ExecuteNonQuery();
               }
           }
           using (SQLiteTransaction tr = HADB.BeginTransaction())
           {
               using (SQLiteCommand cmd = HADB.CreateCommand())
               {

                   cmd.CommandText = "PRAGMA temp_store = 1";
                   cmd.ExecuteNonQuery();

                   cmd.CommandText = "PRAGMA count_changes = OFF";
                   cmd.ExecuteNonQuery();

                   cmd.CommandText = "PRAGMA page_size=4096";
                   cmd.ExecuteNonQuery();


               }
           }
           using (SQLiteTransaction tr = PGDB.BeginTransaction())
           {
               using (SQLiteCommand cmd = PGDB.CreateCommand())
               {
                   cmd.CommandText = "PRAGMA temp_store = 2";
                   cmd.ExecuteNonQuery();
                   cmd.CommandText = "PRAGMA page_size=4096";
                   cmd.ExecuteNonQuery();

                   cmd.CommandText = "PRAGMA count_changes = OFF";
                   cmd.ExecuteNonQuery();



               }
           }
           using (SQLiteTransaction tr = WDB.BeginTransaction())
           {
               using (SQLiteCommand cmd = WDB.CreateCommand())
               {

                   cmd.CommandText = "PRAGMA temp_store = 1";
                   cmd.ExecuteNonQuery();
                   cmd.CommandText = "PRAGMA page_size=4096";
                   cmd.ExecuteNonQuery();
                   cmd.CommandText = "PRAGMA count_changes = OFF";
                   cmd.ExecuteNonQuery();

               }
           }
       }

       #region Select
       public static int definitions;
       public static int VDBDefinitions()
       {
           using (SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT version FROM VERSION WHERE vd='{0}'", "2"), SDB))
           {
               object result = cmd.ExecuteScalar();
               definitions = Int32.Parse(result.ToString());

               return definitions;
           }
       }
       public static BlackListResult CheckUrlHash(string hashedUrl)
       {
           SQLiteConnection conn = WDB;
           BlackListResult blackListResult = BlackListResult.NotFound;

           using (SQLiteCommand cmd = new SQLiteCommand(conn))
           {
               cmd.CommandText = string.Format("SELECT blacklistid FROM {0} WHERE hash MATCH '{1}';", FileFormat.GetTable(hashedUrl), hashedUrl);
               object result = cmd.ExecuteScalar();
               if (result == null)
                   blackListResult = BlackListResult.NotFound;
               else
               {
                   int blackListId = Convert.ToInt32(result);
                   int phishingId = 1;
                   if (blackListId == phishingId)
                   {
                       blackListResult = BlackListResult.PhishingAttack;
                   }
                   else if (blackListId == 3)
                   {
                       blackListResult = BlackListResult.PornAttack;
                   }
                   else
                   {
                       blackListResult = BlackListResult.MalwareAttack;
                   }
               }
           }


           return blackListResult;
       }
       public static int GetIDPCount()
       {
           SQLiteCommand cmd = new SQLiteCommand();
           cmd.Connection = SDB;
           cmd.CommandText = "SELECT COUNT(*) FROM IDP";
           object dr = cmd.ExecuteScalar();
           return Convert.ToInt32(dr);

       }
       public static object GetScript(string hex)
       {
           using (SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT virusname FROM {0} WHERE hex MATCH '{1}';", "TEXTDB", hex), SDB))
           {
               return cmd.ExecuteScalar();
           }
       }
    
       public static object GetMD5(string hash)
       {
           string table = FileFormat.GetTable(hash);
           if (table != "false")
           {
               using (SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT virusname FROM {0} WHERE hash MATCH '{1}';", table, hash), HADB))
               {
                   return cmd.ExecuteScalar();
               }

           }
           else
           {
               return null;
           }
       }
       public static object GetPEMD5(string hash)
       {
           string table = FileFormat.GetTable(hash);
           if (table != "false")
           {
               using (SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT virusname FROM {0} WHERE hash MATCH '{1}';", table, hash), PEDB))
               {
                   return cmd.ExecuteScalar();
               }

           }
           else
           {
               return null;
           }
       }
       public static object GetHSCript(string hex)
       {
           using (SQLiteCommand cmd = new SQLiteCommand(SDB))
           {
               cmd.CommandText = "SELECT * FROM TEXTDB";
               SQLiteDataReader dr = cmd.ExecuteReader();
               while (dr.Read())
               {
                   if (hex.Contains((string)dr["hex"]))
                   {
                      return dr["virusname"];

                   }
     }
           }
           return null;
       }
       public static bool GetIDP(string filename, string hash)
       {
           string sresult = "false";

           using (SQLiteCommand cmd = new SQLiteCommand(SDB))
           {
               cmd.CommandText = string.Format("SELECT hash FROM IDP WHERE file MATCH '{0}';", filename);
               sresult = (string)cmd.ExecuteScalar();
               if (sresult != null)
               {
                   return true;
               }
               else
               {
                   cmd.CommandText = string.Format("SELECT file FROM IDP WHERE hash MATCH '{0}';", hash);
                   sresult = (string)cmd.ExecuteScalar();
                   if (sresult != null)
                   {
                       return false;
                   }
                   else
                   {
                       return false;
                   }
               }
           }
       }
       public static object GetSpam(string text, out string word)
       {
           using (SQLiteCommand cmd = new SQLiteCommand(PGDB))
           {
               cmd.CommandText = "SELECT * FROM SPAM";
               SQLiteDataReader dr = cmd.ExecuteReader();
               while (dr.Read())
               {
                   if (text.Contains((string)dr["spam"]))
                   {
                       word = (string)dr["spam"];
                       return dr["virusname"];

                   }

               }
           }
           word = null;
           return null;
       }
       public static string GetRepair(string source)
       {
           string sresult;
           using (SQLiteCommand cmd = new SQLiteCommand(SDB))
           {
               cmd.CommandText = string.Format("SELECT hex FROM TEXTDB WHERE virusname MATCH '{0}';", source);
               object result = cmd.ExecuteScalar();

               if (result != null)
               {
                   sresult = (string)result;
               }
               else
               {
                   sresult = "false";
               }
           }

           return sresult;
       }
       #endregion

       #region  Methods
       public static void Setversion(string ver)
       {
           SQLiteCommand cmd = new SQLiteCommand();
           cmd.Connection = SDB;
           cmd.CommandText = string.Format("UPDATE VERSION SET version='{0}' WHERE vd='1'", ver);
           version = Int32.Parse(ver);
           cmd.ExecuteNonQuery();
       }
       public static void SetCount(string count)
       {
           int scount = 0;
           using (SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT version FROM VERSION WHERE vd='{0}'", "2"), SDB))
           {
               object sresult = cmd.ExecuteScalar();
               scount = Int32.Parse(sresult.ToString());

           }
           int result = scount + Int32.Parse(count.ToString());
           using (SQLiteTransaction trans = SDB.BeginTransaction())
           {
               SQLiteCommand scmd = new SQLiteCommand();
               scmd.Connection = SDB;
               scmd.Transaction = trans;
               scmd.CommandText = string.Format("UPDATE VERSION SET version='{0}' WHERE vd='2'", result);

               scmd.ExecuteNonQuery();

               trans.Commit();
           }
       }

       #endregion

       #region Cloud
       public static string CheckCloud(string hash)
       {
           using (SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT state FROM {0} WHERE hash MATCH '{1}';", "CLOUDB", hash), SDB))
           {
               return (string)cmd.ExecuteScalar();
           }
       }
       public static void InsertCloud(string hash, string filename, string state)
       {
           using (SQLiteTransaction trans = SDB.BeginTransaction())
           {

               using (SQLiteCommand addKeysCmd = new SQLiteCommand(SDB))
               {
                   string sqlIns = "INSERT INTO CLOUDB (state, filename, hash, Scanned) VALUES('" + state + "', '" + filename + "', '" + hash + "', '" + DateTime.Now.ToString() + "')";
                   addKeysCmd.CommandText = sqlIns;
                   addKeysCmd.ExecuteNonQuery();
               }
               trans.Commit();
           }
       }
#endregion

       #region Insert

       internal static void AddIDP(string[] files, ProgressBarX progress)
       {

           using (SQLiteTransaction trans = SDB.BeginTransaction())
           {
               int c = 0;
               using (SQLiteCommand addKeysCmd = new SQLiteCommand(SDB))
               {
                   foreach (string file in files)
                   {

                       string sqlIns = "INSERT INTO IDP (file, hash) VALUES('" + file + "', '" + Security.GetMD5HashFromFile(file) + "')";
                       addKeysCmd.CommandText = sqlIns;
                       addKeysCmd.ExecuteNonQuery();
                    
                       c++;
                       GUI.UpdateProgress(progress, c, files.Length);

                   }
               }
               trans.Commit();
           }
       }
       internal static void AddKeys(Dictionary<string, string> db, DBT dbtp)
       {
           try
           {
               if (dbtp == DBT.SDB)
               {
                   using (SQLiteTransaction trans = SDB.BeginTransaction())
                   {

                       using (SQLiteCommand addKeysCmd = new SQLiteCommand(SDB))
                       {

                           foreach (KeyValuePair<string, string> pair in db)
                           {
                               string sqlIns = "INSERT INTO TEXTDB (hex, virusname) VALUES('" + pair.Key + "', '" + pair.Value + "');";
                               addKeysCmd.CommandText = sqlIns;
                               addKeysCmd.ExecuteNonQuery();
                           }
                       }
                       trans.Commit();
                   }
                   SetCount(db.Count.ToString());
               }
               else if (dbtp == DBT.HDB)
               {
                   using (SQLiteTransaction trans = HADB.BeginTransaction())
                   {

                       using (SQLiteCommand addKeysCmd = new SQLiteCommand(HADB))
                       {

                           foreach (KeyValuePair<string, string> pair in db)
                           {
                               string sqlIns = string.Format("INSERT INTO {0} (hash, virusname) VALUES('" + pair.Key + "', '" + pair.Value + "');", FileFormat.GetTable(pair.Key));
                               addKeysCmd.CommandText = sqlIns;
                               addKeysCmd.Transaction = trans;
                               addKeysCmd.ExecuteNonQuery();
                           }
                       }
                       trans.Commit();
                   }
                   SetCount(db.Count.ToString());
               }
               else if (dbtp == DBT.HEUR)
               {
                   using (SQLiteTransaction trans = SDB.BeginTransaction())
                   {

                       using (SQLiteCommand addKeysCmd = new SQLiteCommand(SDB))
                       {

                           foreach (KeyValuePair<string, string> pair in db)
                           {
                               string sqlIns = "INSERT INTO HEURISTIC (instruction, rate) VALUES('" + pair.Key + "', '" + pair.Value + "');";
                               addKeysCmd.CommandText = sqlIns;
                               addKeysCmd.Transaction = trans;
                               addKeysCmd.ExecuteNonQuery();
                           }
                       }
                       trans.Commit();
                   }
                   SetCount(db.Count.ToString());
               }
               else if (dbtp == DBT.WDB)
               {
                   using (SQLiteTransaction trans = WDB.BeginTransaction())
                   {

                       using (SQLiteCommand addKeysCmd = new SQLiteCommand(WDB))
                       {

                           foreach (KeyValuePair<string, string> pair in db)
                           {
                               string sqlIns = string.Format("INSERT INTO {0} (blacklistid, hash) VALUES('{1}', '{2}');",
                                                   FileFormat.GetTable(pair.Key), pair.Value, pair.Key);
                               addKeysCmd.CommandText = sqlIns;
                               addKeysCmd.Transaction = trans;
                               addKeysCmd.ExecuteNonQuery();
                           }
                       }
                       trans.Commit();
                   }
                   SetCount(db.Count.ToString());
               }
               else
               {
                   using (SQLiteTransaction trans = PEDB.BeginTransaction())
                   {

                       using (SQLiteCommand addKeysCmd = new SQLiteCommand(PEDB))
                       {

                           foreach (KeyValuePair<string, string> pair in db)
                           {
                               string sqlIns = string.Format("INSERT INTO {0} (hash, virusname) VALUES('" + pair.Key + "', '" + pair.Value + "');", FileFormat.GetTable(pair.Key));
                               addKeysCmd.CommandText = sqlIns;
                               addKeysCmd.ExecuteNonQuery();
                           }
                       }
                       trans.Commit();
                   }
                   SetCount(db.Count.ToString());
               }
           }
           catch
           {

           }
           finally
           {
           }
       }
       #endregion

      
   }
}
