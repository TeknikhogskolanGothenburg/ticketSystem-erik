﻿using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketApi.Db.Models;
using TicketApi.Db.Interface;
using Microsoft.Extensions.Configuration;
using DbExtensions;

namespace TicketApi.Db
{
    public class TicketRepository : ITicketRepository
    {
        private readonly string _connectionString;

        public TicketRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Ticket AddTicket(Ticket ticket)
        {
            var query = @"INSERT INTO Tickets(TicketEventDateID)
                        VALUES(@ticketEventDateId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Query(query, new { ticketEventDateId = ticket.TicketEventDateID });
                var addedItemQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Tickets') AS Current_Identity").First();
                var result = connection.Query<Ticket>("SELECT * FROM Tickets WHERE TicketID=@id", new { id = addedItemQuery }).FirstOrDefault();

                return result;
            }
        }

        public Ticket AddTicketOrder(Ticket ticket)
        {
            var query = @"INSERT INTO Tickets(TicketEventDateID)
                        VALUES(@ticketEventDateId)";
                        

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Query(query, new { ticketEventDateId = ticket.TicketEventDateID });
                var addedItemQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Tickets') AS Current_Identity").First();

                query = @"INSERT INTO TicketsToTransactions(TicketID, TransactionID)
                        VALUES(@addedItemQuery, @TransactionID)";
                connection.Query(query, new { addedItemQuery, TransactionID = ticket.TransactionID });
                var result = connection.Query<Ticket>("SELECT * FROM Tickets WHERE TicketID=@id", new { id = addedItemQuery }).FirstOrDefault();

                return result;
            }
        }

        public Ticket UpdateTicket(Ticket ticket)
        {
            var query = SQL
               .UPDATE("Tickets")
               .SET("TicketEventDateID = @ticketEventDateId")
               .WHERE("TicketID = @id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query.ToString(), new { ticketEventDateID = ticket.TicketEventDateID });
                var result = connection.Query<Ticket>("SELECT * FROM Ticket WHERE TicketID = @id", new { id = ticket.TicketID }).FirstOrDefault();

                return result;
            }
        }

        public bool DeleteTicket(int ticketId)
        {
            var query = SQL
                .DELETE_FROM("Tickets")
                .WHERE("TicketID = @id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                if (connection.Execute(query.ToString(), new { id = ticketId }) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Ticket> GetTickets(int offset = 0, int maxLimit = 20)
        {

            if (maxLimit > 30)
            {
                maxLimit = 30;
            }

            var query = @"SELECT * FROM Tickets
                        ORDER BY TicketID
                        OFFSET @offset ROWS
                        FETCH NEXT @maxLimit ROWS ONLY";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<Ticket>(query, new { offset, maxLimit }).ToList();
                return result;
            }
        }

        public List<Ticket> GetTicketsByUserId(string userId, int offset = 0, int maxLimit = 20)
        {
            if (maxLimit > 30)
            {
                maxLimit = 30;
            }

            var query = @"SELECT * FROM TicketTransactions t
                        JOIN TicketToTransactions ttt ON t.TransactionID = ttt.TransactionID
                        JOIN Tickets ON ttt.TicketID = Tickets.TicketID
                        JOIN SeatsAtEventDate ON Tickets.SeatID = SeatsAtEventDate.SeatID
                        JOIN TicketEventDates ted ON SeatsAtEventDate.TicketEventDateID = ted.TicketEventDateID
                        JOIN Venues v ON ted.VenueID = v.VenueID
                        JOIN TicketEvent te ON ted.TicketEventID = te.TicketEventID
                        WHERE t.BuyerUserId = @userId
                        ORDER BY TransactionID
                        OFFSET @offset ROWS
                        FETCH NEXT @maxLimit ROWS ONLY";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<Ticket>(query, new { userId, offset, maxLimit }).ToList();

                return result;
            }
        }

        public List<Ticket> GetTicketsByTransactionId(int transactionId, int offset = 0, int maxLimit = 20)
        {
            if (maxLimit > 30)
            {
                maxLimit = 30;
            }

            var query = @"SELECT Tickets.TicketID, Tickets.TicketEventDateID FROM TicketTransactions t
                        JOIN TicketToTransactions ttt ON t.TransactionID = ttt.TransactionID
                        JOIN Tickets ON ttt.TicketID = Tickets.TicketID
                        WHERE t.TransActionID = @transactionId
                        ORDER BY Tickets.TicketID
                        OFFSET @offset ROWS
                        FETCH NEXT @maxLimit ROWS ONLY";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<Ticket>(query, new { transactionId, offset, maxLimit }).ToList();

                return result;
            }
        }

        public Ticket GetTicketById(int id)
        {
            var query = @"SELECT * FROM Tickets WHERE TicketID = @ticketId";


            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<Ticket>(query, new { ticketId = id }).FirstOrDefault();

                return result;
            }
        }

        public FullTicket GetFullTicketById(int id)
        {
            var query = @"SELECT * FROM Tickets t
                        JOIN TicketToTransactions ttt ON t.TicketID = ttt.TicketID
                        JOIN TicketsTransactions tt ON ttt.TransactionID = tt.TransactionID
                        JOIN SeatsAtEventDate ON Tickets.SeatID = SeatsAtEventDate.SeatID
                        JOIN TicketEventDates ted ON SeatsAtEventDate.TicketEventDateID = ted.TicketEventDateID
                        JOIN Venues v ON ted.VenueID = v.VenueID
                        JOIN TicketEvent te ON ted.TicketEventID = te.TicketEventID
                        WHERE t.TicketID = @ticketId";


            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<FullTicket>(query, new { ticketId = id }).FirstOrDefault();

                return result;
            }
        }

        public List<FullTicket> GetFullTicketsByTransactionId(int transactionId)
        {
            var query = @"SELECT * FROM TicketTransactions t
                        JOIN TicketToTransactions ttt ON t.TransactionID = ttt.TransactionID
                        JOIN Tickets ON ttt.TicketID = Tickets.TicketID
                        JOIN SeatsAtEventDate ON Tickets.SeatID = SeatsAtEventDate.SeatID
                        JOIN TicketEventDates ted ON SeatsAtEventDate.TicketEventDateID = ted.TicketEventDateID
                        JOIN Venues v ON ted.VenueID = v.VenueID
                        JOIN TicketEvent te ON ted.TicketEventID = te.TicketEventID
                        WHERE t.TransActionID = @transactionId
                        ORDER BY Tickets.TicketID";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<FullTicket>(query, new { transactionId }).ToList();

                return result;
            }
        }


    }
}
