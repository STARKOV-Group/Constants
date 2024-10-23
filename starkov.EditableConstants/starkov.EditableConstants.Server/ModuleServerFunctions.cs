using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace starkov.EditableConstants.Server
{
  public class ModuleFunctions
  {

    #region	Работа с константами
    
    #region Работа со значениями констант
    
    /// <summary>
    /// Получить константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="typeValue">Тип константы.</param>
    /// <returns>Константа. Если не найдена, то null.</returns>
    public virtual starkov.EditableConstants.IConstantsEntity GetConstant(string name, Sungero.Core.Enumeration typeValue)
    {
      return this.GetConstant(name, typeValue, true);
    }
    
    /// <summary>
    /// Получить константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="typeValue">Тип константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Константа. Если не найдена, то null.</returns>
    public virtual starkov.EditableConstants.IConstantsEntity GetConstant(string name, Sungero.Core.Enumeration typeValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      
      if (!string.IsNullOrEmpty(name))
      {
        var constantEntity = starkov.EditableConstants.ConstantsEntities.GetAll().Where(c => c.Name.Trim() == name.Trim() && c.TypeValue == typeValue).FirstOrDefault();
        if (constantEntity != null)
          return 	constantEntity;
        else
        {
          string textError = Resources.ConstantNotFindFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      else
      {
        string textError = Resources.ConstantNotName;
        SendNoticeAndCreateExeption(subjectError, textError, genException);
      }
      
      return null;
    }
    
    
    #region Строковые значения
    
    /// <summary>
    /// Получить строковое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
    [Remote, Public]
    public virtual string GetValueStringByName(string name)
    {
      return this.GetValueStringByName(name, true);
    }
    
    /// <summary>
    /// Получить строковое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
    [Remote, Public]
    public virtual string GetValueStringByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValString;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (string.IsNullOrEmpty(constantEntity.ValueString) || string.IsNullOrWhiteSpace(constantEntity.ValueString))
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return constantEntity.ValueString;
      }
      return string.Empty;
    }
    
    /// <summary>
    /// Установить значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueStringByName(string name, string constValue)
    {
      return this.SetValueStringByName(name, constValue, true);
    }
    
    /// <summary>
    /// Установить значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueStringByName(string name, string constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValString;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueString = constValue;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Целые значения
    
    /// <summary>
    /// Получить целое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual int? GetValueIntByName(string name)
    {
      return this.GetValueIntByName(name, true);
    }
    
    /// <summary>
    /// Получить целое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual int? GetValueIntByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValInt;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueInt.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return constantEntity.ValueInt;
      }
      
      return null;
    }
    
    /// <summary>
    /// Установить целое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueIntByName(string name, int constValue)
    {
      return this.SetValueIntByName(name, constValue, true);
    }
    
    /// <summary>
    /// Установить целое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueIntByName(string name, int constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValInt;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueInt = constValue;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Идентификаторы
    
    /// <summary>
    /// Получить значение константы - идентификатор.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual long? GetValueLongByName(string name)
    {
      return this.GetValueLongByName(name, true);
    }
    
    /// <summary>
    /// Получить значение константы - идентификатор.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual long? GetValueLongByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValLong;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueLong.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return constantEntity.ValueLong;
      }
      
      return null;
    }
    
    /// <summary>
    /// Установить значение константы - идентификатор.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueLongByName(string name, long constValue)
    {
      return this.SetValueLongByName(name, constValue, true);
    }
    
    /// <summary>
    /// Установить значение константы - идентификатор.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueLongByName(string name, long constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValLong;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueLong = constValue;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Вещественные значения
    /// <summary>
    /// Получить вещественное значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual double? GetValueDoubleByName(string name)
    {
      return this.GetValueDoubleByName(name, true);
    }
    
    /// <summary>
    /// Получить вещественное значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual double? GetValueDoubleByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDouble;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueDouble.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return constantEntity.ValueDouble;
      }
      
      return null;
    }
    
    /// <summary>
    /// Установить вещественное значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueDoubleByName(string name, double constValue)
    {
      return this.SetValueDoubleByName(name, constValue, true);
    }
    
    /// <summary>
    /// Установить вещественное значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueDoubleByName(string name, double constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDouble;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueDouble = constValue;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Логические значения
    
    /// <summary>
    /// Получить логическое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual bool? GetValueBooleanByName(string name)
    {
      return this.GetValueBooleanByName(name, true);
    }
    
    /// <summary>
    /// Получить логическое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual bool? GetValueBooleanByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBool;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueBool.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return constantEntity.ValueBool;
      }
      
      return null;
    }
    
    /// <summary>
    /// Установить логическое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueBooleanByName(string name, bool constValue)
    {
      return this.SetValueBooleanByName(name, constValue, true);
    }
    
    
    /// <summary>
    /// Установить логическое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueBooleanByName(string name, bool constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBool;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueBool = constValue;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Текстовые значения
    
    /// <summary>
    /// Получить текстовое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
    [Remote, Public]
    public virtual string GetValueTextByName(string name)
    {
      return this.GetValueTextByName(name, true);
    }
    
    /// <summary>
    /// Получить текстовое значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
    [Remote, Public]
    public virtual string GetValueTextByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValText;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (string.IsNullOrEmpty(constantEntity.ValueText) || string.IsNullOrWhiteSpace(constantEntity.ValueText))
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return constantEntity.ValueText;
      }
      return string.Empty;
    }
    
    /// <summary>
    /// Установить значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueTextByName(string name, string constValue)
    {
      return this.SetValueStringByName(name, constValue, true);
    }
    
    /// <summary>
    /// Установить значение константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueTextByName(string name, string constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValText;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueText = constValue;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Даты
    
    /// <summary>
    /// Получить дату из константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual DateTime? GetValueDateTimeByName(string name)
    {
      return this.GetValueDateTimeByName(name, true);
    }
    
    /// <summary>
    /// Получить дату из константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual DateTime? GetValueDateTimeByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDateTime;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueDateTime.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return constantEntity.ValueDateTime;
      }
      
      return null;
    }
    
    /// <summary>
    /// Установить дату в константе.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueDateTimeByName(string name, DateTime constValue)
    {
      return this.SetValueDateTimeByName(name, constValue, true);
    }
    
    /// <summary>
    /// Установить дату в константе.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueDateTimeByName(string name, DateTime constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValDateTime;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueDateTime = constValue;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    
    #region Список строковых значений
    
    /// <summary>
    /// Получить список строковых значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</returns>
    [Remote, Public]
    public virtual List<string> GetValueListStringByName(string name)
    {
      return this.GetValueListStringByName(name, true);
    }
    
    /// <summary>
    /// Получить список строковых значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</returns>
    [Remote, Public]
    public virtual List<string> GetValueListStringByName(string name, bool genException)
    {
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListString;
      var listValues = new List<string> {};
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        foreach (string constValue in constantEntity.ValueCollection.Where(c => !string.IsNullOrEmpty(c.ValueString) && !string.IsNullOrWhiteSpace(c.ValueString)).Select(c => c.ValueString))
          listValues.Add(constValue);
      }
      
      return listValues;
    }
    
    /// <summary>
    /// Добавить список строковых значений в константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListStringByName(string name, List<string> listValue)
    {
      return this.SetValueListStringByName(name, listValue, true);
    }
    
    /// <summary>
    /// Добавить список строковых значений в константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListStringByName(string name, List<string> listValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListString;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          foreach (var newValue in listValue)
          {
            var newLine = constantEntity.ValueCollection.AddNew();
            newLine.ValueString = newValue;
          }
          constantEntity.Save();
          
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Список целых значений
    
    /// <summary>
    /// Получить список целочисленных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
    [Remote, Public]
    public virtual List<int> GetValueListIntByName(string name)
    {
      return this.GetValueListIntByName(name, true);
    }
    
    /// <summary>
    /// Получить список целочисленных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
    [Remote, Public]
    public virtual List<int> GetValueListIntByName(string name, bool genException)
    {
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListInt;
      var listValues = new List<int> {};
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        foreach (int constValue in constantEntity.ValueCollection.Where(c => c.ValueInt.HasValue).Select(c => c.ValueInt))
          listValues.Add(constValue);
      }
      
      return listValues;
    }
    
    /// <summary>
    /// Добавить список целочисленных значений в константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListIntByName(string name, List<int> listValue)
    {
      return this.SetValueListIntByName(name, listValue, true);
    }
    
    /// <summary>
    /// Добавить список целочисленных значений в константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListIntByName(string name, List<int> listValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListInt;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          foreach (var newValue in listValue)
          {
            var newLine = constantEntity.ValueCollection.AddNew();
            newLine.ValueInt = newValue;
          }
          constantEntity.Save();
          
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Список вещественных значений
    
    /// <summary>
    /// Получить список вещественных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
    [Remote, Public]
    public virtual List<double> GetValueListDoubleByName(string name)
    {
      return this.GetValueListDoubleByName(name, true);
    }
    
    /// <summary>
    /// Получить список вещественных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
    [Remote, Public]
    public virtual List<double> GetValueListDoubleByName(string name, bool genException)
    {
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListDouble;
      var listValues = new List<double> {};
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        foreach (double constValue in constantEntity.ValueCollection.Where(c => c.ValueDouble.HasValue).Select(c => c.ValueDouble))
          listValues.Add(constValue);
      }
      
      return listValues;
    }
    
    /// <summary>
    /// Добавить список вещественных значений в константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListIntByName(string name, List<double> listValue)
    {
      return this.SetValueListIntByName(name, listValue, true);
    }
    
    /// <summary>
    /// Добавить список вещественных значений в константу.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListIntByName(string name, List<double> listValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListString;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          foreach (var newValue in listValue)
          {
            var newLine = constantEntity.ValueCollection.AddNew();
            newLine.ValueDouble = newValue;
          }
          constantEntity.Save();
          
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Список идентификаторов
    
    /// <summary>
    /// Получить список значений константы - идентификаторы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
    [Remote, Public]
    public virtual List<long> GetValueListLongByName(string name)
    {
      return this.GetValueListLongByName(name, true);
    }
    
    /// <summary>
    /// Получить список значений константы - идентификаторы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
    [Remote, Public]
    public virtual List<long> GetValueListLongByName(string name, bool genException)
    {
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListLong;
      var listValues = new List<long> {};
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        foreach (long constValue in constantEntity.ValueCollection.Where(c => c.ValueLong.HasValue).Select(c => c.ValueLong))
          listValues.Add(constValue);
      }
      
      return listValues;
    }
    
    /// <summary>
    /// Добавить список значений в константу - идентификаторы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListLongByName(string name, List<long> listValue)
    {
      return this.SetValueListLongByName(name, listValue, true);
    }
    
    /// <summary>
    /// Добавить список значений в константу - идентификаторы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="listValue">Список новых значений.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueListLongByName(string name, List<long> listValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValListLong;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          foreach (var newValue in listValue)
          {
            var newLine = constantEntity.ValueCollection.AddNew();
            newLine.ValueLong = newValue;
          }
          constantEntity.Save();
          
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    
    #region Диапазон целочисленных значений
    
    /// <summary>
    /// Получить диапазон целочисленных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Структура RangeIntValues {From, By}.</returns>
    [Remote, Public]
    public virtual starkov.EditableConstants.Structures.Module.IRangeIntValues GetValueRangeIntByName(string name)
    {
      return this.GetValueRangeIntByName(name, true);
    }
    
    /// <summary>
    /// Получить диапазон целочисленных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Структура RangeIntValues {From, By}.</returns>
    [Remote, Public]
    public virtual starkov.EditableConstants.Structures.Module.IRangeIntValues GetValueRangeIntByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeInt;

      var rangeInt = starkov.EditableConstants.Structures.Module.RangeIntValues.Create();
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueIntFrom.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          rangeInt.From = constantEntity.ValueIntFrom.Value;
        
        if (!constantEntity.ValueIntBy.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          rangeInt.By = constantEntity.ValueIntBy.Value;
      }
      
      return rangeInt;
    }
    
    /// <summary>
    /// Установить диапазон целочисленных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValueFrom">Новое значение константы (Значение С).</param>
    /// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueRangeIntByName(string name, int constValueFrom, int constValueBy)
    {
      return this.SetValueRangeIntByName(name, constValueFrom, constValueBy, true);
    }
    
    /// <summary>
    /// Установить диапазон целочисленных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValueFrom">Новое значение константы (Значение С).</param>
    /// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueRangeIntByName(string name, int constValueFrom, int constValueBy, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeInt;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueIntFrom = constValueFrom;
          constantEntity.ValueIntBy = constValueBy;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Диапазон вещественных значений
    
    /// <summary>
    /// Получить диапазон вещественных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Структура RangeDoubleValues {From, By}.</returns>
    [Remote, Public]
    public virtual starkov.EditableConstants.Structures.Module.IRangeDoubleValues GetValueRangeDoubleByName(string name)
    {
      return this.GetValueRangeDoubleByName(name, true);
    }
    
    /// <summary>
    /// Получить диапазон вещественных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Структура RangeDoubleValues {From, By}.</returns>
    [Remote, Public]
    public virtual starkov.EditableConstants.Structures.Module.IRangeDoubleValues GetValueRangeDoubleByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeDouble;

      var rangeDouble = starkov.EditableConstants.Structures.Module.RangeDoubleValues.Create();
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueDoubleFrom.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          rangeDouble.From = constantEntity.ValueDoubleFrom.Value;
        
        if (!constantEntity.ValueDoubleBy.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          rangeDouble.By = constantEntity.ValueDoubleBy.Value;
      }
      
      return rangeDouble;
    }
    
    /// <summary>
    /// Установить диапазон вещественных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValueFrom">Новое значение константы (Значение С).</param>
    /// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueRangeDoubleByName(string name, double constValueFrom, double constValueBy)
    {
      return this.SetValueRangeDoubleByName(name, constValueFrom, constValueBy, true);
    }
    
    /// <summary>
    /// Установить диапазон вещественных значений константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValueFrom">Новое значение константы (Значение С).</param>
    /// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueRangeDoubleByName(string name, double constValueFrom, double constValueBy, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeDouble;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueDoubleFrom = constValueFrom;
          constantEntity.ValueDoubleBy = constValueBy;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #region Диапазон идентификаторов
    
    /// <summary>
    /// Получить диапазон значений константы - идентификаторов.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Структура RangeLongValues {From, By}.</returns>
    [Remote, Public]
    public virtual starkov.EditableConstants.Structures.Module.IRangeLongValues GetValueRangeLongByName(string name)
    {
      return this.GetValueRangeLongByName(name, true);
    }
    
    /// <summary>
    /// Получить диапазон значений константы - идентификаторов.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Структура RangeLongValues {From, By}.</returns>
    [Remote, Public]
    public virtual starkov.EditableConstants.Structures.Module.IRangeLongValues GetValueRangeLongByName(string name, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeLong;

      var rangeLong = starkov.EditableConstants.Structures.Module.RangeLongValues.Create();
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (!constantEntity.ValueLongFrom.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          rangeLong.From = constantEntity.ValueLongFrom.Value;
        
        if (!constantEntity.ValueLongBy.HasValue)
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          rangeLong.By = constantEntity.ValueLongBy.Value;
      }
      
      return rangeLong;
    }
    
    /// <summary>
    /// Установить диапазон значений константы - идентификаторов.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValueFrom">Новое значение константы (Значение С).</param>
    /// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueRangeLongByName(string name, long constValueFrom, long constValueBy)
    {
      return this.SetValueRangeLongByName(name, constValueFrom, constValueBy, true);
    }
    
    /// <summary>
    /// Установить диапазон значений константы - идентификаторов.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValueFrom">Новое значение константы (Значение С).</param>
    /// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueRangeLongByName(string name, long constValueFrom, long constValueBy, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValRangeLong;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueLongFrom = constValueFrom;
          constantEntity.ValueLongBy = constValueBy;
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    
    #region Base64 значения
    
    /// <summary>
    /// Получить значение Base64 константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы в формате Base64, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual string GetValueBase64ByName(string name)
    {
      return this.GetValueBase64ByName(name, false, true);
    }
    
    /// <summary>
    /// Получить значение Base64 константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="base64Decode">Преобразовать значение из Base64.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual string GetValueBase64ByName(string name, bool base64Decode)
    {
      return this.GetValueBase64ByName(name, base64Decode, true);
    }
    
    /// <summary>
    /// Получить значение Base64 константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="base64Decode">Преобразовать значение из Base64.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>Значение константы, если константа не найдена, то null.</returns>
    [Remote, Public]
    public virtual string GetValueBase64ByName(string name, bool base64Decode, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBase64;

      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        if (string.IsNullOrEmpty(constantEntity.ValueBase64) || string.IsNullOrWhiteSpace(constantEntity.ValueBase64))
        {
          string textError = Resources.TextErrorFormat(name);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
        else
          return base64Decode ? Base64Decode(constantEntity.ValueBase64) : constantEntity.ValueBase64;
      }
      
      return null;
    }
    
    /// <summary>
    /// Установить значение Base64 константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueBase64ByName(string name, string constValue)
    {
      return this.SetValueBase64ByName(name, constValue, true);
    }
    
    /// <summary>
    /// Установить значение Base64 константы.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <param name="constValue">Новое значение константы.</param>
    /// <param name="genException">Генерировать исключения.</param>
    /// <returns>True - если значение установлено, иначе - False.</returns>
    [Remote, Public]
    public virtual bool SetValueBase64ByName(string name, string constValue, bool genException)
    {
      string subjectError = Resources.SubjectError;
      var typeValue = starkov.EditableConstants.ConstantsEntity.TypeValue.ValBase64;
      
      var constantEntity = GetConstant(name, typeValue, genException);
      if (constantEntity != null)
      {
        try
        {
          constantEntity.ValueBase64 = Base64Encode(constValue);
          constantEntity.Save();
          return true;
        }
        catch (Exception e)
        {
          string textError = Resources.ErrorMessageFormat(name, e.Message);
          SendNoticeAndCreateExeption(subjectError, textError, genException);
        }
      }
      
      return false;
    }
    
    #endregion
    
    #endregion
    
    #region Уведомления
    
    /// <summary>
    /// Отправить уведомление администраторам и сгенерировать исключение.
    /// </summary>
    /// <param name="subject">Тема.</param>
    /// <param name="text">Текст.</param>
    /// <param name="genException">Генерировать исключения.</param>
    [Remote, Public]
    public static void SendNoticeAndCreateExeption(string subject, string text, bool genException)
    {
      SendNotice(subject, text);
      
      if (genException)
        throw new InvalidOperationException(text);
      else
        Logger.ErrorFormat("{0}", text);
    }
    
    /// <summary>
    /// Отправить уведомление.
    /// </summary>
    /// <param name="subject">Тема.</param>
    /// <param name="text">Текст.</param>
    [Remote, Public]
    public static void SendNotice(string subject, string text)
    {
      if (subject.Length > 250)
        subject = subject.Substring(0, 250);
      
      var administrators = Roles.Administrators;
      foreach (var administrator in administrators.RecipientLinks)
      {
        var task = Sungero.Workflow.SimpleTasks.CreateWithNotices(subject, administrator.Member);
        task.ActiveText = text;

        try
        {
          task.Save();
          task.Start();
        }
        catch (Exception ex)
        {
          Logger.ErrorFormat("SendNotice error: {0}", ex.Message);
        }
      }
    }
    
    #endregion
    
    #region Прочие функции
    
    /// <summary>
    /// Получить все константы
    /// </summary>
    [Remote(IsPure=true), Public]
    public static IQueryable<starkov.EditableConstants.IConstantsEntity> GetConstants()
    {
      return starkov.EditableConstants.ConstantsEntities.GetAll();
    }
    
    #endregion
    
    #endregion
    
    
    #region Общие функции
    
    /// <summary>
    /// Преобразование строки в Base64
    /// </summary>
    /// <param name="plainText">Строка</param>
    /// <returns>Значение преобразованное в Base64</returns>
    [Remote(IsPure = true)]
    public static string Base64Encode(string plainText) {
      var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
      return System.Convert.ToBase64String(plainTextBytes);
    }
    
    /// <summary>
    /// Преобразование из Base64 в строку
    /// </summary>
    /// <param name="base64EncodedData">Строка в Base64</param>
    /// <returns>Значение преобразованное из Base64 в строку UTF8</returns>
    [Remote(IsPure = true)]
    public static string Base64Decode(string base64EncodedData) {
      var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
      return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    
    #endregion
    
  }
}
