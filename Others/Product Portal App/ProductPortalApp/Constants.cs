namespace ProductPortalApp
{
    internal static class Constants
    {
        //------------------- レジストリパス -------------------//
        internal static class Registry
        {
            internal const string EXPATH = @"SOFTWARE\SystemConsultant\Excellent\System";
            internal const string FWMGRPATH = @"SOFTWARE\SystemConsultant\FreeWay\ManagementTools\Env";
        }

        //------------------- 起動サイン -------------------//
        internal static class LaunchSign
        {
            internal const string SIGNFEM = "Connect_FEM";
            internal const string SIGNWQMGR = "WQ_TOOL_URL";
            internal const string SIGNWQ = "WQ_URL";
            internal const string SIGNDHMGR = "DH_ADMIN_URL";
            internal const string SIGNDH = "DH_URL";
        }

        //------------------- デフォルトポート -------------------//
        internal static class Port
        {
            internal const int FWPORT = 8080;
            internal const int WQPORT = 8080;
            internal const int DHPORT = 8081;
        }

        //------------------- URLテンプレート -------------------//
        internal static class UrlTemplate
        {
            internal const string URLFEMMGR = "/fem/admin/";
            internal const string URLFEM = "/fem/";
            internal const string URLWQMGR = "/";
            internal const string URLWQ = "/sn/webquery/html/webquery.html";
            internal const string URLDHMGR = "/dc-admin";
            internal const string URLDH = "/";
        }

        //------------------- パネル -------------------//
        internal static class Panel
        {
            internal const int COLUMNNUM = 4;
            internal const int RADIUSNUM = 8;
        }
    }
}
