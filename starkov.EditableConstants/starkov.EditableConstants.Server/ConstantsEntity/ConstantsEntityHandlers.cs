using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using starkov.EditableConstants.ConstantsEntity;

namespace starkov.EditableConstants
{

	partial class ConstantsEntityServerHandlers
	{

		public override void Created(Sungero.Domain.CreatedEventArgs e)
		{
			_obj.AllowDelete = false;
		}

		public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
		{
			//TODO: Для списков делаем проверки тут, т.к. IsRequired не работает, почему-то
			
			#region Типы констант
			// Тип константы - список строк
			var typeListString = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListString;
			// Тип константы - список целых
			var typeListInt = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListInt;
			// Тип константы - список вещественных
			var typeListDouble = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListDouble;
			
			var typeList = new List<Sungero.Core.Enumeration> {typeListString, typeListInt, typeListDouble};
			#endregion
			
			if (typeList.Contains(_obj.TypeValue.Value))
			{
				if (!_obj.ValueCollection.Any())
					e.AddError("Список значений не может быть пустым !");
				
				var message = "Значения в списке не могут быть пустыми !";
				if (_obj.TypeValue.Value == typeListString && _obj.ValueCollection.Any(c => string.IsNullOrEmpty(c.ValueString) || string.IsNullOrWhiteSpace(c.ValueString)))
					e.AddError(message);
				
				if (_obj.TypeValue.Value == typeListInt && _obj.ValueCollection.Any(c => !c.ValueInt.HasValue))
					e.AddError(message);
				
				if (_obj.TypeValue.Value == typeListDouble && _obj.ValueCollection.Any(c => !c.ValueDouble.HasValue))
					e.AddError(message);
				
				if (_obj.TypeValue.Value == typeListString)
					_obj.Value = string.Format("Список: {0}", string.Join(", ", _obj.ValueCollection.Select(c => c.ValueString).Where(d => !string.IsNullOrEmpty(d) && !string.IsNullOrWhiteSpace(d))));
				
				if (_obj.TypeValue.Value == typeListInt)
					_obj.Value = string.Format("Список: {0}", string.Join(", ", _obj.ValueCollection.Select(c => c.ValueInt).Where(d => d != null)));
				
				if (_obj.TypeValue.Value == typeListDouble)
				{
					var stringValue = string.Join("*|*", _obj.ValueCollection.Select(c => c.ValueDouble).Where(d => d != null)).Replace(",", ".");
					_obj.Value = string.Format("Список: {0}", stringValue.Replace("*|*", ", "));
				}
			}
			else if (_obj.TypeValue.Value == starkov.EditableConstants.ConstantsEntity.TypeValue.ValBase64 && string.IsNullOrEmpty(_obj.Value))
				_obj.Value = "Строка в формате Base64";
		}

		public override void BeforeDelete(Sungero.Domain.BeforeDeleteEventArgs e)
		{
			if (!_obj.AllowDelete == true || Sungero.CoreEntities.Users.Current.Login.LoginName != "Integration Service")
			{
				e.AddError("Удаление константы запрещено !");
				return;
			}
			
			var message = string.Format("ВНИМАНИЕ:\r\nЗафиксировано удаление константы \"{0}\" пользователем Integration Service!\r\nДата и время удаления: {1}", _obj.Name, Calendar.Now);
			Functions.Module.SendNoticeAndCreateExeption("Зафиксировано удаление константы", message, false);
		}
	}
}