using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
namespace Food_Recipe_Appplication
{
    class HelperFunctions
    {
        public static string RemovedUTF(string input)
        {

            int checkContainsInReplaceString(char input_char)
            {
                string toReplace = "ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýđð";
                for (int i = 0; i < toReplace.Length; ++i)
                {
                    if (toReplace[i] == input_char)
                    {
                        return i;
                    }
                }
                return -1;
            }
            string result = input;

            string toMatch = "aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyydd";

            for (int i = 0; i < result.Length; ++i)
            {
                int checkValue = checkContainsInReplaceString(result[i]);
                if (checkValue != -1)
                {
                    result = result.Replace(result[i], toMatch[checkValue]);
                }
            }

            return result;
        }

        public static int rateSearchResult(string searchText, string matchedText)
        {
            // longer match => higher rate
            int result = searchText.Length;
            string removedUTFSearchText = HelperFunctions.RemovedUTF(searchText);
            string removedUTFMatchedText = HelperFunctions.RemovedUTF(matchedText);

            
            

            int indexOfMatch = matchedText.IndexOf(searchText);

            // match at the beginning => higher rate
            result -= indexOfMatch;

            // match with better Vietnamse symbol => higher rate

            for(int i =0; i < searchText.Length; ++i)
            {
                if(searchText[i] == matchedText[indexOfMatch + i])
                {
                    result++; 
                }
            }

            string separator = " ";
            string[] searchTextTokens = searchText.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            string[] matchedTextTokens = matchedText.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);


          

            return result;
        }

        public static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return path;
        }
    }
}
