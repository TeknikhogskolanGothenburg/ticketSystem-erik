/*
 * Ticket system api
 *
 * An API which gives access to all parts of the ticket system
 *
 * OpenAPI spec version: 1.0.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using TicketApi.Attributes;
using TicketApi.Db.Models;
using TicketApi.Db;
using TicketApi.Settings;
using Microsoft.Extensions.Options;

namespace TicketApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class EventDateApiController : Controller
    {
        private DbSettings _dbSettings;
        private EventDateRepository _eventDateRepository;

        public EventDateApiController(IOptions<DbSettings> db)
        {
            this._dbSettings = db.Value;
            this._eventDateRepository = new EventDateRepository(_dbSettings.DefaultConnection);
        }
        /// <summary>
        /// Create new event date
        /// </summary>
        /// <remarks>Adds a new event date in system</remarks>
        /// <param name="body">EventDate data</param>
        /// <response code="200">Event date created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Route("/api/eventdate")]
        [ValidateModelState]
        [SwaggerOperation("AddEventDate")]
        [SwaggerResponse(200, typeof(Object), "Event date created")]
        [SwaggerResponse(400, typeof(Object), "Bad request")]
        public virtual IActionResult AddEventDate([FromBody]EventDate body)
        {
            var result = _eventDateRepository.AddEventDate(body);
            if (result == null)
            {
                return BadRequest();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Delete event date
        /// </summary>
        /// <remarks>Delete an event date</remarks>
        /// <param name="eventDateId">Event date ID</param>
        /// <response code="200">Event date deleted</response>
        /// <response code="404">Event date not found</response>
        [HttpDelete]
        [Route("/api/eventdate/{eventDateId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteEventDate")]
        [SwaggerResponse(200, typeof(Object), "EventDate deleted")]
        [SwaggerResponse(404, typeof(Object), "EventDate not found")]
        public virtual IActionResult DeleteEventDate([FromRoute]int? eventDateId)
        {
            var result = _eventDateRepository.DeleteEventDate((int)eventDateId);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Get all event dates matching query
        /// </summary>
        /// <remarks>Returns all event date matching query</remarks>
        /// <param name="query">query to find events, type Search</param>
        /// <response code="200">Event date search result loaded</response>
        /// <response code="404">No events found</response>
        [HttpPost]
        [Route("/api/eventdate/search/{query}")]
        [ValidateModelState]
        [SwaggerOperation("FindEventDates")]
        [SwaggerResponse(200, typeof(List<EventDate>), "Event date search result loaded")]
        [SwaggerResponse(404, typeof(List<EventDate>), "No events found")]
        public virtual IActionResult FindEventDates([FromRoute]Search query)
        {
            var result = _eventDateRepository.FindEventDates(query.Searchstring);
            if (result == null || result.Count() == 0)
            {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get event date by parent EventID in system
        /// </summary>
        /// <remarks>Returns an event date by parent EventID</remarks>
        /// <param name="eventId">Event ID</param>
        /// <response code="200">Event loaded</response>
        /// <response code="404">Event not found</response>
        [HttpGet]
        [Route("/api/eventdate/event/{eventId}")]
        [ValidateModelState]
        [SwaggerOperation("GetEventDateByEventId")]
        [SwaggerResponse(200, typeof(Object), "Event loaded")]
        [SwaggerResponse(404, typeof(Object), "Event not found")]
        public virtual IActionResult GetEventDateByEventId([FromRoute]int? eventId)
        {
            var result = _eventDateRepository.GetEventDateByEventId((int)eventId);
            if (result == null)
            {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get event date by Id in system
        /// </summary>
        /// <remarks>Returns an event date by ID</remarks>
        /// <param name="eventDateId">Event date ID</param>
        /// <response code="200">Event date loaded</response>
        /// <response code="404">Event date not found</response>
        [HttpGet]
        [Route("/api/eventdate/{eventDateId}")]
        [ValidateModelState]
        [SwaggerOperation("GetEventDateById")]
        [SwaggerResponse(200, typeof(Object), "Event date loaded")]
        [SwaggerResponse(404, typeof(Object), "Event date not found")]
        public virtual IActionResult GetEventDateById([FromRoute]int? eventDateId)
        {
            var result = _eventDateRepository.GetEventDateById((int)eventDateId);
            if (result == null)
            {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get all eventdate in system
        /// </summary>
        /// <remarks>Returns all eventdates</remarks>
        /// <response code="200">Events loaded</response>
        /// <response code="400">Bad request</response>
        [HttpGet]
        [Route("/api/eventdate")]
        [Route("/api/eventdate/{offset}/{maxLimit}")]
        [ValidateModelState]
        [SwaggerOperation("GetEventDates")]
        [SwaggerResponse(200, typeof(List<EventDate>), "Events loaded")]
        [SwaggerResponse(400, typeof(List<EventDate>), "Bad request")]
        public virtual IActionResult GetEventDates([FromRoute]int offset = 0, [FromRoute]int maxLimit = 20)
        {
            var result = _eventDateRepository.GetEventDates(offset, maxLimit);
            if (result == null || result.Count() == 0)
            {
                return BadRequest();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get event date by Id in system
        /// </summary>
        /// <remarks>Returns an event date by ID</remarks>
        /// <param name="eventDateId">Event date ID</param>
        /// <response code="200">Event date loaded</response>
        /// <response code="404">Event date not found</response>
        [HttpGet]
        [Route("/api/eventdate/full/{eventDateId}")]
        [ValidateModelState]
        [SwaggerOperation("GetFullEventDateById")]
        [SwaggerResponse(200, typeof(Object), "Event date loaded")]
        [SwaggerResponse(404, typeof(Object), "Event date not found")]
        public virtual IActionResult GetFullEventDateById([FromRoute]int? eventDateId)
        {
            var result = _eventDateRepository.GetFullEventDateById((int)eventDateId);
            if (result == null)
            {
                return BadRequest();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Update event date
        /// </summary>
        /// <remarks>Update an event date</remarks>
        /// <param name="eventDate">Event date data</param>
        /// <response code="200">Event date updated</response>
        /// <response code="404">Event date not found</response>
        [HttpPut]
        [Route("/api/eventdate")]
        [ValidateModelState]
        [SwaggerOperation("UpdateEventDate")]
        [SwaggerResponse(200, typeof(Object), "Event date updated")]
        [SwaggerResponse(404, typeof(Object), "Event date not found")]
        public virtual IActionResult UpdateEventDate([FromBody]EventDate eventDate)
        {
            var result = _eventDateRepository.UpdateEventDate(eventDate);
            if (result == null)
            {
                return BadRequest();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get number of sold tickets to EventDate
        /// </summary>
        /// <remarks>Returns a number of sold tickets</remarks>
        /// <param name="eventDateId">EventDate ID</param>
        /// <response code="200">Event loaded</response>
        /// <response code="404">Event not found</response>
        [HttpGet]
        [Route("/api/eventdate/soldtickets/{eventDateId}")]
        [ValidateModelState]
        [SwaggerOperation("GetSoldTicketCount")]
        [SwaggerResponse(200, typeof(int?), "Event loaded")]
        [SwaggerResponse(404, typeof(int?), "Event not found")]
        public virtual IActionResult GetSoldTicketCount([FromRoute]int? eventDateId)
        {
            var result = _eventDateRepository.GetSoldTicketCount((int)eventDateId);
            if (result == 0)
            {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
