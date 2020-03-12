using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TicketsOOP
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
            try
            {
                StreamReader sr = new StreamReader(fileName);
                if (sr.EndOfStream)
                {
                    sr.Close();
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine("ID,Summary,Status,Priority,Submitter,Assigned,Watching");
                    sw.Close();
                }
                else
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] data = line.Split(",");
                        Ticket ticket = new Ticket();
                        ticket.id = UInt64.Parse(data[0]);
                        ticket.summary = data[1];
                        ticket.status = data[2];
                        ticket.priority = data[3];
                        ticket.submitter = data[4];
                        ticket.assigned = data[5];
                        ticket.watching = data[6].Split("|").ToList();
                        tickets.Add(ticket);
                    }
                    sr.Close();
                }
                logger.Info("Tickets in file {Count}", tickets.Count);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public void addTicket(Ticket ticket)
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
                StreamWriter sw = new StreamWriter(fileName,true);
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", ticket.id, ticket.summary, ticket.status, ticket.priority, ticket.submitter, ticket.assigned, string.Join("|",ticket.watching));
                sw.Close();
                tickets.Add(ticket);
                logger.Info("Ticket id {Id} added", ticket.id);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
