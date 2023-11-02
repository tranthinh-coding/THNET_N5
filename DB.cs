using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace N5
{
    public class DB
    {
        public static SqlConnection Conn = null;
        public static string ConnectionString = "Data Source=THINK\\SQLEXPRESS;Initial Catalog=QLBanHangTapHoa;Integrated Security=True";
        public static string TABLE;

        public List<string> WHERE = new List<string>();

        DB()
        {
            Conn = new SqlConnection(ConnectionString);
            Open();
        }

        ~DB()
        {
            Close();
        }

        public DB Open()
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Open();
            }
            return this;
        }

        public DB Close()
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            return this;
        }

        public static DB Table(string _table)
        {
            TABLE = _table.Trim();

            return new DB();
        }

        public SqlDataAdapter RawQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);

            return new SqlDataAdapter(cmd);
        }

        public SqlDataAdapter Select(String[] columns)
        {
            var sql = new StringBuilder($"SELECT {string.Join(",", columns)} FROM {TABLE}");

            if (WHERE.Count > 0)
            {
                string whereClause = string.Join(" AND ", WHERE.Cast<string>());
                sql.Append($" WHERE {whereClause}");
            }

            SqlCommand cmd = new SqlCommand(sql.ToString(), Conn);

            return new SqlDataAdapter(cmd);
        }

        public DB Where(string column, object value, string opera = "=")
        {
            string semi = "";
            if (value.GetType() == typeof(string))
            {
                semi = "'";
            }

            WHERE.Add(
                 $" {column} {opera} {semi + value + semi} "
            );
  
            return this;
        }

        public int Delete()
        {
            var sql = new StringBuilder($"DELETE FROM {TABLE}");

            if (WHERE.Count > 0)
            {
                sql.Append($" WHERE {string.Join(" AND ", WHERE)}");
            }

            SqlCommand cmd = new SqlCommand(sql.ToString(), Conn);

            return cmd.ExecuteNonQuery();
        }

        public int Update(IDictionary<string, object> values) {
            var sql = new StringBuilder("UPDATE " + TABLE);

            sql.Append(" SET ");

            // them cac gia tri
            foreach (var pair in values)
            {
                sql.Append($"{pair.Key} = @{pair.Key},");
            }

            sql.Length -= 1; // xoa ki tu dau phay ^^^^ o cuoi

            if (WHERE.Count > 0)
            {
                sql.Append(
                    " WHERE " + string.Join(" AND ", WHERE)    
                );
            }

            SqlCommand cmd = new SqlCommand(sql.ToString(), Conn);
            foreach (var pair in values)
            {
                cmd.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
            }

            return cmd.ExecuteNonQuery();
        }
    }
}
