using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///数据库辅助类
///</summary>
public class SQLiteHelp
{
    private SqliteConnection dbConnection;
    private SqliteCommand dbCommand;
    private SqliteDataReader dbReader;

    public SQLiteHelp(string conStr)
    {
        OpenSQLite(conStr);
    }

    //打开数据库
    public void OpenSQLite(string conStr)
    {
        try
        {
            dbConnection = new SqliteConnection(conStr);
            dbConnection.Open();
            Debug.Log("Connect successful!");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    //连接数据库
    public void CloseSqlConnection()

    {
        if (dbCommand != null)
        {
            dbCommand.Dispose();
        }
        dbCommand = null;

        if (dbReader != null)
        {
            dbReader.Dispose();
        }
        dbReader = null;

        if (dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;

        Debug.Log("Disconnected from db.");
    }

    //执行sqlQuery操作 
    public SqliteDataReader ExecuteQuery(string sqlQuery)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = sqlQuery;
        dbReader = dbCommand.ExecuteReader();

        return dbReader;
    }

    //插入数据
    public SqliteDataReader InsertInto(string tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];

        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }

        query += ")";

        return ExecuteQuery(query);
    }

    //查找表中符合两个条件的数据
    public SqliteDataReader ReadFullTable(string tableName, string selectvalue1, string selectvalue2)
    {
        string query = "SELECT * FROM " + tableName + " where acount = "+selectvalue1+" and password = "+selectvalue2;

        return ExecuteQuery(query);
    }
    //查找表中指定数据
    public SqliteDataReader ReadSpecificData(string tableName, string selectkey, string selectvalue1, string selectvalue2)
    {
        string query = "SELECT * FROM " + tableName + " where acount = " + selectvalue1 + " and password = " + selectvalue2 +" and "+ selectkey + " IS NOT NULL ";

        return ExecuteQuery(query);
    }


    //更新数据  SQL语法：UPDATE table_name SET column1 = value1, column2 = value2....columnN = valueN[WHERE  CONDITION];
    public SqliteDataReader UpdateInto(string tableName, string col, int selectvalue)
    {
        string query = "UPDATE " + tableName + " SET " + col + " = " + selectvalue;

        return ExecuteQuery(query);
    }

    //删除表中的内容  DELETE FROM table_name  WHERE  {CONDITION or CONDITION}(删除所有符合条件的内容）
    public SqliteDataReader Delete(string tableName, string[] cols, string[] colsvalues)
    {
        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];

        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += " or " + cols[i] + " = " + colsvalues[i];
        }

        return ExecuteQuery(query);
    }

    //插入指定的数据
    public SqliteDataReader InsertIntoSpecific(string tableName, string[] cols, string[] values)
    {
        if (cols.Length != values.Length)
        {
            throw new SqliteException("columns.Length != values.Length");
        }

        string query = "INSERT INTO " + tableName + "(" + cols[0];

        for (int i = 1; i < cols.Length; ++i)
        {
            query += ", " + cols[i];
        }

        query += ") VALUES (" + values[0];

        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }

        query += ")";

        return ExecuteQuery(query);
    }

    //判断在指定列名中是否存在输入的值
    //public bool ExitItem(string tableName, string itemName, string itemValue)
    //{
    //    bool flag = false;

    //    dbReader = ReadFullTable(tableName);

    //    while (dbReader.Read())
    //    {
    //        for (int i = 0; i < dbReader.FieldCount; i++)
    //        {
    //            if (dbReader.GetName(i) == itemName)
    //            {
    //                if (dbReader.GetValue(i).ToString() == itemValue)
    //                {
    //                    flag = true;
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    return flag;
    //}
}
