//===============================================================================
// CommandMethodAttribute.cs
//
// Object:	n/a
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
using System.Data;

namespace mhcb.cs.Shared.CmdBuilder
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SqlCommandMethodAttribute : Attribute
    {
        private string _commandText;
        private CommandType _commandType;

        public SqlCommandMethodAttribute(CommandType commandType, string commandText)
        {
            CommandType = commandType;
            CommandText = commandText;
        }

        public SqlCommandMethodAttribute(CommandType commandType) : this(commandType, null)
        {
        }

        public string CommandText
        {
            get { return _commandText ?? string.Empty; }
            set { _commandText = value; }
        }

        public CommandType CommandType
        {
            get { return _commandType; }
            set { _commandType = value; }
        }
    }
}
