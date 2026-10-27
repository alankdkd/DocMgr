//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static DocMgr.FindRtf;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DocMgr
{
public static class FindRtf
    {
        // --- Windows API Constants and Structures ---
        private const int WM_USER = 0x0400;
        private const int EM_FINDTEXTEX = WM_USER + 79;

        [Flags]
        public enum FindFlags
        {
            None = 0,
            WholeWord = 2,
            MatchCase = 4,
            Reverse = 0x10 // Note: This requires RichEdit 2.0+
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARRANGE
        {
            public int cpMin;
            public int cpMax;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct FINDTEXTEX
        {
            public CHARRANGE chrg;
            public string lpstrText;
            public CHARRANGE chrgText; // This will hold the found range
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref FINDTEXTEX lParam);

        // --- The Search Method ---
        public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
        {
            if (string.IsNullOrEmpty(searchText)) return -1;

            FINDTEXTEX ft = new FINDTEXTEX();

            // Define search range
            // For standard search: Min to Max. For reverse search: Max to Min.
            if (searchReverse)
            {
                ft.chrg.cpMin = endIndex;
                ft.chrg.cpMax = startIndex;
            }
            else
            {
                ft.chrg.cpMin = startIndex;
                ft.chrg.cpMax = endIndex;
            }

            ft.lpstrText = searchText;

            // Set flags
            FindFlags flags = FindFlags.None;
            if (matchCase) flags |= FindFlags.MatchCase;
            if (wholeWord) flags |= FindFlags.WholeWord;

            // Note: The native 'Reverse' flag is only supported in specific RichEdit versions.
            // If your OS/Control doesn't support it, the range swap above handles the direction.

            // Send the message directly to the control's handle
            int resultIndex = (int)SendMessage(rtb.Handle, EM_FINDTEXTEX, (IntPtr)flags, ref ft);

            if (resultIndex != -1)
            {
                // Use the indices returned by the control itself
                rtb.Focus();
                rtb.SelectionStart = ft.chrgText.cpMin;
                rtb.SelectionLength = ft.chrgText.cpMax - ft.chrgText.cpMin;
                rtb.ScrollToCaret();
            }

            return resultIndex;
        }
    }


    //public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
    //{
    //    if (string.IsNullOrEmpty(searchText) || rtb == null) return -1;

    //    // 1. Get all lines and their starting internal indices
    //    string[] lines = rtb.Lines;
    //    StringComparison comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    //    // Determine the starting and ending line indices
    //    int startLine = rtb.GetLineFromCharIndex(startIndex);
    //    int endLine = rtb.GetLineFromCharIndex(endIndex);

    //    // 2. Loop through lines in the requested direction
    //    int step = searchReverse ? -1 : 1;
    //    for (int i = searchReverse ? endLine : startLine;
    //             searchReverse ? i >= startLine : i <= endLine;
    //             i += step)
    //    {
    //        string currentLine = lines[i];
    //        int lineInternalStart = rtb.GetFirstCharIndexFromLine(i);

    //        // Find the text within this specific line
    //        int lineIndex = searchReverse
    //            ? currentLine.LastIndexOf(searchText, comparison)
    //            : currentLine.IndexOf(searchText, comparison);

    //        if (lineIndex != -1)
    //        {
    //            // Calculate the absolute internal index
    //            int finalIndex = lineInternalStart + lineIndex;

    //            // Optional: Validate "Whole Word" locally
    //            if (wholeWord)
    //            {
    //                bool isStart = (lineIndex == 0 || !char.IsLetterOrDigit(currentLine[lineIndex - 1]));
    //                bool isEnd = (lineIndex + searchText.Length == currentLine.Length || !char.IsLetterOrDigit(currentLine[lineIndex + searchText.Length]));
    //                if (!isStart || !isEnd) continue;
    //            }

    //            // 3. Highlight and return
    //            rtb.Focus();
    //            rtb.SelectionStart = finalIndex;
    //            rtb.SelectionLength = searchText.Length;
    //            rtb.ScrollToCaret();

    //            return finalIndex;
    //        }
    //    }

    //    return -1;
    //}


    //public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
    //{
    //    if (string.IsNullOrEmpty(searchText) || rtb == null) return -1;

    //    // 1. Find the occurrence in the plain text first
    //    string plainText = rtb.Text;
    //    StringComparison comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    //    int plainTextIndex = searchReverse
    //        ? plainText.LastIndexOf(searchText, endIndex, endIndex - startIndex, comparison)
    //        : plainText.IndexOf(searchText, startIndex, endIndex - startIndex, comparison);

    //    if (plainTextIndex == -1) return -1;

    //    // 2. Validate "Whole Word"
    //    if (wholeWord)
    //    {
    //        bool isStart = (plainTextIndex == 0 || !char.IsLetterOrDigit(plainText[plainTextIndex - 1]));
    //        bool isEnd = (plainTextIndex + searchText.Length == plainText.Length || !char.IsLetterOrDigit(plainText[plainTextIndex + searchText.Length]));
    //        if (!isStart || !isEnd) return -1;
    //    }

    //    // 3. Resolve the "Internal Index" using Visual Pointers
    //    // We get the physical screen position of the character to force the RTB 
    //    // to reconcile its internal line-ending/formatting count.
    //    System.Drawing.Point pos = rtb.GetPositionFromCharIndex(plainTextIndex);
    //    int verifiedIndex = rtb.GetCharIndexFromPosition(pos);

    //    // 4. Execute the selection with the verified index
    //    rtb.Focus();
    //    rtb.SelectionStart = verifiedIndex;
    //    rtb.SelectionLength = searchText.Length;
    //    rtb.ScrollToCaret();

    //    return verifiedIndex;
    //}


    //public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
    //{
    //    if (string.IsNullOrEmpty(searchText) || rtb == null) return -1;

    //    // 1. Perform the search on the .Text property (Plain Text)
    //    string plainText = rtb.Text;
    //    StringComparison comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    //    int plainTextIndex = -1;
    //    if (searchReverse)
    //        plainTextIndex = plainText.LastIndexOf(searchText, endIndex, endIndex - startIndex, comparison);
    //    else
    //        plainTextIndex = plainText.IndexOf(searchText, startIndex, endIndex - startIndex, comparison);

    //    if (plainTextIndex == -1) return -1;

    //    // 2. Validate "Whole Word" if requested
    //    if (wholeWord)
    //    {
    //        bool before = (plainTextIndex == 0 || !char.IsLetterOrDigit(plainText[plainTextIndex - 1]));
    //        bool after = (plainTextIndex + searchText.Length == plainText.Length || !char.IsLetterOrDigit(plainText[plainTextIndex + searchText.Length]));
    //        if (!before || !after) return -1;
    //    }

    //    // 3. CORRECT THE INDEX DRIFT
    //    // We calculate how many '\n' exist before our found index.
    //    // Each '\n' in .Text actually represents 2 characters (\r\n) in the internal RTB index.
    //    int lineBreaksCount = 0;
    //    for (int i = 0; i < plainTextIndex; i++)
    //    {
    //        if (plainText[i] == '\n') lineBreaksCount++;
    //    }

    //    // The corrected index adds 1 for every carriage return that was stripped out
    //    int correctedIndex = plainTextIndex + lineBreaksCount;

    //    // 4. Final Selection
    //    rtb.Focus();
    //    rtb.SelectionStart = correctedIndex;
    //    rtb.SelectionLength = searchText.Length;
    //    rtb.ScrollToCaret();

    //    return correctedIndex;
    //}


    //public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
    //{
    //    if (string.IsNullOrEmpty(searchText) || rtb == null) return -1;

    //    // 1. Normalize the text and bounds
    //    string plainText = rtb.Text;
    //    int safeStart = Math.Max(0, startIndex);
    //    int safeEnd = Math.Min(plainText.Length, endIndex);

    //    if (safeStart >= safeEnd) return -1;

    //    // 2. Extract the specific search range
    //    string searchArea = plainText.Substring(safeStart, safeEnd - safeStart);

    //    // 3. Build the Regex pattern
    //    string pattern = Regex.Escape(searchText);
    //    if (wholeWord)
    //    {
    //        // Using \b (word boundary) is more reliable than internal Find() logic
    //        pattern = $@"\b{pattern}\b";
    //    }

    //    RegexOptions options = RegexOptions.None;
    //    if (!matchCase) options |= RegexOptions.IgnoreCase;
    //    if (searchReverse) options |= RegexOptions.RightToLeft;

    //    // 4. Execute Search
    //    Match match = Regex.Match(searchArea, pattern, options);

    //    if (match.Success)
    //    {
    //        // The index is match.Index (relative to substring) + safeStart (the offset)
    //        int finalIndex = safeStart + match.Index;

    //        // 5. To fix the "Missing Instance" issue, we must ensure the UI selects 
    //        // the text using the plain-text index, which rtb.Select() accepts.
    //        rtb.Focus();
    //        rtb.SelectionStart = finalIndex;
    //        rtb.SelectionLength = match.Length;
    //        rtb.ScrollToCaret();

    //        finalIndex = GetCorrectedIndex(rtb, finalIndex);
    //        return finalIndex;
    //    }

    //    return -1;
    //}


    //public static int GetCorrectedIndex(RichTextBox rtb, int regexIndex)
    //{
    //    // Find which line the match is on based on the plain text index
    //    int lineIndex = rtb.GetLineFromCharIndex(regexIndex);

    //    // Get the internal starting index of that specific line
    //    int lineStartIndex = rtb.GetFirstCharIndexFromLine(lineIndex);

    //    // Calculate how many characters into that specific line the match starts
    //    // We do this by finding the start of the line in the plain text
    //    string plainText = rtb.Text;
    //    int lineStartInPlainText = 0;
    //    for (int i = 0; i < lineIndex; i++)
    //    {
    //        lineStartInPlainText = plainText.IndexOf('\n', lineStartInPlainText) + 1;
    //    }

    //    int columnOffset = regexIndex - lineStartInPlainText;

    //    // The corrected index is the internal line start + the column offset
    //    return lineStartIndex + columnOffset;
    //}
    //public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
    //    {
    //        if (string.IsNullOrEmpty(searchText) || rtb == null) return -1;

    //        // 1. Get the substring within your bounds
    //        string totalText = rtb.Text;
    //        if (startIndex < 0) startIndex = 0;
    //        if (endIndex > totalText.Length) endIndex = totalText.Length;
    //        if (startIndex >= endIndex) return -1;

    //        string searchArea = totalText.Substring(startIndex, endIndex - startIndex);

    //        // 2. Prepare the Regex pattern
    //        // Escape special characters in the search string (e.g., '.', '?', '*')
    //        string pattern = Regex.Escape(searchText);

    //        // Add boundary markers if Whole Word is requested
    //        if (wholeWord)
    //            pattern = @"\b" + pattern + @"\b";

    //        // 3. Set Regex Options
    //        RegexOptions options = RegexOptions.None;
    //        if (!matchCase)
    //            options |= RegexOptions.IgnoreCase;

    //        if (searchReverse)
    //            options |= RegexOptions.RightToLeft;

    //        // 4. Execute search
    //        Match match = Regex.Match(searchArea, pattern, options);

    //        if (match.Success)
    //        {
    //            // The index is relative to the start of 'searchArea', 
    //            // so we add 'startIndex' to get the absolute position.
    //            int absoluteIndex = startIndex + match.Index;

    //            // Optionally select it in the UI
    //            rtb.SelectionStart = absoluteIndex;
    //            rtb.SelectionLength = match.Length;

    //            return absoluteIndex;
    //        }

    //        return -1;
    //    }

    //public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
    //    {
    //        // 1. Validate input to prevent crashes
    //        if (string.IsNullOrEmpty(searchText) || rtb == null)
    //            return -1;

    //        // Ensure bounds are within the text length
    //        if (startIndex < 0) startIndex = 0;
    //        if (endIndex > rtb.TextLength) endIndex = rtb.TextLength;

    //        // Validating range; if start is after end, search is impossible
    //        if (startIndex > endIndex) return -1;

    //        // 2. Build the search options based on arguments
    //        RichTextBoxFinds options = RichTextBoxFinds.None;

    //        if (matchCase)
    //            options |= RichTextBoxFinds.MatchCase;

    //        if (wholeWord)
    //            options |= RichTextBoxFinds.WholeWord;

    //        if (searchReverse)
    //            options |= RichTextBoxFinds.Reverse;

    //        // 3. Perform the search
    //        // The Find method selects the text if found and returns the index.

    //        try
    //        {
    //            // Note: When 'Reverse' is enabled, the 'Find' method searches specifically 
    //            // for the LAST occurrence within the range [startIndex, endIndex).

    //            int index = rtb.Find(searchText, startIndex, endIndex, options);
    //            return index;
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }

    //public static int FindInRichTextBox(RichTextBox rtb, string searchText, int startIndex, int endIndex, bool matchCase, bool wholeWord, bool searchReverse)
    //    {
    //        // 1. Validate input to prevent crashes
    //        if (string.IsNullOrEmpty(searchText) || rtb == null)
    //            return -1;

    //        // Ensure bounds are within the text length
    //        if (startIndex < 0) startIndex = 0;
    //        if (endIndex > rtb.TextLength) endIndex = rtb.TextLength;
    //        if (startIndex > endIndex) return -1;

    //        // 2. Build the search options based on arguments
    //        RichTextBoxFinds options = RichTextBoxFinds.None;

    //        if (matchCase)
    //            options |= RichTextBoxFinds.MatchCase;

    //        if (wholeWord)
    //            options |= RichTextBoxFinds.WholeWord;

    //        if (searchReverse)
    //            options |= RichTextBoxFinds.Reverse;

    //        // 3. Perform the search
    //        // The Find method selects the text if found and returns the index.
    //        // To prevent the UI from scrolling/selecting, you can add RichTextBoxFinds.NoHighlight 
    //        // to the options, but usually 'Find' implies navigation.

    //        try
    //        {
    //            // Note: When 'Reverse' is enabled, the 'Find' method typically expects the range 
    //            // to be defined from the *start* of the document up to the *end* point you want to search back from.
    //            // However, the overload Find(text, start, end, options) strictly respects the integer bounds provided.
    //            // It will find the LAST occurrence within [startIndex, endIndex) if Reverse is set.

    //            int index = rtb.Find(searchText, startIndex, endIndex, options);
    //            return index;
    //        }
    //        catch
    //        {
    //            // Handle potential edge cases where bounds might still be invalid for specific RTF content
    //            return -1;
    //        }
    //    }
    //RichTextBox rtb,
    //string search,
    //int start,
    //int end,
    //bool matchCase = false,
    //bool wholeWord = false,
    //bool reverse = false)
    //    {
    //        string originalText = rtb.Text;
    //        string normalizedText = originalText.Normalize(NormalizationForm.FormC);
    //        string normalizedSearch = search.Normalize(NormalizationForm.FormC);

    //        end = Math.Min(end, originalText.Length);

    //        StringComparison comparison = matchCase
    //            ? StringComparison.Ordinal
    //            : StringComparison.OrdinalIgnoreCase;

    //        // Map normalized text positions to original text positions
    //        int[] map = BuildNormalizationMap(originalText, normalizedText);

    //        int normIndex;
    //        if (!reverse)
    //            normIndex = normalizedText.IndexOf(normalizedSearch, 0, normalizedText.Length, comparison);
    //        else
    //            normIndex = normalizedText.LastIndexOf(normalizedSearch, normalizedText.Length - 1, normalizedText.Length, comparison);

    //        if (normIndex < 0)
    //            return -1;

    //        // Map back to original text index
    //        int origIndex = map[normIndex];

    //        if (wholeWord)
    //        {
    //            int length = search.Length;
    //            if (!IsWholeWord(originalText, origIndex, length))
    //                return -1;
    //        }

    //        int count = CountStringOccurrences(originalText, "\r");
    //        return origIndex;
    //    }

    //    public static int CountStringOccurrences(string text, string subString)
    //    {
    //        if (string.IsNullOrEmpty(subString))
    //            return 0;

    //        int count = 0;
    //        int index = 0;

    //        // Continue looping as long as IndexOf finds the substring
    //        while ((index = text.IndexOf(subString, index)) != -1)
    //        {
    //            // A match was found, increment the count
    //            count++;

    //            // Move the index past the end of the found substring to find the next one
    //            index += subString.Length;
    //        }

    //        return count;
    //    }
    //    // Build a mapping: normalized text index → original text index
    //    private static int[] BuildNormalizationMap(string original, string normalized)
    //    {
    //        int o = 0, n = 0;
    //        var map = new int[normalized.Length];

    //        while (o < original.Length && n < normalized.Length)
    //        {
    //            string origChar = char.IsSurrogatePair(original, o) ? new string(new[] { original[o], original[o + 1] }) : original[o].ToString();
    //            string normChar = char.IsSurrogatePair(normalized, n) ? new string(new[] { normalized[n], normalized[n + 1] }) : normalized[n].ToString();

    //            map[n] = o;

    //            // Advance by one code unit (or surrogate pair)
    //            o += origChar.Length;
    //            n += normChar.Length;
    //        }

    //        // Fill remaining positions
    //        while (n < normalized.Length)
    //        {
    //            map[n++] = original.Length;
    //        }

    //        return map;
    //    }

    //    private static bool IsWholeWord(string text, int index, int length)
    //    {
    //        bool leftOk = index == 0 || !char.IsLetterOrDigit(text[index - 1]);
    //        int right = index + length;
    //        bool rightOk = right >= text.Length || !char.IsLetterOrDigit(text[right]);
    //        return leftOk && rightOk;
    //    }

    //public static int FindUnicodeSafe(
    //    RichTextBox rtb,
    //    string search,
    //    int start,
    //    int end,
    //    bool matchCase = false,
    //    bool wholeWord = false,
    //    bool reverse = false)
    //    {
    //        if (string.IsNullOrEmpty(search))
    //            return -1;

    //        string text = rtb.Text;
    //        end = Math.Min(end, text.Length);

    //        if (start < 0 || start >= end)
    //            return -1;

    //        // Break search string into text elements
    //        var searchElems = GetTextElements(search);
    //        int searchElemCount = searchElems.Count;

    //        var comparer = matchCase
    //            ? StringComparer.Ordinal
    //            : StringComparer.OrdinalIgnoreCase;

    //        // Enumerate text elements in the RichTextBox text
    //        var textElems = GetTextElements(text);

    //        // Map text-element index → UTF-16 char index
    //        var elemToCharIndex = new List<int>(textElems.Count);
    //        int charPos = 0;
    //        foreach (string e in textElems)
    //        {
    //            elemToCharIndex.Add(charPos);
    //            charPos += e.Length;
    //        }

    //        int elemStart = FindElementIndex(elemToCharIndex, start);
    //        int elemEnd = FindElementIndex(elemToCharIndex, end);

    //        if (!reverse)
    //        {
    //            for (int i = elemStart; i <= elemEnd - searchElemCount; i++)
    //            {
    //                if (MatchAt(textElems, searchElems, i, comparer))
    //                {
    //                    int charIndex = elemToCharIndex[i];
    //                    int charLength = GetCharLength(textElems, i, searchElemCount);

    //                    if (!wholeWord || IsWholeWord(text, charIndex, charLength))
    //                        return charIndex;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int i = elemEnd - searchElemCount; i >= elemStart; i--)
    //            {
    //                if (MatchAt(textElems, searchElems, i, comparer))
    //                {
    //                    int charIndex = elemToCharIndex[i];
    //                    int charLength = GetCharLength(textElems, i, searchElemCount);

    //                    if (!wholeWord || IsWholeWord(text, charIndex, charLength))
    //                        return charIndex;
    //                }
    //            }
    //        }

    //        return -1;
    //    }

    //        private static List<string> GetTextElements(string s)
    //        {
    //            var list = new List<string>();
    //            var e = StringInfo.GetTextElementEnumerator(s);
    //            while (e.MoveNext())
    //                list.Add((string)e.Current);
    //            return list;
    //        }

    //        private static int FindElementIndex(List<int> elemToChar, int charIndex)
    //        {
    //            for (int i = 0; i < elemToChar.Count; i++)
    //            {
    //                if (elemToChar[i] >= charIndex)
    //                    return i;
    //            }
    //            return elemToChar.Count;
    //        }

    //        private static bool MatchAt(
    //    List<string> textElems,
    //    List<string> searchElems,
    //    int index,
    //    StringComparer comparer)
    //        {
    //            for (int i = 0; i < searchElems.Count; i++)
    //            {
    //                if (!comparer.Equals(
    //                        textElems[index + i].Normalize(NormalizationForm.FormC),
    //                        searchElems[i].Normalize(NormalizationForm.FormC)))
    //                    return false;
    //            }
    //            return true;
    //        }

    //        private static int GetCharLength(
    //    List<string> elems,
    //    int index,
    //    int count)
    //        {
    //            int len = 0;
    //            for (int i = 0; i < count; i++)
    //                len += elems[index + i].Length;
    //            return len;
    //        }

    //        private static bool IsWholeWord(string text, int index, int length)
    //        {
    //            bool leftOk =
    //                index == 0 ||
    //                !char.IsLetterOrDigit(text[index - 1]);

    //            int right = index + length;

    //            bool rightOk =
    //                right >= text.Length ||
    //                !char.IsLetterOrDigit(text[right]);

    //            return leftOk && rightOk;
    //        }

    //public static int FindUnicodeSafe(
    //    RichTextBox rtb,
    //    string search,
    //    int start,
    //    int end,
    //    bool matchCase = false,
    //    bool wholeWord = false,
    //    bool reverse = false)
    //    {
    //        if (string.IsNullOrEmpty(search))
    //            return -1;

    //        string text = rtb.Text;
    //        end = Math.Min(end, text.Length);

    //        if (start < 0 || start >= end)
    //            return -1;

    //        CompareInfo cmp = CultureInfo.InvariantCulture.CompareInfo;

    //        CompareOptions options =
    //            (matchCase ? CompareOptions.None : CompareOptions.IgnoreCase) |
    //            CompareOptions.IgnoreNonSpace;

    //        if (!reverse)
    //        {
    //            int index = start;

    //            while (index < end)
    //            {
    //                int pos = cmp.IndexOf(text, search, index, end - index, options);
    //                if (pos < 0)
    //                    return -1;

    //                // 🔧 FIX: advance to the true grapheme start
    //                pos = AdjustForward(text, search, pos, end, matchCase);

    //                if (pos >= 0 &&
    //                    (!wholeWord || IsWholeWord(text, pos, search.Length)))
    //                    return pos;

    //                index = pos + 1;
    //            }
    //        }
    //        else
    //        {
    //            int index = end - 1;

    //            while (index >= start)
    //            {
    //                int pos = cmp.LastIndexOf(
    //                    text,
    //                    search,
    //                    index,
    //                    index - start + 1,
    //                    options);

    //                if (pos < 0)
    //                    return -1;

    //                // 🔧 FIX: adjust backward
    //                pos = AdjustBackward(text, search, pos, start, matchCase);

    //                if (pos >= 0 &&
    //                    (!wholeWord || IsWholeWord(text, pos, search.Length)))
    //                    return pos;

    //                index = pos - 1;
    //            }
    //        }

    //        return -1;
    //    }

    //        private static int AdjustForward(
    //    string text,
    //    string search,
    //    int pos,
    //    int end,
    //    bool matchCase)
    //        {
    //            var comparison = matchCase
    //                ? StringComparison.Ordinal
    //                : StringComparison.OrdinalIgnoreCase;

    //            for (int i = pos; i <= end - search.Length; i++)
    //            {
    //                if (string.Compare(
    //                        text, i,
    //                        search, 0,
    //                        search.Length,
    //                        comparison) == 0)
    //                    return i;
    //            }

    //            return -1;
    //        }

    //        private static int AdjustBackward(
    //            string text,
    //            string search,
    //            int pos,
    //            int start,
    //            bool matchCase)
    //        {
    //            var comparison = matchCase
    //                ? StringComparison.Ordinal
    //                : StringComparison.OrdinalIgnoreCase;

    //            for (int i = pos; i >= start; i--)
    //            {
    //                if (i + search.Length <= text.Length &&
    //                    string.Compare(
    //                        text, i,
    //                        search, 0,
    //                        search.Length,
    //                        comparison) == 0)
    //                    return i;
    //            }

    //            return -1;
    //        }

    //        private static bool IsWholeWord(string text, int index, int length)
    //        {
    //            bool leftOk =
    //                index == 0 ||
    //                !char.IsLetterOrDigit(text[index - 1]);

    //            int right = index + length;

    //            bool rightOk =
    //                right >= text.Length ||
    //                !char.IsLetterOrDigit(text[right]);

    //            return leftOk && rightOk;
    //        }

    //public static int FindUnicodeSafe(
    //    RichTextBox rtb,
    //    string search,
    //    int start,
    //    int end,
    //    bool matchCase = false,
    //    bool wholeWord = false,
    //    bool reverse = false)
    //    {
    //        if (string.IsNullOrEmpty(search))
    //            return -1;

    //        string text = rtb.Text;
    //        end = Math.Min(end, text.Length);

    //        if (start < 0 || start >= end)
    //            return -1;

    //        CompareInfo cmp = CultureInfo.InvariantCulture.CompareInfo;

    //        CompareOptions options =
    //            CompareOptions.IgnoreNonSpace |
    //            (matchCase ? CompareOptions.None : CompareOptions.IgnoreCase);

    //        if (!reverse)
    //        {
    //            int index = start;

    //            while (index < end)
    //            {
    //                int pos = cmp.IndexOf(text, search, index, end - index, options);
    //                if (pos < 0)
    //                    return -1;

    //                if (!wholeWord || IsWholeWord(text, pos, search.Length))
    //                    return pos;

    //                index = pos + 1;
    //            }
    //        }
    //        else
    //        {
    //            int index = end - 1;

    //            while (index >= start)
    //            {
    //                int pos = cmp.LastIndexOf(
    //                    text,
    //                    search,
    //                    index,
    //                    index - start + 1,
    //                    options);

    //                if (pos < 0)
    //                    return -1;

    //                if (!wholeWord || IsWholeWord(text, pos, search.Length))
    //                    return pos;

    //                index = pos - 1;
    //            }
    //        }

    //        return -1;
    //    }

    //        private static bool IsWholeWord(string text, int index, int length)
    //        {
    //            bool leftOk =
    //                index == 0 ||
    //                !char.IsLetterOrDigit(text[index - 1]);

    //            int rightIndex = index + length;

    //            bool rightOk =
    //                rightIndex >= text.Length ||
    //                !char.IsLetterOrDigit(text[rightIndex]);

    //            return leftOk && rightOk;
    //        }

    //        [Flags]
    //        public enum NormalizedFindOptions
    //        {
    //            None = 0,
    //            MatchCase = 1,
    //            WholeWord = 2,
    //            Reverse = 4
    //        }


    //public static int FindNormalized(
    //    RichTextBox rtb,
    //    string search,
    //    int start,
    //    int end,
    //    NormalizedFindOptions options = NormalizedFindOptions.None)
    //    {
    //        if (string.IsNullOrEmpty(search))
    //            return -1;

    //        string text = rtb.Text;
    //        end = Math.Min(end, text.Length);

    //        if (start < 0 || start >= end)
    //            return -1;

    //        bool matchCase = options.HasFlag(NormalizedFindOptions.MatchCase);
    //        bool wholeWord = options.HasFlag(NormalizedFindOptions.WholeWord);
    //        bool reverse = options.HasFlag(NormalizedFindOptions.Reverse);

    //        var comparison = matchCase
    //            ? StringComparison.Ordinal
    //            : StringComparison.OrdinalIgnoreCase;

    //        string needle = search.Normalize(NormalizationForm.FormC);

    //        int step = reverse ? -1 : 1;
    //        int i = reverse ? end - 1 : start;

    //        while (reverse ? i >= start : i <= end - 1)
    //        {
    //            // Expand slice length dynamically to allow composed/decomposed matches
    //            int maxLen = Math.Min(search.Length + 4, end - i);
    //            for (int len = 1; len <= maxLen; len++)
    //            {
    //                string slice = text.Substring(i, len);
    //                string normSlice = slice.Normalize(NormalizationForm.FormC);

    //                if (normSlice.Equals(needle, comparison))
    //                {
    //                    if (wholeWord && !IsWholeWord(text, i, len))
    //                        continue;

    //                    return i; // index is correct for Select()
    //                }
    //            }

    //            i += step;
    //        }

    //        return -1;
    //    }
    //        private static bool IsWholeWord(string text, int index, int length)
    //        {
    //            bool leftOk = index == 0 ||
    //                          !char.IsLetterOrDigit(text[index - 1]);

    //            int rightIndex = index + length;
    //            bool rightOk = rightIndex >= text.Length ||
    //                           !char.IsLetterOrDigit(text[rightIndex]);

    //            return leftOk && rightOk;
    //        }

    //    }
    //}

    //using System.Globalization;

    //namespace DocMgr
    //{
    //    internal class FindRtf
    //    {
    //        public static int FindUnicodeSafe(
    //        RichTextBox rtb,
    //        string search,
    //        int start,
    //        int end,
    //        bool matchCase = false,
    //        bool wholeWord = false,
    //        bool reverse = false)
    //    {
    //        if (string.IsNullOrEmpty(search))
    //            return -1;

    //        string text = rtb.Text;
    //        end = Math.Min(end, text.Length);

    //        if (start < 0 || start >= end)
    //            return -1;

    //        CompareInfo cmp = CultureInfo.InvariantCulture.CompareInfo;

    //        CompareOptions options =
    //            CompareOptions.IgnoreNonSpace |
    //            (matchCase ? CompareOptions.None : CompareOptions.IgnoreCase);

    //        if (!reverse)
    //        {
    //            int index = start;
    //            while (index <= end - search.Length)
    //            {
    //                int pos = cmp.IndexOf(text, search, index, end - index, options);
    //                if (pos < 0)
    //                    return -1;

    //                if (!wholeWord || IsWholeWord(text, pos, search.Length))
    //                    return pos;

    //                index = pos + 1;
    //            }
    //        }
    //        else
    //        {
    //            int index = end - 1;
    //            while (index >= start)
    //            {
    //                int pos = cmp.LastIndexOf(text, search, index, index - start + 1, options);
    //                if (pos < 0)
    //                    return -1;

    //                if (!wholeWord || IsWholeWord(text, pos, search.Length))
    //                    return pos;

    //                index = pos - 1;
    //            }
    //        }

    //        return -1;
    //    }

    //    private static bool IsWholeWord(string text, int index, int length)
    //    {
    //        bool leftOk = index == 0 ||
    //                      !char.IsLetterOrDigit(text[index - 1]);

    //        int rightIndex = index + length;
    //        bool rightOk = rightIndex >= text.Length ||
    //                       !char.IsLetterOrDigit(text[rightIndex]);

    //        return leftOk && rightOk;
    //        }
    //    }
}