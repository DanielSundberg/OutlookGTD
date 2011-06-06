using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookGTD.Common
{
    public interface IMailItemFinder
    {
        void Get(IMessageWrapper messageWrapper, string storeId, string entryId, string guid);
    }
    public interface IMessageWrapper
    {
        string Guid { get; set; }
        string Subject { get; set; }
        string Sender { get; set; }
        string Body { get; set; }
        string EntryId { get; set; }
        string StoreId { get; set; }
        bool InvalidLink { get; set; }
        string LinkLine { get; set; }
    }
}
