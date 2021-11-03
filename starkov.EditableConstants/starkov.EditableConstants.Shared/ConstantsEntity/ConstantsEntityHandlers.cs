using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using starkov.EditableConstants.ConstantsEntity;

namespace starkov.EditableConstants
{


	partial class ConstantsEntitySharedHandlers
	{

		public virtual void ValueTextChanged(Sungero.Domain.Shared.TextPropertyChangedEventArgs e)
		{
			var val = e.NewValue;
			
			if (val.Length > 1000)
				val = val.Substring(0, 1000);
			
			_obj.Value = val;
		}

		public virtual void ValueDoubleByChanged(Sungero.Domain.Shared.DoublePropertyChangedEventArgs e)
		{
			var strValue = string.Empty;
			
			if (_obj.ValueDoubleFrom.HasValue)
				strValue = string.Format("с {0}", _obj.ValueDoubleFrom.Value);
			
			if (e.NewValue.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = string.Format("по {0}", e.NewValue.Value);
				else
					strValue = string.Format("{0} по {1}", strValue, e.NewValue.Value);
			}
			
			_obj.Value = string.Format("Диапазон: {0}", strValue);
		}

		public virtual void ValueDoubleFromChanged(Sungero.Domain.Shared.DoublePropertyChangedEventArgs e)
		{
			var strValue = string.Empty;
			
			if (e.NewValue.HasValue)
				strValue = string.Format("с {0}", e.NewValue.Value);
			
			if (_obj.ValueDoubleBy.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = string.Format("по {0}", _obj.ValueDoubleBy.Value);
				else
					strValue = string.Format("{0} по {1}", strValue, _obj.ValueDoubleBy.Value);
			}
			
			_obj.Value = string.Format("Диапазон: {0}", strValue);
		}

		public virtual void ValueIntByChanged(Sungero.Domain.Shared.IntegerPropertyChangedEventArgs e)
		{
			var strValue = string.Empty;
			
			if (_obj.ValueIntFrom.HasValue)
				strValue = string.Format("с {0}", _obj.ValueIntFrom.Value);
			
			if (e.NewValue.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = string.Format("по {0}", e.NewValue.Value);
				else
					strValue = string.Format("{0} по {1}", strValue, e.NewValue.Value);
			}
			
			_obj.Value = string.Format("Диапазон: {0}", strValue);
		}

		public virtual void ValueIntFromChanged(Sungero.Domain.Shared.IntegerPropertyChangedEventArgs e)
		{
			var strValue = string.Empty;
			
			if (e.NewValue.HasValue)
				strValue = string.Format("с {0}", e.NewValue.Value);
			
			if (_obj.ValueIntBy.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = string.Format("по {0}", _obj.ValueIntBy.Value);
				else
					strValue = string.Format("{0} по {1}", strValue, _obj.ValueIntBy.Value);
			}
			
			_obj.Value = string.Format("Диапазон: {0}", strValue);
		}

		public virtual void ValueBoolChanged(Sungero.Domain.Shared.BooleanPropertyChangedEventArgs e)
		{
			if (e.NewValue.HasValue && e.NewValue.Value)
				_obj.Value = "Да";
			else
				_obj.Value = "Нет";
		}

		public virtual void ValueStringChanged(Sungero.Domain.Shared.StringPropertyChangedEventArgs e)
		{
			_obj.Value = e.NewValue;
		}

		public virtual void ValueDoubleChanged(Sungero.Domain.Shared.DoublePropertyChangedEventArgs e)
		{
			if (e.NewValue.HasValue)
				_obj.Value = e.NewValue.Value.ToString();
			else
				_obj.Value = string.Empty;
		}

		public virtual void ValueIntChanged(Sungero.Domain.Shared.IntegerPropertyChangedEventArgs e)
		{
			if (e.NewValue.HasValue)
				_obj.Value = e.NewValue.Value.ToString();
			else
				_obj.Value = string.Empty;
		}

	}
}