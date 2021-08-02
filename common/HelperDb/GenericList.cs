using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using System.Collections.Concurrent;

namespace Sigesoft.Common.HelperDb
{
   public class GenericList
   {
       ConcurrentDictionary<Type, Delegate> ExpressionCache =
                    new ConcurrentDictionary<Type, Delegate>();

       public List<T> CreateList<T>(SqlDataReader reader)
       {
            try
            {
                List<string> readerColumns = new List<string>();
                List<string> dtoColumns = new List<string>();

                // obtener los campos a leer desde la BD (stored procedure)
                for (int index = 0; index < reader.FieldCount; index++)              
                    readerColumns.Add(reader.GetName(index));
               
                // Obtener las propiedades de la entidad de transporte (DTO)
                foreach (PropertyInfo prop in typeof(T).GetProperties())               
                    dtoColumns.Add(prop.Name);             

                #region Validaciones

                // Obtener los campos que faltan crearse en el DTO           
                var results = readerColumns.FindAll(m => !dtoColumns.Contains(m));

                if (results != null && results.Count > 0)
                {               
                    var msgbox = string.Format("Los siguientes campos no existen en la entidad {0} :\n{1} necesitan ser agregados para ser mapeados.", typeof(T).Name, string.Join(", ", results.Select(p => p)));
                    throw new Exception(msgbox);
                }

                #endregion

                var list = new List<T>();
                Func<SqlDataReader, T> readRow = GetReader<T>(readerColumns);

                while (reader.Read())
                {
                    list.Add(readRow(reader));
                }

                return list;
           }
           catch (Exception ex)
           {
               throw ex;
           }
          
       }

       public Func<SqlDataReader, T> GetReader<T>(List<string> readerColumns)
       {
           Delegate resDelegate;

           try
           {
               if (!ExpressionCache.TryGetValue(typeof(T), out resDelegate))
               {
                   // Get the indexer property of SqlDataReader 
                   var indexerProperty = typeof(SqlDataReader).GetProperty("Item", new[] { typeof(string) });

                   // List of statements in our dynamic method 
                   var statements = new List<Expression>();
                   // Instance type of target entity class 
                   ParameterExpression instanceParam = Expression.Variable(typeof(T));
                   // Parameter for the SqlDataReader object
                   ParameterExpression readerParam =
                       Expression.Parameter(typeof(SqlDataReader));

                   // Create and assign new T to variable. Ex. var instance = new T(); 
                   BinaryExpression createInstance = Expression.Assign(instanceParam,
                       Expression.New(typeof(T)));
                   statements.Add(createInstance);

                   foreach (var prop in typeof(T).GetProperties())
                   {
                       // determine the default value of the property
                       object defaultValue = null;
                       if (prop.PropertyType.IsValueType)
                           defaultValue = Activator.CreateInstance(prop.PropertyType);
                       else if (prop.PropertyType.Name.ToLower().Equals("string"))
                           defaultValue = string.Empty;

                       if (readerColumns.Contains(prop.Name))
                       {
                           // instance.Property 
                           MemberExpression getProperty =
                           Expression.Property(instanceParam, prop);
                           // row[property] The assumption is, column names are the 
                           // same as PropertyInfo names of T 
                           IndexExpression readValue =
                               Expression.MakeIndex(readerParam, indexerProperty,
                               new[] { Expression.Constant(prop.Name) });

                           var testExp = Expression.NotEqual(readValue, Expression.Constant(DBNull.Value));
                           var ifTrue = Expression.Convert(readValue, prop.PropertyType);
                           var ifFalse = Expression.Convert(Expression.Constant(defaultValue), prop.PropertyType);

                           // instance.Property = row[property] 
                           BinaryExpression assignProperty = Expression.Assign(getProperty, Expression.Condition(testExp, ifTrue, ifFalse));

                           statements.Add(assignProperty);
                       }
                   }

                   var returnStatement = instanceParam;
                   statements.Add(returnStatement);

                   var body = Expression.Block(instanceParam.Type,
                       new[] { instanceParam }, statements.ToArray());

                   var lambda =
                   Expression.Lambda<Func<SqlDataReader, T>>(body, readerParam);
                   resDelegate = lambda.Compile();

                   // Cache the dynamic method into ExpressionCache dictionary
                   ExpressionCache[typeof(T)] = resDelegate;
               }

               return (Func<SqlDataReader, T>)resDelegate;
           }
           catch (Exception ex)
           {               
               throw ex;
           }

           
       }   

   }

}
