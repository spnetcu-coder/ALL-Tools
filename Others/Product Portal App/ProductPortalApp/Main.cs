namespace ProductPortalApp
{

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Display_Program());
        }
    }
}