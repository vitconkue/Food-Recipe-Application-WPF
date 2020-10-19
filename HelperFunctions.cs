using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
