namespace StoreBlzr.Server.Help
{
    public static class Ex
    {
        public static string Check(string oldString, string newString)
        {
            var result = string.IsNullOrEmpty(newString) ? oldString : newString;
            return result;
        }
    }
}