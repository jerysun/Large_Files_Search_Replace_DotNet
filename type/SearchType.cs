/**
 * class SerachType contains type enums and search string
 * @author jerysun
 * @version 1.00 2018-07-08
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace fsr
{
  public class SearchType
  {
    public SearchType(TypeEnum typeEnum, string searchString)
    {
      this.typeEnum = typeEnum;
      this.searchString = searchString;
    }

    public TypeEnum typeEnum { get; }

    public string searchString { get; }
  }
}
