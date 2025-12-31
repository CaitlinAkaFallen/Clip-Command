Clip-Command

Clip-Command is a backup solution for creating Twitch clips, posting them in chat, and sending them to Discord while waiting for the official Streamer.bot Twitch API update.

⚠️ Note: This is a temporary solution until the Streamer.bot owner updates the Twitch API integration.

Features

Creates a Twitch clip from chat commands.

Posts the clip URL directly in Twitch chat.

Sends the clip URL to a Discord channel via webhook.

Fully importable into Streamer.bot.

Easy to customize your Discord webhook.

Installation

Clone this repository or download the files.

Open Streamer.bot.

Import the Clip-Command script into Streamer.bot as a C# Inline Action.

Update the Discord webhook in the script to your own:

string discordWebhook = "YOUR_DISCORD_WEBHOOK_URL_HERE";

Usage

Trigger the command in Twitch chat.

The bot will create a clip.

The clip URL will appear in chat and be sent to Discord automatically.

Notes

Some parts of the code may need adjustments depending on your Streamer.bot setup.

Keep your Streamer.bot updated as the official Twitch API integration may change how clips are handled.

Contributions and fixes are welcome!
