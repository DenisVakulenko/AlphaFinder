using System;
using System.Collections.Generic;
using System.Text;

namespace TestHelpers
{
    public class TagClass
    {
        public void Get(string name, ref string singer, ref string title, ref string len)
        {
            try
            {
                TagLib.File mp3File = TagLib.File.Create(name);

                singer = String.Join(", ", mp3File.Tag.Performers);
                title = mp3File.Tag.Title;
                len = Math.Truncate(mp3File.Properties.Duration.TotalMinutes).ToString() + ".";
                if (Math.Round(mp3File.Properties.Duration.TotalSeconds % 60) < 10) len += "0";
                len += Math.Round(mp3File.Properties.Duration.TotalSeconds % 60).ToString();
            }
            catch { }
        }
    }
}
