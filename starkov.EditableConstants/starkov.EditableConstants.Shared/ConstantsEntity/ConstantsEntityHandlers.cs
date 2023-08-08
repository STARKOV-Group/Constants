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

    public virtual void ValueLongByChanged(Sungero.Domain.Shared.LongIntegerPropertyChangedEventArgs e)
    {
      var strValue = string.Empty;
			
			if (_obj.ValueLongFrom.HasValue)
				strValue = ConstantsEntities.Resources.Range_WithFormat(_obj.ValueLongFrom.Value);
			
			if (e.NewValue.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = ConstantsEntities.Resources.Range_ByFormat(e.NewValue.Value);
				else
					strValue = ConstantsEntities.Resources.Range_BysFormat(strValue, e.NewValue.Value);
			}
			
			_obj.Value = ConstantsEntities.Resources.RangeFormat(strValue);
    }

    public virtual void ValueLongFromChanged(Sungero.Domain.Shared.LongIntegerPropertyChangedEventArgs e)
    {
      var strValue = string.Empty;
			
			if (e.NewValue.HasValue)
				strValue = ConstantsEntities.Resources.Range_WithFormat(e.NewValue.Value);
			
			if (_obj.ValueLongBy.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = ConstantsEntities.Resources.Range_ByFormat(_obj.ValueLongBy.Value);
				else
					strValue = ConstantsEntities.Resources.Range_BysFormat(strValue, _obj.ValueLongBy.Value);
			}
			
			_obj.Value = ConstantsEntities.Resources.RangeFormat(strValue);
    }

    public virtual void ValueLongChanged(Sungero.Domain.Shared.LongIntegerPropertyChangedEventArgs e)
    {
      if (e.NewValue.HasValue)
				_obj.Value = e.NewValue.Value.ToString();
			else
				_obj.Value = string.Empty;
    }

		public virtual void ValueChanged(Sungero.Domain.Shared.StringPropertyChangedEventArgs e)
		{
			if (e.NewValue.Length > 1000)
				_obj.Value = e.NewValue.Substring(0, 1000);
		}

		public virtual void ValueDateTimeChanged(Sungero.Domain.Shared.DateTimePropertyChangedEventArgs e)
		{
			if (e.NewValue.HasValue)
				_obj.Value = e.NewValue.Value.ToString("G");
			else
				_obj.Value = string.Empty;
		}

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
				strValue = ConstantsEntities.Resources.Range_WithFormat(_obj.ValueDoubleFrom.Value);
			
			if (e.NewValue.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = ConstantsEntities.Resources.Range_ByFormat(e.NewValue.Value);
				else
					strValue = ConstantsEntities.Resources.Range_BysFormat(strValue, e.NewValue.Value);
			}
			
			_obj.Value = ConstantsEntities.Resources.RangeFormat(strValue);
		}

		public virtual void ValueDoubleFromChanged(Sungero.Domain.Shared.DoublePropertyChangedEventArgs e)
		{
			var strValue = string.Empty;
			
			if (e.NewValue.HasValue)
				strValue = ConstantsEntities.Resources.Range_WithFormat(e.NewValue.Value);
			
			if (_obj.ValueDoubleBy.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = ConstantsEntities.Resources.Range_ByFormat(_obj.ValueDoubleBy.Value);
				else
					strValue = ConstantsEntities.Resources.Range_BysFormat(strValue, _obj.ValueDoubleBy.Value);
			}
			
			_obj.Value = ConstantsEntities.Resources.RangeFormat(strValue);
		}

		public virtual void ValueIntByChanged(Sungero.Domain.Shared.IntegerPropertyChangedEventArgs e)
		{
			var strValue = string.Empty;
			
			if (_obj.ValueIntFrom.HasValue)
				strValue = ConstantsEntities.Resources.Range_WithFormat(_obj.ValueIntFrom.Value);
			
			if (e.NewValue.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = ConstantsEntities.Resources.Range_ByFormat(e.NewValue.Value);
				else
					strValue = ConstantsEntities.Resources.Range_BysFormat(strValue, e.NewValue.Value);
			}
			
			_obj.Value = ConstantsEntities.Resources.RangeFormat(strValue);
		}

		public virtual void ValueIntFromChanged(Sungero.Domain.Shared.IntegerPropertyChangedEventArgs e)
		{
			var strValue = string.Empty;
			
			if (e.NewValue.HasValue)
				strValue = ConstantsEntities.Resources.Range_WithFormat(e.NewValue.Value);
			
			if (_obj.ValueIntBy.HasValue)
			{
				if (string.IsNullOrEmpty(strValue))
					strValue = ConstantsEntities.Resources.Range_ByFormat(_obj.ValueIntBy.Value);
				else
					strValue = ConstantsEntities.Resources.Range_BysFormat(strValue, _obj.ValueIntBy.Value);
			}
			
			_obj.Value = ConstantsEntities.Resources.RangeFormat(strValue);
		}

		public virtual void ValueBoolChanged(Sungero.Domain.Shared.BooleanPropertyChangedEventArgs e)
		{
			if (e.NewValue.HasValue && e.NewValue.Value)
				_obj.Value = ConstantsEntities.Resources.Yes;
			else
				_obj.Value = ConstantsEntities.Resources.No;
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