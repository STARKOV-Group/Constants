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
      #region Типы констант
      // Тип константы - список строк
      var typeListString = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListString;
      // Тип константы - список целых
      var typeListInt = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListInt;
      // Тип константы - список вещественных
      var typeListDouble = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListDouble;
      // Тип константы - список идентификаторов
      var typeListLong = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListLong;
      
      var typeList = new List<Sungero.Core.Enumeration> {typeListString, typeListInt, typeListDouble, typeListLong};
      #endregion
      
      if (typeList.Contains(_obj.TypeValue.Value))
      {
        if (!_obj.ValueCollection.Any())
          e.AddError(ConstantsEntities.Resources.ListNullMessage);
        
        var message = ConstantsEntities.Resources.ValuesNullMessage;
        if (_obj.TypeValue.Value == typeListString && _obj.ValueCollection.Any(c => string.IsNullOrEmpty(c.ValueString) || string.IsNullOrWhiteSpace(c.ValueString)))
          e.AddError(message);
        
        if (_obj.TypeValue.Value == typeListInt && _obj.ValueCollection.Any(c => !c.ValueInt.HasValue))
          e.AddError(message);
        
        if (_obj.TypeValue.Value == typeListDouble && _obj.ValueCollection.Any(c => !c.ValueDouble.HasValue))
          e.AddError(message);
        
        if (_obj.TypeValue.Value == typeListLong && _obj.ValueCollection.Any(c => !c.ValueLong.HasValue))
          e.AddError(message);
        
        if (_obj.TypeValue.Value == typeListString)         
          _obj.Value = ConstantsEntities.Resources.TitleListFormat(string.Join(", ", _obj.ValueCollection.Select(c => c.ValueString).Where(d => !string.IsNullOrEmpty(d) && !string.IsNullOrWhiteSpace(d))));
        
        if (_obj.TypeValue.Value == typeListInt)
          _obj.Value = ConstantsEntities.Resources.TitleListFormat(string.Join(", ", _obj.ValueCollection.Select(c => c.ValueInt).Where(d => d != null)));
        
        if (_obj.TypeValue.Value == typeListLong)
          _obj.Value = ConstantsEntities.Resources.TitleListFormat(string.Join(", ", _obj.ValueCollection.Select(c => c.ValueLong).Where(d => d != null)));
        
        if (_obj.TypeValue.Value == typeListDouble)
        {
          var stringValue = string.Join("*|*", _obj.ValueCollection.Select(c => c.ValueDouble).Where(d => d != null)).Replace(",", ".");
          _obj.Value = ConstantsEntities.Resources.TitleListFormat(stringValue.Replace("*|*", ", "));
        }
      }
      else if (_obj.TypeValue.Value == starkov.EditableConstants.ConstantsEntity.TypeValue.ValBase64 && string.IsNullOrEmpty(_obj.Value))
        _obj.Value = ConstantsEntities.Resources.Base64Value;
    }

    public override void BeforeDelete(Sungero.Domain.BeforeDeleteEventArgs e)
    {
      var current = Sungero.CoreEntities.Users.Current;
      if (!_obj.AllowDelete.GetValueOrDefault() || !current.IsSystem.GetValueOrDefault())
      {
        e.AddError(ConstantsEntities.Resources.RemoveMessage);
        return;
      }
      
      var message = ConstantsEntities.Resources.DeleteMessageFormat(_obj.Name, current.Name, Calendar.Now);
      Functions.Module.SendNoticeAndCreateExeption(ConstantsEntities.Resources.DeleteSubject, message, false);
    }
  
  }
}