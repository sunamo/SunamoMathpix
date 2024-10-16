namespace SunamoMathpix;
using System.Management.Automation;

public class MathpixHelper
{
    private const string contentType = "content-type";
    private const string appId = "app_id";
    private const string appKey = "app_key";
    private const string appJson = "application/json";
    private const string uri = "https://api.mathpix.com/v3/text";

    private readonly string app_id;
    private readonly string app_key;
    private readonly string directoryOfCurl;

    public MathpixHelper(string app_id, string app_key, string directoryOfCurl)
    {
        this.app_id = app_id;
        this.app_key = app_key;
        this.directoryOfCurl = directoryOfCurl;
    }

    /// <summary>
    ///     A1 with starting part like data:image/jpeg;base64,
    ///     Pass A2 if want to convert to unicode - package SunamoLaTex
    /// </summary>
    /// <param name="base64"></param>
    /// <param name="convertToUnicode"></param>
    public string Text(string base64, Func<string, string> latexHelperConvertToUnicode)
    {
        var text = string.Empty;

        using (var ps = PowerShell.Create())
        {
            var commands = new[]
            {
                "cd \"" + directoryOfCurl + "\"", @"$uri = 'https://api.mathpix.com/v3/text'" + Environment.NewLine +
                                                  "$headers = @{" +
                                                  "app_id = '" + app_id + "'" + Environment.NewLine +
                                                  "app_key= '" + app_key + "'" +
                                                  "} " + Environment.NewLine +
                                                  "$json = @{" +
                                                  "src = \"" + base64 + "\"" +
                                                  "} |ConvertTo-Json" + Environment.NewLine +
                                                  "$resp = Invoke-RestMethod -Uri $uri -Body $json -Headers $headers -Method Post -ContentType 'application/json'" +
                                                  Environment.NewLine +
                                                  "echo $resp.text"
            };

            // Add the command you want to run

            foreach (var item in commands) ps.AddCommand(item);

            // Execute the command synchronously and get results
            var results = ps.Invoke();

            text += string.Join(string.Empty, results);
        }


        if (latexHelperConvertToUnicode != null) text = latexHelperConvertToUnicode(text);

        text = text.TrimStart('(', '\\');
        text = text.TrimEnd(')', '\\');
        text = text.Trim();

        return text;
    }
}