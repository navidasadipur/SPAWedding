using SPAWedding.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAWedding.Web.Providers
{
    public interface ISMSProvider
    {

        void SendSMS(ref SMSLog smsLogs);

        void SendSMSByPattern(List<SMSLog> smsLogs, IDictionary<string, string> parameters);

    }
}