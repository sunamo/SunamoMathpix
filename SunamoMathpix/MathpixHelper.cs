namespace SunamoMathpix;

/// <summary>
/// Helper class for converting images with math notation using Mathpix API to plain text.
/// </summary>
/// <param name="appId">The Mathpix application ID for API authentication.</param>
/// <param name="appKey">The Mathpix application key for API authentication.</param>
/// <param name="curlDirectory">The directory path where curl executable is located.</param>
public class MathpixHelper(string appId, string appKey, string curlDirectory)
{
    /// <summary>
    /// Converts a base64-encoded image containing mathematical notation to plain text using Mathpix API.
    /// </summary>
    /// <param name="base64Image">The base64-encoded image string with starting part like data:image/jpeg;base64,</param>
    /// <param name="convertLatexToUnicode">Optional function to convert LaTeX to Unicode. Pass null if conversion is not needed. Use SunamoLaTex package for conversion.</param>
    /// <returns>The extracted mathematical text from the image, trimmed and optionally converted to Unicode.</returns>
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
