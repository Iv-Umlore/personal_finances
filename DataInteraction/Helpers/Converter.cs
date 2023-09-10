namespace DataInteraction.Helpers
{
    public static class Converter
    {
        public static string DateToString(DateTime dt)
        {
            return dt.ToString("yyyy.MM.dd HH:mm");
        }

        public static string DoubleToString(double val)
        {
            return val.ToString("0.##").Replace(',', '.');
        }

        public static DateTime StringToDate(string str)
        {
            return DateTime.MinValue;
        }
    }
}
