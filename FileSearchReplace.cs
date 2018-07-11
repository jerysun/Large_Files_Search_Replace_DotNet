/**
 * An app to search and replace strings and components in LARGE files minimizing
 * the memory footprint, which supports the file formats such as text, xml and
 * more in future.
 * @author jerysun
 * @version 1.00 2018-07-08
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsr
{
  class FileSearchReplace
  {
    static void Main(string[] args)
    {
      string path = string.Empty;
      string searchString = string.Empty;
      int type = 0;
      FileType fileType = FileType.UNKNOWN;
      XMLPartEnum xmlPartEnum = XMLPartEnum.UNKNOWN;

      Console.WriteLine("Please input the file path:");
      while (string.IsNullOrWhiteSpace(path = Console.ReadLine()))
      {
        Console.WriteLine("Please input the file path:");
      }
      int idx = path.LastIndexOf('.');
      string extensionName = path.Substring(++idx);
      if (extensionName.ToLower().Equals("txt"))
      {
        fileType = FileType.TEXT;
      } else if (extensionName.ToLower().Equals("xml"))
      {
        fileType = FileType.XML;

        Console.WriteLine("Please input the XML part type(1: element name 2. attribute 3. text node), just input the number:");
        while(true)
        {
          while (!Int32.TryParse(Console.ReadLine(), out type))
          {
            Console.WriteLine("Error! You must input a number: 1 or 2 or 3! Please input:");
          }

          if (type == 1 || type == 2 || type == 3)
          {
            switch (type)
            {
              case 1:
                xmlPartEnum = XMLPartEnum.ELEMENT_NAME;
                break;
              case 2:
                xmlPartEnum = XMLPartEnum.ATTRIBUTE;
                break;
              case 3:
                xmlPartEnum = XMLPartEnum.TEXT_NODE;
                break;
              default:
                break;
            }
            break;
          }
          else
          {
            Console.WriteLine("Please input the XML part type(1: element name 2. attribute 3. text node), just input the number:");
          }
        }
      }

      Console.WriteLine("Please input the string you want to search:");
      while (true)
      {
        if (string.IsNullOrWhiteSpace(searchString = Console.ReadLine()))
        {
          Console.WriteLine("Please input the string you want to search:");
        } else { // verify the xml syntax
          if (xmlPartEnum == XMLPartEnum.ELEMENT_NAME && ! searchString.Contains("<"))
          {
            Console.WriteLine("Error! The xml element name must be prefixed by a <");
            Console.WriteLine("Please input the string you want to search:");
          } else if (xmlPartEnum == XMLPartEnum.ATTRIBUTE && !searchString.Contains("="))
          {
            Console.WriteLine("Error! The xml attribute must contain a =");
            Console.WriteLine("Please input the string you want to search:");
          } else
          {
            break;
          }
        }
      }

      Console.WriteLine("Please input the search type(1: Text phrease 2. Pattern), just input the number:");
      while (true)
      {
        if (!Int32.TryParse(Console.ReadLine(), out type))
        {
          Console.WriteLine("Error! You must input a number either 1 or 2!");
          continue;
        }

        if (type == 1 || type == 2)
        {
          break;
        }
        else
        {
          Console.WriteLine("Please input the search type(1: Text phrease 2. Pattern), just input the number:");
        }
      }
      Console.WriteLine("Path: " + path + ", searchString: " + searchString + ", type: " + type);

      SearchType searchType = new SearchType(type == 1 ? TypeEnum.PHRASE : TypeEnum.PATTERN, searchString);

      Console.WriteLine("Please input the replacement string(if you input no, that means you skip the replace operation):");
      string replaceString = Console.ReadLine();
      if (!replaceString.Equals("no"))
      {
        while (true)
        {
          if (xmlPartEnum == XMLPartEnum.ELEMENT_NAME && !replaceString.Contains("<"))
          {
            Console.WriteLine("Error! The xml element name must be prefixed by a <");
            Console.WriteLine("Please input the replacement string if you want:");
            replaceString = Console.ReadLine();
          } else if (xmlPartEnum == XMLPartEnum.ATTRIBUTE && !replaceString.Contains("="))
          {
            Console.WriteLine("Error! The xml attribute must contain a =");
            Console.WriteLine("Please input the replacement string if you want:");
            replaceString = Console.ReadLine();
          } else
          {
            break;
          }
        }
      }

      SearchReplace searchReplace = null;
      if (fileType == FileType.TEXT)
      {
        searchReplace = new TextSearchReplace(path, searchType);
      } else if (fileType == FileType.XML)
      {
        searchReplace = new XMLSearchReplace(path, searchType, xmlPartEnum);
      }

      bool found = false;
      using (TextReader tr = File.OpenText(path))
      {
        if (found = searchReplace.fSearch(tr, searchType))
        {
          Console.WriteLine("\"" + searchType.searchString + "\" is found in file: " + path);
        } else
        {
          Console.WriteLine("\"" + searchType.searchString + "\" is NOT found in file: " + path);
        }
      }

      if (found && !replaceString.Equals("no"))
      {
        using (TextReader tr = File.OpenText(path))
        {
          searchReplace.fReplace(tr, searchType, replaceString);
          Console.WriteLine("Replacement is done, please check the new generated file: " + searchReplace.getOutPath(path));
        }
      }
      //Console.ReadLine();
    }
  }
}
