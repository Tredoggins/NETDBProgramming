using System;
using System.IO;

namespace TicketOOPTypes
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string file = "tickets.csv";
            logger.Info("Program started");
            FileScrubber scrubber = new FileScrubber(file);
            BugDefectFile bugDefectFile = scrubber.getFile();
            EnhancementFile enhancementFile = new EnhancementFile("enhancements.csv");
            TaskFile taskFile = new TaskFile("task.csv");
            string choice;
            string type = "";
            string which = "Bug/Defect";
            string what = "End";
            string inBetween = ", ";
            Console.WriteLine("___________________________________________________________________________");
            do
            {
                Console.WriteLine("1) Read Tickets From File");
                Console.WriteLine("2) Write Tickets To File");
                Console.WriteLine("Type Anything Else To Exit");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        inBetween = ", ";
                        what = "Read From File";
                        break;
                    case "2":
                        inBetween = ", ";
                        what = "Write To File";
                        break;
                    default:
                        choice = "";
                        inBetween = "";
                        what = "End";
                        break;
                }
                logger.Info($"User Choice: {choice}{inBetween}{what}");
                Console.WriteLine("___________________________________________________________________________");
                if (choice == "1" || choice == "2")
                {
                    Console.WriteLine("1) Bug/Defect");
                    Console.WriteLine("2) Enhancement");
                    Console.WriteLine("3) Task");
                    type = Console.ReadLine();
                    switch (type)
                    {
                        case "1":
                            inBetween = ", ";
                            which = "Bug/Defect";
                            break;
                        case "2":
                            inBetween = ", ";
                            which = "Enhancement";
                            break;
                        case "3":
                            inBetween = ", ";
                            which = "Task";
                            break;
                        default:
                            type = "";
                            inBetween = "";
                            which = "End";
                            break;
                    }
                    logger.Info($"User Choice: {type}{inBetween}{which}");
                    Console.WriteLine("___________________________________________________________________________");
                }
                if (choice == "1" && type != "")
                {
                    foreach (Ticket t in (type.Equals("1") ? bugDefectFile.tickets : (type.Equals("2") ? enhancementFile.tickets : taskFile.tickets)))
                    {
                        Console.WriteLine(t.display());
                        Console.WriteLine("___________________________________________________________________________");
                    }
                }
                else if (choice == "2" && type != "")
                {
                    string anothert;
                    do
                    {
                        string next;
                        switch (type)
                        {
                            case "1":
                                BugDefectTicket bDticket = new BugDefectTicket();
                                doCommon(bDticket);
                                Console.WriteLine("Ticket Severity:");
                                bDticket.severity = Console.ReadLine();
                                bugDefectFile.addTicket(bDticket);
                                break;
                            case "2":
                                EnhancementTicket eticket = new EnhancementTicket();
                                doCommon(eticket);
                                Console.WriteLine("Ticket Software:");
                                eticket.software = Console.ReadLine();
                                Console.WriteLine("Ticket Cost:");
                                Console.Write("$");
                                eticket.cost = Decimal.Parse(Console.ReadLine());
                                Console.WriteLine("Ticket Reason:");
                                eticket.reason = Console.ReadLine();
                                Console.WriteLine("Ticket Estimate:");
                                Console.Write("$");
                                eticket.estimate = Decimal.Parse(Console.ReadLine());
                                enhancementFile.addTicket(eticket);
                                break;
                            default:
                            case "3":
                                TaskTicket ticket = new TaskTicket();
                                doCommon(ticket);
                                Console.WriteLine("Ticket Project Name:");
                                ticket.projectName = Console.ReadLine();
                                Console.WriteLine("Ticket Due Date Month:");
                                int month = int.Parse(Console.ReadLine());
                                Console.WriteLine("Ticket Due Date Day:");
                                int day = int.Parse(Console.ReadLine());
                                Console.WriteLine("Ticket Due Date Year:");
                                int year = int.Parse(Console.ReadLine());
                                ticket.dueDate = DateTime.Parse($"{month}/{day}/{year}");
                                taskFile.addTicket(ticket);
                                break;
                        }
                        Console.WriteLine("___________________________________________________________________________");
                        Console.WriteLine($"Another {which} Ticket?(Y/N)");
                        anothert = Console.ReadLine();
                        Console.WriteLine("___________________________________________________________________________");
                    } while (anothert.ToUpper().Equals("Y"));
                }
            } while (choice == "1" || choice == "2");
            logger.Info("Program Ended");
        }
        private static void doCommon(Ticket ticket)
        {
            string next;
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
            do
            {
                Console.WriteLine("One Person Watching this Ticket (done to end): ");
                next = Console.ReadLine();
                if (next.ToLower() != "done" && next.Length > 0)
                {
                    ticket.watching.Add(next);
                }
            } while (next != "done");
        }
    }
}
