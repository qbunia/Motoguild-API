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
            if (!UserExists(createGroupDto.OwnerId))
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

        [HttpPut("{id:int}")]
        public IActionResult AddMember([FromBody] GroupAddMemberDto addMemberDto, int id)
        {
            var group = DataManager.Current.Groups.FirstOrDefault(g => g.Id == id);
            var member = DataManager.Current.Users.FirstOrDefault(u => u.Id == addMemberDto.MemberId);
            if (group == null || member == null)
            {
                return NotFound();
            }
            if (group.Members.Any(u => u.Id == addMemberDto.MemberId) || group.Owner.Id == addMemberDto.MemberId)
            {
                ModelState.AddModelError(key: "Description", errorMessage: "User is already in group");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var memberSelectedData = new UserSelectedDataDto()
                { Email = member.Email, Id = member.Id, Rating = member.Rating, UserName = member.UserName };
            if (group.IsPrivate)
            {
                group.PendingMembers.Add(memberSelectedData);
                return NoContent();
            }
            group.Members.Add(memberSelectedData);
            //member.Groups.Add(group);
            return NoContent();

        }

        [HttpPut("accept/{id:int}")]
        public IActionResult AcceptMember([FromBody] GroupAddMemberDto addMemberDto, int id)
        {
            var group = DataManager.Current.Groups.FirstOrDefault(g => g.Id == id);
            var member = DataManager.Current.Users.FirstOrDefault(u => u.Id == addMemberDto.MemberId);
            if (group == null || member == null)
            {
                return NotFound();
            }
            var memberSelectedData = new UserSelectedDataDto()
            { Email = member.Email, Id = member.Id, Rating = member.Rating, UserName = member.UserName };
            if (group.PendingMembers.FirstOrDefault(m => m.Id == memberSelectedData.Id) == null)
            {
                ModelState.AddModelError(key: "Description", errorMessage: "User not found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            group.PendingMembers.Remove(memberSelectedData);
            //member.Groups.Add(group);
            group.Members.Add(memberSelectedData);
            return NoContent();
        }

        private bool UserExists(int id)
        {
            return DataManager.Current.Users.FirstOrDefault(u => u.Id == id) != null;
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
            //owner.Groups.Add(group);
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
