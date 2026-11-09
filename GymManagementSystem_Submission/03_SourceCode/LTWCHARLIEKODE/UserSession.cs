using System.Collections.Generic;

namespace LTWCHARLIEKODE
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string FullName { get; set; }
        public static int RoleId { get; set; }
        public static string RoleName { get; set; }

        public static void Clear()
        {
            UserId = 0;
            Username = string.Empty;
            FullName = string.Empty;
            RoleId = 0;
            RoleName = string.Empty;
        }
    }
}