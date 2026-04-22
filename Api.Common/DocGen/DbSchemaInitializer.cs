using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Api.Common.DocGen
{
    public static class DbSchemaInitializer
    {
        public static void UpdateDescriptionsByEFAPI(this DbContext context)
        {
            var dbConnection = context.Database.GetDbConnection();
            dbConnection.Open();
            var models = context.Model.GetEntityTypes();
            foreach (var model in models)
            {
                var tableName = model.GetTableName();
                var modelType = model.ClrType;
                var modelName = modelType.Name;
                if (modelName != tableName)
                {
                    syncTableDescription(dbConnection, tableName, modelName, "EntityName");
                }


                var descAttr = modelType.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(DescriptionAttribute));
                if (descAttr != null)
                {
                    var desc = descAttr.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(desc))
                    {
                        //Sync to database
                        syncTableDescription(dbConnection, tableName, desc);
                    }
                }
            }
        }

        public static void UpdateDescriptionsByReflection(this DbContext context)
        {
            var dbsetProps = context.GetDbSetProperties();
            var dbConnection = context.Database.GetDbConnection();
            dbConnection.Open();
            dbsetProps.ForEach(prop =>
            {
                #region Get DAO type
                //Get DbSet's model. For example, DbSet<MyModel> => MyModel
                Type typeArgument = prop.PropertyType.GetGenericArguments()[0];
                #endregion

                #region Get Table description
                string tableName = string.Empty, desc = string.Empty;
                Object tableNameAttr = getTableAttribute(typeArgument, "TableAttribute");

                Object descAttr = getTableAttribute(typeArgument, "DescriptionAttribute");
                if (tableNameAttr != null)
                    tableName = (tableNameAttr as System.ComponentModel.DataAnnotations.Schema.TableAttribute).Name;
                else
                {
                    tableName = typeArgument.Name;
                }
                if (descAttr != null)
                    desc = (descAttr as System.ComponentModel.DescriptionAttribute).Description;

                if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(desc))
                {
                    //Sync to database
                    syncTableDescription(dbConnection, tableName, desc);
                }

                #endregion

            });
        }

        /// Get attribute of class
        private static object getTableAttribute(Type typeArgument, string attribute)
        {
            var method = typeof(AttributeUtility).GetMethod("GetClassAttributes");
            var generic = method.MakeGenericMethod(typeArgument);
            var result = generic.Invoke(null, new object[] { false });
            var dics = (result as Dictionary<string, object>);

            Object value = null;
            if (dics.TryGetValue(attribute, out value))
                return value;
            else
                return null;
        }

        private static List<PropertyInfo> GetDbSetProperties(this DbContext context)
        {
            var dbSetProperties = new List<PropertyInfo>();
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
                if (isDbSet)
                {
                    dbSetProperties.Add(property);
                }
            }

            return dbSetProperties;
        }

        private static void syncTableDescription(DbConnection dbConnection, string tableName, string description, string descKey = "MS_Description")
        {

            var sqlcmd = dbConnection.CreateCommand();
            sqlcmd.CommandText = $@"SELECT (SELECT value 
                                FROM sys.fn_listextendedproperty(NULL, 'user', tb.TABLE_SCHEMA, 'table', tb.TABLE_NAME, DEFAULT, DEFAULT)
                                WHERE name = '{descKey}'
                                AND objtype = 'TABLE')
                        AS 'TableDescription'
								 FROM 
                  INFORMATION_SCHEMA.TABLES tb
				  where tb.TABLE_NAME='{tableName}'";
            var reader = sqlcmd.ExecuteReader();
            var hasData = reader.HasRows;
            if (hasData == false)
            {
                return;
            }

            reader.Read();
            var desc = reader["TableDescription"].ToString();
            if (string.IsNullOrEmpty(desc) == true)
            {
                var addExtendPropsSqlCmd = dbConnection.CreateCommand();
                addExtendPropsSqlCmd.CommandText =
                    $@"EXEC sp_addextendedproperty   
     @name = N'{descKey}',  
     @value = '{description}',  
     @level0type = N'Schema', @level0name = dbo,  
     @level1type = N'Table',  @level1name = {tableName}";
                addExtendPropsSqlCmd.ExecuteNonQuery();

            }
            else
            {
                if (desc != description)
                {
                    var updateExtendPropsCmd = dbConnection.CreateCommand();
                    updateExtendPropsCmd.CommandText =
                        $@"EXEC sp_updateextendedproperty   
     @name = N'{descKey}',  
     @value = '{description}',  
     @level0type = N'Schema', @level0name = dbo,  
     @level1type = N'Table',  @level1name = {tableName}";
                    updateExtendPropsCmd.ExecuteNonQuery();
                }
            }

        }
    }

}
