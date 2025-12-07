using System;
using System.Text;

public static class HtmlToRtf_NoNuGet
{
    public static string Convert(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return @"{\rtf1\ansi }";

        html = ConvertHtmlLinksToRtf(html);

        StringBuilder rtf = new StringBuilder();
        rtf.Append(@"{\rtf1\ansi ");

        int pos = 0;
        while (pos < html.Length)
        {
            int imgIndex = html.IndexOf("<img", pos, StringComparison.OrdinalIgnoreCase);
            if (imgIndex == -1)
            {
                // No more images → add remaining text
                string text = html.Substring(pos);
                rtf.Append(HtmlTextToRtfTextOnly(text));
                break;
            }

            // Add text before <img>
            string before = html.Substring(pos, imgIndex - pos);
            rtf.Append(HtmlTextToRtfTextOnly(before));

            // Find src=
            int srcIndex = html.IndexOf("src=", imgIndex, StringComparison.OrdinalIgnoreCase);
            if (srcIndex == -1) break;

            int quote1 = html.IndexOf('"', srcIndex);
            int quote2 = html.IndexOf('"', quote1 + 1);
            if (quote1 == -1 || quote2 == -1) break;

            string src = html.Substring(quote1 + 1, quote2 - quote1 - 1);

            // Move cursor past img tag
            int tagEnd = html.IndexOf('>', quote2);
            if (tagEnd == -1) tagEnd = quote2 + 1;
            pos = tagEnd + 1;

            //if (src.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
            //{
            //    rtf.Append(ConvertBase64ImageToRtfPict(src));
            //}

            if (src.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
            {
                rtf.Append(ConvertBase64ImageToRtfPict(src));  // base64 version
            }
            else if (src.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                rtf.Append(ConvertUrlImageToRtfPict(src));     // new URL version
            }

        }

        rtf.Append("}");
        return rtf.ToString();
    }

    // -------------------------------------------------------------
    //  TEXT CONVERSION (NO TAGS, ESCAPES TO RTF)
    // -------------------------------------------------------------
    private static string HtmlTextToRtfTextOnly(string html)
    {
        if (string.IsNullOrEmpty(html)) return "";

        // Convert <br> to newline
        html = html.Replace("<br>", "\n")
                   .Replace("<br/>", "\n")
                   .Replace("<br />", "\n");

        // Remove HTML tags
        bool inside = false;
        StringBuilder text = new StringBuilder();
        foreach (char c in html)
        {
            if (c == '<') inside = true;
            else if (c == '>') inside = false;
            else if (!inside) text.Append(c);
        }

        string t = text.ToString();

        // Escape RTF specials
        t = t.Replace(@"\", @"\\")
             .Replace("{", @"\{")
             .Replace("}", @"\}")
             .Replace("\n", "\\par ");

        return t;
    }

    // -------------------------------------------------------------
    //  IMAGE HANDLING: Convert data: URLs to \pict blocks
    // -------------------------------------------------------------
    private static string ConvertBase64ImageToRtfPict(string dataUrl)
    {
        try
        {
            // Example: data:image/png;base64,XXXXXXXX
            int comma = dataUrl.IndexOf(',');
            if (comma < 0) return "";

            string meta = dataUrl.Substring(0, comma);
            string base64 = dataUrl.Substring(comma + 1);

            byte[] bytes = System.Convert.FromBase64String(base64);

            // RTF needs hex bytes
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.Append(b.ToString("X2"));

            bool isPng = meta.Contains("png", StringComparison.OrdinalIgnoreCase);
            bool isJpg = meta.Contains("jpeg", StringComparison.OrdinalIgnoreCase) ||
                         meta.Contains("jpg", StringComparison.OrdinalIgnoreCase);

            string rtfPic = "";

            if (isPng)
                rtfPic = "{\\pict\\pngblip\n" + hex + "}\n";
            else if (isJpg)
                rtfPic = "{\\pict\\jpegblip\n" + hex + "}\n";
            else
                return ""; // unsupported type

            return rtfPic;
        }
        catch
        {
            return "";
        }
    }
    private static string ConvertUrlImageToRtfPict(string url)
    {
        try
        {
            using (var wc = new System.Net.WebClient())
            {
                byte[] bytes = wc.DownloadData(url);

                using (var ms = new System.IO.MemoryStream(bytes))
                using (var img = System.Drawing.Image.FromStream(ms))
                {
                    int origWidth = img.Width;
                    int origHeight = img.Height;

                    // --- Adjustable limits ---
                    const int maxWidth = 500;   // max width in pixels
                    const int maxHeight = 500;  // max height in pixels
                                                // --------------------------

                    int newWidth = origWidth;
                    int newHeight = origHeight;

                    // Calculate scale factors for width and height
                    double scaleW = (double)maxWidth / origWidth;
                    double scaleH = (double)maxHeight / origHeight;

                    // Choose the *smallest* scale (strongest reduction)
                    double scale = Math.Min(1.0, Math.Min(scaleW, scaleH));

                    if (scale < 1.0)
                    {
                        newWidth = (int)(origWidth * scale);
                        newHeight = (int)(origHeight * scale);
                    }

                    // RTF requires:
                    // picw/pich = pixel size
                    // picwgoal/pichgoal = twips (1 px = 15 twips at 96 DPI)
                    int picw = newWidth;
                    int pich = newHeight;
                    int picwgoal = newWidth * 15;
                    int pichgoal = newHeight * 15;

                    // Turn bytes into hexadecimal string
                    StringBuilder hex = new StringBuilder(bytes.Length * 2);
                    foreach (byte b in bytes)
                        hex.Append(b.ToString("X2"));

                    // PNG or JPEG?
                    string header =
                        url.ToLower().EndsWith(".png") ? "\\pngblip" : "\\jpegblip";

                    return
                        "\n\\par\n" +                 // blank line before image
                        "{\\pict" + header +
                        "\\picw" + picw +
                        "\\pich" + pich +
                        "\\picwgoal" + picwgoal +
                        "\\pichgoal" + pichgoal +
                        "\n" +
                        hex.ToString() +
                        "}\n" +
                        "\\par\n\n";                 // blank line after image

                    //return "{\\pict" + header +
                    //       "\\picw" + picw +
                    //       "\\pich" + pich +
                    //       "\\picwgoal" + picwgoal +
                    //       "\\pichgoal" + pichgoal +
                    //       "\n" +
                    //       hex.ToString() +
                    //       "}\n";
                }
            }
        }
        catch
        {
            return "";
        }
    }
    private static string ConvertHtmlLinksToRtf(string html)
    {
        StringBuilder output = new StringBuilder();
        int pos = 0;

        while (true)
        {
            int aStart = html.IndexOf("<a ", pos, StringComparison.OrdinalIgnoreCase);
            if (aStart == -1)
            {
                // no more links → append remaining text
                output.Append(html.Substring(pos));
                break;
            }

            // append text before <a>
            output.Append(html.Substring(pos, aStart - pos));

            // find href=
            int hrefIndex = html.IndexOf("href=", aStart, StringComparison.OrdinalIgnoreCase);
            if (hrefIndex == -1) break;

            int q1 = html.IndexOf('"', hrefIndex);
            int q2 = html.IndexOf('"', q1 + 1);
            if (q1 == -1 || q2 == -1) break;

            string url = html.Substring(q1 + 1, q2 - q1 - 1);

            // find end of <a>
            int tagEnd = html.IndexOf('>', q2);
            if (tagEnd == -1) break;

            // find closing </a>
            int aEnd = html.IndexOf("</a>", tagEnd, StringComparison.OrdinalIgnoreCase);
            if (aEnd == -1) break;

            // extract inner text
            string inner = html.Substring(tagEnd + 1, aEnd - (tagEnd + 1));

            // Append RTF hyperlink
            string rtf = $@"{{\field{{\*\fldinst HYPERLINK ""{url}""}}{{\fldrslt {inner}}}}}";
            output.Append(rtf);

            // move forward
            pos = aEnd + 4;
        }

        return output.ToString();
    }

}
