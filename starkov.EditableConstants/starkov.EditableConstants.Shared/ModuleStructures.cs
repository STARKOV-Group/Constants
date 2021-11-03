using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace starkov.EditableConstants.Structures.Module
{
	/// <summary>
	/// Диапазон целочисленных значений (From - С, By - По).
	/// </summary>
	[Sungero.Core.PublicAttribute]
	partial class RangeIntValues
	{
		/// <summary>
		/// С
		/// </summary>	
		public int? From { get; set; }
		
		/// <summary>
		/// По
		/// </summary>
		public int? By { get; set; }
	}
	
	/// <summary>
	/// Диапазон вещественных значений (From - С, By - По).
	/// </summary>
	[Sungero.Core.PublicAttribute]
	partial class RangeDoubleValues
	{
		/// <summary>
		/// С
		/// </summary>
		public double? From { get; set; }
		
		/// <summary>
		/// По
		/// </summary>
		public double? By { get; set; }
	}
}