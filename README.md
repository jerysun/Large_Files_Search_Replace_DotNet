# Large Files Search Replace

## I. Requirements:

We regularly receive documents from an external system: weekly status reports, monthly overviews, detailed reports. Sometimes, these documents are plain text files, and sometimes they are xml files. We want to upload these documents to our database. However, before we upload them, we want to search and replace certain text phrases in the documents, so-called "data laundering".

Currently, we only want to search for specific text phrases, but we expect that we will want to be able to use a more flexible search in the future; patterns or wildcards or variables – we have not decided yet. We might also want to be able to perform multiple search / replace actions at once.

The design of the program aims to easily maintain and extend, so other developers can later add some more flexible search methods, or other file types.

## II. Accomplishments:

- The program can handle large files efficiently, e.g. gigabytes of data.
- The memory consumption is less than 200 megabytes when processing gigabytes large files.
- When the input xml file is always well formed, the output xml file is also well formed.
- In case of xml, he following tokens can be searched and replaced: element names, attribute values, text nodes.
- The program has an user-friendly interactive command-line user interface.
- Most errors (e.g. in user input) are timely and appropriately handled, a few others are done by throwing exceptions.
- It is based on the file extension names (txt or xml) to determine file type.
- The current version supports plain ASCII and UTF-8 formats.
- It supports both text phrase and regex patterns, and reserves the interfaces for wildcards and variables in future.

## III. TODO:

Perform multiple search / replace actions at once by using a Dictionary<SearchType, string> data structure.

## IV. Usage:

```bash
C:\Tmp>FileSearchReplace.exe
Please input the file path:
C:\Tmp\server.xml
Please input the XML part type(1: element name 2. attribute 3. text node), just input the number:
2
Please input the string you want to search:
name="JA(.?)PER_LOG"
Please input the search type(1: Text phrease 2. Pattern), just input the number:
2
Please input the replacement string(if you input no, that means you skip the replace operation):
SASPER_LAUGH
Error! The xml attribute must contain a =
Please input the replacement string if you want:
name="SASPER_LAUGH"
"name="JA(.?)PER_LOG"" is found in file: C:\Tmp\server.xml
Replacement is done, please check the new generated file: C:\Tmp\server_out.xml

C:\Tmp>
```

Please feel free to ask me if you have any questions.

Have fun,

Jerry Sun

Email:    jerysun007@hotmail.com

Website:  https://sites.google.com/site/geekssmallworld