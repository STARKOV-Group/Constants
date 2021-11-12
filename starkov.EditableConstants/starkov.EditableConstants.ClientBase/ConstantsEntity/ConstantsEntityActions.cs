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
			var message = ConstantsEntities.Resources.DeleteDialog_MessageFormat(_obj.Name);
			var dialog = Dialogs.CreateTaskDialog(ConstantsEntities.Resources.DeleteDialog_Subject, message, MessageType.Information, ConstantsEntities.Resources.DeleteDialog_Confirm);
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
			var dialog = Dialogs.CreateInputDialog(ConstantsEntities.Resources.SetDialog_Subject);
			var newValue = dialog.AddString(ConstantsEntities.Resources.SetDialog_FieldName, true);
			dialog.Buttons.AddOkCancel();
			
			if (dialog.Show() == DialogButtons.Ok)
			{
				_obj.ValueBase64 = Functions.Module.Remote.Base64Encode(newValue.Value);
				Dialogs.ShowMessage(ConstantsEntities.Resources.SetDialog_Message);
			}
		}

		public virtual bool CanSetValue(Sungero.Domain.Client.CanExecuteActionArgs e)
		{
			return true;
		}

	}

}