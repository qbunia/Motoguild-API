using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetGroups()
        {
            var groups = DataManager.Current.Groups;
            var groupsSelectedData = SelectBasicInformationOfGroups(groups);
            return Ok(groupsSelectedData);
        }

        [HttpGet("{id:int}", Name = "GetGroup")]
        public IActionResult GetGroup(int id, [FromQuery] bool selectedData = false)
        {
            if (selectedData)
            {
                return GetGroupWithSelectedData(id);
            }
            return GetGroupData(id);
        }

        [HttpPost]
        public IActionResult CreateGroup([FromBody] CreateGroupDto createGroupDto)
        {
            if (!OwnerExists(createGroupDto))
            {
                ModelState.AddModelError(key: "Description", errorMessage: "User not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newId = GetNewId();
            var group = SaveGroupToDataManager(createGroupDto, newId);
            return CreatedAtRoute("GetGroup", new { id = group.Id }, group);
        }

        [HttpPut]


        private bool OwnerExists(CreateGroupDto createGroupDto)
        {
            return DataManager.Current.Users.FirstOrDefault(u => u.Id == createGroupDto.OwnerId) != null;
        }

        private GroupDto SaveGroupToDataManager(CreateGroupDto createGroupDto, int id)
        {
            var owner = DataManager.Current.Users.FirstOrDefault(u => u.Id == createGroupDto.OwnerId);
            var ownerSelectedData = new UserSelectedDataDto()
            {
                Email = owner.Email, Id = owner.Id,
                Rating = owner.Rating, UserName = owner.UserName
            };
            var group = new GroupDto()
            {
                Id = id, CreationDate = DateTime.Now, IsPrivate = createGroupDto.IsPrivate, Members = new List<UserSelectedDataDto>(),
                Owner = ownerSelectedData, Name = createGroupDto.Name,
                PendingMembers = new List<UserSelectedDataDto>(), Posts = new List<PostDto>()
            };
            DataManager.Current.Groups.Add(group);
            owner.Groups.Add(group);
            return group;
        }

        private int GetNewId()
        {
            if (DataManager.Current.Groups.Count == 0)
            {
                return 1;
            }
            return DataManager.Current.Groups.Max(g => g.Id) + 1;
        }

        private IActionResult GetGroupWithSelectedData(int id)
        {
            var group = DataManager.Current.Groups.FirstOrDefault(u => u.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            var groupSelectedData = new GroupSelectedDataDto()
                { Id = group.Id, IsPrivate = group.IsPrivate, Members = group.Members, Name = group.Name, Owner = group.Owner};
            return Ok(groupSelectedData);
        }

        private IActionResult GetGroupData(int id)
        {
            var group = DataManager.Current.Groups.FirstOrDefault(g => g.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }

        private List<GroupSelectedDataDto> SelectBasicInformationOfGroups(List<GroupDto> groups)
        {
            var groupsSelectedData = new List<GroupSelectedDataDto>();
            foreach (var group in groups)
            {
                groupsSelectedData.Add(new GroupSelectedDataDto() 
                    { Id = group.Id, IsPrivate = group.IsPrivate, Members = group.Members, Name = group.Name, Owner = group.Owner});
            }
            return groupsSelectedData;
        }
    }
}
