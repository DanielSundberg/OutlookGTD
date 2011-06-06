using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookGTD.Common;

namespace OutlookGTP.UI
{


    public class MessageWrapper : IMessageWrapper
    {
        public MessageWrapper(IMailItemFinder mailItemFinder)
        {
            _mailItemFinder = mailItemFinder;
        }

        public string Guid { get; set; }
        public string Subject { get; set; }
        private string _sender;
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        private string _body;
        private IMailItemFinder _mailItemFinder;

        public string Body
        {
            get
            {
                if (InvalidLink)
                {
                    _mailItemFinder.Get(this, StoreId, EntryId, Guid);
                }
                return _body;
            }
            set { _body = value; }
        }

        public string EntryId { get; set; }
        public string StoreId { get; set; }
        public bool InvalidLink { get; set; }

        public string LinkLine { get; set; }
    }

}
