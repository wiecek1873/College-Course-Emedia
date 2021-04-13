using System;
using System.Collections.Generic;
namespace EmediaWPF
{
	internal class tEXt : Chunk
	{
		public char[] Keyword { get; private set; }
		public char[] Text { get; private set; }

		public tEXt(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			int i = 0;

			List<char> keywordList = new List<char>();
			List<char> textList = new List<char>();

			while (data[i] != 0)
			{
				keywordList.Add((char)data[i]);
				i++;
			}
			Keyword = keywordList.ToArray();

			while(i < length)
			{
				textList.Add((char)data[i]);
				i++;
			}
			Text = textList.ToArray();
		}

		public override string ToString()
		{
			return $"{base.ToString()} | keyword: {string.Join("",Keyword)} | text: {string.Join("",Text)}";
		}
	}
}
