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

        public static void CenterCursorInButton(this Button but, int dx = 0, int dy = 0)
        {
            Point screenTopLeft = but.PointToScreen(Point.Empty);
            int centerX = screenTopLeft.X + but.Width / 2 + dx;
            int centerY = screenTopLeft.Y + but.Height / 2 + dy;
            Cursor.Position = new Point(centerX, centerY);
        }
    }
}