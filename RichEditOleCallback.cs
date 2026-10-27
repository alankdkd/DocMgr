using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMgr
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport]
    [Guid("0000000b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStorage
    {
        // Simplified for basic RichTextBox usage
        void CreateStream(string pwcsName, int grfMode, int reserved1, int reserved2, out IStream ppstm);
        void OpenStream(string pwcsName, IntPtr reserved1, int grfMode, int reserved2, out IStream ppstm);
        void CreateStorage(string pwcsName, int grfMode, int reserved1, int reserved2, out IStorage ppstg);
        void OpenStorage(string pwcsName, IStorage pstgPriority, int grfMode, IntPtr snbExclude, int reserved, out IStorage ppstg);
        // Other methods can be left as stubs
    }

    [ComImport]
    [Guid("00020D03-0000-0000-c000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IRichEditOleCallback
    {
        [PreserveSig]
        int GetNewStorage(out IStorage ppstg);
        [PreserveSig]
        int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo);
        [PreserveSig]
        int ShowContainerUI(int fShow);
        [PreserveSig]
        int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp);
        [PreserveSig]
        int DeleteObject(IntPtr lpoleobj);
        [PreserveSig]
        int QueryAcceptData(IntPtr lpdataobj, ref int lpcfFormat, int reco, int fReally, IntPtr hMetaPict);
        [PreserveSig]
        int ContextSensitiveHelp(int fEnterMode);
        [PreserveSig]
        int GetClipboardData(IntPtr lpchrg, int reco, out IntPtr lplpdataobj);
        [PreserveSig]
        int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect);
        [PreserveSig]
        int GetContextMenu(short seltype, IntPtr lpoleobj, IntPtr lpchrg, out IntPtr lphmenu);
    }
    public class RichEditOleCallback : IRichEditOleCallback
    {
        [DllImport("ole32.dll")]
        static extern int StgCreateDocfile(string pwcsName, int grfMode, int reserved, out IStorage ppstg);

        public int GetNewStorage(out IStorage ppstg)
        {
            // STGM_READWRITE | STGM_SHARE_EXCLUSIVE | STGM_CREATE
            int mode = 0x00000002 | 0x00000010 | 0x00001000;
            return StgCreateDocfile(null, mode, 0, out ppstg);
        }

        public int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo) => 0; // S_OK
        public int ShowContainerUI(int fShow) => 0;
        public int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp) => 0;
        public int DeleteObject(IntPtr lpoleobj) => 0;
        public int QueryAcceptData(IntPtr lpdataobj, ref int lpcfFormat, int reco, int fReally, IntPtr hMetaPict) => 0;
        public int ContextSensitiveHelp(int fEnterMode) => 0;
        public int GetClipboardData(IntPtr lpchrg, int reco, out IntPtr lplpdataobj)
        {
            lplpdataobj = IntPtr.Zero;
            return 1; // E_NOTIMPL
        }
        public int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect) => 0;
        public int GetContextMenu(short seltype, IntPtr lpoleobj, IntPtr lpchrg, out IntPtr lphmenu)
        {
            lphmenu = IntPtr.Zero;
            return 0;
        }
    }
}
