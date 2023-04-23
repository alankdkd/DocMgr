namespace DocMgr
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new DocMgr());
        }

        public static void CenterCursorInButton(this Button but)
        {
            Point loc = but.PointToScreen(Point.Empty);
            Cursor.Position = new Point(loc.X + but.Left + but.Width / 2, loc.Y + but.Top + but.Height / 2);

        }
    }
}