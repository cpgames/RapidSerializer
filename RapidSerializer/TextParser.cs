using System;
using System.Collections.Generic;
using System.Text;

namespace cpGames.RapidSerializer
{
    public class TextParser
    {
        #region Fields
        private readonly string[] _lines;
        private int _indent;
        private int _lineIndex;
        private string _line;
        #endregion

        #region Properties
        private bool End => _lineIndex >= _lines.Length;
        #endregion

        #region Constructors
        public TextParser(string str)
        {
            _lines = str.Split('\n');
            if (_lines.Length > 0)
            {
                _line = _lines[0];
                _indent = GetIndent(_line);
            }
        }
        #endregion

        #region Methods
        private static int GetIndent(string str)
        {
            var indent = 0;
            foreach (var c in str)
            {
                if (c == '\t')
                {
                    indent++;
                }
                else
                {
                    break;
                }
            }
            return indent;
        }

        private bool ParseNext(out string key, out object value)
        {
            key = string.Empty;
            value = null;

            if (End)
            {
                return false;
            }

            var lineIndex = _lineIndex + 1;
            var line = _lines[_lineIndex + 1];
            var indent = GetIndent(line);

            if (indent < _indent)
            {
                _indent = indent;
                return false;
            }

            _line = line.Substring(_indent);
            _lineIndex = lineIndex;

            if (string.IsNullOrEmpty(line))
            {
                return ParseNext(out key, out value);
            }

            var keySB = new StringBuilder();
            var typeSB = new StringBuilder();
            var valueSB = new StringBuilder();
            var s = 0;

            foreach (var c in _line)
            {
                switch (s)
                {
                    case 0:
                        if (c != ':')
                        {
                            keySB.Append(c);
                        }
                        else
                        {
                            s++;
                        }
                        break;
                    case 1:
                        if (c != ':')
                        {
                            typeSB.Append(c);
                        }
                        else
                        {
                            s++;
                        }
                        break;
                    case 2:
                        valueSB.Append(c);
                        break;
                }
            }

            key = keySB.ToString();
            var type = Type.GetType(typeSB.ToString());
            if (type == null)
            {
                throw new Exception(string.Format("Could not parse type <{0}>.", typeSB));
            }

            if (type == typeof(object[]))
            {
                _indent++;
                value = StringToArray();
            }
            else if (type == typeof(Dictionary<string, object>))
            {
                _indent++;
                value = StringToDictionary();
            }
            else
            {
                value = Convert.ChangeType(valueSB.ToString(), type);
            }
            return true;
        }

        public Dictionary<string, object> StringToDictionary()
        {
            var data = new Dictionary<string, object>();
            while (ParseNext(out var key, out var value))
            {
                data.Add(key, value);
            }
            return data;
        }

        private object[] StringToArray()
        {
            var data = new List<object>();
            while (ParseNext(out _, out var value))
            {
                data.Add(value);
            }
            return data.ToArray();
        }
        #endregion
    }
}