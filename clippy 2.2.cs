// created by fallenoneart
//the title doesn't change the clip but it does appear in the discord message
// Replace the webhook URL with your own and Change the webhook bot name if desired
// Note: i had tried to  get the GameName in the discord message but it didn't work out

using System;
using System.Net;
using System.Text;
using System.Threading;

public class CPHInline
{
    public bool Execute()
    {
        // -------------------------
        // Get clip title from chat
        // -------------------------
        string clipTitle = "New Twitch Clip";
        if (CPH.TryGetArg("rawInput", out string rawInput))
        {
            if (!string.IsNullOrWhiteSpace(rawInput))
                clipTitle = rawInput.Trim();
        }

        // Get broadcaster name
        CPH.TryGetArg("broadcastUser", out string broadcastUser);

        // -------------------------
        // Get triggering user
        // -------------------------
        string user = CPH.TryGetArg("user", out string u) ? u : "Someone";

        // -------------------------
        // MESSAGE 1 – Progress gag
        // -------------------------
        CPH.SendMessage(
            "Capturing this legendary moment... please hold!\n" +
            "[■□□□□□□□□□] 10% – Frame by frame, we're preserving your shame.\n" +
            "[■■■■□□□□□□] 40% – Oof, that whiff will live forever.\n" +
            "[■■■■■■■■□□] 80% – Polishing the pixels of disaster.\n" +
            "[■■■■■■■■■■] 100% – Clip complete! 💾 Uploading embarrassment to the Internet Archive of Regret.",
            true,
            true
        );

        // -------------------------
        // Create clip (retry up to 3 times if Twitch API rate limits)
        // -------------------------
        object clipObj = null;
        int attempts = 0;

        while (attempts < 3 && clipObj == null)
        {
            try
            {
                clipObj = CPH.CreateClip();
            }
            catch (Exception ex)
            {
                CPH.SendMessage("❌ Exception while creating clip: " + ex.Message, true);
                return false;
            }

            if (clipObj == null)
            {
                attempts++;
                if (attempts < 3)
                    Thread.Sleep(5000);
                else
                {
                    CPH.SendMessage("❌ Failed to create clip after multiple attempts. Try again in a few seconds.", true);
                    return false;
                }
            }
        }

        string clipUrl = TryGetPropertyString(clipObj, "Url");

        if (string.IsNullOrEmpty(clipUrl))
        {
            CPH.SendMessage("❌ Clip created but URL could not be read.", true);
            return false;
        }

        // -------------------------
        // Delay 3000 ms
        // -------------------------
        Thread.Sleep(3000);

        // -------------------------
        // MESSAGE 2 – Hype message with clip URL
        // -------------------------
        CPH.SendMessage(
            $@"{user}! Behold: @{broadcastUser}'s Epic Fail – now live on the server! Get it while it’s hot... and still slightly humiliating. 🔥🤣
            {clipTitle} → {clipUrl}",
            true,
            true
        );

        // -------------------------
        // Discord webhook (Daniela)
        // -------------------------
        string webhookUrl =
            "DISCORD_WEBHOOK_URL_HERE"; // REPLACE WITH YOUR OWN WEBHOOK URL 

        string discordJson =
            "{"
            + "\"username\":\"DISCORD_BOT_NAME_HERE\"," // Webhook bot name
            + "\"content\":\"**Oh great…I have been caught in the most embarrassing way. "
            + "Big thanks to @" + EscapeJson(user) + " for spilling all my dark secrets**\\n"
            + "**Title:** " + EscapeJson(clipTitle) + "\\n"
            + EscapeJson(clipUrl) + "\""
            + "}";

        try
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                client.Encoding = Encoding.UTF8;
                client.UploadString(webhookUrl, "POST", discordJson);
            }
        }
        catch
        {
            // silently fail; clip is still valid
        }

        return true;
    }

    // -------------------------
    // Reflection helper
    // -------------------------
    private string TryGetPropertyString(object obj, string propName)
    {
        try
        {
            var t = obj.GetType();
            var p = t.GetProperty(propName);
            if (p != null)
                return p.GetValue(obj)?.ToString();
        }
        catch { }
        return null;
    }

    // -------------------------
    // JSON escape
    // -------------------------
    private string EscapeJson(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "");
    }
}

