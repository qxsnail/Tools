using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EIP.Library.Data
{
    public static class DataExtension
    {
        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            var result = new List<T>();

            DataTable dt = reader.GetSchemaTable();
            try
            {
                while (reader.Read())
                {
                    var t = new T();

                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            // 当前列名&属性名
                            string columnName = dr[0].ToString();
                            PropertyInfo pro = typeof(T).GetProperty(columnName);

                            if (pro == null)
                            {
                                continue;
                            }

                            if (!pro.CanWrite)
                            {
                                continue;
                            }

                            pro.SetValue(t, ConvertExtension.ConvertHelper(reader[columnName], pro.PropertyType), null);
                        }
                    }

                    result.Add(t);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Dispose();
                    reader.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">dt</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var result = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                try
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        // 当前列名&属性名
                        string columnName = column.ColumnName;
                        PropertyInfo pro = typeof(T).GetProperty(columnName);

                        if (pro == null)
                        {
                            continue;
                        }

                        if (!pro.CanWrite)
                        {
                            continue;
                        }

                        pro.SetValue(t, ConvertExtension.ConvertHelper(dr[columnName], pro.PropertyType), null);
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                result.Add(t);
            }

            return result;
        }

        /// <summary>
        /// 将DataTable 转换成list 字段对照方式
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">dt</param>
        /// <param name="field">转换字典</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataTable dt, Dictionary<string, string> field) where T : class, new()
        {
            var result = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                try
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        // 当前列名&属性名
                        string columnName = column.ColumnName;
                        PropertyInfo pro = null;
                        if (field.ContainsKey(columnName))
                        {
                            pro = typeof(T).GetProperty(field[columnName]);
                        }
                        else
                        {
                            pro = typeof(T).GetProperty(columnName);
                        }

                        if (pro == null)
                        {
                            continue;
                        }

                        if (!pro.CanWrite)
                        {
                            continue;
                        }

                        pro.SetValue(t, ConvertExtension.ConvertHelper(dr[columnName], pro.PropertyType), null);
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                result.Add(t);
            }

            return result;
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataSet ds) where T : class, new()
        {
            return ds.Tables[0].ToList<T>();
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <param name="dataTableIndex">dataTableIndex</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataSet ds, int dataTableIndex) where T : class, new()
        {
            return ds.Tables[dataTableIndex].ToList<T>();
        }

        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this IDataReader reader) where T : class, new()
        {
            var t = new T();
            DataTable dt = reader.GetSchemaTable();
            try
            {
                while (reader.Read())
                {
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            // 当前列名&属性名
                            string columnName = dr[0].ToString();
                            PropertyInfo pro = typeof(T).GetProperty(columnName);

                            if (pro == null)
                            {
                                continue;
                            }

                            if (!pro.CanWrite)
                            {
                                continue;
                            }

                            pro.SetValue(t, ConvertExtension.ConvertHelper(reader[columnName], pro.PropertyType), null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Dispose();
                    reader.Close();
                }
            }

            return t;
        }

        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">dt</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this DataTable dt) where T : class, new()
        {
            var t = new T();
            if (dt.Rows.Count <= 0)
            {
                return t;
            }

            try
            {
                foreach (DataColumn column in dt.Columns)
                {
                    // 当前列名&属性名
                    string columnName = column.ColumnName;
                    PropertyInfo pro = typeof(T).GetProperty(columnName);
                    if (pro == null)
                    {
                        continue;
                    }

                    if (!pro.CanWrite)
                    {
                        continue;
                    }

                    pro.SetValue(t, ConvertExtension.ConvertHelper(dt.Rows[0][columnName], pro.PropertyType), null);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return t;
        }

        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <param name="dataTableIndex">dataTableIndex</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this DataSet ds, int dataTableIndex = 0) where T : class, new()
        {
            return ds.Tables[0].ToModel<T>();
        }

        /// <summary>
        /// 通过链接名拿链接
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static MySqlConnection GetConnection(string connectionName)
        {
            MySqlConnection result = null;

            string connectionStringTemp = ConfigurationManager.ConnectionStrings["eipdbEntities"].ConnectionString;
            // 这个链接字符串有点日隆，所以下面处理一下用正则来拼
            Match serverMatch = Regex.Match(connectionStringTemp, "server=(?<Text>\\S*?)[;|\"]", RegexOptions.IgnoreCase);
            string serverStr = serverMatch.Success ? serverMatch.Groups["Text"].Value : string.Empty;
            Match uidMatch = Regex.Match(connectionStringTemp, "user id=(?<Text>\\S*?)[;|\"]", RegexOptions.IgnoreCase);
            string uidStr = uidMatch.Success ? uidMatch.Groups["Text"].Value : string.Empty;
            Match passwordMatch = Regex.Match(connectionStringTemp, "password=(?<Text>\\S*?)[;|\"]", RegexOptions.IgnoreCase);
            string passwordStr = passwordMatch.Success ? passwordMatch.Groups["Text"].Value : string.Empty;
            Match perMatch = Regex.Match(connectionStringTemp, "persistsecurityinfo=(?<Text>\\S*?)[;|\"]", RegexOptions.IgnoreCase);
            string perStr = perMatch.Success ? perMatch.Groups["Text"].Value : string.Empty;
            Match databaseMatch = Regex.Match(connectionStringTemp, "database=(?<Text>\\S*?)[;|\"]", RegexOptions.IgnoreCase);
            string databaseStr = databaseMatch.Success ? databaseMatch.Groups["Text"].Value : string.Empty;

            string connectionString = $"server={serverStr};user id={uidStr};password={passwordStr};persistsecurityinfo={perStr};database={databaseStr}";
            result = new MySqlConnection(connectionString);

            return result;
        }
    }

    public static class ConvertExtension
    {
        /// <summary>
        /// The convert helper.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="conversionType">The conversion type.</param>
        /// <returns>The <see cref="object" />.</returns>
        public static object ConvertHelper(object value, Type conversionType)
        {
            Type nullableType = Nullable.GetUnderlyingType(conversionType);

            //// 判断当前类型是否可为 null
            if (nullableType != null)
            {
                if (value == DBNull.Value)
                {
                    return null;
                }

                //// 若是枚举 则先转换为枚举
                if (nullableType.IsEnum)
                {
                    value = System.Enum.Parse(nullableType, value.ToString());
                }

                return Convert.ChangeType(value, nullableType);
            }

            if (conversionType.IsEnum)
            {
                return System.Enum.Parse(conversionType, value.ToString());
            }

            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// The convert to decimal null.
        /// </summary>
        /// <param name="targetObj">The target obj.</param>
        /// <returns>The <see cref="decimal" />.</returns>
        public static decimal? ConvertToDecimalNull(object targetObj)
        {
            if (targetObj == null || targetObj == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDecimal(targetObj);
        }

        /// <summary>
        /// The convert to int null.
        /// </summary>
        /// <param name="targetObj">The target obj.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int? ConvertToIntNull(object targetObj)
        {
            if (targetObj == null || targetObj == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(targetObj);
        }

        /// <summary>
        /// The convert to string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ConvertToString(object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="entitys">泛类型集合</param>
        /// <returns>DataTable</returns>
        /// <exception cref="System.Exception">
        /// 需转换的集合为空
        /// or
        /// 要转换的集合元素类型不一致
        /// </exception>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            // 检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new System.Exception("需转换的集合为空");
            }

            // 取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            // 生成DataTable的structure
            // 生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            foreach (PropertyInfo t in entityProperties)
            {
                dt.Columns.Add(t.Name);
            }

            // 将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                // 检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new System.Exception("要转换的集合元素类型不一致");
                }

                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }

                dt.Rows.Add(entityValues);
            }

            return dt;
        }
    }
}
