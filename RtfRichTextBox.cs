using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class RtfRichTextBox : RichTextBox
{
    private WebBrowser hiddenBrowser;

    public RtfRichTextBox()
    {
        // Hidden WebBrowser for HTML → RTF conversion
        hiddenBrowser = new WebBrowser
        {
            ScrollBarsEnabled = false,
            Visible = false,
            ScriptErrorsSuppressed = true
        };

        // Force creation of the underlying ActiveX handle so Document/ExecCommand works reliably.
        var _ = hiddenBrowser.Handle;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.V) ||
            keyData == (Keys.Shift | Keys.Insert))
        {
            PasteRtfOrHtml();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    public void PasteRtfOrHtml()
    {
        bool isSingleHtmlImage = ContainsSingleHtmlImage();

        if (isSingleHtmlImage)
        {
            // For single HTML images, use default paste to get the image data.
            Paste();
            return;
        }

        IDataObject data = Clipboard.GetDataObject();
        if (data == null) return;

        // Prefer raw RTF payload (may be available as string or MemoryStream)
        if (data.GetDataPresent(DataFormats.Rtf))
        {
            try
            {
                var rtfObj = Clipboard.GetData(DataFormats.Rtf);
                if (rtfObj is string rtfStr)
                {
                    this.SelectedRtf = rtfStr;
                    return;
                }
                else if (rtfObj is MemoryStream ms)
                {
                    ms.Position = 0;
                    using var sr = new StreamReader(ms);
                    var txt = sr.ReadToEnd();
                    this.SelectedRtf = txt;
                    return;
                }
            }
            catch
            {
                // fall through to other formats
            }
        }

        // If only HTML is available, convert it to RTF and paste.
        if (data.GetDataPresent(DataFormats.Html))
        {
            string html = GetHtmlFragmentFromClipboard();
            if (!string.IsNullOrEmpty(html))
            {
                // Try converter (SautinSoft) first:
                // if (HtmlToRtfHelper.TryConvertHtmlToRtf(html, out string rtfFromLib))
                // {
                //     this.SelectedRtf = rtfFromLib;
                //     return;
                // }

                // Otherwise fallback to WebBrowser copy approach:
                ConvertHtmlToRtfAndPaste(html);
                return;
            }
        }

        // Fallback to plain text
        if (data.GetDataPresent(DataFormats.UnicodeText))
        {
            this.SelectedText = Clipboard.GetText(TextDataFormat.UnicodeText);
        }
    }
    public static bool ContainsSingleHtmlImage()
    {
        IDataObject dataObject = Clipboard.GetDataObject();

        if (dataObject == null)
        {
            return false;
        }

        // Check if both an image format (Bitmap) and the HTML format are present
        bool containsImage = dataObject.GetDataPresent(DataFormats.Bitmap);
        bool containsHtml = dataObject.GetDataPresent(DataFormats.Html);

        if (containsImage && containsHtml)
        {
            string htmlContent = dataObject.GetData(DataFormats.Html) as string;

            if (!string.IsNullOrEmpty(htmlContent))
            {
                // Use a regular expression to find all <img> tags in the HTML
                // The RegexOptions.Singleline is useful if the HTML spans multiple lines.
                MatchCollection matches = Regex.Matches(htmlContent, @"<img\b[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                // A single HTML image scenario usually means exactly one <img> tag
                if (matches.Count == 1)
                {
                    // Optional: Further logic can be added to check if other elements 
                    // (like text) are present. Often, the HTML fragment for a single 
                    // image will contain just the <img> tag and some boilerplate HTML
                    // fragment headers.

                    return true;
                }
            }
        }

        return false;
    }
    private void ConvertHtmlToRtfAndPaste(string html)
    {
        // Wrap fragment in minimal HTML doc to ensure consistent rendering
        string doc = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"></head><body>" + html + "</body></html>";

        hiddenBrowser.DocumentCompleted -= Browser_DocumentCompleted;
        hiddenBrowser.DocumentCompleted += Browser_DocumentCompleted;

        // Load document (triggers DocumentCompleted on same UI thread)
        hiddenBrowser.DocumentText = doc;
    }

    private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        hiddenBrowser.DocumentCompleted -= Browser_DocumentCompleted;

        try
        {
            // Select all and copy HTML content; some IE versions put RTF on the clipboard after copy.
            hiddenBrowser.Document?.ExecCommand("SelectAll", false, null);
            hiddenBrowser.Document?.ExecCommand("Copy", false, null);

            // Retry reading RTF from clipboard (clipboard may be briefly busy)
            if (TryGetRtfFromClipboard(out string rtf))
            {
                this.SelectedRtf = rtf;
                return;
            }

            // No RTF available — fallback to plain text from the WebBrowser
            string plain = hiddenBrowser.Document?.Body?.InnerText ?? string.Empty;
            this.SelectedText = plain;
        }
        catch
        {
            // Final fallback: paste plain text from system clipboard if available
            if (Clipboard.ContainsText(TextDataFormat.UnicodeText))
                this.SelectedText = Clipboard.GetText(TextDataFormat.UnicodeText);
        }
    }

    // Attempt to read an RTF payload from the clipboard with retries.
    private bool TryGetRtfFromClipboard(out string rtf)
    {
        rtf = null!;
        const int retries = 10;
        const int delayMs = 50;

        for (int i = 0; i < retries; i++)
        {
            try
            {
                if (Clipboard.ContainsData(DataFormats.Rtf))
                {
                    var obj = Clipboard.GetData(DataFormats.Rtf);
                    if (obj is string s)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            rtf = s;
                            return true;
                        }
                    }
                    else if (obj is MemoryStream ms)
                    {
                        ms.Position = 0;
                        using var sr = new StreamReader(ms);
                        var txt = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(txt))
                        {
                            rtf = txt;
                            return true;
                        }
                    }
                }

                // Some apps place the RTF payload under "Text" / Rtf text format
                if (Clipboard.ContainsText(TextDataFormat.Rtf))
                {
                    var txt = Clipboard.GetText(TextDataFormat.Rtf);
                    if (!string.IsNullOrEmpty(txt))
                    {
                        rtf = txt;
                        return true;
                    }
                }
            }
            catch
            {
                // Clipboard busy — retry
            }

            Thread.Sleep(delayMs);
            Application.DoEvents();
        }

        return false;
    }

    private string GetHtmlFragmentFromClipboard()
    {
        if (!Clipboard.ContainsData(DataFormats.Html))
            return null;

        string html = Clipboard.GetText(TextDataFormat.Html);
        if (string.IsNullOrEmpty(html))
            return null;

        const string startMarker = "StartFragment:";
        const string endMarker = "EndFragment:";

        int startIndexMarker = html.IndexOf(startMarker, StringComparison.OrdinalIgnoreCase);
        int endIndexMarker = html.IndexOf(endMarker, StringComparison.OrdinalIgnoreCase);

        if (startIndexMarker >= 0 && endIndexMarker > startIndexMarker)
        {
            try
            {
                int startLineEnd = html.IndexOf('\n', startIndexMarker);
                int endLineEnd = html.IndexOf('\n', endIndexMarker);

                int start = int.Parse(html.Substring(startIndexMarker + startMarker.Length, startLineEnd - (startIndexMarker + startMarker.Length)).Trim());
                int end = int.Parse(html.Substring(endIndexMarker + endMarker.Length, endLineEnd - (endIndexMarker + endMarker.Length)).Trim());

                if (end > html.Length)          // Shouldn't ever happen but it does.
                    end = html.Length;

                if (start >= 0 && end > start && end <= html.Length)
                    return html.Substring(start, end - start);
            }
            catch
            {
                // ignore parsing errors and return full HTML below
            }
        }

        return html;
    }

    //// Diagnostic: write RTF to a temp file and show a short preview so you can inspect whether
    //// the RTF contains a \fonttbl and bold control words (\b).
    //private void InspectRtfIfNeeded(string rtf, string context)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(rtf)) return;

    //        string preview = rtf.Length <= 512 ? rtf : rtf.Substring(0, 512);
    //        string tmp = Path.Combine(Path.GetTempPath(), "clipboard_rtf_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".rtf");
    //        File.WriteAllText(tmp, rtf);

    //        // Show minimal information so you can copy/paste here.
    //        string msg = $"{context} — preview (first 512 chars):\n\n{preview}\n\nSaved full RTF to:\n{tmp}\n\nLook for \"\\fonttbl\" and \"\\b\" in the preview or file.";
    //        MessageBox.Show(msg, "RTF Diagnostic", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //    }
    //    catch
    //    {
    //        // If diagnostics fail don't block main flow.
    //    }
    //}
}
