using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using starkov.EditableConstants.ConstantsEntity;

namespace starkov.EditableConstants.Client
{
	partial class ConstantsEntityActions
	{
		public override void DeleteEntity(Sungero.Domain.Client.ExecuteActionArgs e)
		{
			var message = string.Format("ВНИМАНИЕ!\r\nУдаление константы \"{0}\" может привести к ошибкам в работе системы.\r\nВы уверены, что хотите выполнить удаление?", _obj.Name);
			var dialog = Dialogs.CreateTaskDialog("Удаление константы!", message, MessageType.Information, "Подтвердите удаление");
			dialog.Buttons.AddYesNo();
			if (dialog.Show() != DialogButtons.Yes)
				return;
			
			base.DeleteEntity(e);
		}

		public override bool CanDeleteEntity(Sungero.Domain.Client.CanExecuteActionArgs e)
		{
			return base.CanDeleteEntity(e);
		}

		public virtual void SetValue(Sungero.Domain.Client.ExecuteActionArgs e)
		{
			var dialog = Dialogs.CreateInputDialog("Изменение значения константы");
			var newValue = dialog.AddString("Новое значение", true);
			dialog.Buttons.AddOkCancel();
			
			if (dialog.Show() == DialogButtons.Ok)
			{
				_obj.ValueBase64 = Functions.Module.Remote.Base64Encode(newValue.Value);
				Dialogs.ShowMessage("Значение константы изменено.\r\nДля применения изменений сохраните запись.");
			}
		}

		public virtual bool CanSetValue(Sungero.Domain.Client.CanExecuteActionArgs e)
		{
			return true;
		}

	}

}