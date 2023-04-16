using System.Net.Mail;
using System.Net;
using System.Text;

namespace BrandMatrix.Utils
{
    public static class SendEmail
    {
        public static readonly string EmailTemp = "<html>\r\n<body>\r\n<strong>Successful Registration of {Organization Name} at Brand Matrix</strong>\r\n    <br>\r\n    <p>Dear {Owner Name},</p>\r\n    <p>On behalf of the entire team at Brand Matrix, I am delighted to welcome you to our platform. We are pleased to inform you that your organization, <b>{Organization Name}</b>, has been successfully registered with us. We appreciate your trust in our services and look forward to a fruitful business partnership.</p>\r\n    <p>At Brand Matrix, we are committed to providing you with a seamless and user-friendly experience. Our team will soon contact you to initiate the onboarding process and guide you through the platform's features and functionalities. Meanwhile, please feel free to reach out to us if you have any questions or concerns.</p>\r\n    <p>Thank you for choosing Brand Matrix for your business needs. We are confident that our platform will meet your expectations and help you achieve your business goals.</p>\r\n    <br>\r\n    <p>Best regards,</p>\r\n    <p>M. Hussain Saqib<br>CEO, Brand Matrix</p>\r\n  </body>\r\n</html>\r\n";
        public static readonly string EmailTempAdmin = "<html>\r\n  <body>\r\n    <p><strong>Subject:</strong> New Subscription Request</p>\r\n    <br>\r\n    <p>Hello,</p>\r\n    <p>A new subscription request has been received. The details of the customer are as follows:</p>\r\n    <ul>\r\n      <li><strong>Organization Name:</strong> {organizationName}</li>\r\n      <li><strong>Owner Name:</strong> {ownerName}</li>\r\n      <li><strong>Email:</strong> {email}</li>\r\n      <li><strong>Phone:</strong> {phone}</li>\r\n    <li><strong>Address:</strong> {address}, {city}, {state}, {country}, {zipCode}</li>\r\n    </ul>\r\n    <p>Please take the necessary steps to process the subscription request.</p>\r\n    <br>\r\n    <p>Best regards,</p>\r\n    <p>The Brand Matrix Team</p>\r\n  </body>\r\n</html>\r\n";


        public static async Task<bool> PostAnEmail(string senderEmail, string senderEmailKey, string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderEmailKey),
                    EnableSsl = true
                };
                var message = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };
                message.To.Add(to);
                // Add attachments
                // message.Attachments.Add(new Attachment("path/to/file.pdf"));
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
