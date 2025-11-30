using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    private static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    private static char[] TRIM_CHARS = { '\"' };

    public static CSVObject Read(string content)
    {
        var list = new List<Dictionary<string, object>>();
        CSVObject csv = new CSVObject();

        var lines = Regex.Split(content, LINE_SPLIT_RE);

        if (lines.Length <= 1)
        {
            csv.SetDictionary(list);
            return csv;
        }

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\n", "\n");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        csv.SetDictionary(list);
        return csv;
    }
}

public class CSVObject
{
    private List<Dictionary<string, object>> dict;

    public List<Dictionary<string, object>> Table { get => dict; }

    public void SetDictionary(List<Dictionary<string, object>> dict)
    {
        this.dict = dict;
    }

    public Dictionary<string, object> GetDataByCode(string code)
    {
        for (int i = 0; i < dict.Count; i++)
        {            
            // Object - String 간 Equals 연산 때문에 Hit가 안되는 경우가 있다.
            //if (dict[i]["CODE"].Equals(code))
            if (dict[i]["CODE"].ToString().Equals(code))
            {
                return dict[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Code가 인덱스번호와 같을때 사용할수있다
    /// </summary>
    /// <param name="inCodeNum"></param>
    /// <returns></returns>
    public Dictionary<string, object> GetDataSequence(int inCodeNum)
    {
        string codeNumString = inCodeNum.ToString();

        if (inCodeNum < dict.Count && dict[inCodeNum]["CODE"].ToString().Equals(codeNumString))
        {
            return dict[inCodeNum];
        }
        else
        {
            return null;
        }
    }

    public int GetCount()
    {
        return dict.Count;
    }
}