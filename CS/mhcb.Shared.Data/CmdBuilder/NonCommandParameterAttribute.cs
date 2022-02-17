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

namespace mhcb.cs.Shared.CmdBuilder
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class NonCommandParameterAttribute : Attribute
    {
    }
}
