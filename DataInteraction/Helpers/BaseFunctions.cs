namespace DataInteraction.Helpers
{
    public static class BaseFunctions
    {
        /// <summary>
        /// return int - count of equal symbols
        /// </summary>
        public static int GetLikelyIndex(string firstString, string secondString)
        {
            int result = 0;
            for (int i = 0; i < firstString.Length && i < secondString.Length; i++) {
                if (firstString[i] == secondString[i]) { 
                    result++;
                }
            }

            return result;
        }
    }
}
