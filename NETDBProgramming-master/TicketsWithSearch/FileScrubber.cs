using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace TicketsWithSearch
{
    class FileScrubber
    {
        public BugDefectFile bdfile;
        public FileScrubber(string fileName)
        {
            List<BugDefectTicket> tickets = new List<BugDefectTicket>();
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] data = line.Split(",");
                    BugDefectTicket ticket = new BugDefectTicket();
                    ticket.id = UInt64.Parse(data[0]);
                    ticket.summary = data[1];
                    ticket.status = data[2];
                    ticket.priority = data[3];
                    ticket.submitter = data[4];
                    ticket.assigned = data[5];
                    ticket.watching = data[6].Split("|").ToList();
                    if (tickets.Count < 8)
                    {
                        ticket.severity = "Low";
                    }
                    else
                    {
                        ticket.severity = data[7];
                    }
                    tickets.Add(ticket);
                }
                sr.Close();
            }
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("ID,Summary,Status,Priority,Submitter,Assigned,Watching,Severity");
            foreach (Ticket t in tickets)
            {
                sw.WriteLine(t.fileDisplay());
            }
            sw.Close();
            bdfile = new BugDefectFile(fileName);
        }
        public BugDefectFile getFile()
        {
            return bdfile;
        }
    }
}
