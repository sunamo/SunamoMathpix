using System;
using System.Net;
using System.Net.Http;
using System.Text;
using SunamoLaTeX;
using win.Helpers.Powershell;

namespace SunamoMathpix
{
    public class MathpixHelper
    {
        const string contentType = "content-type";
        const string appId = "app_id";
        const string appKey = "app_key";
        const string appJson = "application/json";
        const string uri = "https://api.mathpix.com/v3/text";

        string app_id; 
        string app_key;
        string directoryOfCurl;
        public MathpixHelper(string app_id, string app_key, string directoryOfCurl)
        {
            this.app_id = app_id;
            this.app_key = app_key;
            this.directoryOfCurl = directoryOfCurl;
        }

        /// <summary>
        /// A1 with starting part like data:image/jpeg;base64,
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="convertToUnicode"></param>
        public string Text(string base64, bool convertToUnicode)
        {
            string text = string.Empty;
            PowershellRunner powershellRunner = new PowershellRunner();
            var result = PowershellRunner.Invoke(CA.ToListString("cd \""+directoryOfCurl+"\"", @"$uri = 'https://api.mathpix.com/v3/text'" + Environment.NewLine +
"$headers = @{" +
"app_id = '"+app_id+"'" + Environment.NewLine +
"app_key= '"+app_key+"'" +
"} " + Environment.NewLine +
"$json = @{" +
"src = \""+base64+ "\"" +
"} |ConvertTo-Json" + Environment.NewLine +
"$resp = Invoke-RestMethod -Uri $uri -Body $json -Headers $headers -Method Post -ContentType 'application/json'" + Environment.NewLine +
"echo $resp.text"));

            foreach (var item in result)
            {
                text += SH.Join(string.Empty, item);
            }

            if (convertToUnicode)
            {
                text = LatexHelper.ConvertToUnicode(text);
            }

            text = text.TrimStart(AllChars.lb, AllChars.bs);
            text = text.TrimEnd(AllChars.rb, AllChars.bs);
            text = text.Trim();

            return text;
        }
    }
}