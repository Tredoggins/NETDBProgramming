using System;
using System.Collections.Generic;
using System.Text;

namespace TicketsWithSearch
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
        public virtual string display()
        {
            return $"Ticket ID: {id}\nSummary: {summary}\nStatus: {status}\nPriority: {priority}\nSubmitter: {submitter}\nAssigned: {assigned}\nWatcher(s): {string.Join(", ", watching)}\n";
        }
        public virtual string fileDisplay()
        {
            return $"{id},{summary},{status},{priority},{submitter},{assigned},{string.Join("|", watching)}";
        }
    }
    class BugDefectTicket : Ticket
    {
        public string severity { get; set; }
        public BugDefectTicket() : base()
        {

        }
        public override string display()
        {
            return $"{base.display()}Severity: {severity}";
        }
        public override string fileDisplay()
        {
            return $"{base.fileDisplay()},{severity}";
        }
    }
    class EnhancementTicket : Ticket
    {
        public string software { get; set; }
        public Decimal cost { get; set; }
        public string reason { get; set; }
        public Decimal estimate { get; set; }
        public EnhancementTicket() : base()
        {

        }
        public override string display()
        {
            return $"{base.display()}Software: {software}\nCost: ${cost}\nReason: {reason}\nEstimate: ${estimate}";
        }
        public override string fileDisplay()
        {
            return $"{base.fileDisplay()},{software},{cost},{reason},{estimate}";
        }
    }
    class TaskTicket : Ticket
    {
        public string projectName { get; set; }
        public DateTime dueDate { get; set; }
        public TaskTicket() : base()
        {

        }
        public override string display()
        {
            return $"{base.display()}Project Name: {projectName}\nDue Date:{dueDate.ToShortDateString()}";
        }
        public override string fileDisplay()
        {
            return $"{base.fileDisplay()},{projectName},{dueDate.ToShortDateString()}";
        }
    }
}
