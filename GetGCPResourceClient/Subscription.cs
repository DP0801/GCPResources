using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetGCPResourceClient
{
    public class Subscription
    {
        private string subscriptionfriendlyname;
        public string SubscriptionFriendlyName
        {
            get { return "projectname"; }
            set { subscriptionfriendlyname = "projectname"; }
        }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
