using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace starkov.EditableConstants.Server
{
  public partial class ModuleInitializer
  {
    
    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      InitializationLogger.Debug("Init: Старт инициализации модуля \"Константы\".");
      
      SetGrantRights();
      TransferToParams();
    }
    
    #region Перенос Констант в Параметры
    /// <summary>
    /// Перенос существующих Констант на Параметры.
    /// </summary>
    public static void TransferToParams()
    {
      const string groupsDescription = "26.2: Перенесено в Группу параметров системы";
      const string paramsDescription = "26.2: Перенесено в Параметр системы";
      
      var loggerTemplate = $"Init.EditableConstants: {{0}}";
      
      var constants = ConstantsEntities.GetAll(_ => _.Status == starkov.EditableConstants.ConstantsEntity.Status.Active
                                               && (_.Note == null || _.Note != paramsDescription));
      if (!constants.Any())
      {
        InitializationLogger.DebugFormat(loggerTemplate, "Отсутствуют Константы для переноса в Параметры");
        return;
      }
      
      var currentConstantGroups = ConstantsGroups.GetAll(_ => _.Status == starkov.EditableConstants.ConstantsGroup.Status.Active);
      if (!currentConstantGroups.Any())
        currentConstantGroups = constants.Select(_ => _.Group).Distinct();
      
      foreach (var constantGroup in currentConstantGroups)
      {
        var paramGroup = Sungero.Commons.ParametersGroups.Null;
        if (constantGroup != null)
        {
          if (!Locks.TryLock(constantGroup))
          {
            InitializationLogger.DebugFormat(loggerTemplate, $"Группа Констант с ИД {constantGroup.Id} заблокирована, обработка пропущена");
            continue;
          }
          
          var note = constantGroup.Note;
          paramGroup = GetOrCreateParamGroup(constantGroup, note);
        }
        
        foreach (var constant in constants.Where(_ => Equals(_.Group, constantGroup)))
        {
          try
          {
            var param = GetOrCreateParam(constant, paramGroup);
            
            constant.AllowDelete = true;
            constant.Note = paramsDescription;
            constant.Status = starkov.EditableConstants.ConstantsEntity.Status.Closed;
            UnsetEntityRequiredProperties(constant);
            constant.Save();
            
            InitializationLogger.DebugFormat(loggerTemplate, $"Константа с ИД {constant.Id} закрыта");
          }
          catch (Exception ex)
          {
            Logger.ErrorFormat(loggerTemplate, ex, $"Не удалось сохранить Константу с ИД {constant.Id}");
          }
        }
        
        if (constantGroup != null)
        {
          try
          {
            constantGroup.Note = groupsDescription;
            constantGroup.Status = starkov.EditableConstants.ConstantsGroup.Status.Closed;
            UnsetEntityRequiredProperties(constantGroup);
            constantGroup.Save();
            
            InitializationLogger.DebugFormat(loggerTemplate, $"Группа Констант с ИД {constantGroup.Id} закрыта");
          }
          catch (Exception ex)
          {
            Logger.ErrorFormat(loggerTemplate, ex, $"Не удалось сохранить Группу Констант с ИД {constantGroup.Id}");
          }
        }
      }
    }
    
    /// <summary>
    /// Создать или найти параметр.
    /// </summary>
    /// <param name="constantsGroup"></param>
    /// <returns></returns>
    private static Sungero.Commons.IParameter GetOrCreateParam(IConstantsEntity constantEntity,
                                                               Sungero.Commons.IParametersGroup parametersGroup)
    {
      Enumeration? valueType = constantEntity.TypeValue == EditableConstants.ConstantsEntity.TypeValue.ValBool ? Sungero.Commons.Parameter.ValueType.BooleanValue :
        constantEntity.TypeValue == EditableConstants.ConstantsEntity.TypeValue.ValDateTime                    ? Sungero.Commons.Parameter.ValueType.DateValue    :
        constantEntity.TypeValue == EditableConstants.ConstantsEntity.TypeValue.ValDouble                      ? Sungero.Commons.Parameter.ValueType.DoubleValue  :
        constantEntity.TypeValue == EditableConstants.ConstantsEntity.TypeValue.ValInt                         ? Sungero.Commons.Parameter.ValueType.IntegerValue :
        
        constantEntity.TypeValue == EditableConstants.ConstantsEntity.TypeValue.ValString                      ? Sungero.Commons.Parameter.ValueType.StringValue  :
        constantEntity.TypeValue == EditableConstants.ConstantsEntity.TypeValue.ValText                        ? Sungero.Commons.Parameter.ValueType.StringValue  :
        
        null;
      
      if (valueType == null)
        throw AppliedCodeException.Create("Константа ИД {constantEntity.Id} пропущена, т.к. тип значения '{constantEntity.TypeValue}' не поддерживается.");
      
      var param = Sungero.Commons.Parameters.GetAll(_ => _.Code == constantEntity.Name).FirstOrDefault();
      if (param != null && param.ValueType != valueType)
        throw AppliedCodeException.Create("Константа ИД {constantEntity.Id} пропущена, т.к. найден Параметр ИД {param.Id} с другим типом значения '{constantEntity.TypeValue}'.");
      
      if (param == null)
      {
        param = Sungero.Commons.Parameters.Create();
        param.Name = constantEntity.Name;
        param.Code = constantEntity.Name;
        param.Note = constantEntity.Note;
        param.ValueType = valueType;
        param.Group = parametersGroup;
        
        if (param.ValueType == Sungero.Commons.Parameter.ValueType.BooleanValue)
          param.BooleanValue = constantEntity.ValueBool;
        
        else if (param.ValueType == Sungero.Commons.Parameter.ValueType.DateValue)
          param.DateValue = constantEntity.ValueDateTime;
        
        else if (param.ValueType == Sungero.Commons.Parameter.ValueType.DoubleValue)
          param.DoubleValue = constantEntity.ValueDouble;
        
        else if (param.ValueType == Sungero.Commons.Parameter.ValueType.IntegerValue)
          param.IntegerValue = constantEntity.ValueInt;
        
        else if (param.ValueType == Sungero.Commons.Parameter.ValueType.StringValue)
          param.StringValue = constantEntity.TypeValue == EditableConstants.ConstantsEntity.TypeValue.ValString
            ? constantEntity.ValueString
            : constantEntity.ValueText;
        
        UnsetEntityRequiredProperties(param);
        param.Save();
        
        InitializationLogger.Debug($"Init.EditableConstants: Создан Параметр ИД {param.Id} из Константы (ИД {constantEntity.Id})");
      }
      
      return param;
    }
    
    /// <summary>
    /// Создать или найти группу параметров.
    /// </summary>
    /// <param name="constantsGroup"></param>
    /// <returns></returns>
    private static Sungero.Commons.IParametersGroup GetOrCreateParamGroup(IConstantsGroup constantsGroup, string note)
    {
      var paramGroup = Sungero.Commons.ParametersGroups.GetAll(_ => _.Name == constantsGroup.Name).FirstOrDefault();
      if (paramGroup == null)
      {
        paramGroup = Sungero.Commons.ParametersGroups.Create();
        paramGroup.Name = constantsGroup.Name;
        paramGroup.Note = note;
        
        UnsetEntityRequiredProperties(paramGroup);
        paramGroup.Save();
        
        InitializationLogger.Debug($"Init.EditableConstants: Создана Группа Параметров ИД {paramGroup.Id} из Группы Констант (ИД {constantsGroup.Id})");
      }
      
      return paramGroup;
    }
    
    /// <summary>
    /// Снять обязательность свойств.
    /// </summary>
    /// <param name="entity"></param>
    private static void UnsetEntityRequiredProperties(Sungero.Domain.Shared.IEntity entity)
    {
      if (entity == null)
        return;
      
      foreach (var p in entity.State.Properties)
        p.IsRequired = false;
    }
    #endregion
    
    #region Создание записей справочника "Константы"
    
    #region Выдача прав на объекты
    
    /// <summary>
    /// Выдать права пользователям
    /// </summary>
    public static void SetGrantRights()
    {
      // Выдача прав на просмотр всем пользователям.
      GrantRightsOnConstants(Roles.AllUsers, DefaultAccessRightsTypes.Read);
      InitializationLogger.Debug("Init: Для справочника \"Константы\" выданы права на просмотр всем пользователям.");
    }
    
    /// <summary>
    /// Выдать права пользователям на справочник.
    /// </summary>
    /// <param name="users">Группа пользователей.</param>
    /// <param name="accessRights">Тип прав.</param>
    [Public]
    public static void GrantRightsOnConstants(IRole users, Guid accessRights)
    {
      if (!EditableConstants.ConstantsEntities.AccessRights.IsGranted(accessRights, users))
      {
        EditableConstants.ConstantsEntities.AccessRights.Grant(users, accessRights);
        EditableConstants.ConstantsEntities.AccessRights.Save();
      }
    }
    
    #endregion
    
    
    #region Создание групп констант
    
    /// <summary>
    /// Создание записи справочника группы констант.
    /// </summary>
    /// <param name="name">Имя группы.</param>
    /// <param name="note">Примечание константы.</param>
    [Public]
    public static void CreateGroup(string name, string note)
    {
      if (!EditableConstants.ConstantsGroups.GetAll().Any(c => c.Name == name))
      {
        InitializationLogger.DebugFormat("Init: Создаем группу констант {0} в справочнике \"Группы констант\".", name);
        var entity = EditableConstants.ConstantsGroups.Create();
        
        entity.Name = name;
        
        if (!string.IsNullOrEmpty(note))
          entity.Note = note;

        entity.Save();
      }
    }
    
    #endregion
    
    
    #region Создание констант
    
    /// <summary>
    /// Создание записи справочника "Константы" (без значения/значений)
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="typeValue">Тип константы</param>
    /// <param name="note">Примечание.</param>
    /// <param name="groupName">Имя группы.</param>
    /// <returns>Константа без значения.</returns>
    [Public]
    public static starkov.EditableConstants.IConstantsEntity GetNewConstant(string name, Sungero.Core.Enumeration typeValue, string note, string groupName)
    {
      var constant = EditableConstants.ConstantsEntities.GetAll().Where(c => c.Name == name && c.TypeValue == typeValue).FirstOrDefault();
      
      if (constant == null)
      {
        InitializationLogger.DebugFormat("Init: Создаем константу {0} в справочнике \"Константы\".", name);
        constant = EditableConstants.ConstantsEntities.Create();
        
        constant.TypeValue = typeValue;
        constant.Name = name;

        if (!string.IsNullOrEmpty(note))
          constant.Note = note;
        
        if (!string.IsNullOrEmpty(groupName))
        {
          var group = EditableConstants.ConstantsGroups.GetAll().Where(c => c.Name == groupName).FirstOrDefault();
          if (group != null)
            constant.Group = group;
        }
        
        return constant;
      }
      else
      {
        if (!string.IsNullOrEmpty(groupName) && constant.Group == null)
        {
          var group = EditableConstants.ConstantsGroups.GetAll().Where(c => c.Name == groupName).FirstOrDefault();
          if (group != null)
          {
            InitializationLogger.DebugFormat("Init: Обновляем группу в константе {0} в справочнике \"Константы\".", name);
            constant.Group = group;
            constant.Save();
          }
        }
      }
      
      return null;
    }
    
    #region Простые типы
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="typeValue">Тип константы</param>
    /// <param name="note">Примечание.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, string constantValue, string note, string groupName)
    {
      // Тип константы - строка
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValString;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        if (constantValue.Length > 500)
          constantValue = constantValue.Substring(0, 500);
        
        constant.ValueString = constantValue;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="typeValue">Тип константы</param>
    /// <param name="isText">Текстовая константа</param>
    /// <param name="note">Примечание.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, string constantValue, bool isText, string note, string groupName)
    {
      if (!isText && constantValue.Length < 501)
      {
        CreateConstants(name, constantValue, note, groupName);
        return;
      }
      
      // Тип константы - строка
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValText;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueText = constantValue;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, int constantValue, string note, string groupName)
    {
      // Тип константы - целое
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValInt;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueInt = constantValue;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, double constantValue, string note, string groupName)
    {
      // Тип константы - вещественное
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDouble;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueDouble = constantValue;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, bool constantValue, string note, string groupName)
    {
      // Тип константы - логическое
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBool;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueBool = constantValue;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, DateTime constantValue, string note, string groupName)
    {
      // Тип константы - Дата/время
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDateTime;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueDateTime = constantValue;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, long constantValue, string note, string groupName)
    {
      // Тип константы - идентификатор
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValLong;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueLong = constantValue;
        constant.Save();
      }
    }
    
    #endregion
    
    #region Диапазоны простых типов
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValueFrom">Значение константы (Значение С).</param>
    /// <param name="constantValueFrom">Значение константы (Значение ПО).</param>
    /// <param name="typeValue">Тип константы</param>
    /// <param name="note">Примечание.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, int constantValueFrom, int constantValueBy, string note, string groupName)
    {
      // Тип константы - диапазон целых
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeInt;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueIntFrom = constantValueFrom;
        constant.ValueIntBy = constantValueBy;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValueFrom">Значение константы (Значение С).</param>
    /// <param name="constantValueFrom">Значение константы (Значение ПО).</param>
    /// <param name="typeValue">Тип константы</param>
    /// <param name="note">Примечание.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, double constantValueFrom, double constantValueBy, string note, string groupName)
    {
      // Тип константы - диапазон вещественных
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeDouble;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueDoubleFrom = constantValueFrom;
        constant.ValueDoubleBy = constantValueBy;
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValueFrom">Значение константы (Значение С).</param>
    /// <param name="constantValueFrom">Значение константы (Значение ПО).</param>
    /// <param name="typeValue">Тип константы</param>
    /// <param name="note">Примечание.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, long constantValueFrom, long constantValueBy, string note, string groupName)
    {
      // Тип константы - диапазон вещественных
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeLong;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueLongFrom = constantValueFrom;
        constant.ValueLongBy = constantValueBy;
        constant.Save();
      }
    }
    
    #endregion
    
    #region Списки простых типов
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, List<string> constantValuesList, string note, string groupName)
    {
      // Тип константы - список строк
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListString;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        foreach (var constantValue in constantValuesList)
        {
          var newLine = constant.ValueCollection.AddNew();
          newLine.ValueString = constantValue;
          
        }
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, List<int> constantValuesList, string note, string groupName)
    {
      // Тип константы - список целых
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListInt;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        foreach (var constantValue in constantValuesList)
        {
          var newLine = constant.ValueCollection.AddNew();
          newLine.ValueInt = constantValue;
          
        }
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, List<double> constantValuesList, string note, string groupName)
    {
      // Тип константы - список вещественных
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListDouble;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        foreach (var constantValue in constantValuesList)
        {
          var newLine = constant.ValueCollection.AddNew();
          newLine.ValueDouble = constantValue;
          
        }
        constant.Save();
      }
    }
    
    /// <summary>
    /// Создание записи справочника Константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="note">Примечание константы.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateConstants(string name, List<long> constantValuesList, string note, string groupName)
    {
      // Тип константы - список вещественных
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListLong;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        foreach (var constantValue in constantValuesList)
        {
          var newLine = constant.ValueCollection.AddNew();
          newLine.ValueLong = constantValue;
          
        }
        constant.Save();
      }
    }
    
    #endregion
    
    #region Base64 значения
    
    /// <summary>
    /// Создание записи справочника Константы с переобразованием значения в Base64.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constantValue">Значение константы.</param>
    /// <param name="typeValue">Тип константы</param>
    /// <param name="note">Примечание.</param>
    /// <param name="name">Имя группы.</param>
    [Public]
    public static void CreateBase64Constants(string name, string constantValue, string note, string groupName)
    {
      // Тип константы - строка в Base64
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBase64;
      
      var constant = GetNewConstant(name, typeValue, note, groupName);
      if (constant != null)
      {
        constant.ValueBase64 = Functions.Module.Base64Encode(constantValue);
        constant.Save();
      }
    }
    
    #endregion
    
    #endregion
    
    #endregion
    
  }
}
