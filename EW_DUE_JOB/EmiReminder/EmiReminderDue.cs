using PU_EmiReminder_Due.Common;
using PU_EmiReminder_Due.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PU_EmiReminder_Due.EmiReminder
{
    public class EmiReminderDue
    {
        bool IsDev = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDev"]);
        string ReminderDay = ConfigurationManager.AppSettings["ReminderDay"];
        string emailTo = ConfigurationManager.AppSettings["EmailTo"];
        string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
        string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
        string emailCc = ConfigurationManager.AppSettings["Emailcc"];
        string emailBcc = ConfigurationManager.AppSettings["EmailBcc"];
        string connectionString = ConfigurationManager.ConnectionStrings["CrediCash_Dev"].ConnectionString;
        string sqlquery = "";
        public async Task Due_EmiReminder()
        {
            string account_number = string.Empty;
            string subject = ConfigurationManager.AppSettings["Subject"] ?? string.Empty;
            string body = string.Empty;
            EmailContentModel emailContentModel = new EmailContentModel();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("USP_EmiReminder", conn))
            {
                DataTable dt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReminderDay", ReminderDay);
                cmd.Parameters.AddWithValue("@Action", "Due_EmiReminder");
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0 && dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        emailContentModel.customerName = row["full_name"]?.ToString() ?? string.Empty;
                        emailContentModel.lead_id = row["lead_id"]?.ToString() ?? string.Empty;
                        emailContentModel.company_id = row["company_id"]?.ToString() ?? string.Empty;
                        emailContentModel.product_name = row["product_name"]?.ToString() ?? string.Empty;
                        emailContentModel.amount = row["installment"] == DBNull.Value ? 0.0 : Convert.ToDouble(row["installment"]);
                        emailContentModel.loanAccountNumber = row["loan_id"]?.ToString() ?? string.Empty;
                        emailContentModel.dueDate = row["emi_due_date"]?.ToString() ?? string.Empty;
                        emailContentModel.paymentLink = row["paymentLink"]?.ToString() ?? string.Empty;
                        emailContentModel.custphoneNumber = row["mobile_number"]?.ToString() ?? string.Empty;
                        emailContentModel.custemailAddress = row["email_id"]?.ToString() ?? string.Empty;
                        if (row["account_number"] != DBNull.Value && !string.IsNullOrEmpty(row["account_number"].ToString()))
                        {
                            byte[] data = Convert.FromBase64String(row["account_number"].ToString());
                            string decoded = System.Text.Encoding.UTF8.GetString(data);
                            account_number = decoded.Length >= 4 ? decoded.Substring(decoded.Length - 4) : decoded;
                        }
                        emailContentModel.account_number = account_number;
                        emailContentModel.MobileNo = ConfigurationManager.AppSettings["MobileNo"] ?? string.Empty;
                        emailContentModel.EMail_ID = ConfigurationManager.AppSettings["EMail_ID"] ?? string.Empty;
                        emailTo = IsDev ? emailTo : (emailContentModel.custemailAddress ?? string.Empty);emailContentModel.MobileNoFor_SMS = IsDev? (ConfigurationManager.AppSettings["MobileNoFor_SMS"] ?? string.Empty): (row["mobile_number"]?.ToString() ?? string.Empty);
                        emailContentModel.updated_by = System.Environment.MachineName ?? string.Empty;
                        body = EmailContentBuilder.GetBody(emailContentModel);
                        Task.Delay(5000).Wait();

                        string mailstatus = EmailSender.SendEMail(emailFrom, emailTo, emailCc, emailBcc, emailPassword, subject, body, null);
                        mailstatus = (mailstatus == "Sent") ? "1" : "0";

                        sqlquery += $" " +
                            $"exec USP_applicant_maintain_lead_history " +
                            $"@lead_id='{emailContentModel.lead_id}'" +
                            $",@status=17,@reason='{subject}'" +
                            $",@To='{emailTo}'" +
                            $",@updated_by='{emailContentModel.updated_by}'" +
                            $",@company_id='{emailContentModel.company_id}'" +
                            $",@product_name='{emailContentModel.product_name}'" +
                            $",@mail_or_sms_flg='{mailstatus}'";

                        Task.Delay(5000).Wait();
                        await Msg91SmsClass.SendEmiReminderNotificationAsync(emailContentModel);                        
                    }
                    string result = SqlHelper.MultipleTransactions(sqlquery, connectionString);
                }
            }
        }
    }
}
