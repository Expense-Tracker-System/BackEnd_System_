using MimeKit.Text;
using MimeKit;
using backend_dotnet7.Core.Dtos.Reminder;

namespace backend_dotnet7.Core.Template
{
    public class EmailTemplate
    {
        public TextPart remindertoday(string ReminderName , double amount , string description)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text =  $@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            color: #333;
            margin: 0;
            padding: 0;
        }}
        .container {{
            width: 80%;
            max-width: 600px;
            margin: auto;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }}
        .header {{
            background-color: #07271F;
            color: #ffffff;
            padding: 20px;
            text-align: center;
        }}
        .content {{
            padding: 20px;
        }}
        .footer {{
            background-color: #07271F;
            color: #ffffff;
            text-align: center;
            padding: 10px;
            font-size: 12px;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            font-size: 16px;
            color: #ffffff;
            background-color: #28a745;
            border-radius: 5px;
            text-decoration: none;
            margin-top: 20px;
        }}
        .details {{
            background-color: #f2f2f2;
            padding: 10px;
            border-radius: 5px;
            margin-top: 10px;
        }}
        .details p {{
            margin: 5px 0;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Payment Reminder</h2>
        </div>
        <div class='content'>
            <p>Dear User,</p>
            <p>This is a reminder that you have a payment due today.</p>
            <div class='details'>
                <p><strong>Payment Name:</strong> {ReminderName}</p>
                <p><strong>Amount:</strong> {amount:C}</p>
                <p><strong>Description:</strong> {description}</p>
            </div>
            <p>Thank you for using our Expense Tracker system.</p>
        </div>
        <div class='footer'>
            <p>&copy; {DateTime.Now.Year} Expense Tracker. All rights reserved.</p>
        </div>
    </div>
</body>
</html>"

        };
            return htmlBody;
        }
        public TextPart reminderset(string reminderName , DateTime reminderDate , double reminderAmount , string reminderDescription)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333;'>
    <div style='background-color: #f7f7f7; padding: 20px; border: 1px solid #ddd; border-radius: 10px; max-width: 600px; margin: auto;'>
        <h2 style='color: #07271F;'>Reminder Set Successfully</h2>
        <p>Dear User,</p>
        <p style='font-size: 16px;'>Your reminder has been set successfully.</p>
        <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Reminder Name:</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>{reminderName}</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Reminder Date:</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>{reminderDate:MMMM dd, yyyy}</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Amount:</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>{reminderAmount:C}</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Description:</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>{reminderDescription}</td>
            </tr>
        </table>
        <p style='font-size: 16px; margin-top: 20px;'>Thank you for using our Expense Tracker system.</p>
    </div>
</body>
</html>"
            };

            return htmlBody;
        }
    }
}
