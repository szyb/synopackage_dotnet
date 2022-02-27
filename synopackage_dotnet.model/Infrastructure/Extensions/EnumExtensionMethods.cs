﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synopackage_dotnet.model
{
  public static class EnumExtensionMethods
  {
    public static string GetEnumDescription(this Enum enumValue)
    {
      var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

      var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

      return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
    }
  }
}
