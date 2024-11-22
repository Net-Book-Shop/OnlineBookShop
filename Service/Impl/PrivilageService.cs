using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Repository;
using System.Data;

namespace OnlineBookShop.Service.Impl
{
    public class PrivilageService : IPrivilageService
    {
        private readonly PrivilegeRepository _privilageRepository;
        private readonly RoleRepository _roleRepository;
        private readonly PrivilegeDetailsRepository _privilegeDetailsRepository;

        public PrivilageService(PrivilegeRepository privilageRepository, RoleRepository roleRepository, PrivilegeDetailsRepository privilegeDetailsRepository)
        {
            _privilageRepository = privilageRepository;
            _roleRepository = roleRepository;
            _privilegeDetailsRepository = privilegeDetailsRepository;
        }

        public async Task<ResponseMessage> AddPrivilage(PrivilageRequestDTO requestDTO)
        {
            if (string.IsNullOrEmpty(requestDTO.privilegeName))
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Privilage Name is required."
                };
            }
            var newPrivilage = new Privilege
            {
                IsActive = 1,
                PrivilegeName = requestDTO.privilegeName,

            };
            await _privilageRepository.AddPrivilage(newPrivilage);
            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "successfully."
            };
        }


        public async Task<ResponseMessage> AssignPrivileges(PrivilageRequestDTO requestDTO)
        {
            if (requestDTO.privilegeIds.Count <0 || string.IsNullOrEmpty(requestDTO.role))
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Privilege IDs and Role are required."
                };
            }

            var role = await _roleRepository.FindByRoleName(requestDTO.role);
            if (role == null)
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "Role not found."
                };
            }

            var privilegeIds = requestDTO.privilegeIds
                                             .Where(id => Guid.TryParse(id, out _)) 
                                             .Select(Guid.Parse)
                                             .ToList();
            if (!privilegeIds.Any())
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Invalid privilege IDs provided."
                };
            }

            var privileges = await _privilageRepository.FindPrivilegesByIdsAsync(privilegeIds);
            if (privileges.Count != privilegeIds.Count)
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "Some privileges were not found."
                };
            }

            var privilegeDetails = privileges.Select(privilege => new PrivilegeDetails
            {
                RoleId = role.Id,
                PrivilegeId = privilege.Id,
                IsActive = 1
            }).ToList();

            await _privilegeDetailsRepository.AddPrivilegeDetailsAsync(privilegeDetails);

            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "Privileges successfully assigned to role."
            };


        }

        public async Task<ActionResult<ResponseMessage>> GetAllPrivileges()
        {
            var privilageList = await _privilageRepository.FindAllPrivilages();
            var privilegeDtoList = privilageList.Select(detail => new PrivilageRequestDTO
            {
                privilegeName = detail.PrivilegeName,
                privilegeId = detail.Id.ToString(),
                IsActive = detail.IsActive
            }).ToList();

            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "success",
                Data = privilegeDtoList
            };
        }

        public async Task<ResponseMessage> GetRoleWisePrivileges(PrivilageRequestDTO requestDTO)
        {
            if (string.IsNullOrEmpty(requestDTO.role))
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Role not found."
                };
            }

            var role = await _roleRepository.FindByRoleName(requestDTO.role);
            if (role == null)
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "Role Data not found."
                };
            }
            var privilageDetailList = await _privilegeDetailsRepository.FindAllRoleWisePrivilageDetails(role.Id);
            if (privilageDetailList== null)
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "Privilage details is empty"
                };
            }
            var privilegeDtoList = privilageDetailList.Select(detail => new PrivilageRequestDTO
            {
                privilegeName = detail.Privilege?.PrivilegeName ?? string.Empty, 
                role = role.Name,
                privilegeId = detail.PrivilegeId.ToString(),
                IsActive = detail.IsActive
            }).ToList();

            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "success",
                Data = privilegeDtoList 
            };

        }

        public async Task<ResponseMessage> UpdatePrivilages(PrivilageRequestDTO requestDTO)
        {
            if (requestDTO.privilegeIds.Count < 0 || string.IsNullOrEmpty(requestDTO.role))
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Privilege IDs and Role are required."
                };
            }

            var role = await _roleRepository.FindByRoleName(requestDTO.role);
            if (role == null)
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "Role not found."
                };
            }

            // Parse and validate privilege IDs
            var privilegeIds = requestDTO.privilegeIds
                .Where(id => Guid.TryParse(id, out _))
                .Select(Guid.Parse)
                .ToList();

            if (!privilegeIds.Any())
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Invalid privilege IDs provided."
                };
            }

            // Validate that privilege IDs exist
            var privileges = await _privilageRepository.FindPrivilegesByIdsAsync(privilegeIds);
            if (privilegeIds.Count != privilegeIds.Count)
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "Some privileges were not found."
                };
            }
            var privilegeIdList = privileges.Select(p => p.Id).ToList();

            var privilegeDetailsList = await _privilegeDetailsRepository.FindPrivilegeDetailRoleIdAndPrivilageIDs(privilegeIdList, role.Id);

            if (!privilegeDetailsList.Any())
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "No privilege details found for the specified role and privileges."
                };
            }

            // Mark privilege details as inactive
            foreach (var privilegeDetail in privilegeDetailsList)
            {
                privilegeDetail.IsActive = 0;
            }

            await _privilegeDetailsRepository.UpdatePrivilegeDetailsAsync(privilegeDetailsList);

            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "Privileges successfully removed from role."
            };
        }

    }
}
