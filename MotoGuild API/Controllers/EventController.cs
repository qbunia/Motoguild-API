using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Models.Event;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEvent()
        {
            var events = DataManager.Current.Events;
            return Ok(events);
        }

        [HttpGet("{id:int}", Name = "GetEvent")]
        public IActionResult GetEvent(int id, [FromQuery] bool selectedData = false)
        {
            if (selectedData)
            {
                return GetEventWithSelectedData(id);
            }
            return GetAllEventData(id);
        }

        [HttpPost]
        public IActionResult CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int newId = GetNewId();
            var events = SaveEventToDataManager(createEventDto, newId);
            var owner = DataManager.Current.Users.FirstOrDefault(u=> u.Id == events.Owner.Id);
            owner.Events.Add(events);
            return CreatedAtRoute("GetEvent", new { id = events.Id }, events);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEvent(int id)
        {
            var events = DataManager.Current.Events.FirstOrDefault(e => e.Id == id);
            var eventSelectedData = DataManager.Current.EventSelectedData.FirstOrDefault(e => e.Id == id);
            if (events == null || eventSelectedData == null)
            {
                return NotFound();
            }
            DataManager.Current.EventSelectedData.Remove(eventSelectedData);
            DataManager.Current.Events.Remove(events);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateEvent(int id, [FromBody] UpdateEventDto updateEventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var eventDto = DataManager.Current.Events.FirstOrDefault(i => i.Id == id);
            var eventSelectDataDto = DataManager.Current.EventSelectedData.FirstOrDefault(i => i.Id == id);
            if (eventDto == null || eventSelectDataDto == null)
            {
                return NotFound();
            }
            UpdateAllEventData(eventDto, updateEventDto);
            UpdateEventSelectData(eventSelectDataDto, updateEventDto);
            return NoContent();
        }

        private void UpdateAllEventData(EventDto eventDto, UpdateEventDto updateEventDto)
        {

            eventDto.Name = updateEventDto.Name;
            eventDto.Description = updateEventDto.Description;
            eventDto.Participants = updateEventDto.Participants;
            eventDto.Place = updateEventDto.Place;
            eventDto.Start = updateEventDto.Start;
            eventDto.Stop = updateEventDto.Stop;
            eventDto.Posts = updateEventDto.Posts;

        }

        private void UpdateEventSelectData(EventSelectedDataDto eventSelectDataDto, UpdateEventDto updateEventDto)
        {
            eventSelectDataDto.Name = updateEventDto.Name;
        }

        private EventDto SaveEventToDataManager(CreateEventDto createEventDto, int id)
        {
            var owner = DataManager.Current.Users.FirstOrDefault(u => u.Id == createEventDto.OwnerId);
            EventDto ewent = new EventDto()
            {
                Id = id,
                Name = createEventDto.Name,
                Owner = new UserSelectedDataDto() { UserName = owner.UserName, Id = owner.Id, Email = owner.Email, Rating = owner.Rating },
                Description = createEventDto.Description,
                Participants = createEventDto.Participants,
                Place = createEventDto.Place,
                Start = createEventDto.Start,
                Stop = createEventDto.Stop,
                Posts = createEventDto.Posts,
            };
            DataManager.Current.Events.Add(ewent);
            return ewent;
        }

        private void SaveEventSelectedDataToDataManager(CreateEventDto createEventDto, int id)
        {
            EventSelectedDataDto eventSelectedData = new EventSelectedDataDto()
            {
                Id = id,
                Name = createEventDto.Name,
            };
            DataManager.Current.EventSelectedData.Add(eventSelectedData);
        }

        private IActionResult GetEventWithSelectedData(int id)
        {
            var ewent = DataManager.Current.EventSelectedData.FirstOrDefault(e => e.Id == id);
            if (ewent == null)
            {
                return NotFound();
            }
            return Ok(ewent);
        }

        private IActionResult GetAllEventData(int id)
        {
            var ewent = DataManager.Current.Events.FirstOrDefault(u => u.Id == id);
            if (ewent == null)
            {
                return NotFound();
            }
            return Ok(ewent);
        }

        private int GetNewId()
        {
            if (DataManager.Current.Events.Count == 0)
            {
                return 1;
            }
            return DataManager.Current.Events.Max(u => u.Id) + 1;
        }
    }
}
