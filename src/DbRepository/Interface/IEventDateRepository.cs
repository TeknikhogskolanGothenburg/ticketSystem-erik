﻿using System;
using System.Collections.Generic;
using TicketApi.Db.Models;

namespace TicketApi.Db.Interface
{
    public interface IEventDateRepository
    {

        
        /// <summary>
        /// Add event date to database
        /// </summary>
        /// <param name="eventDate">EventDate object</param>
        /// <returns>An object representation of the created EventDate</returns>
        EventDate AddEventDate(EventDate eventDate);

        /// <summary>
        /// Get Event Dates
        /// </summary>
        /// <param name="offset">Result set offset</param>
        /// <param name="maxLimit">Max result rows to fetch</param>
        /// <returns>A list of object representations of EventDates</returns>
        List<EventDate> GetEventDates(int offset = 0, int maxLimit = 20);
        /// <summary>
        /// Get Full Event Dates
        /// </summary>
        /// <param name="offset">Result set offset</param>
        /// <param name="maxLimit">Max result rows to fetch</param>
        /// <returns>A list of object representations of FullEventDates</returns>
        List<FullEventDate> GetFullEventDates(int offset = 0, int maxLimit = 20);

        /// <summary>
        /// Get EventDate by ID
        /// </summary>
        /// <param name="id">ID of EventDate</param>
        /// <returns>An object representation of the EventDate</returns>
        EventDate GetEventDateById(int id);

        /// <summary>
        /// Get EventDate by parent Event ID
        /// </summary>
        /// <param name="eventId">ID of Parent Event</param>
        /// <returns>An object representation of the EventDate</returns>
        EventDate GetEventDateByEventId(int eventId);

        /// <summary>
        /// Get the full EventDate w/ all data of venue, event
        /// </summary>
        /// <param name="id">ID of the EventDate</param>
        /// <returns>An object representation of the EventDate</returns>
        FullEventDate GetFullEventDateById(int id);

        /// <summary>
        /// Get a list of FullEventDate objects by TicketEventID
        /// </summary>
        /// <param name="eventId">Ticket event ID</param>
        /// <param name="offset">Result set offset</param>
        /// <param name="maxLimit">Max number of rows to fetch</param>
        /// <returns>A list of object representations of FullEventDates</returns>
        List<FullEventDate> GetFullEventDatesByEventId(int eventId, int offset = 0, int maxLimit = 20);

        /// <summary>
        /// Update the EventDate
        /// </summary>
        /// <param name="eventDate">EventDate object</param>
        /// <returns>An object representation of the updated EventDate</returns>
        EventDate UpdateEventDate(EventDate eventDate);

        /// <summary>
        /// Delete EventDate by ID
        /// </summary>
        /// <param name="id">ID of the EventDate</param>
        /// <returns>bool, true if delete succeeded</returns>
        bool DeleteEventDate(int id);

        /// <summary>
        /// Find FullEventDates matching the search query
        /// </summary>
        /// <param name="searchStr">Search query</param>
        /// <returns>A list of object representations of FullEventDates matching the query</returns>
        List<FullEventDate> FindFullEventDates(string searchStr);

              
        /// <summary>
        /// Get number of available seats at EventDate
        /// </summary>
        /// <param name="id">ID of EventDate</param>
        /// <returns>Number of available seats at EventDate</returns>
        int GetSoldTicketCount(int id);


    }
}
