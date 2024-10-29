namespace KSH.Api.Constants
{
    public static class UserConstants
    {
        public const string AdminRole = "admin";
        public const string ManagerRole = "manager";
        public const string StaffRole = "staff";
        public const string CustomerRole = "customer";
        public enum Gender
        {
            Other = 0,
            Male = 1,
            Female = 2,
        }
        public static string GetGenderString(Gender gender)
        {
            switch ((int)gender)
            {
                case 0:
                    return "Khác";
                case 1:
                    return "Nam";
                case 2:
                    return "Nữ";
                default:
                    return "Không xác định";
            }
        }
    }
}