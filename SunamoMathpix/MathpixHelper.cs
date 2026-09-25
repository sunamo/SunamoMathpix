namespace SunamoMathpix;

public class MathpixHelper(string appId, string appKey, string curlDirectory)
{
    public string Text(string base64Image, Func<string, string>? convertLatexToUnicode)
    {
        var text = string.Empty;
        using (var powerShell = PowerShell.Create())
        {
            var commands = new[]
            {
                "cd \"" + curlDirectory + "\"", @"$uri = 'https://api.mathpix.com/v3/text'" + Environment.NewLine +
                                                  "$headers = @{" +
                                                  "app_id = '" + appId + "'" + Environment.NewLine +
                                                  "app_key= '" + appKey + "'" +
                                                  "} " + Environment.NewLine +
                                                  "$json = @{" +
                                                  "src = \"" + base64Image + "\"" +
                                                  "} |ConvertTo-Json" + Environment.NewLine +
                                                  "$resp = Invoke-RestMethod -Uri $uri -Body $json -Headers $headers -Method Post -ContentType 'application/json'" +
                                                  Environment.NewLine +
                                                  "echo $resp.text"
            };
            foreach (var command in commands) powerShell.AddCommand(command);
            var results = powerShell.Invoke();
            text += string.Join(string.Empty, results);
        }
        if (convertLatexToUnicode != null) text = convertLatexToUnicode(text);
        text = text.TrimStart('(', '\\');
        text = text.TrimEnd(')', '\\');
        text = text.Trim();
        return text;
    }
}
