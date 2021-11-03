using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using starkov.EditableConstants.ConstantsGroup;

namespace starkov.EditableConstants
{
	partial class ConstantsGroupClientHandlers
	{

		public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
		{
			if (Sungero.CoreEntities.Users.Current.Login.LoginName != "Integration Service")
				e.HideAction(_obj.Info.Actions.DeleteEntity);			
		}
	}
}