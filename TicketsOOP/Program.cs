using System;
using System.IO;

namespace TicketsOOP
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string file = "Tickets.csv";
            logger.Info("Program started");
            TicketFile ticketFile = new TicketFile(file);
            string choice;
            do
            {
                Console.WriteLine("1) Read Tickets From File");
                Console.WriteLine("2) Write Tickets To File");
                Console.WriteLine("Type Anything Else To Exit");
                choice = Console.ReadLine();
                logger.Info("User Choice: {0}", choice);
                if (choice == "1")
                {
                    foreach(Ticket t in ticketFile.tickets)
                    {
                        Console.WriteLine(t.display());
                        Console.WriteLine("______________________");
                    }
                }
                else if (choice == "2")
                { 
                    string anothert;
                    do
                    {
                        Ticket ticket = new Ticket();
                        Console.WriteLine("Ticket Summary:");
                        ticket.summary = Console.ReadLine();
                        Console.WriteLine("Ticket Status:");
                        ticket.status = Console.ReadLine();
                        Console.WriteLine("Ticket Priority:");
                        ticket.priority = Console.ReadLine();
                        Console.WriteLine("Ticket Submitter:");
                        ticket.submitter = Console.ReadLine();
                        Console.WriteLine("Assigned to Ticket:");
                        ticket.assigned = Console.ReadLine();
                        string next;
                        do
                        {
                            Console.WriteLine("One Person Watching this Ticket (done to end): ");
                            next = Console.ReadLine();
                            if (next.ToLower() != "done"&&next.Length>0)
                            {
                                ticket.watching.Add(next);
                            }
                        } while (next != "done");
                        ticketFile.addTicket(ticket);
                        Console.WriteLine("Another Ticket?(Y/N)");
                        anothert = Console.ReadLine();
                    } while (anothert.ToUpper().Equals("Y"));
                }
            } while (choice == "1" || choice == "2");
            logger.Info("Program Ended");
        }
    }
}
