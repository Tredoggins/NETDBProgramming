using System;
using System.Collections.Generic;
using System.Text;

namespace TicketsOOP
{
    class Ticket
    {
        public UInt64 id { get; set; }
        public string summary { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public string submitter { get; set; }
        public string assigned { get; set; }
        public List<string> watching { get; set; }
        public Ticket()
        {
            watching = new List<string>();
        }
        public string display()
        {
            return $"Ticket ID: {id} Summary: {summary}\nStatus: {status} Priority: {priority}\nSubmitter: {submitter} Assigned: {assigned}\nWatcher(s): {string.Join(", ", watching)}";
        }
    }
}
