/**
 * This abstract base is the abstract of the search and replace function,
 * it adopts the StreamReader aiming to reduce the memory consumption to
 * hundreds of mega bytes when processing the large file which could be
 * up to several giga bytes.
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
  public abstract class SearchReplace
  {
    public abstract bool fSearch(TextReader rin, SearchType searchType);

    public abstract void fReplace(TextReader rin, SearchType searchType, String replaceString);

    public string getOutPath(string path)
    {
      int idx = path.LastIndexOf('.');

      StringBuilder sb = new StringBuilder(path.Substring(0, idx));
      sb.Append('_');
      sb.Append("out");
      sb.Append(path.Substring(idx));

      return sb.ToString();
    }

    public bool TryDetectNewLine(string path, out string newLine)
    {
      using (var fileStream = File.OpenRead(path))
      {
        char prevChar = '\0';

        // Read the first 4000 characters to try and find a newline
        for (int i = 0; i < 4000; i++)
        {
          int b;
          if ((b = fileStream.ReadByte()) == -1) break;

          char curChar = (char)b;

          if (curChar == '\n')
          {
            newLine = prevChar == '\r' ? "\r\n" : "\n";
            return true;
          }

          prevChar = curChar;
        }

        // Returning false means could not determine linefeed convention
        newLine = Environment.NewLine;
        return false;
      }
    }
  }
}