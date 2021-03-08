using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackingApp.Infrastructure.Data
{
    public class SqlQueryBuilder<T> where T : class
    {
        private readonly T _item;

        public SqlQueryBuilder(T item)
        {
            _item = item;
        }

        #region Insert

        public SqlCommand GetInsertCommand()
        {
            var table = GetTableName();
            if (string.IsNullOrEmpty(table))
                throw new Exception("No Table attribute was found.");
            var query = $"INSERT INTO {table} SELECT {GetInsertFieldList()}";
            return new SqlCommand(query);
        }
        private string GetInsertFieldList()
        {
            var sb = new StringBuilder();
            var properties = _item.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var property = GetSqlValue(_item, propertyInfo);
                if(string.Equals(propertyInfo.Name,"Id"))
                    continue;
                sb.Append(GetFormattedInsertField(propertyInfo, property));
            }
            var query = sb.ToString();
            return query.Remove(query.Length - 1);
        }

        private string GetFormattedInsertField(PropertyInfo propertyInfo, SqlString property)
        {
            // int
            var result = $"{property.Value} as {propertyInfo.Name},";
            // string
            if (propertyInfo.PropertyType == typeof(string))
            {
                result = $"'{property.Value}' as {propertyInfo.Name},";
            }
            // datetime
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                result = $"'{property.Value:u}' as {propertyInfo.Name},";
            }
            return result;
        }

        #endregion

        #region Update

        public SqlCommand GetUpdateCommand()
        {
            var table = GetTableName();
            if (String.IsNullOrEmpty(table))
                throw new Exception("No Table attribute was found.");
            var query = $"UPDATE {table} SET {GetUpdateFieldList()} WHERE {GetKeyFieldName()}={GetKeyFieldValue()}";
            return new SqlCommand(query);
        }

        private string GetUpdateFieldList()
        {
            var sb = new StringBuilder();
            var properties = _item.GetType().GetProperties();
            var keyField = GetKeyFieldName();
            foreach (var propertyInfo in properties)
            {
                if (keyField == propertyInfo.Name) continue;
                var property = GetSqlValue(_item, propertyInfo);
                sb.Append(GetFormattedUpdateField(propertyInfo, property));
            }
            var query = sb.ToString();
            return query.Remove(query.Length - 1);
        }

        private string GetFormattedUpdateField(PropertyInfo propertyInfo, SqlString property)
        {
            // int
            var result = $"{propertyInfo.Name}={property.Value},";
            // string
            if (propertyInfo.PropertyType == typeof(string))
            {
                result = $"{propertyInfo.Name}='{property.Value}',";
            }
            // datetime
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                result = $"{propertyInfo.Name}='{property.Value:u}',";
            }
            return result;
        }

        #endregion

        #region Delete

        public SqlCommand GetDeleteCommand()
        {
            var table = GetTableName();
            if (string.IsNullOrEmpty(table))
                throw new Exception("No Table attribute was found.");
            var query = $"DELETE FROM {table} WHERE {GetKeyFieldName()}={GetKeyFieldValue()}";
            return new SqlCommand(query);
        }

        #endregion

        #region Helper methods
        protected string GetTableName()
        {
            var tableAttr = Attribute.GetCustomAttribute(typeof(T),
                typeof(TableAttribute));
            return tableAttr != null
                ? (tableAttr as TableAttribute).Name
                : String.Empty;
        }
        private SqlString GetSqlValue(T item, PropertyInfo propertyInfo)
        {
            return new SqlString(propertyInfo.GetValue(item).ToString());
        }
        private string GetKeyFieldName()
        {
            var result = GetKeyField();
            return result.Name;
        }
        private string GetKeyFieldValue()
        {
            var result = GetKeyField();
            return result.GetValue(_item).ToString();
        }
        private PropertyInfo GetKeyField()
        {
            var keyField = _item
                .GetType()
                .GetProperties()
                .FirstOrDefault(e => Attribute.IsDefined(e, typeof(KeyAttribute)));
            if (keyField != null)
            {
                return keyField;
            }
            throw new Exception("Key on a property could not be found");
        }

        #endregion
    }
}
