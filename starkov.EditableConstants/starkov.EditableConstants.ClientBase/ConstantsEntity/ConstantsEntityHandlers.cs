using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using starkov.EditableConstants.ConstantsEntity;

namespace starkov.EditableConstants
{
  partial class ConstantsEntityClientHandlers
  {

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      #region Типы констант
      
      // Тип константы - строка
      var typeString = starkov.EditableConstants.ConstantsEntity.TypeValue.ValString;
      // Тип константы - целое
      var typeInt = starkov.EditableConstants.ConstantsEntity.TypeValue.ValInt;
      // Тип константы - вещественное
      var typeDouble = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDouble;
      // Тип константы - логическое
      var typeBoolean = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBool;
      // Тип константы - текст
      var typeText = starkov.EditableConstants.ConstantsEntity.TypeValue.ValText;
      // Тип константы - дата/время
      var typeDateTime = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDateTime;
      // Тип константы - идентификатор
      var typeLong = starkov.EditableConstants.ConstantsEntity.TypeValue.ValLong;
      
      // Тип константы - список строк
      var typeListString = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListString;
      // Тип константы - список целых
      var typeListInt = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListInt;
      // Тип константы - список вещественных
      var typeListDouble = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListDouble;
      // Тип константы - список идентификаторов
      var typeListLong = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListLong;
      
      // Тип константы - диапазон целых
      var typeRangeInt = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeInt;
      // Тип константы - диапазон вещественных
      var typeRangeDouble = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeDouble;
      // Тип константы - диапазон идентификаторов
      var typeRangeLong = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeLong;
      
      // Тип константы - строка в Base64
      var typeBase64 = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBase64;
      
      #endregion
      
      var properties = _obj.State.Properties;
      
      #region Значения
      
      if (_obj.TypeValue == typeString)
      {
        properties.ValueString.IsVisible = true;
        properties.ValueString.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeInt)
      {
        properties.ValueInt.IsVisible = true;
        properties.ValueInt.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeDouble)
      {
        properties.ValueDouble.IsVisible = true;
        properties.ValueDouble.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeBoolean)
      {
        properties.ValueBool.IsVisible = true;
        properties.ValueBool.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeText)
      {
        properties.ValueText.IsVisible = true;
        properties.ValueText.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeDateTime)
      {
        properties.ValueDateTime.IsVisible = true;
        properties.ValueDateTime.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeLong)
      {
        properties.ValueLong.IsVisible = true;
        properties.ValueLong.IsRequired = true;
      }
      
      #endregion
      
      #region Диапазоны значений
      
      if (_obj.TypeValue == typeRangeInt)
      {
        properties.ValueIntFrom.IsVisible = true;
        properties.ValueIntFrom.IsRequired = true;
        
        properties.ValueIntBy.IsVisible = true;
        properties.ValueIntBy.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeRangeDouble)
      {
        properties.ValueDoubleFrom.IsVisible = true;
        properties.ValueDoubleFrom.IsRequired = true;
        
        properties.ValueDoubleBy.IsVisible = true;
        properties.ValueDoubleBy.IsRequired = true;
      }
      
      if (_obj.TypeValue == typeRangeLong)
      {
        properties.ValueLongFrom.IsVisible = true;
        properties.ValueLongFrom.IsRequired = true;
        
        properties.ValueLongBy.IsVisible = true;
        properties.ValueLongBy.IsRequired = true;
      }
      
      #endregion
      
      #region Списки значений
      
      var typeList = new List<Sungero.Core.Enumeration> {typeListString, typeListInt, typeListDouble, typeListLong};
      if (_obj.TypeValue != null && typeList.Contains(_obj.TypeValue.Value))
      {
        properties.ValueCollection.IsVisible = true;
        properties.ValueCollection.IsRequired = true;
        
        var collection = properties.ValueCollection.Properties;
        
        if (_obj.TypeValue == typeListString)
          collection.ValueString.IsVisible = true;
        
        if (_obj.TypeValue == typeListInt)
          collection.ValueInt.IsVisible = true;
        
        if (_obj.TypeValue == typeListDouble)
          collection.ValueDouble.IsVisible = true;
        
        if (_obj.TypeValue == typeListLong)
          collection.ValueLong.IsVisible = true;
      }
      
      #endregion
      
      #region Base64 значения
      
      if (_obj.TypeValue != typeBase64)
        e.HideAction(_obj.Info.Actions.SetValue);
      
      #endregion
      
      var isSystem = Sungero.CoreEntities.Users.Current.IsSystem.GetValueOrDefault();
      if (!_obj.AllowDelete.HasValue || (_obj.AllowDelete.HasValue && !_obj.AllowDelete.Value) || !isSystem)
        e.HideAction(_obj.Info.Actions.DeleteEntity);

      properties.AllowDelete.IsVisible = isSystem;
      properties.AllowDelete.IsEnabled = isSystem;
    }
  }

}