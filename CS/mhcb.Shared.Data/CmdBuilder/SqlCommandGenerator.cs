//===============================================================================
// SqlCommandGenerator.cs
//
// Object:	this is designed to build SqlCommand object with .NET Reflection
//          and Attributes. There is only one public interface 'GenerateCommand'
//          which will return a sqlcommand object.
//
// Author: Ben Li
// Date Created:  12/05/2005
// Last Modified: 12/05/2005 
// 
//===============================================================================
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//==============================================================================

using System;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using Debug = System.Diagnostics.Debug;
using StackTrace = System.Diagnostics.StackTrace;

namespace mhcb.cs.Shared.CmdBuilder
{
    public sealed class SqlCommandGenerator
    {
        // The private structure, does not allow to use 
        // the non-parameter a structure example
        private SqlCommandGenerator()
        {
            throw new NotSupportedException();
        }

        //The static read-only field, the definition uses in to return to the value the parameter name
        public static readonly string ReturnValueParameterName = "RETURN_VALUE";


        //The static read-only field, uses in not to bring the parameter the memory process
        public static readonly object[] NoValues = new object[] { };



        public static SqlCommand GenerateCommand(SqlConnection oConnection, MethodInfo oMethod, object[] values)
        {
            //read from StackTrack()if no oMethod
            if (oMethod == null)
                oMethod = (MethodInfo)(new StackTrace().GetFrame(1).GetMethod());

            // get command attributes via commandAttrbute
            SqlCommandMethodAttribute commandAttribute =
                (SqlCommandMethodAttribute)Attribute.GetCustomAttribute(oMethod, typeof(SqlCommandMethodAttribute));

            Debug.Assert(commandAttribute != null);
            Debug.Assert(commandAttribute.CommandType == CommandType.StoredProcedure ||
                                     commandAttribute.CommandType == CommandType.Text);

            SqlCommand oCommand = new SqlCommand();
            oCommand.Connection = oConnection;
            oCommand.CommandType = commandAttribute.CommandType;

            // if CommandText is provided, use it as CommandText.
            // otherwise use the method name as CommandText
            if (commandAttribute.CommandText.Length == 0)
            {
                Debug.Assert(commandAttribute.CommandType == CommandType.StoredProcedure);
                oCommand.CommandText = oMethod.Name;
            }
            else
            {
                oCommand.CommandText = commandAttribute.CommandText;
            }

            //Prepare Command Parameter collection
            GenerateCommandParameters(oCommand, oMethod, values);

            //Append another parameter for return value
            oCommand.Parameters.Add(ReturnValueParameterName, SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return oCommand;
        }


        private static void GenerateCommandParameters(SqlCommand oCommand, MethodInfo oMethod, object[] values)
        {
            // get parameter attributes array via parameter attribute of of MethodInfo
            ParameterInfo[] methodParameters = oMethod.GetParameters();
            int paramIndex = 0;

            // ** add parameter to method by loop
            foreach (ParameterInfo oParamInfo in methodParameters)
            {
                // Neglects the parameter marks for [ NonCommandParameter ] the parameter
                if (Attribute.IsDefined(oParamInfo, typeof(NonCommandParameterAttribute)))
                    continue;

                // Gain parameter SqlParameter attribute, if has not assigned, 
                // then founds and uses it to lack the province establishment.
                SqlParameterAttribute oParamAttribute =
                    (SqlParameterAttribute)Attribute.GetCustomAttribute(oParamInfo, typeof(SqlParameterAttribute));

                if (oParamAttribute == null)
                    oParamAttribute = new SqlParameterAttribute();

                SqlParameter sqlParameter = new SqlParameter();

                if (oParamAttribute.IsNameDefined)
                    sqlParameter.ParameterName = oParamAttribute.Name;
                else
                    sqlParameter.ParameterName = oParamInfo.Name;

                if (!sqlParameter.ParameterName.StartsWith("@"))
                    sqlParameter.ParameterName = "@" + sqlParameter.ParameterName;

                if (oParamAttribute.IsTypeDefined)
                    sqlParameter.SqlDbType = oParamAttribute.SqlDbType;

                if (oParamAttribute.IsSizeDefined)
                    sqlParameter.Size = oParamAttribute.Size;

                if (oParamAttribute.IsScaleDefined)
                    sqlParameter.Scale = oParamAttribute.Scale;


                // please be careful the following codes
                if (oParamAttribute.IsPrecisionDefined)
                    sqlParameter.Precision = oParamAttribute.Precision;

                if (oParamAttribute.IsDirectionDefined)
                {
                    sqlParameter.Direction = oParamAttribute.Direction;
                }
                else
                {
                    if (oParamInfo.ParameterType.IsByRef)
                    {

                        //sqlParameter.Direction = oParamInfo.IsOut ? ParameterDirection.Output : ParameterDirection.InputOutput;

                        //  Modified by Ben Li 18/04/2007
                        //  You need to add the following three declare in your Stored procedure if you are going to control 
                        //  some of return values from it.
                        //		@AffectedID int = 0 OUTPUT,
                        //		@RowAffected int = 0 OUTPUT,
                        //		@ValidCode int = 0 OUTPUT,
                        if ((oParamInfo.IsOut))
                        {
                            sqlParameter.Direction = ParameterDirection.Output;
                        }
                        else
                        {
                            if (sqlParameter.ParameterName == "@AffectedID")
                            {
                                sqlParameter.Direction = ParameterDirection.Output;
                            }
                            else if (sqlParameter.ParameterName == "@RowAffected")
                            {
                                sqlParameter.Direction = ParameterDirection.Output;
                            }
                            else if (sqlParameter.ParameterName == "@ValidCode")
                            {
                                sqlParameter.Direction = ParameterDirection.Output;
                            }
                            else
                            {
                                sqlParameter.Direction = ParameterDirection.InputOutput;
                            }
                        }
                    }
                    else
                    {
                        sqlParameter.Direction = ParameterDirection.Input;
                    }
                }

                // Examines whether provides enough parameter object value
                Debug.Assert(paramIndex < values.Length);

                //Bestows on the corresponding object value the parameter.
                sqlParameter.Value = values[paramIndex];
                oCommand.Parameters.Add(sqlParameter);

                paramIndex++;
            }

            //Examines whether has the unnecessary parameter object value
            Debug.Assert(paramIndex == values.Length);
        }
    }
}

