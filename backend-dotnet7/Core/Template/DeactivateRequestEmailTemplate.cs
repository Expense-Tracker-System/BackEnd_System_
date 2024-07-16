using MimeKit;
using MimeKit.Text;

namespace backend_dotnet7.Core.Template
{
    public class DeactivateRequestEmailTemplate
    {
        public TextPart DeactivateRequestEmail(string userName,string reason,DateTime date)
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
                                <h1>Deactivated Request,</h1>
                                <p>User Name: {userName}</p>
                                <p>Reason: {reason}</p>
                                <p>Date: {date}</p>
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
