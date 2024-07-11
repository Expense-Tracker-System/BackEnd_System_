using MimeKit;
using MimeKit.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace backend_dotnet7.Core.Template
{
    public class RegistrationConfirmEmailTemplate
    {
        public TextPart RegistrationConfirmEmail(string callback_url, string email)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text = $@"
                <!DOCTYPE html>
                <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Reservation Details</title>
                        <style>
                            body, html {{ 
                                margin: 0; 
                                padding: 0; 
                                font-family: Arial, 
                                sans-serif; 
                                background-color: #f9f9f9 
                            }}
                            .container {{ 
                                max-width: 600px; 
                                margin: 0px auto; 
                                padding: 20px; 
                                background-color: #ffffff; 
                                border: 1px solid #e0e0e0; 
                                border-radius: 8px; 
                            }}
                            .header {{ 
                                padding: 20px; 
                                text-align: center; 
                            }}
                            .content {{ 
                                text - align: center; 
                                padding: 20px; 
                            }}
                            .content h1 {{ 
                                font - size: 24px; 
                                color: #333333; 
                            }}
                            .content p {{
                                font - size: 16px; 
                                color: #555555; }}
                            .content a {{ 
                                display: inline-block;
                                padding: 10px 20px;
                                margin: 20px 0;
                                font-size: 16px;
                                color: #ffffff;
                                background-color: #4CAF50;
                                text-decoration: none;
                                border: 2px solid #07271F
                                border-radius: 4px;
                            }}
                            .footer {{ 
                                text - align: center;
                                padding: 20px;
                                font-size: 12px;
                                color: #777777;
                            }}
                            .footer a {{
                                color: #777777;
                                text-decoration: none;
                                margin: 0 5px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Expensy</h1>
                            </div>
                            <div class='content'>
                                <h1>Confirm your email address to get started on Expensy</h1>
                                <p>Once you’ve confirmed that <a href=""#"">{email}</a> is your email address, we’ll help you find your Slack workspaces or create a new one.</p>
                                <p>From your device, tap the button below to confirm:</p>
                                <a href=""{callback_url}"">Confirm Email Address</a>
                                <p>If you didn’t request this email, there’s nothing to worry about — you can safely ignore it.</p>
                            </div>
                            <div class='footer'>
                                <p>Made by Slack Technologies, Inc</p>
                                <p>500 Howard Street | San Francisco, CA 94105 | United States</p>
                                <p><a href=""#"">Our Blog</a> | <a href=""#"">Email Preferences</a> | <a href=""#"">Policies</a></p>
                            </div>
                        </div>
                    </body>
                </html>"
            };

            return htmlBody;
        }
    }
}
