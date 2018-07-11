/**
 * This is a derived class of the abstract class SearchReplace
 * for text files processing, using StreamReader and 
 * StreamWriter to reduce the memory consumption.
 * @author jerysun
 * @version 1.00 2018-07-08
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace fsr
{
  public class TextSearchReplace : SearchReplace
  {
    public string path { get;  }

    public SearchType searchType { get; }

    private string newline;
    public string newLine { get { return newline; } }

    public TextSearchReplace(string path, SearchType searchType)
    {
      this.path = path;
      this.searchType = searchType;
      TryDetectNewLine(path, out this.newline);
    }

    public override bool fSearch(TextReader rin, SearchType searchType)
    {
      if (rin == null || searchType == null)
        return false;

      TypeEnum typeEnum = searchType.typeEnum;

      while (rin.Peek() > -1)
      {
        string line = rin.ReadLine();

        switch(typeEnum)
        {
          case TypeEnum.PHRASE:
            if (line.Contains(searchType.searchString))
              return true;
            break;
          case TypeEnum.PATTERN:
            Regex rgx = new Regex(searchType.searchString);
            if (rgx.IsMatch(line))
              return true;
            break;
          case TypeEnum.WILDCARD:
            //TODO
            return false;
          case TypeEnum.VARIABLE:
            //TODO
            return false;
          default:
            break;
        }
      }

      return false;
    }

    public override void fReplace(TextReader rin, SearchType searchType, String replaceString)
    {
      if (rin == null || searchType == null || string.IsNullOrEmpty(replaceString))
			return;

      TypeEnum typeEnum = searchType.typeEnum;

      using (TextWriter tw = File.CreateText(getOutPath(path)))
      {
        while (rin.Peek() > -1)
        {
          string line = rin.ReadLine();

          switch (typeEnum)
          {
            case TypeEnum.PHRASE:
              phraseReplace(line, searchType.searchString, replaceString, tw);
              break;
            case TypeEnum.PATTERN:
              patternReplace(line, searchType.searchString, replaceString, tw);
              break;
            case TypeEnum.WILDCARD:
              //TODO
              break;
            case TypeEnum.VARIABLE:
              //TODO
              break;
            default:
              break;
          }
        }
      }
    }

    public void phraseReplace(string line, string targetString, string replaceString, TextWriter tw)
    {
      string changedString = line.Replace(targetString, replaceString);
      StringBuilder sb = new StringBuilder(changedString);
      sb.Append(newLine);
      tw.Write(sb.ToString());
    }

    public void patternReplace(string line, string targetString, string replaceString, TextWriter tw)
    {
      Regex rgx = new Regex(targetString);
      //string result = rgx.Replace(input, replacement);
      string changedString = rgx.Replace(line, replaceString);

      string result = changedString + newLine;
      tw.Write(result);
    }
  }
}
