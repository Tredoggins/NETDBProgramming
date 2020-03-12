using System;
using System.Collections.Generic;
using System.IO;

namespace TicketsWithSearch
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
                Console.WriteLine("3) Search for Tickets in File");
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
                    case "3":
                        inBetween = ", ";
                        what = "Search in File";
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
                else if (choice == "3")
                {
                    Console.WriteLine("1) By Status");
                    Console.WriteLine("2) By Priority");
                    Console.WriteLine("3) By Submitter");
                    type = Console.ReadLine();
                    switch (type)
                    {
                        case "1":
                            inBetween = ", ";
                            which = "By Status";
                            break;
                        case "2":
                            inBetween = ", ";
                            which = "By Priority";
                            break;
                        case "3":
                            inBetween = ", ";
                            which = "By Submitter";
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
                else if(choice == "3" && type != "")
                {
                    string part;
                    List<Ticket> bdresults;
                    List<Ticket> eresults;
                    List<Ticket> tresults;
                    List<Ticket> results;
                    switch (type)
                    {
                        case "1":
                            Console.WriteLine("Part Of Status To Search By: ");
                            part = Console.ReadLine();
                            bdresults=bugDefectFile.searchByStatus(part);
                            eresults = enhancementFile.searchByStatus(part);
                            tresults = taskFile.searchByStatus(part);
                            results = bdresults;
                            results.AddRange(eresults);
                            results.AddRange(tresults);
                            Console.WriteLine($"Number of Bug/Defect Ticket Results: {bdresults.Count}");
                            Console.WriteLine($"Number of Enhancement Ticket Results: {eresults.Count}");
                            Console.WriteLine($"Number of Task Ticket Results: {tresults.Count}");
                            Console.WriteLine($"Number of Total Ticket Results: {results.Count}\n");
                            foreach(Ticket t in results)
                            {
                                Console.WriteLine(t.display());
                            }
                            break;
                        case "2":
                            Console.WriteLine("Part Of Priority To Search By: ");
                            part = Console.ReadLine();
                            bdresults = bugDefectFile.searchByPriority(part);
                            eresults = enhancementFile.searchByPriority(part);
                            tresults = taskFile.searchByPriority(part);
                            results = bdresults;
                            results.AddRange(eresults);
                            results.AddRange(tresults);
                            Console.WriteLine($"Number of Bug/Defect Ticket Results: {bdresults.Count}");
                            Console.WriteLine($"Number of Enhancement Ticket Results: {eresults.Count}");
                            Console.WriteLine($"Number of Task Ticket Results: {tresults.Count}");
                            Console.WriteLine($"Number of Total Ticket Results: {results.Count}\n");
                            foreach (Ticket t in results)
                            {
                                Console.WriteLine(t.display());
                            }
                            break;
                        case "3":
                            Console.WriteLine("Part Of Submitter To Search By: ");
                            part = Console.ReadLine();
                            bdresults = bugDefectFile.searchBySubmitter(part);
                            eresults = enhancementFile.searchBySubmitter(part);
                            tresults = taskFile.searchBySubmitter(part);
                            results = bdresults;
                            results.AddRange(eresults);
                            results.AddRange(tresults);
                            Console.WriteLine($"Number of Bug/Defect Ticket Results: {bdresults.Count}");
                            Console.WriteLine($"Number of Enhancement Ticket Results: {eresults.Count}");
                            Console.WriteLine($"Number of Task Ticket Results: {tresults.Count}");
                            Console.WriteLine($"Number of Total Ticket Results: {results.Count}\n");
                            foreach (Ticket t in results)
                            {
                                Console.WriteLine(t.display());
                            }
                            break;
                    }
                    Console.WriteLine("___________________________________________________________________________");
                }
            } while (choice == "1" || choice == "2" || choice == "3");
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
