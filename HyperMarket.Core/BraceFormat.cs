using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket
{
    internal class BraceFormat
    {
        private readonly StringBuilder Builder;
        private readonly Dictionary<string, object> Args;
        public BraceFormat(string target, Dictionary<string, object> args)
        {
            Guard.NotNull(target, "target");
            Guard.NotNull(args, "args");
            Builder = new StringBuilder(target);
            Args = args;
            Format();
        }

        public void Format()
        {
            for (var i = 0; i < Builder.Length; i++)
            {
                if ((Builder[i] == '{' || Builder[i] == '}') && Builder.Length == i + 1)
                    throw ErrorHelper.FormatException();
                else if ((Builder[i] == '{' && Builder[i + 1] == '{') || (Builder[i] == '}' && Builder[i] == '}'))
                    Builder.Remove(i, 1);
                else if (Builder[i] == '{')
                {
                    var key = GetKey(i + 1);
                    var value = GetValue(key);
                    Builder.Remove(i, key.Length + 2).Insert(i, value);
                    i = i + value.Length - 1;
                }
                else if (Builder[i] == '}')
                    throw ErrorHelper.FormatException();
            }
        }

        public override string ToString()
        {
            return Builder.ToString();
        }

        private string GetKey(int start)
        {
            var counter = 0;
            while (Builder[start + counter] != '}')
            {
                counter++;
                if (Builder.Length == start + counter)
                    throw ErrorHelper.FormatException();
            }
            return Builder.ToString(start, counter);
        }

        private string GetValue(string key)
        {
            if (!Args.ContainsKey(key))
                throw ErrorHelper.ArgRange(string.Format("Specified key is not found: {0}", key));
            return Args[key].AsString() ?? "";
        }
    }
}
