using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TicketOOPTypes
{
    class TicketFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public List<Ticket> tickets { get; set; }
        public string fileName { get; set; }
        public TicketFile(string fileName)
        {
            this.tickets = new List<Ticket>();
            this.fileName = fileName;
        }
        public virtual void addTicket(Ticket ticket)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName);
                sr.ReadLine();
                if (sr.EndOfStream)
                {
                    ticket.id = 1;
                }
                else
                {
                    ticket.id = this.tickets.Max(t => t.id) + 1;
                }
                sr.Close();
                StreamWriter sw = new StreamWriter(fileName, true);
                sw.WriteLine(ticket.fileDisplay());
                sw.Close();
                tickets.Add(ticket);
                logger.Info("Ticket id {Id} added", ticket.id);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public void setNormals(Ticket ticket,string[] data)
        {
            ticket.id = UInt64.Parse(data[0]);
            ticket.summary = data[1];
            ticket.status = data[2];
            ticket.priority = data[3];
            ticket.submitter = data[4];
            ticket.assigned = data[5];
            ticket.watching = data[6].Split("|").ToList();
        }
    }
    class BugDefectFile : TicketFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public BugDefectFile(string fileName) : base(fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine("ID,Summary,Status,Priority,Submitter,Assigned,Watching,Severity");
                    sw.Close();
                }
                else
                {
                    StreamReader sr = new StreamReader(fileName);
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] data = line.Split(",");
                        BugDefectTicket ticket = new BugDefectTicket();
                        base.setNormals(ticket, data);
                        ticket.severity = data[7];
                        tickets.Add(ticket);
                    }
                    sr.Close();
                }
                logger.Info("Bug/Defect Tickets in file {Count}", tickets.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
    class EnhancementFile : TicketFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public EnhancementFile(string fileName) : base(fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine("ID,Summary,Status,Priority,Submitter,Assigned,Watching,Software,Cost,Reason,Estimate");
                    sw.Close();
                }
                else
                {
                    StreamReader sr = new StreamReader(fileName);
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] data = line.Split(",");
                        EnhancementTicket ticket = new EnhancementTicket();
                        base.setNormals(ticket, data);
                        ticket.software = data[7];
                        ticket.cost = Decimal.Parse(data[8]);
                        ticket.reason = data[9];
                        ticket.estimate = Decimal.Parse(data[10]);
                        tickets.Add(ticket);
                    }
                    sr.Close();
                }
                logger.Info("Enhancement Tickets in file {Count}", tickets.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
    class TaskFile : TicketFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public TaskFile(string fileName) : base(fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine("ID,Summary,Status,Priority,Submitter,Assigned,Watching,ProjectName,DueDate");
                    sw.Close();
                }
                else
                {
                    StreamReader sr = new StreamReader(fileName);
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] data = line.Split(",");
                        TaskTicket ticket = new TaskTicket();
                        base.setNormals(ticket, data);
                        ticket.projectName = data[7];
                        ticket.dueDate = DateTime.Parse(data[8]);
                        tickets.Add(ticket);
                    }
                    sr.Close();
                }
                logger.Info("Task Tickets in file {Count}", tickets.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
