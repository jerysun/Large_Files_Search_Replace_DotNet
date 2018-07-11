/**
 * This is a derived class of class TextSearchReplace
 * for XML files processing, using StreamReader and 
 * StreamWriter to reduce the memory consumption, using
 * XMLPartEnum to guarantee only the proper part will be
 * searched and replaced.
 * @author jerysun
 * @version 1.00 2018-07-08
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace fsr
{
  public class XMLSearchReplace : TextSearchReplace
  {
    public XMLPartEnum xmlPartEnum { get; }

    public XMLSearchReplace(string path, SearchType searchType, XMLPartEnum xmlPartEnum) : base(path, searchType)
    {
      this.xmlPartEnum = xmlPartEnum;
    }

    public override bool fSearch(TextReader rin, SearchType searchType)
    {
      if (xmlPartEnum == XMLPartEnum.UNKNOWN)
      {
        return false;
      }

      switch (xmlPartEnum)
      {
        case XMLPartEnum.ELEMENT_NAME:
          if (!searchType.searchString.Contains("<"))
          {
            return false;
          }
          break;
        case XMLPartEnum.ATTRIBUTE:
          if (!searchType.searchString.Contains("="))
          {
            return false;
          }
          break;
        default:
          break;
      }

      return base.fSearch(rin, searchType);
    }

    public override void fReplace(TextReader rin, SearchType searchType, String replaceString)
    {
      if (rin == null || searchType == null || string.IsNullOrEmpty(replaceString) || xmlPartEnum == XMLPartEnum.UNKNOWN)
			return;

      string newEndTag = null;
      string oldEndTag = null;
      switch(xmlPartEnum)
      {
        case XMLPartEnum.ELEMENT_NAME:
          if (!searchType.searchString.Contains("<") || !replaceString.Contains("<")) {
            return;
          }
          StringBuilder sb = new StringBuilder(replaceString);
          sb.Insert(1, '/');
          newEndTag = sb.ToString();
          sb = new StringBuilder(searchType.searchString);
          sb.Insert(1, '/');
          oldEndTag = sb.ToString();
          break;
        case XMLPartEnum.ATTRIBUTE:
          if (!searchType.searchString.Contains("=") || !replaceString.Contains("="))
          {
            return;
          }
          break;
        default:
          break;
      }

      //Console.WriteLine("oldEndTab: " + oldEndTag + ", newEndTag: " + newEndTag);

      TypeEnum typeEnum = searchType.typeEnum;

      using(TextWriter tw = File.CreateText(getOutPath(path)))
      {
        while (rin.Peek() > -1)
        {
          string line = rin.ReadLine();

          switch(typeEnum)
          {
            case TypeEnum.PHRASE:
              if (line.Contains(searchType.searchString))
              {
                phraseReplace(line, searchType.searchString, replaceString, tw);
              } else if (oldEndTag != null && newEndTag != null && line.Contains(oldEndTag))
              {
                phraseReplace(line, oldEndTag, newEndTag, tw);
              } else
              {
                lineReplace(line, tw);
              }
              break;
            case TypeEnum.PATTERN:
              Regex rgx = new Regex(searchType.searchString);
              if (rgx.IsMatch(line))
              {
                patternReplace(line, searchType.searchString, replaceString, tw);
              } else if (oldEndTag != null && newEndTag != null)
              {
                Regex rx = new Regex(oldEndTag);
                if (rx.IsMatch(line))
                {
                  patternReplace(line, oldEndTag, newEndTag, tw);
                } else
                {
                  lineReplace(line, tw);
                }
              } else
              {
                lineReplace(line, tw);
              }
              break;
            case TypeEnum.WILDCARD:
              //TODO
              return;
            case TypeEnum.VARIABLE:
              //TODO
              return;
            default:
              break;
          }
        }
      }
    }

    private void lineReplace(string line, TextWriter tw)
    {
      string result = line + newLine;
      tw.Write(result);
    }
  }
}
