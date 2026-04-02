using PU_EmiReminder_Due.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PU_EmiReminder_Due.Common
{
    public static class EmailContentBuilder
    {
        public static string GetBody(EmailContentModel model)
        {
            StringBuilder sBody = new StringBuilder();
            sBody.Append("<!DOCTYPE html><html><body>");
            sBody.Append($"<p>Dear <b>{model.customerName},</b></p>");
            sBody.Append("<p>We hope this message finds you well.</p>");
            sBody.Append($"<p>This is a gentle reminder that your repayment of <b>₹{model.amount}</b> for your loan account <b>{model.loanAccountNumber}</b> is due on <b>{model.dueDate}</b>. We kindly request you to ensure the payment is made on or before the due date to avoid any late payment charges and maintain a positive credit history. Suggesting you to login to portal and make payment.</p>");
            sBody.Append($"<p>Maintain a balance of <b>₹{model.amount}</b> in the bank account ending <b>{model.account_number}</b> between <b>6 am - 9 pm</b> on <b>{model.dueDate}</b> to ensure successful autopay.</p>");
            sBody.Append("<p><b>Payment Details:</b></p>");
            sBody.Append($"<p><b>• Amount Due:</b> ₹{model.amount}</p>");
            sBody.Append($"<p><b>• Due Date:</b> {model.dueDate}</p>");
            sBody.Append($"<p><b>• Loan Account Number:</b> {model.loanAccountNumber}</p>");
            sBody.Append($"<p><b>• Payment Link/Bank Details:</b> <a href=\"{model.paymentLink}\" target=\"_blank\">Click here to pay</a></p>");

            sBody.Append("<p>If you have already made the payment, please ignore this message. Otherwise, we request you to make the payment at your earliest convenience.</p>");
            sBody.Append($"<p>For any queries or assistance, feel free to reach out to us at <b>{model.MobileNo}</b> or {model.EMail_ID}.</p>");
            sBody.Append("<p>Thank you for your prompt attention.</p>");
            sBody.Append("<p>Warm regards,</p>");
            sBody.Append($"<p><b>Team {model.product_name}</b></p>");
            sBody.Append("<p><i><em><b>Disclaimer:</b> This is a system-generated email. No reply is required.</em></i></p>");
            sBody.Append("</body></html>");
            return sBody.ToString();
        }
    }
}
