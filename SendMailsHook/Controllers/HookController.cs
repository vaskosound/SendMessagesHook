using CodeJewels.DataLayer;
using CodeJewels.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Typesafe.Mailgun;

namespace SendMailsHook.Controllers
{
    public class HookController : ApiController
    {
        private DbContext context;
        private DbSet<Email> entitySet;

        public HookController()
        {
            this.context = new CodeJewelsDB();
            this.entitySet = context.Set<Email>();
        }

        public void Post([FromBody] Dictionary<string, object> content)
        {
            if (ConfigurationManager.AppSettings["disableHook"] == "false")
            {
                var buildReport = JsonConvert.SerializeObject(content);
                int startIndex = buildReport.IndexOf("name");
                int endIndex = buildReport.IndexOf(',');
                string appName = buildReport.Substring(startIndex + 7, endIndex - startIndex - 7);
                startIndex = buildReport.IndexOf("message");
                string massage = buildReport.Substring(startIndex + 10);
                endIndex = massage.IndexOf('}');
                string commitMassage = massage.Substring(0, endIndex);
                var mailgunKeyName = "MAILGUN_API_KEY";

                var mailClient = new MailgunClient("app14974.mailgun.org", ConfigurationManager.AppSettings[mailgunKeyName]);
                var emails = this.entitySet;
                foreach (var email in emails)
                {
                    mailClient.SendMail(new System.Net.Mail.MailMessage("itgeorgehook@itgeorge.net", email.EmailName)
                    {
                        Subject = appName,
                        Body = commitMassage
                    });
                }
            }

        }

        [ActionName("post-email")]
        public HttpResponseMessage PostEmail([FromBody]Email value)
        {
            entitySet.Add(value);
            context.SaveChanges();
            
            var response = Request.CreateResponse(HttpStatusCode.Created, value);
            return response;
        }
    }
}
