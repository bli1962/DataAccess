//===============================================================================
// NonCommandParameterAttribute.cs
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
using Debug = System.Diagnostics.Debug;

#region reference for enum AttributeTargets
//public enum AttributeTargets
//{
//	All=16383,
//	Assembly=1,
//	Module=2,
//	Class=4,
//	Struct=8,
//	Enum=16,
//	Constructor=32,
//	Method=64,
//	Property=128,
//	Field=256,
//	Event=512,
//	Interface=1024,
//	Parameter=2048,
//	Delegate=4096,
//	ReturnValue=8192
//}     
# endregion

namespace mhcb.cs.Shared.CmdBuilder
{
	// SqlParemeterAttribute 
	[AttributeUsage(AttributeTargets.Parameter)]
	public class SqlParameterAttribute : Attribute
	{
		private string _name;      							// parameter name
		private bool _paramTypeDefined;						// parameter type define?
		private SqlDbType _paramType;						// parameter type
		private int _size;									// parameter size
		private byte _precision;							// parameter precision
		private byte _scale;								// parameter scale
		private bool _directionDefined;						// parameter direction (in or out)
		private ParameterDirection _direction;				// parameter direction

		// default constructor  
		public SqlParameterAttribute()
		{
		}

		// constructor one
		public SqlParameterAttribute(string name)
		{
			Name = name;
		}

		// constructor two
		public SqlParameterAttribute(int size)
		{
			Size = size;
		}

		// constructor three
		public SqlParameterAttribute(SqlDbType paramType)
		{
			SqlDbType = paramType;
		}

		// constructor four
		public SqlParameterAttribute(string name, SqlDbType paramType)
		{
			Name = name;
			SqlDbType = paramType;
		}

		// constructor five
		public SqlParameterAttribute(SqlDbType paramType, int size)
		{
			SqlDbType = paramType;
			Size = size;
		}

		// constructor six
		public SqlParameterAttribute(string name, int size)
		{
			Name = name;
			Size = size;
		}

		// constructor seven
		public SqlParameterAttribute(string name, SqlDbType paramType, int size)
		{
			Name = name;
			SqlDbType = paramType;
			Size = size;
		}

		public string Name
		{
			get { return _name ?? string.Empty; }
			set { _name = value; }
		}

		public int Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public byte Precision
		{
			get { return _precision; }
			set { _precision = value; }
		}

		public byte Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}

		public ParameterDirection Direction
		{
			get
			{
				Debug.Assert(_directionDefined);
				return _direction;
			}
			set
			{
				_direction = value;
				_directionDefined = true;
			}
		}

		public SqlDbType SqlDbType
		{
			get
			{
				Debug.Assert(_paramTypeDefined);
				return _paramType;
			}
			set
			{
				_paramType = value;
				_paramTypeDefined = true;
			}
		}

		public bool IsNameDefined
		{
			get { return _name != null && _name.Length != 0; }
		}

		public bool IsSizeDefined
		{
			get { return _size != 0; }
		}

		public bool IsTypeDefined
		{
			get { return _paramTypeDefined; }
		}

		public bool IsDirectionDefined
		{
			get { return _directionDefined; }
		}

		public bool IsScaleDefined
		{
			get { return _scale != 0; }
		}

		public bool IsPrecisionDefined
		{
			get { return _precision != 0; }
		}
	}
}
