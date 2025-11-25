using System;
using System.Windows.Forms;

public class RtfRichTextBox : RichTextBox
{
    private WebBrowser hiddenBrowser;

    public RtfRichTextBox()
    {
        hiddenBrowser = new WebBrowser();
        hiddenBrowser.ScrollBarsEnabled = false;
        hiddenBrowser.Visible = false;
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
        IDataObject data = Clipboard.GetDataObject();
        if (data == null) return;

        // RTF first
        if (data.GetDataPresent(DataFormats.Rtf))
        {
            this.SelectedRtf = Clipboard.GetText(TextDataFormat.Rtf);
            return;
        }

        // HTML next
        if (data.GetDataPresent(DataFormats.Html))
        {
            string html = Clipboard.GetText(TextDataFormat.Html);
            if (!string.IsNullOrEmpty(html))
            {
                ConvertHtmlToRtfAndPaste(html);
                return;
            }
        }

        // Fallback plain text
        if (data.GetDataPresent(DataFormats.UnicodeText))
        {
            this.SelectedText = Clipboard.GetText(TextDataFormat.UnicodeText);
        }
    }

    private void ConvertHtmlToRtfAndPaste(string html)
    {
        string rtf = ConvertHtmlToRtf(html);
        if (!string.IsNullOrEmpty(rtf))
        {
            this.SelectedRtf = rtf;
        }
    }

    private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        hiddenBrowser.DocumentCompleted -= Browser_DocumentCompleted;

        hiddenBrowser.Document.ExecCommand("SelectAll", false, null);
        hiddenBrowser.Document.ExecCommand("Copy", false, null);

        if (Clipboard.ContainsText(TextDataFormat.Rtf))
        {
            this.SelectedRtf = Clipboard.GetText(TextDataFormat.Rtf);
        }
    }
    private string ConvertHtmlToRtf(string html)
    {
        // temporary browser
        using (var browser = new WebBrowser())
        {
            browser.ScrollBarsEnabled = false;
            browser.DocumentText = html;

            // Wait for document to load
            while (browser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();

            // Select all and copy as RTF
            browser.Document.ExecCommand("SelectAll", false, null);
            browser.Document.ExecCommand("Copy", false, null);

            IDataObject data = Clipboard.GetDataObject();
            if (data != null && data.GetDataPresent(DataFormats.Rtf))
            {
                string rtf = data.GetData(DataFormats.Rtf) as string;

                // Remove HTML Clipboard headers (Version:, StartHTML:, EndHTML:, etc.)
                // LibreOffice and Chrome often paste those in front of the RTF
                if (!string.IsNullOrEmpty(rtf))
                {
                    int braceIndex = rtf.IndexOf(@"{\rtf");
                    if (braceIndex > 0)
                        rtf = rtf.Substring(braceIndex);
                }

                return rtf;
            }

            return null;
        }
    }
}
